using LibDatabase;
using LibDatabase.Entities;
using LibDatabase.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WpfAppSimple
{
    /// <summary>
    /// Interaction logic for UsersWindow.xaml
    /// </summary>
    public partial class UsersWindow : Window
    {
        private ObservableCollection<UserVM> users = new ObservableCollection<UserVM>();
        private readonly MyDataContext _myDataContext;
        int? page;
        const int pageSize = 10;
        int totalCount = 0;
        int totalPages = 0;
        
        public UsersWindow(MyDataContext myDataContext)
        {
            _myDataContext = myDataContext;
            InitializeComponent();
            //InitDataGrid();
        }
        private async Task InitDataGrid(IQueryable<UserEntity> query)
        {
            var cultureInfo = new CultureInfo("uk-UA");
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            totalCount = query.Count();
            totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            int skip = (page ?? 0) * pageSize;
            var users = await query
                .OrderBy(x => x.Id)
                .Select(x => new UserVM
                {
                    Id = x.Id,
                    Name = x.Name,
                    Phone = x.Phone,
                    DateCreated = x.DateCreated != null ?
                    x.DateCreated.Value.ToString("dd MMMM yyyy HH:mm:ss", cultureInfo) : "",
                    Image=x.Image ?? "noimage.png"
                })
                .Skip((page ?? 0)*pageSize)
                .Take(pageSize)
                .ToListAsync();

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            //MessageBox.Show("RunTime " + elapsedTime);

            labelTime.Content = "Runtime " + elapsedTime;
            labelInfo.Content = $"{skip}-{skip+pageSize}/{totalCount}";
            labelPage.Content = $"{(page ?? 0) + 1}/{totalPages}";

            threadId = Thread.CurrentThread.ManagedThreadId;

            dgUsers.ItemsSource = users;
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            users.Add(new UserVM
            {
                //Id = 1,
                Name="Матрос",
                Phone="3883 s8d8d8 asdljf 883883"
            });
        }

        private void btnEditUser_Click(object sender, RoutedEventArgs e)
        {
            
            if (dgUsers.SelectedItem != null)
                if(dgUsers.SelectedItem is UserVM)
                {
                    var userVM = (UserVM)dgUsers.SelectedItem;
                    var user = _myDataContext.Users.SingleOrDefault(x => x.Id == userVM.Id);
                    EditUserWindow pw = new EditUserWindow(_myDataContext, user);
                    pw.Show();
                    pw.txtName.Text = user.Name;
                    pw.txtPhone.Text = user.Phone;
                    pw.txtPassword.Text = user.Password;
                    pw.txtBitmap.Text = user.Image;
                    string src = string.IsNullOrEmpty(user.Image) ? "noimage.png" : $"600_{user.Image}";
                    string url = $"{MyAppConfig.GetSectionValue("FolderSaveImages")}/{src}";
                    pw.imgPhoto.Source = UserVM.toBitmap(File.ReadAllBytes(url));
                }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var query = ReadDataSearch();
            InitDataGrid(query);

        }

        private IQueryable<UserEntity> ReadDataSearch()
        {
            var query = _myDataContext.Users.AsQueryable();
            SearchUser search = new SearchUser();
            search.Name = txtName.Text;
            if (!string.IsNullOrEmpty(search.Name))
            {
                query = query.Where(x => x.Name.Contains(search.Name));
            }
            int count = query.Count();
            if(cbIsImage.IsChecked == true)
                query = query.Where(x => x.Image!=null);
            return query;
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            int p = (page ?? 0);
            if (p == 0)
                return;
            page=--p;

            var query = ReadDataSearch();
            InitDataGrid(query);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            int p = (page ?? 0);
            if (p >= totalPages)
                return;
            page=++p;

            var query = ReadDataSearch();
            InitDataGrid(query);
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
