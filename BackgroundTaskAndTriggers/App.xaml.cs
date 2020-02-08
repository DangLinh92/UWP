﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace BackgroundTaskAndTriggers
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
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

                    RegisterBackgroundTask();
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        const string NetWorkChangedTaskName = "NetworkChangedTask";
        private async void RegisterBackgroundTask()
        {
            IBackgroundTaskRegistration TaskReg = null;
            foreach (var reg in BackgroundTaskRegistration.AllTasks)
            {
                if(reg.Value.Name == NetWorkChangedTaskName)
                {
                    TaskReg = reg.Value;
                    break;
                }
            }

            if(TaskReg == null)
            {
                await BackgroundExecutionManager.RequestAccessAsync();
                var Builder = new BackgroundTaskBuilder();
                Builder.Name = NetWorkChangedTaskName;
                //Builder.TaskEntryPoint = "RuntimeComponent1.TaskRunner";
                Builder.AddCondition(new SystemCondition(SystemConditionType.UserPresent));
                Builder.SetTrigger(new SystemTrigger(SystemTriggerType.NetworkStateChange, false));
                TaskReg = Builder.Register();
                TaskReg.Completed += TaskReg_Completed;
            }
        }

        private void TaskReg_Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            var setting = Windows.Storage.ApplicationData.Current.LocalSettings;
            var key = sender.TaskId.ToString();
            var message = setting.Values[key].ToString();
            System.Diagnostics.Debug.WriteLine(message);
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

        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            base.OnBackgroundActivated(args);
            System.Diagnostics.Debug.WriteLine("Network state changed");
            ExploreNetworkInfo();
        }

        private void ExploreNetworkInfo()
        {
            var Profiles = NetworkInformation.GetConnectionProfiles();
            bool FoundInternet = false;
            foreach (var P in Profiles)
            {
                var Level = P.GetNetworkConnectivityLevel();
                var wiring = (P.IsWlanConnectionProfile || P.IsWwanConnectionProfile) ? "Wireless" : "Adapter";
                if (Level == NetworkConnectivityLevel.InternetAccess)
                {
                    FoundInternet = true;
                    System.Diagnostics.Debug.WriteLine($"{P.ProfileName} {wiring}");
                }
            }

            if (FoundInternet == false)
            {
                System.Diagnostics.Debug.WriteLine("No internet access profile");
            }
        }
    }
}
