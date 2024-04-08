using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using Physical_Simulations.Enums;

namespace Physical_Simulations.Classes
{
    public class Chart
    {
        private Dictionary<BodyDataTypesEnum, SeriesCollection> seriesCollections;
        public List<string> Labels { get; private set; }
        public Func<double, string> YFormatter { get; private set; }
        private int xTick = 0;

        public Chart()
        {
            seriesCollections = new Dictionary<BodyDataTypesEnum, SeriesCollection>
            {
                { BodyDataTypesEnum.Acceleration_X, new SeriesCollection() },
                { BodyDataTypesEnum.Acceleration_Y, new SeriesCollection() },
                { BodyDataTypesEnum.Acceleration_Z, new SeriesCollection() }
            };
            Labels = new List<string>();
            YFormatter = value => value.ToString("N");
        }

        public static SolidColorBrush ConvertDrawingColorToMediaBrush(Color drawingColor)
        {
            return new SolidColorBrush(Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B));
        }

        public void AddChartData(double newValue, string seriesTitle, Color drawingColor, BodyDataTypesEnum dataType)
        {
            SolidColorBrush newBrush = ConvertDrawingColorToMediaBrush(drawingColor);
            SeriesCollection seriesCollection = seriesCollections[dataType];

            LineSeries series = seriesCollection.FirstOrDefault(s => s.Title == seriesTitle) as LineSeries;
            if (series == null)
            {
                series = new LineSeries
                {
                    Title = seriesTitle,
                    Values = new ChartValues<double>(),
                    Stroke = newBrush,
                    PointForeground = newBrush,
                    Fill = Brushes.Transparent,
                };
                seriesCollection.Add(series);
            }
            series.Values.Add(newValue);
            Labels.Add(xTick.ToString());
            xTick++;
        }

        public void ResetChart()
        {
            foreach (var seriesCollection in seriesCollections)
            {
                if (seriesCollection.Value.Chart != null)
                {
                    seriesCollection.Value.Clear();
                }
            }

            Labels.Clear();
            xTick = 0;
        }

        public SeriesCollection GetSeriesCollection(BodyDataTypesEnum dataType)
        {
            return seriesCollections[dataType];
        }

        public void ToggleSeriesVisibility(string checkBoxName, bool isVisible, BodyDataTypesEnum activeDataType)
        {
            string seriesTitle = checkBoxName.Replace("cbDataSeries", "Body ");
            SeriesCollection seriesCollection = seriesCollections[activeDataType];

            LineSeries series = seriesCollection.FirstOrDefault(s => s.Title == seriesTitle) as LineSeries;
            if (series != null)
            {
                series.Visibility = isVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            }
        }
    }
}
