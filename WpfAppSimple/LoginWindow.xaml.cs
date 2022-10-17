using LibDatabase.Entities;
using LibDatabase;
using System;
using System.Data;
using System.Data.SqlClient;
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
using Npgsql;

namespace WpfAppSimple
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            MyDataContext myData = new MyDataContext();
            UserEntity user = new UserEntity();
            user = myData.Users.FirstOrDefault(u=>u.Name==txtUserName.Text && u.Password==txtPassword.Text);
            if (user!=null)
            {
                ProfileWindow pw = new ProfileWindow();
                pw.Show();
                pw.lblName.Content = user.Name;
                pw.lblPhone.Content = user.Phone;
                this.Hide();
            }
            else
            {
                MessageBox.Show("Incorrect User name or Password! Try again!");
                txtUserName.Text = "";
                txtPassword.Text = "";
            }
        }
    }
}
