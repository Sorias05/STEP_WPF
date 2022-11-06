using Microsoft.Win32;
using System.IO;
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
using WpfAppSimple.Models;
using System.Collections.ObjectModel;
using LibDatabase;
using LibDatabase.Entities;
using System.Globalization;
using System.Text.RegularExpressions;
using Bogus.Bson;
using System.Security.Policy;
using LibDatabase.Helpers;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace WpfAppSimple
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private ObservableCollection<UserVM> users = new ObservableCollection<UserVM>();
        private MyDataContext _myDataContext;
        string source = "";
        string filePath = "";
        public RegisterWindow(MyDataContext myDataContext)
        {
            _myDataContext = myDataContext;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var user = new UserEntity()
            {
                Name = txtUserName.Text,
                Phone = txtPhone.Text,
                Password = txtPassword.Password,
                Image = source,
                DateCreated = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
            };
            File.Copy(filePath, Path.Combine(MyAppConfig.GetSectionValue("FolderSaveImages"), Path.GetFileName(filePath)));
            _myDataContext.Users.Add(user);
            _myDataContext.SaveChanges();
            this.Close();
        }

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Image files (*.PNG;*.JPG;*.GIF)|*.PNG;*.JPG;*.GIF|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)      
            {
                source = Path.GetFileName(dlg.FileName);
                filePath = dlg.FileName;
                Uri imageUri = new Uri(source, UriKind.Relative);
                BitmapImage imageBitmap = new BitmapImage(imageUri);
                imgPhoto.Source = imageBitmap;
            }
        }

    }
}
