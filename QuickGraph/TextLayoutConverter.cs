using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ORM.QuickGraph
{
    public class RenderRotateConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var sourceLeft = values.Double(0) + values.Double(2) / 2;
            var sourceTop = values.Double(1) + values.Double(3) / 2;
            var targetLeft = values.Double(4) + values.Double(6) / 2;
            var targetTop = values.Double(5) + values.Double(7) / 2;

            var dX = sourceLeft - targetLeft;
            var dY = sourceTop - targetTop;

            var direction = (180.0 / Math.PI) * Math.Atan2(dY, dX);
            if (direction > 90)
            {
                //  direction -= 180;
            }
            else if (direction < -90)
            {
                //      direction += 180;
            }

            var tranform = new TransformGroup();
            var rotateTransform = new RotateTransform(direction);
            tranform.Children.Add(rotateTransform);

            return tranform;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TextRotateConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var sourceLeft = values.Double(0) + values.Double(2) / 2;
            var sourceTop = values.Double(1) + values.Double(3) / 2;
            var targetLeft = values.Double(4) + values.Double(6) / 2;
            var targetTop = values.Double(5) + values.Double(7) / 2;

            var dX = sourceLeft - targetLeft;
            var dY = sourceTop - targetTop;

            var direction = (180.0 / Math.PI) * Math.Atan2(dY, dX);

            var tranform = new TransformGroup();

            if (direction > 90)
            {
                var rotateTransform = new RotateTransform(180);
                tranform.Children.Add(rotateTransform);
            }
            else if (direction < -90)
            {
                var rotateTransform = new RotateTransform(180);
                tranform.Children.Add(rotateTransform);
            }


            return tranform;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class LengthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var sourceLeft = values.Double(0) + values.Double(2) / 2;
            var sourceTop = values.Double(1) + values.Double(3) / 2;
            var targetLeft = values.Double(4) + values.Double(6) / 2;
            var targetTop = values.Double(5) + values.Double(7) / 2;

            var dX = sourceLeft - targetLeft;
            var dY = sourceTop - targetTop;

            return Math.Sqrt(dY * dY + dX * dX);

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class MarginConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var sourceLeft = values.Double(0) + values.Double(2) / 2;
            var sourceTop = values.Double(1) + values.Double(3) / 2;
            var targetLeft = values.Double(4) + values.Double(6) / 2;
            var targetTop = values.Double(5) + values.Double(7) / 2;

            return new Thickness(targetLeft, targetTop - 5, 0, 0);

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}