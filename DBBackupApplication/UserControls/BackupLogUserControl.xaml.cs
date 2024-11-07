using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
#nullable disable
namespace DBBackupApplication.UserControls
{
    /// <summary>
    /// Interaction logic for BackupLogUserControl.xaml
    /// </summary>
    public partial class BackupLogUserControl : UserControl
    {
        private BackupLogViewModel viewModel;
        public BackupLogUserControl()
        {
            InitializeComponent();
            viewModel = DataContext as BackupLogViewModel;
        }

        public void RefreshLogsHandler()
        {
            viewModel.LoadBackupLogs();
        }
    }
}
