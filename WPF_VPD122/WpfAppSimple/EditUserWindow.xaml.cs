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
using WpfAppSimple.Models;
using System.Drawing.Imaging;
using System.Drawing;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

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
        public EditUserWindow(MyDataContext myDataContext, int id)
        {
            _myDataContext = myDataContext;
            user = _myDataContext.Users.SingleOrDefault(x => x.Id == id);
            filePath = user.Image;
            InitializeComponent();
        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Image files (*.PNG;*.JPG;*.GIF)|*.PNG;*.JPG;*.GIF|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;
            
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                source = Path.GetFileName(dlg.FileName);
                filePath = dlg.FileName;
                imgPhoto.Source = UserVM.toBitmap(File.ReadAllBytes(filePath));
                txtBitmap.Text = source;
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
                user.Name = txtName.Text;
                user.Phone = txtPhone.Text;
                user.Password = txtPassword.Text;
                user.Image = source;
                user.DateUpdated = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                Bitmap bmp = new Bitmap(filePath);
                string[] sizes = { "32", "100", "300", "600", "1200" };
                foreach (string size in sizes)
                {
                    int width = int.Parse(size);
                    var saveBmp = ImageWorker.CompressImage(bmp, width, width, false);
                    saveBmp.Save($"{MyAppConfig.GetSectionValue("FolderSaveImages")}/{size}_{source}", ImageFormat.Jpeg);
                }
                _myDataContext.SaveChanges();
                this.Close();
        }
    }
}
