using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Windows;
using RijndailAES;
using Me;

namespace CipherGuiRijndail
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // байты считанного файла
        byte[] _userFile;
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Button_FindFile_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                Label_FilePath.Content = fileDialog.FileName;
            }
            else
            {
                return;
            }

            FileInfo infoFile = new FileInfo((string)Label_FilePath.Content);
            _userFile = new byte[infoFile.Length];

            using (BinaryReader strReader = new BinaryReader(File.Open((string)Label_FilePath.Content, FileMode.Open), Encoding.UTF8))
            {
                strReader.Read(_userFile, 0, _userFile.Length);
            }

            MyConsole.AppendText("Считал файл:\n");
            MyConsole.AppendText(Label_FilePath.Content + "\n");

        }

        private void CipherFile_Click(object sender, RoutedEventArgs e)
        {
            if ((_userFile == null) || (_userFile.Length == 0))
            {
                MyConsole.AppendText("Ошибка! Файл пуст или вы забыли его считать\n");
                return;
            }
            if (UserKey.Password.Length == 0)
            {
                MyConsole.AppendText("Ошибка! Не ввели пароль\n");
                return;
            }

            int textLength = Convert.ToInt32(ComboBox_RijndailTextLength.Text);
            int keyLength = Convert.ToInt32(ComboBox_RijndailKeyLength.Text);

            Rijndail des = new Rijndail(textLength, keyLength);

            switch (ComboBox_ModeDes.SelectedIndex)
            {
                case 0: // ECB
                    _userFile = des.ECB_Chipher(_userFile, Encoding.UTF8.GetBytes(UserKey.Password.ToString()));
                    break;
                case 1: // CBC
                    break;
                case 2: // OFB
                    break;
                case 3: // CFB
                    break;
                default: 
                    break;
            }
            MyConsole.AppendText("Зашифровал файл:\n");
            MyConsole.AppendText(Label_FilePath.Content + "\n");

            using (BinaryWriter strWr = new BinaryWriter(File.Open((string)Label_FilePath.Content, FileMode.Create), Encoding.UTF8))
            {
                strWr.Write(_userFile, 0, _userFile.Length);
                strWr.Flush();
            }

            MyConsole.AppendText("Сохранил результат\n");

            _userFile = new byte[0];
            Label_FilePath.Content = "-";
        }

        private void DecipherFile_Click(object sender, RoutedEventArgs e)
        {
            if ((_userFile == null) || (_userFile.Length == 0))
            {
                MyConsole.AppendText("Ошибка! Файл пуст или вы забыли его считать\n");
                return;
            }
            if (UserKey.Password.Length == 0)
            {
                MyConsole.AppendText("Ошибка! Не ввели пароль\n");
                return;
            }

            int textLength = Convert.ToInt32(ComboBox_RijndailTextLength.Text);
            int keyLength = Convert.ToInt32(ComboBox_RijndailKeyLength.Text);

            Rijndail des = new Rijndail(textLength, keyLength);

            switch (ComboBox_ModeDes.SelectedIndex)
            {
                case 0: // ECB
                    _userFile = des.ECB_Dechipher(_userFile, Encoding.UTF8.GetBytes(UserKey.Password.ToString()));
                    break;
                case 1: // CBC
                    break;
                case 2: // OFB
                    break;
                case 3: // CFB
                    break;
                default:
                    break;
            }

            MyConsole.AppendText("Расшифровал файл:\n");
            MyConsole.AppendText(Label_FilePath.Content + "\n");

            using (BinaryWriter strWr = new BinaryWriter(File.Open((string)Label_FilePath.Content, FileMode.Create), Encoding.UTF8))
            {
                strWr.Write(_userFile, 0, _userFile.Length);
                strWr.Flush();
            }
            MyConsole.AppendText("Сохранил результат\n");

            _userFile = new byte[0];
            Label_FilePath.Content = "-";
        }

        ulong ByteArrayToLong(byte[] btArray)
        {
            // если достаточно байт то просто переводим
            if (btArray.Length >= 8)
            {
                return BitConverter.ToUInt64(btArray, 0);
            }

            // если нет то добавляем нули, и переводим
            byte[] eightByte = new byte[8];
            
            for (int i = 0; i < btArray.Length; i++)
            {
                eightByte[i] = btArray[i];
            }

            for (int i = btArray.Length; i < 8; i++)
            {
                eightByte[i] = byte.MinValue;
            }

            return BitConverter.ToUInt64(eightByte, 0);
        }

        private void MenuItem_File_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Author_Click(object sender, RoutedEventArgs e)
        {
            AboutMe abme = new AboutMe();
            abme.Show();
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
