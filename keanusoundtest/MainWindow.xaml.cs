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
        private MediaPlayer lmp = new MediaPlayer();
        private bool canPlay = true;

        public MainWindow()
        {
            InitializeComponent();
            VerTb.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            InitializeSoundFiles();
            mp.MediaEnded += MediaEnded_EventHandler;
            lmp.MediaEnded += LongMediaEnded_EventHandler;
            this.Activated += MainWindow_Activated;
            this.Deactivated += MainWindow_Deactivated;
            ImageSpeakers.Source = new BitmapImage(new Uri("/keanusoundtest;component/BOTHDISABLED.png", UriKind.Relative));
            ImageSpeakers.Tag = "AmbosDesactivados";
            mp.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "SoundTestL.wav"));
            lmp.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "LongSoundTestL.wav"));
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
            var LongSoundTestLeft = AppDomain.CurrentDomain.BaseDirectory + "LongSoundTestL.wav";
            var LongSoundTestEmbeddedFileLeft = "keanusoundtest.LongSoundTestL.wav";
            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(LongSoundTestEmbeddedFileLeft))
            {
                using (var LongSoundFile = new FileStream(LongSoundTestLeft, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(LongSoundFile);
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
            var LongSoundTestRight = AppDomain.CurrentDomain.BaseDirectory + "LongSoundTestR.wav";
            var LongSoundTestEmbeddedFileRight = "keanusoundtest.LongSoundTestR.wav";
            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(LongSoundTestEmbeddedFileRight))
            {
                using (var LongSoundFile = new FileStream(LongSoundTestRight, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(LongSoundFile);
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (canPlay == false) return;
            if (e.Key == Key.Space) { PlaySound(); }
            if (e.Key == Key.Enter) { PlayLongSound(); }
            if (e.Key == Key.Escape) { Application.Current.Shutdown(); }
        }

        private void PlaySound()
        {
            canPlay = false;
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
            if (mp.Source.LocalPath == (AppDomain.CurrentDomain.BaseDirectory + "SoundTestL.wav"))
            {
                ImageSpeakers.Source = new BitmapImage(new Uri("/keanusoundtest;component/IZQDISABLED.png", UriKind.Relative));
                ImageSpeakers.Tag = "IzquierdoDesactivado";
                mp.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "SoundTestR.wav"));
                PlaySound();
            }
            else
            {
                mp.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "SoundTestL.wav"));
                ImageSpeakers.Source = new BitmapImage(new Uri("/keanusoundtest;component/BOTHDISABLED.png", UriKind.Relative));
                ImageSpeakers.Tag = "AmbosDesactivados";
                canPlay = true;
            }
        }

        private void LongMediaEnded_EventHandler(object sender, EventArgs e)
        {
            if (lmp.Source.LocalPath == (AppDomain.CurrentDomain.BaseDirectory + "LongSoundTestL.wav"))
            {
                ImageSpeakers.Source = new BitmapImage(new Uri("/keanusoundtest;component/IZQDISABLED.png", UriKind.Relative));
                ImageSpeakers.Tag = "IzquierdoDesactivado";
                lmp.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "LongSoundTestR.wav"));
                PlayLongSound();
            }
            else
            {
                lmp.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "LongSoundTestL.wav"));
                ImageSpeakers.Source = new BitmapImage(new Uri("/keanusoundtest;component/BOTHDISABLED.png", UriKind.Relative));
                ImageSpeakers.Tag = "AmbosDesactivados";
                canPlay = true;
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
            if (canPlay == false) return;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetSystemVolToMax();
        }

        private void btnPlayLongSound_Click(object sender, RoutedEventArgs e)
        {
            if (canPlay == false) return;
            PlayLongSound();
        }

        private void PlayLongSound()
        {
            canPlay = false;
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
            lmp.Play();
        }
    }
}
