﻿using System;
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
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Configuration;
using ProjectDataService;
using Project;


namespace ProjectGUI
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        DataService dataservice = new DataService();
        AmazonService aws;
        public Registration()
        {
            InitializeComponent();
            string icon = ConfigurationManager.AppSettings["Icon"];
            Icon = new BitmapImage(new Uri(icon, UriKind.Relative));
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        public void Reset()
        {
            textBoxFirstName.Text = "";
            textBoxLastName.Text = "";
            textBoxEmail.Text = "";
            passwordBox1.Password = "";
            passwordBoxConfirm.Password = "";
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxEmail.Text.Length == 0 || textBoxFirstName.Text.Length <= 0
                || textBoxLastName.Text.Length <= 0)
            {
                errormessage.Text = "Enter all fields.";
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
                string firstname = textBoxFirstName.Text.Trim().ToUpper();
                string lastname = textBoxLastName.Text.Trim().ToUpper();
                string email = textBoxEmail.Text;
                string password = passwordBox1.Password;
                
                if (passwordBox1.Password.Length == 0)
                {
                    errormessage.Text = "Enter password.";
                    passwordBox1.Focus();
                }
                else if (passwordBoxConfirm.Password.Length == 0)
                {
                    errormessage.Text = "Enter Confirm password.";
                    passwordBoxConfirm.Focus();
                }
                else if (passwordBox1.Password != passwordBoxConfirm.Password)
                {
                    errormessage.Text = "Confirm password must be same as password.";
                    passwordBoxConfirm.Focus();
                }
                else
                {
                    errormessage.Text = "";
                    int added = 0;
                    aws = new AmazonService();
                    string bucketName = Guid.NewGuid().ToString("N");
                    if (aws.CreateABucket(bucketName))
                    {
                        added = dataservice.RegisterUser(email, firstname, lastname, password, bucketName);
                    }
                    if (added > 0)
                    {
                        errormessage.Text = "You have Registered successfully.";
                        Reset();
                    }
                    else
                    {
                        errormessage.Text = "Problem in Registeration";
                    }

                }
            }
        }
    }
}