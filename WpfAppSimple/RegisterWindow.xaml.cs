using LibDatabase;
using LibDatabase.Entities;
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

namespace WpfAppSimple
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            MyDataContext myData = new MyDataContext();
            UserEntity user = new UserEntity();
            user.Name = txtUserName.Text;
            user.Phone = txtPhone.Text;
            user.Password = txtPassword.Text;
            myData.Add(user);
            myData.SaveChanges();
            txtUserName.Clear();
            txtPhone.Clear();
            txtPassword.Clear();
        }
    }
}
