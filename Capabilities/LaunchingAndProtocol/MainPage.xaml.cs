using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LaunchingAndProtocol
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(@"foodtruck:burgers");//(@"http://www.google.com");


            var handlers = await Launcher.FindUriSchemeHandlersAsync("foodtruck");
            foreach (var handle in handlers)
            {
                System.Diagnostics.Debug.WriteLine(handle.PackageFamilyName);
            }

            var lauchOptions = new LauncherOptions
            {
                TargetApplicationPackageFamilyName = "27a9d0fb-1063-4d14-907b-fcaa93949825_5gyrq6psz227t"
            };

            var set = new ValueSet();
            set.Add("item", "cheese");
            //var success = await Launcher.LaunchUriAsync(uri, lauchOptions,set);

            var launchResult = await Launcher.LaunchUriForResultsAsync(uri, lauchOptions, set);
            if(launchResult.Status == LaunchUriStatus.Success)
            {
                object resultItem = null;
                if(launchResult.Result?.TryGetValue("price",out resultItem) ?? false)
                {
                    var dialog = new MessageDialog((string)resultItem);
                    await dialog.ShowAsync();
                }
            }

        }
    }
}
