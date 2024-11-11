using Business;
using Business.DTO;
using Business.Entities;
using Infrastructure.Cryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace Infrastructure
{
    public class IntervalManager(Config appConfig, List<DatabaseInfo> databases, Action<List<DBBackupResult>> onBackupProcessEnded, Action<double> onUploadProgressUpdated)
    {
        private Timer timer;
        public bool IsStarted { get; set; }
        public bool IsStoped => !IsStarted;

        public void StartTimer()
        {
            if (!appConfig.IsValid)
                return;
            IsStarted = true;

            //Using threading timer to raise action on desired intervals
            var autoEvent = new AutoResetEvent(false);
            timer = new Timer(IntervalProcess, autoEvent, 0, appConfig.Interval);

        }

        private async void IntervalProcess(object stateInfo)
        {
            try
            {
                AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
                var operationResultLogs = new List<DBBackupResult>();
                var ftpManager = new FTPManager(appConfig.FTPUrl, appConfig.FTPUsername, appConfig.FTPPassword, onUploadProgressUpdated);
                foreach (var database in databases)
                {
                    var backupOperation = DBBackupManager.BackupDatabase(database);
                    var allowDirectoryRemoval = false;
                    try
                    {
                        //First Step : DB backup file will be prepared on the backup directory
                        if (backupOperation.Success)
                        {
                            //Second Step : The Backup file will be encrypted by AES and the encrypted file will be generated beside that.
                            var cryptionResult = CryptionManager.EncryptFile(backupOperation.SourceBackupPath, backupOperation.EncryptedBackupPath);
                            backupOperation.Success &= cryptionResult.Success;
                            backupOperation.MessageLog = cryptionResult.Message;


                            if (cryptionResult.Success && appConfig.IsFTPAvailable)
                            {

                                //Third Step : If the FTP config is available, the encrypted file will be uploaded
                                var isConnectionAvailable = await ftpManager.IsConnectionAvailable();
                                backupOperation.Success &= isConnectionAvailable.Success;
                                backupOperation.MessageLog = isConnectionAvailable.Message;
                                if (isConnectionAvailable.Success)
                                {
                                    var checkRemainingStorage = await ftpManager.CheckRemainingHostStorage(backupOperation.EncryptedBackupPath, appConfig.HostSize);
                                    if (checkRemainingStorage.Success)
                                    {
                                        //While the FTP connection stablished, the upload process will be started
                                        var uploadResult = await ftpManager.UploadBackupFile(backupOperation.EncryptedBackupPath, backupOperation.EncryptedBackupFileName);
                                        onUploadProgressUpdated?.Invoke(0);
                                        backupOperation.Success &= uploadResult.Success;
                                        backupOperation.MessageLog = uploadResult.Message;
                                        allowDirectoryRemoval = true;
                                    }
                                  

                                }
                            }
                        }


                        operationResultLogs.Add(backupOperation);
                        if (!backupOperation.Success)
                            allowDirectoryRemoval = true;

                    }
                    catch (Exception ex)
                    {
                        allowDirectoryRemoval = true;
                        backupOperation.Success = false;
                        backupOperation.MessageLog = ex.Message;
                    }
                    finally
                    {
                        if (allowDirectoryRemoval)
                        {
                            //Final Step: Remove the local backup directory release the used storage
                            var removeDirectoryResult = RemoveDirectory(backupOperation.BackupDirectory);
                            backupOperation.Success &= removeDirectoryResult.Success;
                            backupOperation.MessageLog=removeDirectoryResult.Message;
                        }
                         

                    }

                }
                onBackupProcessEnded?.Invoke(operationResultLogs);
                autoEvent.Set(); // Reset for the next interval

            }
            catch
            {

            }


        }

        public void StopTimer()
        {
            IsStarted = false;
            if (timer is not null)
                timer.Dispose();
        }

        private OperationResult RemoveDirectory(string backupDirectory)
        {
            try
            {
                if (Directory.Exists(backupDirectory))
                    Directory.Delete(backupDirectory, true);

                return OperationResult.Get();
            }
            catch (Exception ex)
            {
                return OperationResult.Error(message: ex.Message);
            }

        }


    }
}
