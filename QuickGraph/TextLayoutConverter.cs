using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ORM.QuickGraph
{
    public class TextLayoutConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var sourceLeft = values.Double(0) + values.Double(2) / 2;
            var sourceTop = values.Double(1) + values.Double(3) / 2;
            var targetLeft = values.Double(4) + values.Double(6) / 2;
            var targetTop = values.Double(5) + values.Double(7) / 2;

          //  var textWidth = values.Double(8);
          //  var textHeight = values.Double(9);

            var middlepoint = new Point((sourceLeft + targetLeft) /2, (sourceTop + targetTop)/2);
           


            var dX = sourceLeft - targetLeft;
            var dY = sourceTop - targetTop;

            var direction = (180.0 / Math.PI) * Math.Atan2(dY, dX);
            if (direction > 90)
            {
                direction -= 180;
            }
            else if (direction < -90)
            {
                direction += 180;
            }

              // direction = 45;

            //    var tranform = new TransformGroup();


            //    var rotateTransform = new RotateTransform(direction, 0,0);
            //   tranform.Children.Add(rotateTransform);

            // var translateTransform = new TranslateTransform(middlepoint.X, middlepoint.Y);
            //   tranform.Children.Add(translateTransform);
            //   return tranform;  

            //   newM.Rotate(direction);


            var newM = new Matrix();
            newM.Rotate(direction);
            newM.Translate(middlepoint.X, middlepoint.Y);

            return new MatrixTransform(newM);


        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}