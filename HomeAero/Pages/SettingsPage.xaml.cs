using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace HomeAero.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        ApplicationDataContainer appSettings;

        public SettingsPage()
        {
            this.InitializeComponent();
            appSettings = (App.Current as App).settings;

            var deviceName = appSettings.Values["DeviceName"] != null
                ? appSettings.Values["DeviceName"].ToString()
                : String.Empty;

            var userEmail = appSettings.Values["UserEmail"] != null
                ? appSettings.Values["UserEmail"].ToString()
                : String.Empty;

            DeviceNameText.Text = deviceName;
            EmailText.Text = userEmail;
        }

        private void HandleSave(object sender, RoutedEventArgs e)
        {
            // TODO: Display saving notification
            var deviceName = DeviceNameText.Text;
            var userEmail = EmailText.Text;

            if(deviceName == String.Empty || userEmail == String.Empty)
            {
                // TODO: Display error message
                return;
            }

            appSettings.Values["DeviceName"] = deviceName;
            appSettings.Values["UserEmail"] = userEmail;
            appSettings.Values["CanNavigate"] = true;
        }
    }
}
