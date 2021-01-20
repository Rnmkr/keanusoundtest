using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace keanusoundtest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer mp = new MediaPlayer();
        public MainWindow()
        {
            InitializeComponent();
            InitializeSoundFile();
            this.Activated += MainWindow_Activated;
            this.Deactivated += MainWindow_Deactivated;
        }

        private void MainWindow_Deactivated(object sender, EventArgs e)
        {
            tbTitle.Foreground = Brushes.DimGray;
            btnExit.IsEnabled = false;
            btnPlaySound.IsEnabled = false;
            this.Background = Brushes.DimGray;
            MainGrid.Background = Brushes.Gray;
            Imaged.Source = new BitmapImage(new Uri("/keanusoundtest;component/speakergold-size-d.png", UriKind.Relative));
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            tbTitle.Foreground = Brushes.Black;
            btnExit.IsEnabled = true;
            btnPlaySound.IsEnabled = true;
            this.Background = Brushes.Black;
            MainGrid.Background = Brushes.Wheat;
            Imaged.Source = new BitmapImage(new Uri("/keanusoundtest;component/speakergold-size.png", UriKind.Relative));
        }

        private void InitializeSoundFile()
        {
            var SoundTest = AppDomain.CurrentDomain.BaseDirectory + "SoundTest.wav";
            var SoundTestEmbeddedFile = "keanusoundtest.SoundTest.wav";
            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(SoundTestEmbeddedFile))
            {
                using (var SoundFile = new FileStream(SoundTest, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(SoundFile);
                }
            }
            mp.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "SoundTest.wav"));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) { PlayTestSound(); }
            if (e.Key == Key.Escape) { Application.Current.Shutdown(); }
            //if (e.Key == Key.D1) { Application.Current.Shutdown(0); }
            //if (e.Key == Key.D5) { Application.Current.Shutdown(1); }
        }

        private void PlayTestSound()
        {
            mp.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "SoundTest.wav"));
            mp.Play();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var vc = new VolumeControl();
            vc.SubirVolumenAlMaximo(this);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnPlaySound_Click(object sender, RoutedEventArgs e)
        {
            PlayTestSound();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }

        private void btnNO_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(1);
        }
    }
}
