using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfAppSimple.Models;

namespace WpfAppSimple
{
    /// <summary>
    /// Interaction logic for UsersWindow.xaml
    /// </summary>
    public partial class UsersWindow : Window
    {
        private ObservableCollection<UserVM> users = new ObservableCollection<UserVM>();
        public UsersWindow()
        {
            InitializeComponent();
            InitDataGrid();
        }
        private void InitDataGrid()
        {
            users.Add(new UserVM
            {
                Name = "Іван Петрович",
                Phone = "+380973478382"
            });
            dgUsers.ItemsSource = users;
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            users.Add(new UserVM
            {
                Name = "Матрос",
                Phone = "38834833234"
            });
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if(dgUsers.SelectedItem != null)
                if(dgUsers.SelectedItem is UserVM)
                {
                    var userVM = (UserVM)dgUsers.SelectedItem;
                    userVM.Name = "User Edited";
                }
        }
    }
}
