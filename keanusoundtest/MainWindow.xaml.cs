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
            SetSystemVolToMax();
            InitializeSoundFiles();
            mp.MediaEnded += MediaEnded_EventHandler;
            mp.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "SoundTestL.wav"));
            this.Activated += MainWindow_Activated;
            this.Deactivated += MainWindow_Deactivated;
            ImageSpeakers.Source = new BitmapImage(new Uri("/keanusoundtest;component/BOTHDISABLED.png", UriKind.Relative));
            ImageSpeakers.Tag = "AmbosDesactivados";
        }

        private void MainWindow_Deactivated(object sender, EventArgs e)
        {
            tbTitle.Foreground = Brushes.DimGray;
            btnExit.IsEnabled = false;
            btnPlaySound.IsEnabled = false;
            this.Background = Brushes.DimGray;
            MainGrid.Background = Brushes.Gray;
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            tbTitle.Foreground = Brushes.Black;
            btnExit.IsEnabled = true;
            btnPlaySound.IsEnabled = true;
            this.Background = Brushes.Black;
            MainGrid.Background = Brushes.Wheat;
        }

        private void InitializeSoundFiles()
        {
            var SoundTestLeft = AppDomain.CurrentDomain.BaseDirectory + "SoundTestL.wav";
            var SoundTestEmbeddedFileLeft = "keanusoundtest.SoundTestL.wav";
            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(SoundTestEmbeddedFileLeft))
            {
                using (var SoundFile = new FileStream(SoundTestLeft, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(SoundFile);
                }
            }

            var SoundTestRight = AppDomain.CurrentDomain.BaseDirectory + "SoundTestR.wav";
            var SoundTestEmbeddedFileRight = "keanusoundtest.SoundTestR.wav";
            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(SoundTestEmbeddedFileRight))
            {
                using (var SoundFile = new FileStream(SoundTestRight, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(SoundFile);
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) { PlaySound(); }
            if (e.Key == Key.Escape) { Application.Current.Shutdown(); }
        }

        private void PlaySound()
        {
            switch (ImageSpeakers.Tag.ToString())
            {
                case "AmbosDesactivados":
                    ImageSpeakers.Source = new BitmapImage(new Uri("/keanusoundtest;component/DERDISABLED.png", UriKind.Relative));
                    ImageSpeakers.Tag = "DerechoDesactivado";
                    break;
                case "DerechoDesactivado":
                    ImageSpeakers.Source = new BitmapImage(new Uri("/keanusoundtest;component/IZQDISABLED.png", UriKind.Relative));
                    ImageSpeakers.Tag = "IzquierdoDesactivado";
                    break;
                default:
                    break;
            }
            mp.Play();
        }

        private void MediaEnded_EventHandler(object sender, EventArgs e)
        {
            //                case "IzquierdoDesactivado":
            //        ImageSpeakers.Source = new BitmapImage(new Uri("/keanusoundtest;component/BOTHDISABLED.png", UriKind.Relative));
            //ImageSpeakers.Tag = "AmbosDesactivados";
            //break;
            if (mp.Source.LocalPath == (AppDomain.CurrentDomain.BaseDirectory + "SoundTestL.wav"))
            {
                ImageSpeakers.Source = new BitmapImage(new Uri("/keanusoundtest;component/IZQDISABLED.png", UriKind.Relative));
                mp.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "SoundTestR.wav"));
                PlaySound();
            }
            else
            {
                mp.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "SoundTestL.wav"));
                
            }

        }
        private async void SetSystemVolToMax()
        {
            await Task.Run(() =>
            {
                var vc = new VolumeControl();
                vc.SubirVolumenAlMaximo(this);
            });
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnPlaySound_Click(object sender, RoutedEventArgs e)
        {
            PlaySound();
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
