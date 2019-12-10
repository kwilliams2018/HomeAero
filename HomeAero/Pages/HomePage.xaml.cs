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
        ApplicationDataContainer appSettings;
        DispatcherTimer timer; // Used for updating the time

        public HomePage()
        {
            this.InitializeComponent();
            InitializeTimer();

            appSettings = (App.Current as App).settings;

            var deviceName = appSettings.Values["DeviceName"] != null
                ? appSettings.Values["DeviceName"].ToString()
                : String.Empty;

            DeviceName.Text = $"Device Name: {deviceName}";
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += TimerTick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void TimerTick(object sender, object e)
        {
            var formattedTime = DateTime.Now.ToString("MMM d, h:mm tt");
            DateBlock.Text = formattedTime;
        }
    }
}
