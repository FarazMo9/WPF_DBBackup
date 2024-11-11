using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business;
using FluentFTP;
#nullable disable
namespace Infrastructure
{
    //using FluentFTP nuget package for FTP communications
    //The code is prepared by the below link of FluentFTP documentation
    //https://github.com/robinrodricks/FluentFTP/wiki


    public class FTPManager(string FTPUrl, string FTPUsername, string FTPPassword, Action<double> uploadProgress)
    {
        //Tries to check whether the connection will be stablished or not
        public async Task<OperationResult> IsConnectionAvailable()
        {
            try
            {
                using var client = new AsyncFtpClient(FTPUrl, FTPUsername, FTPPassword); // or set Host & Credentials
                await client.AutoConnect();
                return OperationResult.Get(success: true);
            }
            catch
            {
                return OperationResult.Error();
            }

        }

        public async Task<OperationResult> CheckRemainingHostStorage(string backupPath, long hostStorageSpace)
        {
            try
            {
                var fileInfo = new FileInfo(backupPath);

                var cancellationToken = new CancellationToken();
                using var client = new AsyncFtpClient(FTPUrl, FTPUsername, FTPPassword); // or set Host & Credentials
                var files = await client.GetListing();
                var usedSpace = (files.Any() ? files.Sum(file => file.Size) : 0) + fileInfo.Length;
                if (usedSpace > hostStorageSpace)
                {
                    var item = files.OrderBy(File => File.Created).FirstOrDefault();
                    if (item is not null)
                        await client.DeleteFile(item.Name, cancellationToken);
                }
                return OperationResult.Get(success: true);
            }
            catch
            {
                return OperationResult.Error();
            }
        }

        public async Task<OperationResult> UploadBackupFile(string backupPath, string backupName)
        {
            try
            {
                uploadProgress?.Invoke(0);

                var token = new CancellationToken();
                using (var ftp = new AsyncFtpClient(FTPUrl, FTPUsername, FTPPassword))
                {
                    await ftp.Connect(token);

                    // define the progress tracking callback
                    Progress<FtpProgress> progress = new Progress<FtpProgress>(p =>
                    {
                        uploadProgress?.Invoke(p.Progress);


                    });

                    // upload a file with progress tracking
                    await ftp.UploadFile(backupPath, backupName, FtpRemoteExists.Overwrite, false, FtpVerify.None, progress, token);

                }
                return OperationResult.Get(success: true);
            }
            catch
            {
                return OperationResult.Error();

            }

        }

    }
}
