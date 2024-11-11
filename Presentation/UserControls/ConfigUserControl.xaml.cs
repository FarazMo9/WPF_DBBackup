using Business.Entities;
using Microsoft.Win32;
using Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace Presentation.UserControls
{
    /// <summary>
    /// Interaction logic for ConfigUserControl.xaml
    /// </summary>
    public partial class ConfigUserControl : UserControl
    {
        private readonly Regex numericRegex = new Regex("[^0-9.-]+");
        public ConfigViewModel ViewModel => DataContext as ConfigViewModel;
        public ConfigUserControl()
        {
            InitializeComponent();
        }
        
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (!numericRegex.IsMatch(textBox.Text))
                textBox.Text = numericRegex.Replace(textBox.Text, string.Empty);

        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (!string.IsNullOrEmpty(passwordBox.Password))
                ViewModel.Config.FTPPassword = passwordBox.Password;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Password = ViewModel.Config.FTPPassword;
        }
    }
}
