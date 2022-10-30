using LibDatabase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using LibDatabase.Delegates;
using LibDatabase.Entities;

namespace WpfAppSimple
{
    public partial class MainWindow : Window 
    {
        public static event ConnectionCompleteDelegate _connectionComplete;
        private MyDataContext _myDataContext;
        private CancellationTokenSource ctSource;
        private CancellationToken token;

        private static ManualResetEvent _mre = new ManualResetEvent(false);
        private bool _isPause = false;
        public MainWindow()
        {
            InitializeComponent();
            _connectionComplete += MainWindow__connectionComplete;
            Thread thread = new Thread(ConnectionDatabase);
            thread.Start();
            
        }
        private void MainWindow__connectionComplete(MyDataContext context)
        {
            _myDataContext = context;
            Dispatcher.Invoke(() =>
            {
                lblStatusBar.Content = "The connection is successful";
            });
        }
        private void ConnectionDatabase()
        {
            _myDataContext = new MyDataContext();
            _connectionComplete?.Invoke(_myDataContext);
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

        private void bntAddUsers_Click(object sender, RoutedEventArgs e)
        {
            int count = int.Parse(txtCount.Text);
            pbCount.Minimum = 0;
            pbCount.Maximum = count;
            ctSource = new CancellationTokenSource();
            token = ctSource.Token;
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
                    Phone = "0938783232",
                    Password = "88888888"
                });
                _myDataContext.SaveChanges();
                Dispatcher.Invoke(() =>
                {
                    pbCount.Value = i;
                    lblStatusBar.Content = $"Adding users: {i+1}/{count}";
                });
                _mre.WaitOne(Timeout.Infinite);
                if(token.IsCancellationRequested)
                {
                    Dispatcher.Invoke(() =>
                    {
                        pbCount.Value = 0;
                        lblStatusBar.Content = $"Adding cancelled.";
                    });
                    return;
                }  
            }
        }

        private void bntCancel_Click(object sender, RoutedEventArgs e)
        {
            ctSource.Cancel();
        }

        private void bntPause_Click(object sender, RoutedEventArgs e)
        {
            if(_isPause)
            {
                _mre.Set();
                bntPause.Content = "Pause";
            }
            else
            {
                _mre.Reset();
                bntPause.Content = "Continue";
            }
            _isPause = !_isPause;
        }

        private void mActionUsers_Click(object sender, RoutedEventArgs e)
        {
            UsersWindow usersWindow = new UsersWindow();
            usersWindow.ShowDialog();
        }
    }
}
