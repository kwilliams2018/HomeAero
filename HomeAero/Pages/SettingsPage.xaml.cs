﻿using System;
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
            appSettings = (App.Current as App).Settings;

            var deviceName = appSettings.Values["DeviceName"] != null
                ? appSettings.Values["DeviceName"].ToString()
                : String.Empty;

            var userEmail = appSettings.Values["UserEmail"] != null
                ? appSettings.Values["UserEmail"].ToString()
                : String.Empty;

            var mistInterval = appSettings.Values["MistInterval"] != null
                ? appSettings.Values["MistInterval"].ToString()
                : String.Empty;

            var mistDuration = appSettings.Values["MistDuration"] != null
                ? appSettings.Values["MistDuration"].ToString()
                : String.Empty;

            var sensorInterval = appSettings.Values["SensorInterval"] != null
                ? appSettings.Values["SensorInterval"].ToString()
                : String.Empty;

            DeviceNameText.Text = deviceName;
            EmailText.Text = userEmail;
            MistIntervalText.Text = mistInterval;
            MistDurationText.Text = mistDuration;
            SensorIntervalText.Text = sensorInterval;
        }

        private void HandleSave(object sender, RoutedEventArgs e)
        {
            // TODO: Display saving notification
            var deviceName = DeviceNameText.Text;
            var userEmail = EmailText.Text;
            double.TryParse(MistIntervalText.Text, out var mistInterval);
            double.TryParse(MistDurationText.Text, out var mistDuration);
            double.TryParse(SensorIntervalText.Text, out var sensorInterval);

            if (
                deviceName == String.Empty || 
                userEmail == String.Empty ||
                mistInterval == 0 ||
                mistDuration == 0 ||
                sensorInterval == 0
            )
            {
                // TODO: Display error message
                return;
            }

            appSettings.Values["DeviceName"] = deviceName;
            appSettings.Values["UserEmail"] = userEmail;
            appSettings.Values["MistInterval"] = mistInterval;
            appSettings.Values["MistDuration"] = mistDuration;
            appSettings.Values["SensorInterval"] = sensorInterval;
            appSettings.Values["CanNavigate"] = true;
        }
    }
}
