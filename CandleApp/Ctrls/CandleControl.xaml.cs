using CandleApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CandleApp.Ctrls
{
    /// <summary>
    /// Interaction logic for CandleControl.xaml
    /// </summary>
    public partial class CandleControl : UserControl
    {
        #region -> Data
        private Axistor Axis;
        private DateTime dayFirst;
        private DateTime dayLast;

        IEnumerable<CandleTableLine> Candles;

        private Polyline plHi;
        private Polyline plLo;
        #endregion

        #region -> Methods
        public void Clear()
        {
            // lock (syncPt)
            {
                Axis.Reset();

                chartCanvas.Children.Clear();
            }
        }

        public void SetData(IEnumerable<Data.CandleTableLine> items)
        {
            OnDataIsReadyToBeShown(items);
        }
        #endregion

        #region -> Drawing

        private void AddChart()
        {
            if (
                chartCanvas.ActualWidth == 0
                ||
                chartCanvas.ActualHeight == 0
                )
            {
                return;
            }

            double width = chartCanvas.Width;
            double height = chartCanvas.Height;

            plHi.Points.Clear();
            plLo.Points.Clear();

            chartCanvas.Children.Clear();

            #region -> Draw
            {
                foreach (CandleTableLine item in Candles)
                {
                    double priceCurLo = (double)item.GetData().Low;
                    double priceCurHi = (double)item.GetData().High;
                    double priceCurBeg = (double)item.GetData().Open;
                    double priceCurEnd = (double)item.GetData().Close;
                    int day = 10 * SolveDay(item);

                    #region -> Sticks
                    {
                        Point PLo = Axis.NormalizedPoint(day, priceCurLo, width, height);
                        Point PHi = Axis.NormalizedPoint(day, priceCurHi, width, height);

                        Line ln = new Line();
                        ln.Stroke = Brushes.Black;
                        ln.StrokeThickness = 2;
                        ln.X1 = PLo.X; ln.Y1 = PLo.Y;
                        ln.X2 = PHi.X; ln.Y2 = PHi.Y;

                        chartCanvas.Children.Add(ln);
                    }
                    #endregion

                    #region -> Bar
                    {
                        //*
                        Polygon plCL = new Polygon();
                        plCL.Stroke = Brushes.Black;
                        plCL.Fill =
                            item.Dir == Direction.Up ? Brushes.SpringGreen :
                            item.Dir == Direction.Dn ? Brushes.OrangeRed : Brushes.LightBlue;
                        plCL.StrokeThickness = 1;
                        plCL.HorizontalAlignment = HorizontalAlignment.Left;
                        plCL.VerticalAlignment = VerticalAlignment.Center;
                        plCL.Points = new PointCollection() {
                        Axis.NormalizedPoint(day+3, priceCurBeg, width, height),
                        Axis.NormalizedPoint(day-3, priceCurBeg, width, height),
                        Axis.NormalizedPoint(day-3, priceCurEnd, width, height),
                        Axis.NormalizedPoint(day+3, priceCurEnd, width, height),
                        };
                        chartCanvas.Children.Add(plCL);
                        //*/
                    }
                    #endregion
                }

            }
            #endregion


            #region -> Scale draw
            {
                List<int> scaleValues = new List<int>();

                //  scaleValues.Add(100);
                //  scaleValues.Add(250);

                chartCanvas.Children.Add(Axis.ScalePath(scaleValues, width, height));
            }
            #endregion

            chartCanvas.Children.Add(plHi);
            chartCanvas.Children.Add(plLo);
        }

        private int SolveDay(CandleTableLine item)
        {
            return (item.GetData().Date - dayFirst).Days + 1;
        }
        #endregion



        #region -> Event handlers
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            chartCanvas.Width = chartGrid.ActualWidth - 2;
            chartCanvas.Height = chartGrid.ActualHeight - 2;
            AddChart();
        }

        private void OnDataIsReadyToBeShown(IEnumerable<CandleTableLine> items)
        {
            if (items.Count() == 0)
            {
                Axis.YReassign(new double[] { 50, 550 });
                return;
            }

            dayFirst = items.Min(i => i.GetData().Date);
            dayLast = items.Max(i => i.GetData().Date);

            Axis.XMin = 0;
            Axis.XMax = 10 * ((dayLast - dayFirst).TotalDays + 2);
            Axis.YReassign(new double[] {
                (double)items.Min(i => i.GetData().Low),
                (double)items.Max(i => i.GetData().High),
            });

            Candles = items;


            Application.Current.Dispatcher.Invoke(() =>
            {
                AddChart();
            });
        }
        #endregion



        public CandleControl()
        {
            InitializeComponent();

            plHi = new Polyline();
            plHi.Stroke = Brushes.Gray;

            plLo = new Polyline();
            plLo.Stroke = Brushes.Gray;

            Axis = new Axistor(VisualTreeHelper.GetDpi(this).PixelsPerDip)
            {
                OffRt = 50
            };

            this.SizeChanged += OnSizeChanged;

            OnDataIsReadyToBeShown(Enumerable.Empty<Data.CandleTableLine>());
        }
    }
}
