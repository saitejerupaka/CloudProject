using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using ProjectDataService;
using System.Configuration;

namespace ProjectGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class Login : Window
    {
        DataService dataservice = new DataService();
        public Login()
        {
            InitializeComponent();
            string icon = ConfigurationManager.AppSettings["Icon"];
            Icon = new BitmapImage(new Uri(icon, UriKind.Relative));
            textBoxEmail.Focus();
        }
        Registration registration;
        MainWindow main;

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxEmail.Text.Length == 0)
            {
                errormessage.Text = "Enter an email.";
                textBoxEmail.Focus();
            }
            else if (!Regex.IsMatch(textBoxEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                errormessage.Text = "Enter a valid email.";
                textBoxEmail.Select(0, textBoxEmail.Text.Length);
                textBoxEmail.Focus();
            }
            else
            {
                string email = textBoxEmail.Text;
                string password = passwordBox1.Password;
                User user = dataservice.LoginUser(email, password);
                if (user != null)
                {
                    main = new MainWindow(user);
                    main.Show();
                    Close();
                }
                else
                {
                    errormessage.Text = "Sorry! Please enter existing emailid/password.";
                }

            }
        }

        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            registration = new Registration();
            registration.Show();
            Close();
        }

    }
}