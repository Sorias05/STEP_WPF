using Bogus;
using LibDatabase;
using LibDatabase.Delegates;
using LibDatabase.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppSimple
{
    class MyImage
    {
        public string Name { get; set; }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public event ConnectionCompleteDelegate _connectionComplete;
        private MyDataContext _myDataContext;
        private CancellationTokenSource ctSourse;
        private CancellationToken token;

        private static ManualResetEvent _mre = new ManualResetEvent(false);
        private bool _isPause = false;
        public MainWindow()
        {
            InitializeComponent();
            _connectionComplete += MainWindow__connectionComplete;
            Thread thread = new Thread(ConnnectionDatabase);
            thread.Start();

            var faker = new Faker<MyImage>()
                    //Use an enum outside scope.
                    .RuleFor(u => u.Name, f => f.Image.LoremFlickrUrl());
            var img = faker.Generate();
        }


        private void MainWindow__connectionComplete(MyDataContext context)
        {
            _myDataContext = context;
            Dispatcher.Invoke(() =>
            {
                lblStatusBar.Content = "Підлкючення успішно виконанно!";
            });
            //
        }

        private void ConnnectionDatabase()
        {
            MyDataContext myDataContext = new MyDataContext();
            _connectionComplete?.Invoke(myDataContext);
        }

        private void mFileExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void mActionRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow window = new RegisterWindow();
            window.ShowDialog();
        }

        private void mActionLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow window = new LoginWindow();
            window.ShowDialog();
        }

        private void btnAddUsers_Click(object sender, RoutedEventArgs e)
        {
            int count = int.Parse(txtCount.Text);
            pbCount.Minimum = 0;
            pbCount.Maximum = count;
            ctSourse = new CancellationTokenSource();
            token = ctSourse.Token;
            Task thread = new Task(()=>AddUsers(count), token);
            thread.Start();

            _mre.Set();
            
        }
        private void AddUsers(object count)
        {
            int countAdd = (int)(count);
            for (int i = 0; i < countAdd; i++)
            {
                _myDataContext.Users.Add(new UserEntity
                {
                    Name = "Іван",
                    Phone = "983 d9s9d 88s8",
                    Password = "123456",
                    DateCreated = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                });
                _myDataContext.SaveChanges();
                Dispatcher.Invoke(() =>
                {
                    pbCount.Value = i;
                    lblStatusBar.Content = $"{i+1}/{count}";
                });
                _mre.WaitOne(Timeout.Infinite);
                if (token.IsCancellationRequested)
                {
                    Dispatcher.Invoke(() =>
                    {
                        pbCount.Value = 0;
                        lblStatusBar.Content = $"Додавання відмінено {i+1}/{count}";
                    });
                    return;
                }
            }
        }

        private void btnCansel_Click(object sender, RoutedEventArgs e)
        {
            ctSourse.Cancel();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if(_isPause) //Якщо потік був залочений
            {
                _mre.Set();
                btnPause.Content = "Пауза";
            }
            else
            {
                _mre.Reset(); //Лочимо потік
                btnPause.Content = "Відновити";
            }
            _isPause = !_isPause;
        }

        private void mActionUsers_Click(object sender, RoutedEventArgs e)
        {
            UsersWindow usersWindow = new UsersWindow(_myDataContext);
            usersWindow.ShowDialog();
        }
    }
}
