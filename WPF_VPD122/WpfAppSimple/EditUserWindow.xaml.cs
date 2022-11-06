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
using System.Windows.Shapes;
using Path = System.IO.Path;
using LibDatabase.Entities;
using LibDatabase.Helpers;
using LibDatabase;

namespace WpfAppSimple
{
    /// <summary>
    /// Interaction logic for EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        private MyDataContext _myDataContext;
        string source = "";
        string filePath = "";
        UserEntity user;
        public EditUserWindow(MyDataContext myDataContext, UserEntity userE)
        {
            _myDataContext = myDataContext;
            user = userE;
            InitializeComponent();
        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
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
                txtBitmap.Text = source;
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            user.Name = txtName.Text;
            user.Phone = txtPhone.Text;
            user.Password = txtPassword.Text;
            user.Image = source;
            _myDataContext.SaveChanges();
            File.Copy(filePath, Path.Combine(MyAppConfig.GetSectionValue("FolderSaveImages"), Path.GetFileName(filePath)));
            this.Close();
        }
    }
}
