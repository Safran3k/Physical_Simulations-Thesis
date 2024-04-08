using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Physical_Simulations.Classes
{
    class GridDrawer
    {
        Canvas canvas;
        double step;
        DoubleCollection dashArray;
        double offset;

        public GridDrawer(Canvas canvas, double step = 40)
        {
            this.canvas = canvas;
            this.step = step;
            dashArray = new DoubleCollection(new double[2] { 4, 4 });
            offset = 5;
        }

        public void DrawGrid()
        {
            canvas.Children.Clear();
            DrawDashedGrid();
            DrawNumbers();
        }

        private void DrawDashedGrid()
        {
            double centerX = canvas.ActualWidth / 2;
            double centerY = canvas.ActualHeight / 2;
            DrawCentralLines(centerX, centerY);
            DrawDashedLines(centerX, centerY);
        }

        private void DrawCentralLines(double centerX, double centerY)
        {
            AddLine(0, centerY, canvas.ActualWidth, centerY, Brushes.White, 1);
            AddLine(centerX, 0, centerX, canvas.ActualHeight, Brushes.White, 1);
        }

        private void DrawDashedLines(double centerX, double centerY)
        {
            for (double x = step; x < canvas.ActualWidth; x += step)
            {
                AddLine(centerX + x, 0, centerX + x, canvas.ActualHeight, Brushes.LightGray, 0.5, dashArray);
                AddLine(centerX - x, 0, centerX - x, canvas.ActualHeight, Brushes.LightGray, 0.5, dashArray);
            }

            for (double y = step; y < canvas.ActualHeight; y += step)
            {
                AddLine(0, centerY + y, canvas.ActualWidth, centerY + y, Brushes.LightGray, 0.5, dashArray);
                AddLine(0, centerY - y, canvas.ActualWidth, centerY - y, Brushes.LightGray, 0.5, dashArray);
            }
        }

        private void AddLine(double x1, double y1, double x2, double y2, Brush stroke, double thickness, DoubleCollection dashArray = null)
        {
            Line line = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = stroke,
                StrokeThickness = thickness,
                StrokeDashArray = dashArray
            };
            canvas.Children.Add(line);
        }

        private void DrawNumbers()
        {
            double centerX = canvas.ActualWidth / 2;
            double centerY = canvas.ActualHeight / 2;

            for (double x = step; x < canvas.ActualWidth / 2; x += step)
            {
                AddNumber(centerX + x - offset / 2, centerY + offset, x / step);
                AddNumber(centerX - x - offset / 2, centerY + offset, -x / step);
            }

            for (double y = step; y < canvas.ActualHeight / 2; y += step)
            {
                AddNumber(centerX + offset, centerY + y - offset / 2, -y / step);
                AddNumber(centerX + offset, centerY - y - offset / 2, y / step);
            }
        }

        private void AddNumber(double x, double y, double number)
        {
            TextBlock textBlock = new TextBlock
            {
                Text = number.ToString(),
                Foreground = Brushes.White,
                Background = Brushes.Black,
                FontSize = 8
            };
            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);
            canvas.Children.Add(textBlock);
        }

    }
}
