using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ToastTemplate
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

        private void toastBtn_Click(object sender, RoutedEventArgs e)
        {
            ToastContent content = new ToastContent()
            {
                Launch = "app-defined-string",// pass parameter.
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText(){Text = "Sample"},
                            new AdaptiveText(){Text = "This is a simple toast notification example"}
                        }

                    },
                },

                Audio = new ToastAudio()
                {
                    Src = new Uri("ms-winsoundevent:Notification.Reminder")
                },
                Actions = new ToastActionsCustom()
                {
                    Buttons =
                    {
                        new ToastButton("See event","see"),
                        new ToastButton("Ignore","cancel")
                    },

                    Inputs =
                    {
                        new ToastSelectionBox("interest")
                        {
                            DefaultSelectionBoxItemId = "2",
                            Items =
                            {
                                new ToastSelectionBoxItem("1","Interesting"),
                                new ToastSelectionBoxItem("2","Not sure"),
                                new ToastSelectionBoxItem("3","Giving it a miss")
                            }
                        },

                        new ToastTextBox("comments")
                        {
                            PlaceholderContent = "Type other thoughts"
                        }
                    }
                },
                

                
            };

            var notification = new ToastNotification(content.GetXml());
            notification.ExpirationTime = DateTimeOffset.UtcNow.AddMinutes(10);
            ToastNotificationManager.CreateToastNotifier().Show(notification);
        }
    }
}
