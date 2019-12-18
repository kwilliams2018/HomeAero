using HomeAero.Config;
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
    public sealed partial class HomePage : Page
    {
        ApplicationDataContainer Settings;
        HomeAeroConfiguration HomeAero;
        DispatcherTimer Timer; // Used for updating the time
        DateTimeOffset LastMistTime;
        private DateTimeOffset _lastSensorUpdate;

        public HomePage()
        {
            this.InitializeComponent();
            InitializeTimer();

            Settings = (App.Current as App).Settings;
            HomeAero = (App.Current as App).HomeAero;
            LastMistTime = (App.Current as App).MistingStartTime;

            var deviceName = Settings.Values["DeviceName"] != null
                ? Settings.Values["DeviceName"].ToString()
                : String.Empty;

            DeviceName.Text = $"Device Name: {deviceName}";

            UpdateSensorText();
        }

        private void InitializeTimer()
        {
            Timer = new DispatcherTimer();
            Timer.Tick += TimerTick;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }

        private void TimerTick(object sender, object e)
        {
            var now = DateTimeOffset.Now;
            var formattedTime = now.ToString("MMM d, h:mm tt");
            DateBlock.Text = formattedTime;

            if(now - _lastSensorUpdate > TimeSpan.FromMinutes(1))
            {
                _lastSensorUpdate = now;
                UpdateSensorText();
            }
        }

        private void UpdateSensorText()
        {
            var sensorReadings = HomeAero.GetSensorData();
            var formattedPreviousMist = LastMistTime.ToString("h:mm tt");

            double.TryParse(Settings.Values["MistInterval"].ToString(), out var mistInterval);
            double.TryParse(Settings.Values["MistDuration"].ToString(), out var mistDuration);
            var nextMist = mistInterval + mistDuration;

            var formatttedNextMist = LastMistTime.Add(TimeSpan.FromSeconds(nextMist)).ToString("h:mm tt");

            RootTemp.Text = $"Root Temperature: {sensorReadings.RootTemperature}F";
            RootHumid.Text = $"Root Humidity: {sensorReadings.RootHumidity}%";
            PlantTemp.Text = $"Plant Temperature: {sensorReadings.PlantTemperature}F";
            PlantHumid.Text = $"Plant Humidity: {sensorReadings.PlantHumidity}%";
            LastMisting.Text = $"Last Misting: {formattedPreviousMist}";
            NextMisting.Text = $"Next Misting: {formatttedNextMist}";
        }
    }
}
