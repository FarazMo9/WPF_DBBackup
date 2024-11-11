using Infrastructure;
using Infrastructure.RepositoryAccess;
using Infrastructure.RepositoryImplementation;
using Infrastructure.Cryption;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Business.Entities;
using Business;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
#nullable disable
namespace Application
{
    public class Program
    {
        public IConfiguration Configuration { get; set; }
        public ServiceProvider ServiceProvider { get; set; }
        public bool CryptionKeyExists { get; set; }
        public Func<string> OpenFileDialogAction;
        public Func<string> OpenFolderDialogAction;

        public Action<string> ShowMessageAction;
        private static Program program;
        public Program()
        {
            Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<AppDBContext>();
            serviceCollection.AddTransient<IBackupLogRepository, BackupLogRepository>();
            serviceCollection.AddTransient<IDatabaseInfoRepository, DatabaseInfoRepository>();
            serviceCollection.AddTransient<IConfigRepository, ConfigRepository>();
            ServiceProvider = serviceCollection.BuildServiceProvider();
            CryptionKeyExists= CryptionManager.GetMainKey().Success;
            CheckForAppDataPath();

        }
        public static Program GetInstance()
        {
            if(program is null)
                program = new Program();
            return program;
        }

        private void CheckForAppDataPath()
        {
            if (!Directory.Exists(GeneralInfo.AppDataPath))
            {
                Directory.CreateDirectory(GeneralInfo.AppDataPath);
                var directory = new DirectoryInfo(GeneralInfo.AppDataPath);

                DirectorySecurity directorySecurity = directory.GetAccessControl();
                // Using this instead of the "Everyone" string means we work on non-English systems.
                SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
                directorySecurity.AddAccessRule(new FileSystemAccessRule(everyone, FileSystemRights.Modify | FileSystemRights.Synchronize, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                directory.SetAccessControl(directorySecurity);

            }
        }
        
        public void ShowMesssage(string message)
        {
            ShowMessageAction?.Invoke(message);
        }
    }
}
