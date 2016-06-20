using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APlayTest.Client.Extensions
{
    public static class AplayPointExtensions
    {
        public static bool SameAs(this AplayPoint aplayPoint, AplayPoint other)
        {
            return Math.Abs(aplayPoint.X - other.X) <= double.Epsilon &&
                   Math.Abs(aplayPoint.Y - other.Y) <= double.Epsilon;
        }
    }
}
