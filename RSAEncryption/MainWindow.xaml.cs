using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;

namespace RSAEncryption
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[] encrypted;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
                fileTB.Text = File.ReadAllText(openFileDialog.FileName);
        }

        private void saveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, fileTB.Text);
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void about_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Created by\nihornekhaienko\non 01.24.21");
        }

        private void encrypt_Click(object sender, RoutedEventArgs e)
        {
            RSAEncryptor.GenerateKey();
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            encrypted = RSAEncryptor.Encrypt(fileTB.Text, false);
            fileTB.Text = ByteConverter.GetString(encrypted);
        }

        private void decrypt_Click(object sender, RoutedEventArgs e)
        {
            fileTB.Text = RSAEncryptor.Decrypt(encrypted, false);
        }
    }
}
