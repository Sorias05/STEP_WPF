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
using System.Drawing.Imaging;
using System.Drawing;

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
            
            var index = cmbGender.SelectedIndex;
            var item = (MyComboBoxItem)cmbGender.Items[index];
            if (item.Id == -1 || txtUserName.Text == null || txtPhone.Text == null || txtPassword.Password == null || imgPhoto.Source == null)
            {
                lblError.Content = "Error! Enter all fields!";
            }
            else
            {
                Gender gender = (Gender)item.Id;
                var user = new UserEntity()
                {
                    Name = txtUserName.Text,
                    Phone = txtPhone.Text,
                    Password = txtPassword.Password,
                    Image = source,
                    Gender = gender,
                    DateCreated = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
                };
                Bitmap bmp = new Bitmap(filePath);
                string[] sizes = { "32", "100", "300", "600", "1200" };
                foreach (string size in sizes)
                {
                    int width = int.Parse(size);
                    var saveBmp = ImageWorker.CompressImage(bmp, width, width, false);
                    saveBmp.Save($"{MyAppConfig.GetSectionValue("FolderSaveImages")}/{size}_{source}", ImageFormat.Jpeg);
                }
                _myDataContext.Users.Add(user);
                _myDataContext.SaveChanges();
                this.Close();
            }
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
                imgPhoto.Source = UserVM.toBitmap(File.ReadAllBytes(filePath)); 
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MyComboBoxItem nodata = new MyComboBoxItem
            {
                Id = -1,
                Name = "Не вказано"
            };
            cmbGender.Items.Add(nodata);

            MyComboBoxItem male = new MyComboBoxItem
            {
                Id = (int)Gender.Male,
                Name = "Чоловік"
            };
            cmbGender.Items.Add(male);

            MyComboBoxItem female = new MyComboBoxItem
            {
                Id = (int)Gender.Female,
                Name = "Жінка"
            };
            cmbGender.Items.Add(female);

            cmbGender.SelectedIndex = 0;
        }
    }
}
