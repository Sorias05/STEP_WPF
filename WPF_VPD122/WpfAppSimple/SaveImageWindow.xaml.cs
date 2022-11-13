using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using MessageBox = System.Windows.Forms.MessageBox;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace WpfAppSimple
{
    /// <summary>
    /// Interaction logic for SaveImageWindow.xaml
    /// </summary>
    public partial class SaveImageWindow : Window
    {
        public SaveImageWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string inputFile = dlg.FileName;
                inputImage.Source = UserVM.toBitmap(File.ReadAllBytes(inputFile));
                MessageBox.Show("Select file" + dlg.FileName);
            }
        }
    }
}
