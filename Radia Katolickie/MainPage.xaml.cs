using System;
using System.Net.NetworkInformation;
using Windows.ApplicationModel.DataTransfer;
using Windows.Media;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Radia_Katolickie
{
    public sealed partial class MainPage : Page
    {
        private SystemMediaTransportControls systemControls;

        private string Source = "http://198.27.80.205:5946/stream";

        private string StationName = "Radio Maryja";

        public MainPage()
        {
            InitializeComponent();
            ChangeThemeLogo();
            CheckInternetConnection();

            systemControls = SystemMediaTransportControls.GetForCurrentView();
            systemControls.ButtonPressed += SystemControls_ButtonPressed;
            systemControls.IsPlayEnabled = true;
            systemControls.IsPauseEnabled = true;
            systemControls.IsStopEnabled = true;

            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
        }

        private void ChangeThemeLogo()
        {
            if (Application.Current.RequestedTheme == ApplicationTheme.Dark)
            {
                MaryjaLogo.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Maryja/Maryja-logoLight.png"));
                ViaLogo.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Via/Via-logoLight.png"));            
                NiepokalanowLogo.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Niepokalanow/Niepokalanow-logoLight.png"));
                ProfetoLogo.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Profeto/Profeto-logoLight.png"));
                NadziejaLogo.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Nadzieja/Nadzieja-logoLight.png"));
            }

            else
            {
                MaryjaLogo.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Maryja/Maryja-logoDark.png"));
                ViaLogo.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Via/Via-logoDark.png"));
                NiepokalanowLogo.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Niepokalanow/Niepokalanow-logoDark.png"));
                ProfetoLogo.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Profeto/Profeto-logoDark.png"));
                NadziejaLogo.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Nadzieja/Nadzieja-logoDark.png"));
            }
        }

        private void CheckInternetConnection()
        {
            bool isInternetConnected = NetworkInterface.GetIsNetworkAvailable();

            if (isInternetConnected == false)
            {
                OpenMessageDialog("Aby aplikacja działała, wymagane jest połączenie z internetem.", "Brak połączenia z internetem.");
            }           
        }

        private void LoadingBarVisible(string status)
        {
            LoadingBar.Visibility = Visibility.Visible;

            StatusTextBlock.Text = StationName + status;
            PlayButton.IsEnabled = false;
            PauseButton.IsEnabled = false;
        }

        private void LoadingBarCollapsed(string status)
        {
           LoadingBar.Visibility = Visibility.Collapsed;

            StatusTextBlock.Text = StationName + status;
            PlayButton.IsEnabled = true;
            PauseButton.IsEnabled = false;
        }

        private async void OpenInBrowser(string url)
        {
            var uriBing = new Uri(@url);
            await Launcher.LaunchUriAsync(uriBing);
        }

        private async void OpenMessageDialog(string message, string title)
        {
            var dialog = new MessageDialog(message, title);
            await dialog.ShowAsync();
        }

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            args.Request.Data.SetWebLink(new Uri("https://tinyurl.com/DonateMohairApps"));
            args.Request.Data.Properties.Title = "Radia Katolickie";
            args.Request.Data.Properties.Description = "Aplikacja 'Radia Katolickie' umożliwia odtwarzanie stacji katolickich przez internet.";
            args.Request.Data.SetText("Apliakcja Radia Katolickie z Twoimi ulubionymi stacjami! Już teraz do pobrania z Microsoft Store na wszystkie urządzenia z Windows 10!");
        }

        private void MediaElementStateChanged(object sender, RoutedEventArgs e)
        {
            if (mediaElement.CurrentState == MediaElementState.Opening)
            {
                LoadingBarVisible(" jest ładowane...");
            }

            else if (mediaElement.CurrentState == MediaElementState.Buffering)
            {
                LoadingBarVisible(" jest buforowane...");
            }

            else if (mediaElement.CurrentState == MediaElementState.Stopped)
            {
                LoadingBarCollapsed(" jest zatrzymane...");
                PauseButton.IsEnabled = true;
            }

            else if (mediaElement.CurrentState == MediaElementState.Paused)
            {
                LoadingBarCollapsed(" jest zapauzowane...");
            }

            else if (mediaElement.CurrentState == MediaElementState.Closed)
            {
                LoadingBarCollapsed(" jest zamknięte...");
            }

            else if (mediaElement.CurrentState == MediaElementState.Playing)
            {
                LoadingBarCollapsed(" jest odtwarzane...");
                PlayButton.IsEnabled = false;
                PauseButton.IsEnabled = true;              
            }

            else
            {
                LoadingBarCollapsed("Nieznany status odtwarzacza.");
            }           
        }

        /////////////////////////// APPBAR BUTTONS ///////////////////////////////////////   

        private void EnterDown(object sender, KeyRoutedEventArgs e)
        {
            if (mediaElement.CurrentState == MediaElementState.Playing)
            {
                PauseMedia();
            }
            else
            {
                PlayMedia();
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            PlayMedia();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            PauseMedia();
        }

        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenMessageDialog("Aplikacja stworzona przez Kacpra Trynieckiego.\nStworzona w UWP, przy użyciu języka C# i technologii XAML. \nv1.0.4", "Radia Katolickie");
        }

        private void MoreButton_Click(object sender, RoutedEventArgs e)
        {
            OpenInBrowser("https://www.facebook.com/MohairApps/");
        }

        private void DonationButton_Click(object sender, RoutedEventArgs e)
        {
            OpenInBrowser("https://tinyurl.com/DonateMohairApps");
        }

        ////////////////////////////////////////// BACKGROUND AUDIO //////////////////////////////////////////////

        private void MediaElement_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            switch (mediaElement.CurrentState)
            {
                case MediaElementState.Playing:
                    systemControls.PlaybackStatus = MediaPlaybackStatus.Playing;
                    break;
                case MediaElementState.Paused:
                    systemControls.PlaybackStatus = MediaPlaybackStatus.Paused;
                    break;
                case MediaElementState.Stopped:
                    systemControls.PlaybackStatus = MediaPlaybackStatus.Stopped;
                    break;
                case MediaElementState.Closed:
                    systemControls.PlaybackStatus = MediaPlaybackStatus.Closed;
                    break;
                default:
                    break;
            }
        }

        void SystemControls_ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            switch (args.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                    PlayMedia();
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    mediaElement.Source = new Uri(Source);
                    PauseMedia();
                    break;
                case SystemMediaTransportControlsButton.Stop:
                    mediaElement.Source = new Uri(Source);
                    StopMedia(); ;
                    break;
                default:
                    break;
            }
        }

        private async void StopMedia()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                mediaElement.Stop();
                mediaElement.Source = new Uri(Source);
            });
        }

        private async void PlayMedia()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                mediaElement.Play();
            });
        }

        private async void PauseMedia()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                mediaElement.Pause();
                mediaElement.Source = new Uri(Source);
            });
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PivotItem pivot = null;
            pivot = (PivotItem)(sender as Pivot).SelectedItem;
            switch (pivot.Header.ToString())
            {
                case "Radio Maryja":
                    StationName = "Radio Maryja";
                    Source = "http://198.27.80.205:5946/stream";
                    mediaElement.Source = new Uri(Source);
                    break;
                case "Radio Via":
                    StationName = "Radio Via";
                    Source = "http://62.133.128.18:8040/";
                    mediaElement.Source = new Uri(Source);
                    break;
                case "Radio Niepokalanów":
                    StationName = "Radio Niepokalanów";
                    Source = "http://88.199.169.10:7600/rn.mp3";
                    mediaElement.Source = new Uri(Source);
                    break;
                case "Radio Profeto":
                    StationName = "Radio Profeto";
                    Source = "http://151.80.24.114:80/streammq.mp3";
                    mediaElement.Source = new Uri(Source);
                    break;
                case "Radio Nadzieja":
                    StationName = "Radio Nadzieja";
                    Source = "http://s5.radiohost.pl:8600/";
                    mediaElement.Source = new Uri(Source);
                    break;
            }
        }

        private void Pivot_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ChangeThemeLogo();
        }

        /*private void PhonePageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MaryjaPhonePage));
        }*/
    }
}