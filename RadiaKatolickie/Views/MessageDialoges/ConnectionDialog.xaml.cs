using System;
using System.Net.NetworkInformation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace RadiaKatolickie.Views.MessageDialoges
{
    public sealed partial class ConnectionDialog : ContentDialog
    {
        public ConnectionDialog()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Application.Current.Exit();
        }

        private async void CheckButton_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Hide();

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                await new ConnectionDialog().ShowAsync();
            }
            else
            {
                Hide();
            }
        }
    }
}