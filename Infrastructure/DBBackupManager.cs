using Business;
using Business.DTO;
using Business.Entities;
using crypto;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class DBBackupManager
    {
        public static DBBackupResult BackupDatabase(DatabaseInfo databaseInfo)
        {
            try
            {

                var backupFileName = $"{databaseInfo.Name}{DateTime.Now.ToString("yyyy_MM_dd_HH_mm")}";
                var backupDirectory = $"{GeneralInfo.BackupPath}\\{backupFileName}";
                if (!Directory.Exists(GeneralInfo.BackupPath))
                    Directory.CreateDirectory(GeneralInfo.BackupPath);
                if (!Directory.Exists(backupDirectory))
                    Directory.CreateDirectory(backupDirectory);
                

                var backupFileExtension=string.Empty;
                var result = databaseInfo.Database switch
                {
                    Database.SQLServer => SQLServerBackup(databaseInfo, backupDirectory, backupFileName,out backupFileExtension),
                    Database.MySQL => MySQLBackup(databaseInfo, backupDirectory, backupFileName,out backupFileExtension),
                    _ => OperationResult.Get(success: false, message: GeneralInfo.UnknownDBTypeMessage)
                };

                return new DBBackupResult()
                {
                    Database = databaseInfo,
                    Success = result.Success,
                    BackupDirectory = backupDirectory,
                    BackupFileName= backupFileName,
                    BackupFileExtension= backupFileExtension,
                    MessageLog=result.Message
                };
            }
            catch (Exception ex)
            {
                return new DBBackupResult()
                {
                    Database = databaseInfo,
                    Success = false,
                    MessageLog=ex.Message
                };
            }

        }



        private static OperationResult SQLServerBackup(DatabaseInfo databaseInfo, string targetDirectory, string backupFileName,out string backupFileExtension)
        {
            backupFileExtension = GeneralInfo.SQLServerBackupFileExtension;

            try
            {
                using (SqlConnection con = new SqlConnection(databaseInfo.DecryptedConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = $"BACKUP DATABASE {databaseInfo.Name} TO DISK ='{targetDirectory}\\{backupFileName}.bak'";
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }
                return OperationResult.Get(success: true, result: databaseInfo);

            }
            catch(Exception ex)
            {
                return OperationResult.Error(result: databaseInfo,message: ex.Message);
            }
        }
        private static OperationResult MySQLBackup(DatabaseInfo databaseInfo, string targetDirectory, string backupFileName, out string backupFileExtension)
        {
            backupFileExtension = GeneralInfo.MySQLBackupFileExtension;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(databaseInfo.DecryptedConnectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();
                            mb.ExportToFile($"{targetDirectory}\\{backupFileName}.sql");
                            conn.Close();
                        }
                    }
                }
                return OperationResult.Get(success: true, result: databaseInfo);

            }
            catch (Exception ex)
            {
                return OperationResult.Error(result: databaseInfo, message: ex.Message);
            }

        }

      

    }
}
