using RadiaKatolickie.Views.MessageDialoges;
using System;
using System.Net.NetworkInformation;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Email;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace RadiaKatolickie.Views
{
    public sealed partial class MainPage : Page
    {
        private readonly MediaPlayer mediaPlayer = new MediaPlayer();

        private readonly SystemMediaTransportControls systemControls;
        private Uri Source = new Uri("http://198.27.80.205:5946/stream");
        private string StationName = "Radio Maryja";

        private Uri RadioPageUri = new Uri("http://www.radiomaryja.pl/");

        private BitmapImage AppLogoLight,
                            MaryjaLogoLight,
                            ViaLogoLight,
                            NiepokalanowLogoLight,
                            ProfetoLogoLight,
                            NadziejaLogoLight,
                            GlosLogoLight,
                            FaraLogoLight,
                            ZamoscLogoLight,

                            AppLogoDark,
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

            CoreApplicationViewTitleBar CoreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            CoreTitleBar.ExtendViewIntoTitleBar = true;

            Window.Current.SetTitleBar(DragArea);

            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

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

            mediaPlayer.CurrentStateChanged += MediaPlayer_StateChanged;
        }

        private async void MediaPlayer_StateChanged(MediaPlayer sender, object args)
        {
            if (mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Opening)
            {
                LoadingBarVisible(" jest ładowane...");
            }

            else if (mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Buffering)
            {
                LoadingBarVisible(" jest buforowane...");
            }

            else if (mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.None)
            {
                LoadingBarCollapsed(" jest wyłączone.", false);
            }

            else if (mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Paused)
            {
                LoadingBarCollapsed(" jest zapauzowane...", true);
            }

            else if (mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Playing)
            {
                LoadingBarCollapsed(" jest odtwarzane...", true);

                await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                {
                    PlayButton.IsEnabled = false;
                    PauseButton.IsEnabled = true;
                });
            }

            else
            {
                LoadingBarCollapsed(" ma nieznany status.", false);
            }
        }

        private void LoadImages()
        {
            AppLogoLight = new BitmapImage(new Uri("ms-appx:///Assets/Images/Titlebar/AppLogoLight.png"));
            MaryjaLogoLight = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Maryja/Maryja-logoLight.png"));
            ViaLogoLight = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Via/Via-logoLight.png"));
            NiepokalanowLogoLight = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Niepokalanow/Niepokalanow-logoLight.png"));
            ProfetoLogoLight = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Profeto/Profeto-logoLight.png"));
            NadziejaLogoLight = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Nadzieja/Nadzieja-logoLight.png"));
            GlosLogoLight = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Glos/Glos-logoLight.png"));
            FaraLogoLight = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Fara/Fara-logoLight.png"));
            ZamoscLogoLight = new BitmapImage(new Uri("ms-appx:///Assets/Images/Radio Zamosc/Zamosc-logoLight.png"));

            AppLogoDark = new BitmapImage(new Uri("ms-appx:///Assets/Images/Titlebar/AppLogoDark.png"));
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
                AppLogoImage.Source = AppLogoLight;
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
                AppLogoImage.Source = AppLogoDark;
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
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                await new ConnectionDialog().ShowAsync();
            }
        }

        private async void LoadingBarVisible(string status)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                LoadingBar.Visibility = Visibility.Visible;

                StatusTextBlock.Text = StationName + status;
                PlayButton.IsEnabled = false;
                PauseButton.IsEnabled = false;
            });
        }

        private async void LoadingBarCollapsed(string status, bool IsEnable)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                LoadingBar.Visibility = Visibility.Collapsed;

                StatusTextBlock.Text = StationName + status;

                if (IsEnable)
                {
                    PlayButton.IsEnabled = true;
                    PauseButton.IsEnabled = false;
                }
                else
                {
                    PlayButton.IsEnabled = false;
                    PauseButton.IsEnabled = false;
                }
            });
        }

        private async void LauncherUri(Uri uri)
        {
            await Launcher.LaunchUriAsync(uri);
        }

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            args.Request.Data.SetWebLink(new Uri("https://www.microsoft.com/store/apps/9NTP1FNVNHMW"));
            args.Request.Data.Properties.Title = "Radia Katolickie";
            args.Request.Data.Properties.Description = "Aplikacja 'Radia Katolickie' umożliwia odtwarzanie stacji katolickich przez internet.";
            args.Request.Data.SetText("Aplikacja 'Radia Katolickie' umożliwia odtwarzanie stacji katolickich przez internet. Już teraz do pobrania z Microsoft Store na wszystkie urządzenia z Windows 10!");
        }

        private void EnterDown(object sender, KeyRoutedEventArgs e)
        {
            if (mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Playing)
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
            compactOptions.CustomSize = new Size(300, 250);
            _ = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, compactOptions);

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
            _ = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default);

            StatusTextBlock.Visibility = Visibility.Visible;
            BottomCommandBar.DefaultLabelPosition = CommandBarDefaultLabelPosition.Right;
            MiniButton.Visibility = Visibility.Visible;
            MaxButton.Visibility = Visibility.Collapsed;

            BottomCommandBar.OverflowButtonVisibility = CommandBarOverflowButtonVisibility.Visible;
        }

        private void RateButton_Click(object sender, RoutedEventArgs e)
        {
            LauncherUri(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
        }

        private void FacebookButton_Click(object sender, RoutedEventArgs e)
        {
            LauncherUri(new Uri("https://www.facebook.com/VersatileSoftware"));
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
            LauncherUri(new Uri(string.Format(@"ms-windows-store:publisher?name={0}", "Versatile Software")));
        }

        private void DonationButton_Click(object sender, RoutedEventArgs e)
        {
            LauncherUri(new Uri("https://tinyurl.com/DonateMohairApps"));
        }

        private void SystemControls_ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            switch (args.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                    PlayMedia();
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    PauseMedia();
                    break;
                case SystemMediaTransportControlsButton.Stop:
                    StopMedia();
                    break;
                default:
                    break;
            }
        }

        private async void StopMedia()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                mediaPlayer.Pause();
                //mediaElement.Source = new Uri(Source);
            });
        }

        private async void PlayMedia()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                mediaPlayer.Play();
            });
        }

        private async void PauseMedia()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                mediaPlayer.Pause();
            });
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PivotItem pivot = (PivotItem)(sender as Pivot).SelectedItem;
            switch (pivot.Header.ToString())
            {
                case "Radio Maryja":
                    StationName = "Radio Maryja";
                    Source = new Uri("http://198.27.80.205:5946/stream");
                    mediaPlayer.Source = MediaSource.CreateFromUri(Source);
                    RadioPageUri = new Uri("http://www.radiomaryja.pl/");
                    break;
                case "Radio Via":
                    StationName = "Radio Via";
                    Source = new Uri("http://62.133.128.18:8040/");
                    mediaPlayer.Source = MediaSource.CreateFromUri(Source);
                    RadioPageUri = new Uri("http://radiovia.com.pl/");
                    break;
                case "Radio Niepokalanów":
                    StationName = "Radio Niepokalanów";
                    Source = new Uri("http://88.199.169.10:7600/rn.mp3");
                    mediaPlayer.Source = MediaSource.CreateFromUri(Source);
                    RadioPageUri = new Uri("http://radioniepokalanow.pl/");
                    break;
                case "Radio Profeto":
                    StationName = "Radio Profeto";
                    Source = new Uri("http://151.80.24.114:80/streammq.mp3");
                    mediaPlayer.Source = MediaSource.CreateFromUri(Source);
                    RadioPageUri = new Uri("http://radioprofeto.pl/");
                    break;
                case "Radio Nadzieja":
                    StationName = "Radio Nadzieja";
                    Source = new Uri("http://s5.radiohost.pl:8600/");
                    mediaPlayer.Source = MediaSource.CreateFromUri(Source);
                    RadioPageUri = new Uri("https://radionadzieja.pl/");
                    break;
                case "Radio Głos":
                    StationName = "Radio Głos";
                    Source = new Uri("http://87.204.92.180:8000/");
                    mediaPlayer.Source = MediaSource.CreateFromUri(Source);
                    RadioPageUri = new Uri("http://www.radioglos.pl/");
                    break;
                case "Radio Fara":
                    StationName = "Radio Fara";
                    Source = new Uri("http://62.133.128.22:8000/");
                    mediaPlayer.Source = MediaSource.CreateFromUri(Source);
                    RadioPageUri = new Uri("http://przemyska.pl/radiofara/");
                    break;
                case "Katolickie Radio Zamość":
                    StationName = "Katolickie Radio Zamość";
                    Source = new Uri("http://posluchaj.krz.pl/");
                    mediaPlayer.Source = MediaSource.CreateFromUri(Source);
                    RadioPageUri = new Uri("http://www.radiozamosc.pl/");
                    break;
            }
        }

        private void Page_GotFocus(object sender, RoutedEventArgs e)
        {
            ChangeThemeLogo();
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            switch (UIViewSettings.GetForCurrentView().UserInteractionMode)
            {
                case UserInteractionMode.Mouse:
                    VisualStateManager.GoToState(this, "MouseLayout", true);
                    DragArea.Visibility = Visibility.Visible;
                    Pivot.Title = string.Empty;
                    break;

                case UserInteractionMode.Touch:
                default:
                    VisualStateManager.GoToState(this, "TouchLayout", true);
                    DragArea.Visibility = Visibility.Collapsed;
                    Pivot.Title = "Radia Katolickie";
                    break;
            }
        }
    }
}