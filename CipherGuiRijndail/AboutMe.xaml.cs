using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Me
{
    /// <summary>
    /// Логика взаимодействия для AboutMe.xaml
    /// </summary>
    public partial class AboutMe : Window
    {
        public AboutMe()
        {
            InitializeComponent();        
            MyImage.Source = new BitmapImage(new Uri("/Resources/Ya.jpg", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
