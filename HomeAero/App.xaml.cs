﻿using HomeAero.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
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

namespace HomeAero
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public ApplicationDataContainer Settings { get; set; }
        public HomeAeroConfiguration HomeAero { get; set; }
        // Used for running the HomeAero
        DispatcherTimer Timer;
        private bool _isMisting;
        private DateTimeOffset _currentTime;
        private DateTimeOffset _sensorTime;
        public DateTimeOffset MistingStartTime;
        

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            HomeAero = new HomeAeroConfiguration();
            MistingStartTime = DateTimeOffset.Now;
            _isMisting = false;

            InitializeTimer();
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
            try
            {
                // Get values from settings
                double.TryParse(Settings.Values["MistInterval"].ToString(), out var mistInterval);
                double.TryParse(Settings.Values["MistDuration"].ToString(), out var mistDuration);
                double.TryParse(Settings.Values["SensorInterval"].ToString(), out var sensorInterval);

                _currentTime = DateTimeOffset.Now;

                if (mistInterval > 0 && mistDuration > 0)
                {
                    var durationSinceStart = _currentTime - MistingStartTime;
                    var nextStartTime = MistingStartTime.AddSeconds(mistInterval + mistDuration);

                    if (_isMisting && durationSinceStart.TotalSeconds > mistDuration)
                    {
                        HomeAero.EndMisting();
                        _isMisting = false;
                    }
                    else if (!_isMisting && _currentTime > nextStartTime)
                    {
                        MistingStartTime = DateTimeOffset.Now;
                        _isMisting = true;
                        HomeAero.BeginMisting();
                    }
                }

                if(sensorInterval > 0)
                {
                    var durationSinceSensorReading = _currentTime - _sensorTime;
                    if (durationSinceSensorReading.TotalSeconds > sensorInterval)
                    {
                        _sensorTime = _currentTime;
                        HomeAero.TakeSensorReading();
                    }
                }
            }
            catch(Exception ex)
            {
                // TODO: Add error handling - settings not valid
            }
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            // Get the settings and make it available for all pages
            Settings = ApplicationData.Current.LocalSettings;

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
