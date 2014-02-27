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
using Project;
using ProjectDataService;
using System.IO;
using System.Windows.Threading;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace ProjectGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AmazonService amzservice;
        DispatcherTimer timer;
        DataService dataservice = new DataService();
        FileMetaData selectedfile;
        User _user;
        System.Windows.Forms.NotifyIcon ni;

        public MainWindow(User user)
        {
            InitializeComponent();
            string icon = ConfigurationManager.AppSettings["Icon"];
            Icon = new BitmapImage(new Uri(icon, UriKind.Relative));
            FeedBack.Visibility = Visibility.Hidden;
            _user = user;
            timer = new DispatcherTimer();
            amzservice = new AmazonService();

            timer.Start();
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += new EventHandler(TimerTicks);

            ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = new System.Drawing.Icon(icon);
            ni.BalloonTipText = "The app has been minimised. Click the tray icon to show.";
            ni.BalloonTipTitle = "The App";
            ni.Text = "The App";
            ni.Click += new EventHandler(m_notifyIcon_Click);
        }

        #region notification
        void OnClose(object sender, CancelEventArgs args)
        {
            ni.Dispose();
            ni = null;
        }

        private WindowState m_storedWindowState = WindowState.Normal;
        void OnStateChanged(object sender, EventArgs args)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                if (ni != null)
                    ni.ShowBalloonTip(2000);
            }
            else
                m_storedWindowState = WindowState;
        }
        void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            CheckTrayIcon();
        }

        void m_notifyIcon_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = m_storedWindowState;
        }
        void CheckTrayIcon()
        {
            ShowTrayIcon(!IsVisible);
        }

        void ShowTrayIcon(bool show)
        {
            if (ni != null)
                ni.Visible = show;
        }
#endregion 
        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            FeedBack.Visibility = Visibility.Visible;
            FeedBack.Content = "Uploading";
            if (Upload())
            {
                FeedBack.Content = "Upload Successfull";
            }
            else
            {
                FeedBack.Content = "Upload Failed";
            }
        }

        public void TimerTicks(object sender, EventArgs e)
        {
            FeedBack.Visibility = Visibility.Hidden;
            var curentTime = DateTime.Now;
            var synctime = Convert.ToDateTime(tbSyncTime.Text);

            if (CheckTimeElapsed(synctime,curentTime))
            {
                bool uploaded = Upload();
            }
        }

        private bool CheckTimeElapsed(DateTime synctime, DateTime curentTime)
        {
            if (!synctime.Hour.Equals(curentTime.Hour))
            {
                return false;
            }
            if (!synctime.Minute.Equals(curentTime.Minute))
            {
                return false;
            }
            return true;
        }

        private bool Upload()
        {
            bool uploaded = false;
            List<FileMetaData> files = dataservice.GetUploadFileList(_user.UserEmail);
            foreach (FileMetaData file in files)
            {
                if (file.LastModified.CompareTo(DateTime.Now) != 0)
                {
                    if (amzservice.UploadFile(_user.BucketName, file.FileName, file.FilePath))
                    {
                        uploaded = Convert.ToBoolean(dataservice.UpdateLastModified(file.FileID));
                    }
                }
            }
            return uploaded;
        }

        private void lvFileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                selectedfile = (FileMetaData)e.AddedItems[0];
                SetFieldsForUpdate();
                UpdateLayout();
            }
        }

        private void btnAddUpdatePath_Click(object sender, RoutedEventArgs e)
        {
            if (tbNewFilePath.Text.Trim().Length <= 0)
            {
                lblFileFeedBack.Visibility = Visibility.Visible;
                lblFileFeedBack.Content = "Enter a file Name";
                tbNewFilePath.Focus();
            }
            else
            {
                string filepath = @tbNewFilePath.Text.Trim();
                string filename = System.IO.Path.GetFileName(filepath);
                lblFileFeedBack.Visibility = Visibility.Visible;
                if (!CheckFileAlreadyExists(filepath))
                {
                    lblFileFeedBack.Content = "File already exists";
                    tbNewFilePath.Focus();
                }
                else
                {
                    AddUpdate(filepath, filename);
                }
            }
        }

        private void AddUpdate(string filepath, string filename)
        {
            if (btnAddUpdatePath.Content.ToString() == "Add")
            {
                if (dataservice.AddFilePath(filepath, _user.UserEmail, filename) == 1)
                {
                    lblFileFeedBack.Content = " File Path Added. To upload file now click on upload in Upload Tab ";
                    LoadFileList();
                }
                else
                {
                    lblFileFeedBack.Content = " Problem in Adding File Name.";
                }
            }
            else
            {
                string oldfilepath = @selectedfile.FilePath;
                string oldfilename = System.IO.Path.GetFileName(oldfilepath);
                if (amzservice.UpdateFile(_user.BucketName, oldfilename, filename))
                {
                    if (dataservice.UpdateFilePath(selectedfile.FileID, filepath, filename) > 0)
                    {
                        lblFileFeedBack.Content = " File Path Updated. To upload recent changes click on upload in Upload Tab";
                        LoadFileList();
                    }
                    else
                    {
                        lblFileFeedBack.Content = " Problem in Updating File Name in MetaData";
                    }

                }

                else
                {
                    lblFileFeedBack.Content = " Problem in Updating File Name in Cloud";
                }

            }
            ResetAddFilePath();
        }

        private bool CheckFileAlreadyExists(string filepath)
        {
            List<FileMetaData> files = dataservice.GetUploadFileList(_user.UserEmail);
            bool exists = true;
            foreach (var file in files)
            {
                if (filepath.Equals(file.FilePath)) exists = false;
            }
            return exists;
        }

        private void ResetAddFilePath()
        {
            btnAddUpdatePath.Content = "Add";
            tbNewFilePath.Text = "";
            btnRemove.Visibility = Visibility.Hidden;
            selectedfile = null;
        }

        private void SetFieldsForUpdate()
        {
            if (selectedfile != null)
            {
                tbNewFilePath.Text = selectedfile.FilePath;
                btnAddUpdatePath.Content = "Update";
                btnRemove.Visibility = Visibility.Visible;
            }
        }

        private void AddFilePath_GotFocus(object sender, RoutedEventArgs e)
        {
            LoadFileList();
            lblFileFeedBack.Visibility = Visibility.Hidden;
        }

        private void LoadFileList()
        {
            List<FileMetaData> filelist = dataservice.GetUploadFileList(_user.UserEmail);
            lvFileList.ItemsSource = filelist;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ResetAddFilePath();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                
                tbNewFilePath.Text = dlg.FileName;
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            Close();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            int deleted = 0;
            if (amzservice.DeleteAObject(_user.BucketName, selectedfile.FileName))
            {
                deleted = dataservice.DeleteFile(selectedfile.FileID);
            }
            if (deleted > 0)
            {
                lblFileFeedBack.Visibility = Visibility.Visible;
                lblFileFeedBack.Content = "File is deleted from cloud ";
                ResetAddFilePath();
                LoadFileList();
            }
            else
            {
                lblFileFeedBack.Visibility = Visibility.Visible;
                lblFileFeedBack.Content = "Problem deleting file";
            }
        }










    }
}
