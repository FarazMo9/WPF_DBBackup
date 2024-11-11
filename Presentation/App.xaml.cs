using Microsoft.Win32;
using Application;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public Program ProgramConfig { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            ProgramConfig = Program.GetInstance();
            this.ProgramConfig.OpenFileDialogAction=() =>
            {
                var fileDialog = new OpenFileDialog();
                if(fileDialog.ShowDialog() is true)
                    return fileDialog.FileName;
                return string.Empty;
            };
            this.ProgramConfig.OpenFolderDialogAction = () =>
            {
                var fodlerDialog = new OpenFolderDialog();
                if (fodlerDialog.ShowDialog() is true)
                    return fodlerDialog.FolderName;
                return string.Empty;
            };
            this.ProgramConfig.ShowMessageAction = (message) =>
            {
                MessageBox.Show(message);
            };
            base.OnStartup(e);

        }
    }

}
