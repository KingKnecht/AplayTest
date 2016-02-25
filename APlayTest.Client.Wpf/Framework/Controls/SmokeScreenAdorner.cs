using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using APlayTest.Client.Annotations;

namespace APlayTest.Client.Wpf.Framework.Controls
{
    public class SmokeScreenAdorner : Adorner
    {
        private readonly UIElement _control;

        public SmokeScreenAdorner([NotNull] UIElement adornedElement)
            : base(adornedElement)
        {
            _control = adornedElement;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {

            var _fill = new SolidColorBrush(Color.FromArgb(80, Colors.Coral.R, Colors.Coral.G, Colors.Coral.B));
            drawingContext.DrawRectangle(_fill, new Pen(Brushes.Black, 3.0), WindowRect());
            base.OnRender(drawingContext);
        }

        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            // Add a group that includes the whole window except the adorned control
            GeometryGroup grp = new GeometryGroup();
            grp.Children.Add(new RectangleGeometry(WindowRect()));
            grp.Children.Add(new RectangleGeometry(new Rect(layoutSlotSize)));

            return grp;
        }

        private Rect WindowRect()
        {
            if (_control == null)
            {
                throw new ArgumentException("cannot adorn a null control");
            }
            
            // Get a point of the offset of the window
            Window window = Application.Current.MainWindow;

            if (window == null)
            {
                throw new ArgumentException("can't get main window");
            }
                
            GeneralTransform transformToAncestor = _control.TransformToAncestor(window);
            if (transformToAncestor == null || transformToAncestor.Inverse == null)
            {
                throw new ArgumentException("no transform to window");
            }
                    
            var windowOffset = transformToAncestor.Inverse.Transform(new Point(0, 0));

            // Get a point of the lower-right corner of the window
            Point windowLowerRight = windowOffset;
            windowLowerRight.Offset(window.ActualWidth, window.ActualHeight);

            return new Rect(windowOffset, windowLowerRight);
        }
    }
}
