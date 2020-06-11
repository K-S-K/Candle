using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Shapes;
using System.Collections.Generic;
using CandleApp.Common;

namespace CandleApp.Ctrls
{
    public class Axistor
    {
        #region -> Data
        private readonly double PixelsPerDip;
        #endregion


        #region -> Properties
        public double XMin { get; set; }
        public double XMax { get; set; }
        public double YMin { get; set; }
        public double YMax { get; set; }

        public double XAmp => XMax - XMin;
        public double YAmp => YMax - YMin;

        public double OffUp { get; set; }
        public double OffDn { get; set; }
        public double OffRt { get; set; }
        public double OffLt { get; set; }

        public Typeface FontFace { get; set; }
        #endregion


        #region -> Methods
        public void YReassign(IEnumerable<double> values)
        {
            bool first = true;

            foreach (double val in values)
            {
                if (first)
                {
                    first = false;

                    YMin = val;
                    YMax = val;

                    continue;
                }

                if (YMin > val) { YMin = val; }
                if (YMax < val) { YMax = val; }
            }
        }

        public Point NormalizedPoint(double xIn, double yIn, double width, double height, double offX = 0, double offY = 0)
        {
            double w = width - OffLt - OffRt;
            double h = height - OffUp - OffDn;

            double xOut = OffLt + (xIn - XMin) * w / XAmp + offX;
            double yOut = h + OffUp - (yIn - YMin) * h / YAmp - offY;

            Point ptOut = new Point(xOut, yOut);

            return ptOut;
        }

        public Point NormalizePoint(Point ptIn, double width, double height, double offX = 0, double offY = 0)
        {
            return NormalizedPoint(ptIn.X, ptIn.Y, width, height, offX, offY);
        }

        public void Reset()
        {
            XMin = 0;
            XMax = 1;
            YMin = 0;
            YMax = 1;
        }

        public Path ScalePath(IEnumerable<int> values, double width, double height)
        {
            int offScaleLine = 1;
            int offScalePoint = 5;
            int offScaleLabel = 8;
            double textHeight = 0;  // Высота метки шкалы в пихелях

            GeometryCollection geoData = new GeometryCollection();

            #region -> Scale Draw
            {
                LineGeometry sLine = new LineGeometry()
                {
                    StartPoint = NormalizedPoint(XMax, YMin, width, height, offScaleLine),
                    EndPoint = NormalizedPoint(XMax, YMax, width, height, offScaleLine)
                };

                geoData.Add(sLine);

                foreach (int val in values)
                {
                    LineGeometry sp = new LineGeometry()
                    {
                        StartPoint = NormalizedPoint(XMax, val, width, height, offScaleLine),
                        EndPoint = NormalizedPoint(XMax, val, width, height, offScalePoint)
                    };

                    FormattedText fText = new FormattedText(val.ToStringUI(),
                        CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                        FontFace, 8, Brushes.Black, PixelsPerDip);

                    textHeight = fText.Height;  // Запомним высоту метки шкалы в пихелях

                    Geometry sl =
                        fText.BuildGeometry(NormalizedPoint(
                        XMax, val, width, height, offScaleLabel, 5));

                    geoData.Add(sp);
                    geoData.Add(sl);
                }
            }
            #endregion

            #region -> Grid Draw
            if (YAmp > 1)
            {
                /// У нас шрифт высотой 8 пихелей.
                /// Пусть сетка шкалы будет не чаще 20 пихелей.
                /// 

                double h = height - OffUp - OffDn; // Высота картинки в пихелях

                double nStep = h / textHeight / 3; // Первое приближение количества шагов шкалы, позволяющее избежать слипания меток

                int szStep = nStep == 0 ? 0 : ((int)(YAmp / nStep));

                if (szStep > 1000) szStep = 1000;
                else if (szStep > 100) szStep = 100;
                else if (szStep > 50) szStep = 50;
                else if (szStep > 20) szStep = 20;
                else if (szStep > 10) szStep = 10;
                else if (szStep > 5) szStep = 5;
                else if (szStep > 2) szStep = 2;
                else szStep = 1; // Не нашлось ограничений - на каждый рубль цены

                // Получим нормализованные границы гридов
                int pMax = (int)((YMax / szStep) * szStep) - szStep;
                int pMin = (int)((YMin / szStep) * szStep) + szStep;

                for (int y = pMin; y <= pMax; y += szStep)
                {
                    LineGeometry sLine = new LineGeometry()
                    {
                        StartPoint = NormalizedPoint(XMin, y, width, height),
                        EndPoint = NormalizedPoint(XMax, y, width, height, offScalePoint),
                    };

                    FormattedText fText = new FormattedText(y.ToStringUI(),
                        CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                        FontFace, 8, Brushes.Black, PixelsPerDip);

                    textHeight = fText.Height;  // Запомним высоту метки шкалы в пихелях

                    Geometry sLabel =
                        fText.BuildGeometry(NormalizedPoint(
                        XMax, y, width, height, offScaleLabel, 5));

                    geoData.Add(sLine);
                    geoData.Add(sLabel);
                }
            }
            #endregion

            geoData.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

            Path p = new Path()
            {
                StrokeThickness = 0.25,
                UseLayoutRounding = false,
                SnapsToDevicePixels = true,

                Fill = new SolidColorBrush(Colors.White),
                Stroke = new SolidColorBrush(Colors.Black),
                Data = new GeometryGroup { Children = geoData }
            };

            return p;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"X:{XMin}-{XMax} ");
            sb.Append($"Y:{YMin}-{YMax} ");
            sb.Append($"");

            return sb.ToString();
        }
        #endregion


        #region -> Ctor
        public Axistor(double piPDip)
        {
            XMin = 0;
            XMax = 1;
            YMin = 0;
            YMax = 1;

            OffUp = 10;
            OffDn = 10;
            OffRt = 10;
            OffLt = 10;

            PixelsPerDip = piPDip;

            FontFace = new Typeface("Tahoma");
        }
        #endregion
    }
}
