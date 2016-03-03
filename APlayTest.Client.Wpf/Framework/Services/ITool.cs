using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APlayTest.Client.Wpf.Framework.Services
{

    public enum PaneLocation
    {
        Left,
        Right,
        Bottom
    }


    public interface ITool : ILayoutItem
    {
        PaneLocation PreferredLocation { get; }
        double PreferredWidth { get; }
        double PreferredHeight { get; }

        bool IsVisible { get; set; }
    }
}
