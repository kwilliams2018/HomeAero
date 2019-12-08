using HomeAero.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HomeAero
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // List of ValueTuple holding the Navigation Tag and the relative Navigation Page
        private readonly List<(string Tag, Type Page)> _pages;

        public MainPage()
        {
            this.InitializeComponent();

            // Set the pages
            _pages = new List<(string Tag, Type Page)>
            {
                ("Home", typeof(HomePage)),
                ("Account", typeof(AccountPage)),
                ("Settings", typeof(SettingsPage)),
            };

            // TODO: Load device information
            // TODO: Check if initial setup was completed

            // Set the home page
            var initialPage = _pages.First(p => p.Tag.Equals("Home"));
            ContentFrame.Navigate(initialPage.Page, null);
        }

        private void SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var transitionInfo = args.RecommendedNavigationTransitionInfo;

            if (args.IsSettingsSelected == true)
                Navigate("Settings", transitionInfo);
            else
            {
                var navigationTag = args.SelectedItemContainer.Tag.ToString();
                Navigate(navigationTag, transitionInfo);
            }
        }

        private void Navigate(string navigationTag, NavigationTransitionInfo transitionInfo)
        {
            //https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/navigationview
            Type _page = null;
            if (navigationTag == "settings")
                _page = typeof(SettingsPage);
            else
            {
                var item = _pages.FirstOrDefault(p => p.Tag.Equals(navigationTag));
                _page = item.Page;
            }

            // Get the page type before navigation so you can prevent duplicate
            // entries in the backstack.
            var preNavPageType = ContentFrame.CurrentSourcePageType;

            // Only navigate if the selected page isn't currently loaded.
            if (!(_page is null) && !Type.Equals(preNavPageType, _page))
            {
                ContentFrame.Navigate(_page, null, transitionInfo);
            }
        }
    }
}
