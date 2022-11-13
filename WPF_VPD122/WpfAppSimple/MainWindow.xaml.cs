using Bogus;
using LibDatabase;
using LibDatabase.Delegates;
using LibDatabase.Entities;
using LibDatabase.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
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
using WpfAppSimple.ProductAction;
using BogusGender = Bogus.DataSets.Name.Gender;

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
            myDataContext.ChangeTracker.AutoDetectChangesEnabled = false;
            _connectionComplete?.Invoke(myDataContext);
        }

        private void mFileExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void mActionRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow window = new RegisterWindow(_myDataContext);
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
            
            var testUser = new Faker<UserEntity>("uk")
                .RuleFor(o => o.Gender, f => f.PickRandom<Gender>())
                .RuleFor(o => o.Name, (f,u) => f.Name.FullName((BogusGender)(int)u.Gender))
                .RuleFor(o => o.Phone, f => f.Person.Phone)
                .RuleFor(o => o.Password, f => f.Internet.Password())
                //.RuleFor(o => o.Image, f => f.Image.LoremFlickrUrl())
                .RuleFor(o => o.DateCreated, f => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));
            for (int i = 0; i < countAdd; i++)
            {
                var user = testUser.Generate();
                using (WebClient client = new WebClient())
                {
                    string url = user.Gender == Gender.Male ? "https://loremflickr.com/1280/960/man" :
                        "https://loremflickr.com/1280/960/girl";
                    using(Stream stream = client.OpenRead(url))
                    {
                        Bitmap bmp = new Bitmap(stream);
                        string fileName = System.IO.Path.GetRandomFileName() + ".jpg";
                        string[] sizes = { "32", "100", "300", "600", "1200" };
                        foreach (string size in sizes)
                        {
                            int width = int.Parse(size);
                            var saveBmp = ImageWorker.CompressImage(bmp, width, width, false);
                            saveBmp.Save($"{MyAppConfig.GetSectionValue("FolderSaveImages")}/{size}_{fileName}", ImageFormat.Jpeg);
                        }
                        user.Image = fileName;
                    }
                }
                _myDataContext.Users.Add(user);
                _myDataContext.SaveChanges();
                Dispatcher.Invoke(() =>
                {
                    pbCount.Value = i+1;
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

        private void mActionSelectImage_Click(object sender, RoutedEventArgs e)
        {
            SaveImageWindow siw = new SaveImageWindow();
            siw.ShowDialog();
        }

        private void mActionProducts_Click(object sender, RoutedEventArgs e)
        {
            ProductForm pf = new ProductForm();
            pf.ShowDialog();
        }
    }
}
