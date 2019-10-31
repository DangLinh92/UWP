using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace CustomPanel
{
    class UniformWidthPanel : Panel
    {
        public UniformWidthPanel()
        {

        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size newSize = new Size();
            if (double.IsInfinity(availableSize.Width) == false)
            {
                newSize.Width = availableSize.Width;
            }

            if(double.IsInfinity(availableSize.Height) == false)
            {
                newSize.Height = availableSize.Height;
            }

            double NumberChild = this.Children.Count;
            if(NumberChild > 0)
            {
                Size childSize = availableSize;
                childSize.Width /= NumberChild;

                for (int i = 0; i < NumberChild; i++)
                {
                    var child = this.Children[i];
                    child.Measure(childSize);
                    if(child.DesiredSize.Height > newSize.Height)
                    {
                        newSize.Height = child.DesiredSize.Height;
                    }
                }
            }
            return newSize;
        }
    }

    
}
