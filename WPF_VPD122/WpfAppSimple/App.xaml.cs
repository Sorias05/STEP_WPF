using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using LibDatabase.Helpers;
using Application = System.Windows.Application;

namespace WpfAppSimple
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var folder = MyAppConfig.GetSectionValue("FolderSaveImages");
            if(!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var window = new MainWindow();
            this.MainWindow = window;
            window.Show();
        }
    }
}
