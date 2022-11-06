using LibDatabase.Entities;
using LibDatabase;
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
using System.Windows.Shapes;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;
using LibDatabase.Helpers;
using WpfAppSimple.Models;
using System.IO;

namespace WpfAppSimple
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var cultureInfo = new CultureInfo("uk-UA");
            MyDataContext myData = new MyDataContext();
            UserEntity user = new UserEntity();
            user = myData.Users.FirstOrDefault(u => u.Name == txtUserName.Text && u.Password == txtPassword.Password);
            if (user != null)
            {
                ProfileWindow pw = new ProfileWindow();
                pw.Show();
                pw.lblName.Content = user.Name;
                pw.lblGender.Content = user.Gender;
                pw.lblPhone.Content = user.Phone;
                pw.lblDateCreated.Content = user.DateCreated != null ? user.DateCreated.Value.ToString("dd MMMM yyyy HH:mm:ss", cultureInfo) : "";
                string src = string.IsNullOrEmpty(user.Image) ? "noimage.png" : $"600_{user.Image}";
                string url = $"{MyAppConfig.GetSectionValue("FolderSaveImages")}/{src}";
                pw.imgPhoto.Source = UserVM.toBitmap(File.ReadAllBytes(url));
                this.Hide();
            }
            else
            {
                lblError.Content = "Error! Incorrect username or password!";
                txtUserName.Text = "";
                txtPassword.Password = "";
            }
        }
    }
}
