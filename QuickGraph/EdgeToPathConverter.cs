using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ORM.RelationshipView
{
    public class EdgeToPathConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var sourceLeft = values.Double(0) + values.Double(2)  /2;
            values[0] =sourceLeft;
            var sourceTop = values.Double(1) + values.Double(3)  /2;
            values[1] =sourceTop;

            var targetLeft = values.Double(4) + values.Double(6) / 2;
            values[4] = targetLeft;
            var targetTop = values.Double(5) + values.Double(7) / 2;
            values[5] = targetTop;

            var segment = new LineSegment(new Point(sourceLeft, sourceTop), false );

            var pfc = new PathFigureCollection(1)
            {
                new PathFigure(new Point(targetLeft, targetTop), new List<PathSegment>(1) { segment}, true)
            };

            return pfc;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public static class Extensions
    {
        public static double Double(this object[] values, int i)
        {
            return (double)values[i];

        }
    } 
}
