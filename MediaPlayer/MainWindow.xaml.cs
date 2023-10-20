using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        int seconds = 0;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick +=  new EventHandler(Timer_Tick);
            
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            
            sliderSeek.Value = MediaElement1.Position.TotalSeconds;
            timeTxb.Text = $"{++seconds }/{sliderSeek.Maximum}";
        }

        private void OpenFileClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == true)
            {
                string filename = openFileDialog.FileName;
                if (filename == null)
                {
                    Uri uri = new Uri(filename);
                    MediaElement1.Source = uri;
                    MediaElement1.Play();
                }
            }
        }

        private void exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void stopStartClicked(object sender, RoutedEventArgs e)
        {
            if(stopStartPackIcon.Kind == MaterialDesignThemes.Wpf.PackIconKind.Pause)
            {
                MediaElement1.Stop();
                stopStartPackIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;

            }
            else
            {
                MediaElement1.Play();
                stopStartPackIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
            }

        }

        private void silentModeClicked(object sender, RoutedEventArgs e)
        {
            if(silentModesliderpackIcon.Kind == MaterialDesignThemes.Wpf.PackIconKind.VolumeSource)
            {
                MediaElement1.Volume = 0;
                silentModesliderpackIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.VolumeOff;
            }
            else
            {
                MediaElement1.Volume = sliderVol.Value;
                silentModesliderpackIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.VolumeSource;
            }
            
        }

        private void sliderSeek_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaElement1.Position = TimeSpan.FromSeconds(sliderSeek.Value);
        }

        private void sliderVol_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaElement1.Volume = (double)e.NewValue;
        }

        private void Window_Drped(object sender, DragEventArgs e)
        {
            string filename = (string)((DataObject)e.Data).GetFileDropList()[0];
            MediaElement1.Source = new Uri(filename);
            MediaElement1.LoadedBehavior = MediaState.Manual;
            MediaElement1.UnloadedBehavior = MediaState.Manual;
            MediaElement1.Volume = (double)sliderVol.Value;
            
            MediaElement1.Play();
            if (MediaElement1.NaturalDuration.HasTimeSpan)
                sliderSeek.Maximum = (double)MediaElement1.NaturalDuration.TimeSpan.TotalSeconds;
            else
            {
                MessageBox.Show("hasTimeSpan returns false\n");
            }
        }

        private void Media_opened(object sender, RoutedEventArgs e)
        {
            TimeSpan ts = MediaElement1.NaturalDuration.TimeSpan;
            sliderSeek.Maximum = ts.TotalSeconds;
            timer.Start();
        }
    }
}
