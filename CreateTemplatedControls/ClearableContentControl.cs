using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace CreateTemplatedControls
{
    public sealed class ClearableContentControl : ContentControl
    {
        public ClearableContentControl()
        {
            this.DefaultStyleKey = typeof(ClearableContentControl);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ClearButton = this.GetTemplateChild("ClearAbleBtn") as Button;
            ClearButton.Click += ClearButton_Click;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = null;
        }

        Button ClearButton;



    }
}
