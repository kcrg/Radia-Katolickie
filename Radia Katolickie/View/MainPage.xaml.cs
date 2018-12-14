using Radia_Katolickie.View.MessageDialoges;
using System;
using System.Net.NetworkInformation;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Email;
using Windows.ApplicationModel.Store;
using Windows.Media;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
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

        private string RadioPageUri = "http://www.radiomaryja.pl/";

        private BitmapImage MaryjaLogoLight,
                            ViaLogoLight,
                            NiepokalanowLogoLight,
                            ProfetoLogoLight,
                            NadziejaLogoLight,
                            GlosLogoLight,
                            FaraLogoLight,
                            ZamoscLogoLight,

                            MaryjaLogoDark,
                            ViaLogoDark,
                            NiepokalanowLogoDark,
                            ProfetoLogoDark,
                            NadziejaLogoDark,
                            GlosLogoDark,
                            FaraLogoDark,
                            ZamoscLogoDark;

        public MainPage()
        {
            InitializeComponent();
            LoadImages();
            ChangeThemeLogo();
            CheckInternetConnection();

            systemControls = SystemMediaTransportControls.GetForCurrentView();
            systemControls.ButtonPressed += SystemControls_ButtonPressed;
            systemControls.IsPlayEnabled = true;
            systemControls.IsPauseEnabled = true;
            systemControls.IsStopEnabled = true;

            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;

            // Check if it is supported
            if (ApplicationView.GetForCurrentView().IsViewModeSupported(ApplicationViewMode.CompactOverlay))
            {
                MiniButton.Visibility = Visibility.Visible;
                MaxButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                MiniButton.Visibility = Visibility.Collapsed;
                MaxButton.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadImages()
        {
            MaryjaLogoLight = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Maryja/Maryja-logoLight.png"));
            ViaLogoLight = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Via/Via-logoLight.png"));
            NiepokalanowLogoLight = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Niepokalanow/Niepokalanow-logoLight.png"));
            ProfetoLogoLight = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Profeto/Profeto-logoLight.png"));
            NadziejaLogoLight = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Nadzieja/Nadzieja-logoLight.png"));
            GlosLogoLight = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Glos/Glos-logoLight.png"));
            FaraLogoLight = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Fara/Fara-logoLight.png"));
            ZamoscLogoLight = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Zamosc/Zamosc-logoLight.png"));

            MaryjaLogoDark = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Maryja/Maryja-logoDark.png"));
            ViaLogoDark = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Via/Via-logoDark.png"));
            NiepokalanowLogoDark = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Niepokalanow/Niepokalanow-logoDark.png"));
            ProfetoLogoDark = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Profeto/Profeto-logoDark.png"));
            NadziejaLogoDark = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Nadzieja/Nadzieja-logoDark.png"));
            GlosLogoDark = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Glos/Glos-logoDark.png"));
            FaraLogoDark = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Fara/Fara-logoDark.png"));
            ZamoscLogoDark = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Zamosc/Zamosc-logoDark.png"));
        }

        private void ChangeThemeLogo()
        {
            if (Application.Current.RequestedTheme == ApplicationTheme.Dark)
            {
                MaryjaLogo.Source = MaryjaLogoLight;
                ViaLogo.Source = ViaLogoLight;
                NiepokalanowLogo.Source = NiepokalanowLogoLight;
                ProfetoLogo.Source = ProfetoLogoLight;
                NadziejaLogo.Source = NadziejaLogoLight;
                GlosLogo.Source = GlosLogoLight;
                FaraLogo.Source = FaraLogoLight;
                ZamoscLogo.Source = ZamoscLogoLight;
            }

            else
            {
                MaryjaLogo.Source = MaryjaLogoDark;
                ViaLogo.Source = ViaLogoDark;
                NiepokalanowLogo.Source = NiepokalanowLogoDark;
                ProfetoLogo.Source = ProfetoLogoDark;
                NadziejaLogo.Source = NadziejaLogoDark;
                GlosLogo.Source = GlosLogoDark;
                FaraLogo.Source = FaraLogoDark;
                ZamoscLogo.Source = ZamoscLogoDark;
            }
        }

        private async void CheckInternetConnection()
        {
            bool isInternetConnected = NetworkInterface.GetIsNetworkAvailable();

            if (isInternetConnected == false)
            {
                ConnectionDialog Dialog = new ConnectionDialog();
                await Dialog.ShowAsync();
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

        private async void LauncherUri(string url)
        {
            var uriBing = new Uri(@url);
            await Launcher.LaunchUriAsync(uriBing);
        }

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            args.Request.Data.SetWebLink(new Uri("https://www.microsoft.com/store/apps/9NTP1FNVNHMW"));
            args.Request.Data.Properties.Title = "Radia Katolickie";
            args.Request.Data.Properties.Description = "Aplikacja 'Radia Katolickie' umożliwia odtwarzanie stacji katolickich przez internet.";
            args.Request.Data.SetText("Aplikacja 'Radia Katolickie' umożliwia odtwarzanie stacji katolickich przez internet. Już teraz do pobrania z Microsoft Store na wszystkie urządzenia z Windows 10!");
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

        /////////////////////////// APPBAR BUTTONS ///////////////////////////////////////   

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            PlayMedia();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            PauseMedia();
        }

        private async void MiniButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModePreferences compactOptions = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
            compactOptions.CustomSize = new Windows.Foundation.Size(400, 160);
            bool modeSwitched = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, compactOptions);

            StatusTextBlock.Visibility = Visibility.Collapsed;
            BottomCommandBar.DefaultLabelPosition = CommandBarDefaultLabelPosition.Bottom;
            MiniButton.Visibility = Visibility.Collapsed;
            MaxButton.Visibility = Visibility.Visible;

            BottomCommandBar.OverflowButtonVisibility = CommandBarOverflowButtonVisibility.Collapsed;
        }

        private void RadioPageButton_Click(object sender, RoutedEventArgs e)
        {
            LauncherUri(RadioPageUri);
        }

        private async void MaxButton_Click(object sender, RoutedEventArgs e)
        {
            bool modeSwitched = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default);

            StatusTextBlock.Visibility = Visibility.Visible;
            BottomCommandBar.DefaultLabelPosition = CommandBarDefaultLabelPosition.Right;
            MiniButton.Visibility = Visibility.Visible;
            MaxButton.Visibility = Visibility.Collapsed;

            BottomCommandBar.OverflowButtonVisibility = CommandBarOverflowButtonVisibility.Visible;
        }

        private void RateButton_Click(object sender, RoutedEventArgs e)
        {
            LauncherUri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId);
        }

        private void FacebookButton_Click(object sender, RoutedEventArgs e)
        {
            LauncherUri("https://www.facebook.com/VersatileSoftware");
        }

        private async void EmailButton_Click(object sender, RoutedEventArgs e)
        {
            EmailMessage mail = new EmailMessage
            {
                Subject = "[Radia Katolickie] Kontakt z developerem"
            };
            mail.To.Add(new EmailRecipient("mohairapp@hotmail.com", "Versatile Software"));
            await EmailManager.ShowComposeNewEmailAsync(mail);
        }

        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }

        private void MoreButton_Click(object sender, RoutedEventArgs e)
        {
            LauncherUri(string.Format(@"ms-windows-store:publisher?name={0}", "Versatile Software"));
        }

        private void DonationButton_Click(object sender, RoutedEventArgs e)
        {
            LauncherUri("https://tinyurl.com/DonateMohairApps");
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
                    RadioPageUri = "http://www.radiomaryja.pl/";
                    break;
                case "Radio Via":
                    StationName = "Radio Via";
                    Source = "http://62.133.128.18:8040/";
                    mediaElement.Source = new Uri(Source);
                    RadioPageUri = "http://radiovia.com.pl/";
                    break;
                case "Radio Niepokalanów":
                    StationName = "Radio Niepokalanów";
                    Source = "http://88.199.169.10:7600/rn.mp3";
                    mediaElement.Source = new Uri(Source);
                    RadioPageUri = "http://radioniepokalanow.pl/";
                    break;
                case "Radio Profeto":
                    StationName = "Radio Profeto";
                    Source = "http://151.80.24.114:80/streammq.mp3";
                    mediaElement.Source = new Uri(Source);
                    RadioPageUri = "http://radioprofeto.pl/";
                    break;
                case "Radio Nadzieja":
                    StationName = "Radio Nadzieja";
                    Source = "http://s5.radiohost.pl:8600/";
                    mediaElement.Source = new Uri(Source);
                    RadioPageUri = "https://radionadzieja.pl/";
                    break;
                case "Radio Głos":
                    StationName = "Radio Głos";
                    Source = "http://87.204.92.180:8000/";
                    mediaElement.Source = new Uri(Source);
                    RadioPageUri = "http://www.radioglos.pl/";
                    break;
                case "Radio Fara":
                    StationName = "Radio Fara";
                    Source = "http://62.133.128.22:8000/";
                    mediaElement.Source = new Uri(Source);
                    RadioPageUri = "http://przemyska.pl/radiofara/";
                    break;
                case "Katolickie Radio Zamość":
                    StationName = "Katolickie Radio Zamość";
                    Source = "http://posluchaj.krz.pl/";
                    mediaElement.Source = new Uri(Source);
                    RadioPageUri = "http://www.radiozamosc.pl/";
                    break;
            }
        }

        private void Page_GotFocus(object sender, RoutedEventArgs e)
        {
            ChangeThemeLogo();
        }
    }
}