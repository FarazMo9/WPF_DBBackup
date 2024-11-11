using Application;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for KeySetupWindow.xaml
    /// </summary>
    public partial class KeySetupWindow : Window
    {
        private KeySetupViewModel viewModel => DataContext as KeySetupViewModel;
        public KeySetupWindow()
        {
            InitializeComponent();
            viewModel.CloseDialogWindow = () =>
            {
                Program.GetInstance().CryptionKeyExists = true;
                this.DialogResult = true;
                this.Close();
            };
        }

        public static void CheckForCryptionKey()
        {
            if(!Program.GetInstance().CryptionKeyExists)
            {
                new KeySetupWindow().ShowDialog();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if(Program.GetInstance().CryptionKeyExists)
            {
                base.OnClosing(e);
                return;
            }

            App.Current.Shutdown();

        }

    }
}
