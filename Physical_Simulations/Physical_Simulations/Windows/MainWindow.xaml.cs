using Physical_Simulations.Classes;
using Physical_Simulations.Enums;
using Physical_Simulations.Windows;
using System;
using System.Drawing;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Physical_Simulations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Global variables
        PhysicsEngine physicsEngineForMainSim;
        PhysicsEngine physicsEngineForCompSim1;
        PhysicsEngine physicsEngineForCompSim2;
        DispatcherTimer timerForMainAnimation;
        DispatcherTimer timerForComparisonAnimation;
        DispatcherTimer timerForGraph;
        CentralProjection centralProjection;
        ParallelProjection parallelProjection;
        BodyContainer bodyContainer;
        Chart chartViewModel;
        int remainingSeconds = 30;
        double deltaTime = 0.001;
        float singularityThreshold = 5.0f;
        bool isSimulationRunning = false;
        #endregion

        #region MainWindow constructor
        public MainWindow()
        {
            InitializeComponent();
            chartViewModel = new Chart();
            DataContext = chartViewModel;
            centralProjection = new CentralProjection();
            parallelProjection = new ParallelProjection(60);
            bodyContainer = new BodyContainer();
            InitializeChartBindings();
            cmbSolvers.ItemsSource = Enum.GetValues(typeof(SolversEnum));
            cmbSolversComparison1.ItemsSource = Enum.GetValues(typeof(SolversEnum));
            cmbSolversComparison2.ItemsSource = Enum.GetValues(typeof(SolversEnum));
            cmbProjection.ItemsSource = Enum.GetValues(typeof(ProjectionsEnum));
            cmbDataType.ItemsSource = Enum.GetValues(typeof(BodyDataTypesEnum));
            cmbPreDefinedMovementsComparison.ItemsSource = Enum.GetValues(typeof(PredefinedPositionsEnum));
            mainCanvas.MouseLeftButtonDown += MainCanvas_MouseLeftButtonDown;
        }
        #endregion

        #region Main simulation 

        #region Add Body's by clicking
        private void ConfigureBodyRepresentation(Body body, Vector3 position, ContextMenu contextMenu)
        {
            body.Representation.ContextMenu = contextMenu;
            Canvas.SetLeft(body.Representation, body.Position.X);
            Canvas.SetTop(body.Representation, body.Position.Y);
            mainCanvas.Children.Add(body.Representation);
            mainCanvas.Children.Add(body.Trajectory);
            mainCanvas.Children.Add(body.ForceVector);
        }

        private ContextMenu CreateBodyContextMenu(int bodyNumber)
        {
            ContextMenu menu = new ContextMenu();
            MenuItem menuItemVelocity = new MenuItem { Header = "Set starter velocities" };
            menuItemVelocity.Click += MenuItemVelocity_Click;
            MenuItem menuItemMass = new MenuItem { Header = "Set body mass" };
            menuItemMass.Click += MenuItemMass_Click;
            MenuItem menuItemZAxis = new MenuItem { Header = "Set body Z axis position" };
            menuItemZAxis.Click += MenuItemZCoordinate_Click;

            menu.Items.Add(menuItemVelocity);
            menu.Items.Add(menuItemMass);
            menu.Items.Add(menuItemZAxis);
            menu.Tag = bodyNumber.ToString();

            return menu;
        }

        private void AddNewBody(System.Windows.Point clickPosition, int bodyIndex)
        {
            System.Windows.Media.Color[] colors = { Colors.Red, Colors.Blue, Colors.Green };
            float offset = Body.DefaultInitialSize / 2;
            Vector3 position = new Vector3((float)clickPosition.X - offset, (float)clickPosition.Y - offset, 100);
            Body newBody = new Body(position, Vector3.Zero, Vector3.Zero, colors[bodyIndex]);

            ContextMenu contextMenu = CreateBodyContextMenu(bodyIndex);
            ConfigureBodyRepresentation(newBody, position, contextMenu);
            bodyContainer.AddBody(newBody);
        }

        private void MainCanvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (bodyContainer.PlacedBodiesCount >= 3) return;
            System.Windows.Point clickPosition = e.GetPosition(mainCanvas);
            AddNewBody(clickPosition, bodyContainer.PlacedBodiesCount);
        }
        #endregion

        #region Update body properties
        private void UpdateBodyProperty(MenuItem menuItem, Action<Body> updateAction)
        {
            if (menuItem.Parent is ContextMenu contextMenu && int.TryParse(contextMenu.Tag.ToString(), out int index) && index < bodyContainer.Count)
            {
                Body selectedBody = bodyContainer[index];
                updateAction(selectedBody);
            }
        }

        private void MenuItemVelocity_Click(object sender, RoutedEventArgs e)
        {
            UpdateBodyProperty(sender as MenuItem, selectedBody =>
            {
                CustomVelocityWindow dialog = new CustomVelocityWindow();
                dialog.InitializeValues(selectedBody.Velocity.X, selectedBody.Velocity.Y, selectedBody.Velocity.Z);
                if (dialog.ShowDialog() == true)
                {
                    selectedBody.Velocity = new Vector3(dialog.VelocityX, dialog.VelocityY, dialog.VelocityZ);
                }
            });
        }

        private void MenuItemMass_Click(object sender, RoutedEventArgs e)
        {
            UpdateBodyProperty(sender as MenuItem, selectedBody =>
            {
                CustomMassWindow dialog = new CustomMassWindow();
                dialog.InitializeValues(selectedBody.Mass);
                if (dialog.ShowDialog() == true)
                {
                    selectedBody.Mass = dialog.BodyMass;
                }
            });
        }

        private void MenuItemZCoordinate_Click(object sender, RoutedEventArgs e)
        {
            UpdateBodyProperty(sender as MenuItem, selectedBody =>
            {
                CustomZCoordinateWindow dialog = new CustomZCoordinateWindow();
                dialog.InitializeValues(selectedBody.Position.Z);
                if (dialog.ShowDialog() == true)
                {
                    selectedBody.Position = new Vector3(selectedBody.Position.X, selectedBody.Position.Y, dialog.ZPosition);
                }
            });
        }
        #endregion

        private void TimerForGraph_Tick(object sender, EventArgs e)
        {
            if (isSimulationRunning)
            {
                for (int i = 0; i < bodyContainer.Count; i++)
                {
                    Body body = bodyContainer[i];
                    string dataLabel = $"Body {i + 1}";
                    chartViewModel.AddChartData(body.Acceleration.X, dataLabel, body.Color, BodyDataTypesEnum.Acceleration_X);
                    chartViewModel.AddChartData(body.Acceleration.Y, dataLabel, body.Color, BodyDataTypesEnum.Acceleration_Y);
                    chartViewModel.AddChartData(body.Acceleration.Z, dataLabel, body.Color, BodyDataTypesEnum.Acceleration_Z);
                }
            }
            else
            {
                timerForGraph.Stop();
            }
        }

        private void TimerForMainSimulation_Tick(object sender, EventArgs e)
        {
            if (remainingSeconds > 0)
            {
                remainingSeconds--;
                UpdateTimerLabel(lblTimer);
            }
            else
            {
                isSimulationRunning = false;
                timerForMainAnimation.Stop();
                CompositionTarget.Rendering -= StartMainAnimation;
                lblTimer.Content = "The simulation is over.";
            }
        }

        private void CheckSingularityForMainAnimation()
        {
            if (ArePositionsTooClose(bodyContainer[0].Position, bodyContainer[1].Position, singularityThreshold) ||
                ArePositionsTooClose(bodyContainer[1].Position, bodyContainer[2].Position, singularityThreshold) ||
                ArePositionsTooClose(bodyContainer[0].Position, bodyContainer[2].Position, singularityThreshold))
            {
                StopSimulation();
                lblTimer.Content = "Singularity detected!";
            }
        }

        private void ScaleAndPositionBody(Body body, double initialSize)
        {
            float minimumScale = 0.1f;
            float maximumScale = 2.0f;
            float scaleRange = maximumScale - minimumScale;
            float normalizedMass = (float)(body.Mass - 1.0f) / (1000 - 1.0f);
            float scaleBasedOnMass = minimumScale + (scaleRange * normalizedMass);

            body.Representation.Width = initialSize * scaleBasedOnMass;
            body.Representation.Height = initialSize * scaleBasedOnMass;
            Canvas.SetLeft(body.Representation, body.Position.X);
            Canvas.SetTop(body.Representation, body.Position.Y);
        }

        private void StartMainAnimation(object sender, EventArgs e)
        {
            physicsEngineForMainSim.UpdateState();

            for (int i = 0; i < bodyContainer.Count; i++)
            {
                bodyContainer[i].DrawForceVectorArrow();
                ScaleAndPositionBody(bodyContainer[i], Body.DefaultInitialSize);
                DrawOrbitalPath(bodyContainer[i]);
            }

            UpdateMeasurementsLabel();
            CheckSingularityForMainAnimation();
        }

        #region Label methods
        private double? GetMeasurementValue(Body body, string measurement)
        {
            switch (measurement)
            {
                case "VX": return body.Velocity.X;
                case "VY": return body.Velocity.Y;
                case "VZ": return body.Velocity.Z;
                case "FX": return body.Force.X;
                case "FY": return body.Force.Y;
                case "FZ": return body.Force.Z;
                case "AX": return body.Acceleration.X;
                case "AY": return body.Acceleration.Y;
                case "AZ": return body.Acceleration.Z;
                default: return null;
            }
        }

        private void SetLabelContent(Label label, string measurementName, double? value = null)
        {
            label.Content = value.HasValue ? $"{measurementName}:\t{value:F2}" : $"{measurementName}:\t";
        }

        private void UpdateMeasurementsLabel(bool reset = false)
        {
            string[] measurements = new string[] { "VX", "VY", "VZ", "FX", "FY", "FZ", "AX", "AY", "AZ" };

            foreach (string measurement in measurements)
            {
                for (int i = 0; i < bodyContainer.Count; i++)
                {
                    Label label = FindName($"lbB{i + 1}{measurement.Substring(0, 2)}") as Label;
                    if (label != null)
                    {
                        double? value = reset ? null : GetMeasurementValue(bodyContainer[i], measurement);
                        SetLabelContent(label, measurement, value);
                    }
                }
            }
        }

        private void ResetMeasurementsLabel()
        {
            UpdateMeasurementsLabel(reset: true);
        }
        #endregion

        private void DrawOrbitalPath(Body body)
        {
            body.DrawOrbitLine(
                new System.Windows.Point(
                    body.Position.X + body.Representation.Width / 2,
                    body.Position.Y + body.Representation.Height / 2
                )
            );
        }

        private void EnableOrDisableMainGroupBoxes(bool isEnable)
        {
            gbSetGravConstMain.IsEnabled = isEnable;
            gbSetSimTimeMain.IsEnabled = isEnable;
            gbSetSingThresholdMain.IsEnabled = isEnable;
            gbSetDtMain.IsEnabled = isEnable;
        }

        private void ResetMainSimulation()
        {
            isSimulationRunning = false;
            cmbSolvers.IsEnabled = true;
            remainingSeconds = 30;
            NewtonsGravitationalLaw.GravitationalConstant = 6.6743 * 10e-11 * 10e11;
            deltaTime = 0.001;
            singularityThreshold = 5.0f;
            UpdateTimerLabel(lblTimer);
            ClearTextBoxes(tbGravitationalConstantMain, tbSingularityThresholdMain, tbSimulationTimeMain, tbDtMain);
            EnableOrDisableMainGroupBoxes(true);
            if (timerForMainAnimation != null)
            {
                timerForMainAnimation.Tick -= TimerForMainSimulation_Tick;
            }
            if (timerForGraph != null)
            {
                timerForGraph.Tick -= TimerForGraph_Tick;
            }
            CompositionTarget.Rendering -= StartMainAnimation;
            physicsEngineForMainSim = null;
            bodyContainer.Clear();
            chartViewModel.ResetChart();
            ResetMeasurementsLabel();
            ResetCanvasElements(mainCanvas);
        }

        private void BtnStartSimulation_Click(object sender, RoutedEventArgs e)
        {
            if (!isSimulationRunning && bodyContainer.PlacedBodiesCount == 3)
            {
                physicsEngineForMainSim = new PhysicsEngine(bodyContainer[0], bodyContainer[1], bodyContainer[2], deltaTime, (SolversEnum)cmbSolvers.SelectedIndex);
                isSimulationRunning = true;
                cmbSolvers.IsEnabled = false;
                EnableOrDisableMainGroupBoxes(false);
                InitializeTimer(ref timerForMainAnimation);
                InitializeTimer(ref timerForGraph);
                timerForMainAnimation.Tick += TimerForMainSimulation_Tick;
                timerForGraph.Tick += TimerForGraph_Tick;
                timerForMainAnimation.Start();
                timerForGraph.Start();
                CompositionTarget.Rendering += StartMainAnimation;
            }
        }

        private void BtnStopSimulation_Click(object sender, RoutedEventArgs e)
        {
            ResetMainSimulation();
        }
        #endregion

        #region Comparison simulation
        private void AddBodyToCanvas(Body body, Canvas canvas)
        {
            Canvas.SetLeft(body.Representation, body.Position.X);
            Canvas.SetTop(body.Representation, body.Position.Y);
            canvas.Children.Add(body.Representation);
            canvas.Children.Add(body.Trajectory);
        }

        private void BtnStartComparion_Click(object sender, RoutedEventArgs e)
        {
            if (!isSimulationRunning)
            {
                if (cmbPreDefinedMovementsComparison.SelectedItem is PredefinedPositionsEnum selectedPosition)
                {
                    Vector3[] positions;
                    switch (selectedPosition)
                    {
                        case PredefinedPositionsEnum.Positions_1:
                            positions = new Vector3[]
                            {
                                new Vector3(100, 200, 300),
                                new Vector3(300, 200, 100),
                                new Vector3(300, 400, 250),
                                new Vector3(100, 200, 300),
                                new Vector3(300, 200, 100),
                                new Vector3(300, 400, 250)
                            };
                            break;
                        case PredefinedPositionsEnum.Positions_2:
                            positions = new Vector3[]
                            {
                                new Vector3(250, 110, 100),
                                new Vector3(250, 220, 50),
                                new Vector3(250, 330, 10),
                                new Vector3(250, 110, 100),
                                new Vector3(250, 220, 50),
                                new Vector3(250, 330, 10)
                            };
                            break;
                        case PredefinedPositionsEnum.Positions_3:
                            positions = new Vector3[]
                            {
                                new Vector3(70, 200, 300),
                                new Vector3(300, 200, 100),
                                new Vector3(250, 400, 250),
                                new Vector3(70, 200, 300),
                                new Vector3(300, 200, 100),
                                new Vector3(250, 400, 250)
                            };
                            break;
                        default:
                            return;
                    }

                    System.Windows.Media.Color[] colors = { Colors.Red, Colors.Blue, Colors.Green, Colors.Red, Colors.Blue, Colors.Green };
                    double[] masses = { 250, 550, 100, 250, 550, 100 };

                    for (int i = 0; i < positions.Length; i++)
                    {
                        Body body = new Body(positions[i], Vector3.Zero, Vector3.Zero, colors[i], masses[i]);
                        Canvas canvas = (i < 3) ? comparisonMainCanvas1 : comparisonMainCanvas2;
                        AddBodyToCanvas(body, canvas);
                        bodyContainer.AddBody(body);
                    }

                    physicsEngineForCompSim1 = new PhysicsEngine(bodyContainer[0], bodyContainer[1], bodyContainer[2], deltaTime, (SolversEnum)cmbSolversComparison1.SelectedIndex);
                    physicsEngineForCompSim2 = new PhysicsEngine(bodyContainer[3], bodyContainer[4], bodyContainer[5], deltaTime, (SolversEnum)cmbSolversComparison2.SelectedIndex);
                    isSimulationRunning = true;
                    cmbSolversComparison1.IsEnabled = false;
                    cmbSolversComparison2.IsEnabled = false;
                    cmbPreDefinedMovementsComparison.IsEnabled = false;
                    EnableOrDisableGroupBoxes(false, gbSetGravConstComp, gbSetSimTimeComp, gbSetSingThresholdComp, gbSetDtComp);
                    InitializeTimer(ref timerForComparisonAnimation);
                    timerForComparisonAnimation.Tick += TimerForComparisonSimulation_Tick;
                    timerForComparisonAnimation.Start();
                    CompositionTarget.Rendering += StartComparisonAnimation;
                }
            }
        }

        private void ResetComparisonSimulation()
        {
            isSimulationRunning = false;
            cmbSolversComparison1.IsEnabled = true;
            cmbSolversComparison2.IsEnabled = true;
            cmbPreDefinedMovementsComparison.IsEnabled = true;
            remainingSeconds = 30;
            UpdateTimerLabel(lblComparisonTimer);
            EnableOrDisableGroupBoxes(true, gbSetGravConstComp, gbSetSimTimeComp, gbSetSingThresholdComp, gbSetDtComp);
            ClearTextBoxes(tbGravitationalConstantComp, tbSingularityThresholdComp, tbSimulationTimeComp, tbDtComp);
            if (timerForComparisonAnimation != null)
            {
                timerForComparisonAnimation.Tick -= TimerForComparisonSimulation_Tick;
            }
            CompositionTarget.Rendering -= StartComparisonAnimation;
            physicsEngineForCompSim1 = null;
            physicsEngineForCompSim2 = null;
            bodyContainer.Clear();
            ResetCanvasElements(comparisonMainCanvas1);
            ResetCanvasElements(comparisonMainCanvas2);
        }

        private void BtnStopComparison_Click(object sender, RoutedEventArgs e)
        {
            ResetComparisonSimulation();
        }

        private void TimerForComparisonSimulation_Tick(object sender, EventArgs e)
        {
            if (remainingSeconds > 0)
            {
                remainingSeconds--;
                UpdateTimerLabel(lblComparisonTimer);
            }
            else
            {
                timerForComparisonAnimation.Stop();
                CompositionTarget.Rendering -= StartComparisonAnimation;
                lblComparisonTimer.Content = "The simulation is over.";
            }
        }

        private void ClearAllTrajectories()
        {
            if (bodyContainer != null)
            {
                for (int i = 0; i < bodyContainer.Count; i++)
                {
                    bodyContainer[i].Trajectory.Points.Clear();
                }
            }

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ClearAllTrajectories();
        }

        private void UpdateBodyPositionAndScale(Body body, double[] result, int index)
        {
            PointF projectedPosition = new PointF();

            if (cmbProjection.SelectedItem is ProjectionsEnum selectedProjection)
            {
                switch (selectedProjection)
                {
                    case ProjectionsEnum.Central_projection:
                        if (rbCentralProjectionX.IsChecked == true)
                        {
                            projectedPosition = centralProjection.PerspectiveFromXAxis((float)result[index], (float)result[index + 1], (float)result[index + 2]);
                        }
                        else if (rbCentralProjectionY.IsChecked == true)
                        {
                            projectedPosition = centralProjection.PerspectiveFromYAxis((float)result[index], (float)result[index + 1], (float)result[index + 2]);
                        }
                        else if (rbCentralProjectionZ.IsChecked == true)
                        {
                            projectedPosition = centralProjection.PerspectiveFromZAxis((float)result[index], (float)result[index + 1], (float)result[index + 2]);
                        }
                        break;
                    case ProjectionsEnum.Parallel_projection:
                        projectedPosition = parallelProjection.Project((float)result[index], (float)result[index + 1], (float)result[index + 2]);
                        break;
                }
            }

            Canvas.SetLeft(body.Representation, projectedPosition.X);
            Canvas.SetTop(body.Representation, projectedPosition.Y);

            float minimumScale = 0.1f;
            float scale = Math.Max(centralProjection.ProjectionDistance / (float)result[index + 2], minimumScale);
            body.Representation.Width = Body.DefaultInitialSize * scale;
            body.Representation.Height = Body.DefaultInitialSize * scale;

            body.DrawOrbitLine(
                new System.Windows.Point(
                    projectedPosition.X + body.Representation.Width / 2, projectedPosition.Y + body.Representation.Height / 2)
            );
        }

        private void cmbProjection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gbCentralProjAxis.IsEnabled = (ProjectionsEnum)cmbProjection.SelectedIndex != ProjectionsEnum.Parallel_projection;
            ClearAllTrajectories();
        }

        private void CheckSingularityForComparisonAnimation()
        {
            if (ArePositionsTooClose(bodyContainer[0].Position, bodyContainer[1].Position, singularityThreshold) ||
                ArePositionsTooClose(bodyContainer[1].Position, bodyContainer[2].Position, singularityThreshold) ||
                ArePositionsTooClose(bodyContainer[0].Position, bodyContainer[2].Position, singularityThreshold) ||
                ArePositionsTooClose(bodyContainer[3].Position, bodyContainer[4].Position, singularityThreshold) ||
                ArePositionsTooClose(bodyContainer[4].Position, bodyContainer[5].Position, singularityThreshold) ||
                ArePositionsTooClose(bodyContainer[3].Position, bodyContainer[5].Position, singularityThreshold))
            {
                StopSimulation();
                lblComparisonTimer.Content = "Singularity detected!";
            }
        }

        private void StartComparisonAnimation(object sender, EventArgs e)
        {
            double[] result1 = physicsEngineForCompSim1.UpdateState();
            double[] result2 = physicsEngineForCompSim2.UpdateState();

            for (int i = 0; i < 3; i++)
            {
                int index = i * 6;
                UpdateBodyPositionAndScale(bodyContainer[i], result1, index);
                UpdateBodyPositionAndScale(bodyContainer[i + 3], result2, index);
            }

            CheckSingularityForComparisonAnimation();
        }
        #endregion

        #region Common methods and events
        private void ResetCanvasElements(Canvas canvas)
        {
            canvas.Children.Clear();
            GridDrawer gridDrawer = new GridDrawer(canvas);
            gridDrawer.DrawGrid();
        }

        #region Checkbox events
        private void ModifyTrajectory(Canvas canvas, Action<Canvas, Body> operation)
        {
            if (bodyContainer != null)
            {
                for (int i = 0; i < bodyContainer.Count; i++)
                {
                    operation(canvas, bodyContainer[i]);
                }
            }
        }

        private void AddTrajectoryToCanvas(Canvas canvas, Body body)
        {
            if (!canvas.Children.Contains(body.Trajectory))
            {
                canvas.Children.Add(body.Trajectory);
            }
        }

        private void RemoveTrajectoryFromCanvas(Canvas canvas, Body body)
        {
            if (canvas.Children.Contains(body.Trajectory))
            {
                canvas.Children.Remove(body.Trajectory);
            }
        }

        private void AddForceVectorToCanvas(Canvas canvas, Body body)
        {
            if (!canvas.Children.Contains(body.ForceVector))
            {
                canvas.Children.Add(body.ForceVector);
            }
        }

        private void RemoveForceVectorFromCanvas(Canvas canvas, Body body)
        {
            if (canvas.Children.Contains(body.ForceVector))
            {
                canvas.Children.Remove(body.ForceVector);
            }
        }

        private void CbHideTrajectory_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox == null) return;

            if (checkBox.Name == "cbHideTrajectory")
            {
                ModifyTrajectory(mainCanvas, RemoveTrajectoryFromCanvas);
            }
        }

        private void CbHideTrajectory_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox == null) return;

            if (checkBox.Name == "cbHideTrajectory")
            {
                ModifyTrajectory(mainCanvas, AddTrajectoryToCanvas);
            }
        }

        private void CbHideForceVector_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox == null) return;

            if (checkBox.Name == "cbHideForceVector")
            {
                ModifyTrajectory(mainCanvas, RemoveForceVectorFromCanvas);
            }
        }

        private void CbHideForceVector_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox == null) return;

            if (checkBox.Name == "cbHideForceVector")
            {
                ModifyTrajectory(mainCanvas, AddForceVectorToCanvas);
            }
        }

        private void CbDataSeries_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (chartViewModel != null)
            {
                chartViewModel.ToggleSeriesVisibility(checkBox.Name, true, (BodyDataTypesEnum)cmbDataType.SelectedIndex);
            }
        }

        private void CbDataSeries_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (chartViewModel != null)
            {
                chartViewModel.ToggleSeriesVisibility(checkBox.Name, false, (BodyDataTypesEnum)cmbDataType.SelectedIndex);
            }
        }
        #endregion

        private void UpdateTimerLabel(Label label)
        {
            int minutes = remainingSeconds / 60;
            int seconds = remainingSeconds % 60;
            label.Content = $"Remaining time: {minutes:D2}:{seconds:D2}";
        }

        private void InitializeTimer(ref DispatcherTimer timer)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                // Késleltetett megjelenítés
                if (tabControl.SelectedItem == tabComparisonSimulation)
                {
                    ResetMainSimulation();
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        GridDrawer gridDrawer1 = new GridDrawer(comparisonMainCanvas1);
                        gridDrawer1.DrawGrid();
                        GridDrawer gridDrawer2 = new GridDrawer(comparisonMainCanvas2);
                        gridDrawer2.DrawGrid();
                    }), DispatcherPriority.Loaded);
                }
                else if (tabControl.SelectedItem == tabSimulation)
                {
                    if (bodyContainer.Count > 3)
                    {
                        ResetComparisonSimulation();
                    }
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (mainCanvas.Children.Count == 0)
                        {
                            GridDrawer gridDrawer = new GridDrawer(mainCanvas);
                            gridDrawer.DrawGrid();
                        }
                    }), DispatcherPriority.Loaded);
                }
            }
        }

        private void BtnSimulationInfo_Click(object sender, RoutedEventArgs e)
        {
            Windows.InformationWindow infoWindow = new Windows.InformationWindow();
            infoWindow.ShowDialog();
        }

        private bool ArePositionsTooClose(Vector3 position1, Vector3 position2, float threshold)
        {
            return Vector3.Distance(position1, position2) < threshold;
        }

        private void StopSimulation()
        {
            if (timerForMainAnimation != null && timerForMainAnimation.IsEnabled && timerForGraph != null && timerForGraph.IsEnabled)
            {
                timerForMainAnimation.Stop();
                timerForMainAnimation.Tick -= TimerForMainSimulation_Tick;
                timerForGraph.Tick -= TimerForGraph_Tick;
            }

            if (timerForComparisonAnimation != null && timerForComparisonAnimation.IsEnabled)
            {
                timerForComparisonAnimation.Stop();
                timerForComparisonAnimation.Tick -= TimerForComparisonSimulation_Tick;
            }

            CompositionTarget.Rendering -= StartMainAnimation;
            CompositionTarget.Rendering -= StartComparisonAnimation;
            isSimulationRunning = false;
        }

        private void InitializeChartBindings()
        {
            System.Windows.Data.Binding labelsBinding = new System.Windows.Data.Binding("Labels")
            {
                Source = chartViewModel
            };
            cartesianChart.AxisX[0].SetBinding(LiveCharts.Wpf.Axis.LabelsProperty, labelsBinding);
        }

        private void cmbDataType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (chartViewModel == null) { return; }

            BodyDataTypesEnum selectedDataType = (BodyDataTypesEnum)cmbDataType.SelectedIndex;
            LiveCharts.SeriesCollection selectedSeriesCollection = chartViewModel.GetSeriesCollection(selectedDataType);

            cartesianChart.Series = selectedSeriesCollection;
        }

        #region Set simulation values buttons, and methods
        public void UpdateGravitationalConstant(TextBox tbGravitationalConstant)
        {
            string input = tbGravitationalConstant.Text.Trim();
            string normalizedInput = input.Replace(',', '.');

            try
            {
                System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(normalizedInput, @"^(?<baseValue>\d+(\.\d+)?)(e(?<exponent>-?\d+))?(e(?<scaling>-?\d+))?$");

                if (!match.Success)
                {
                    throw new FormatException("Please enter the gravitational constant in the correct format.");
                }

                double baseValue = double.Parse(match.Groups["baseValue"].Value, System.Globalization.CultureInfo.InvariantCulture);
                int exponent = match.Groups["exponent"].Success ? int.Parse(match.Groups["exponent"].Value) : 0;
                int scaling = match.Groups["scaling"].Success ? int.Parse(match.Groups["scaling"].Value) : 0;

                double finalGravitationalConstant = baseValue * Math.Pow(10, exponent + scaling);
                NewtonsGravitationalLaw.GravitationalConstant = finalGravitationalConstant;

                MessageBox.Show($"Gravitational constant updated to: {finalGravitationalConstant} x 10^{exponent + scaling}", "Update Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while updating gravitational constant: {ex.Message}", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void btnUpdateGravConst_Click(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender == btnUpdateGravConstMain ? tbGravitationalConstantMain : tbGravitationalConstantComp;
            UpdateGravitationalConstant(textBox);
        }

        public void UpdateSimulationValue(TextBox textBox, Action<double> updateAction, string errorMessage)
        {
            if (double.TryParse(textBox.Text.Trim().Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double value))
            {
                updateAction(value);
                MessageBox.Show($"Value updated to: {value}", "Update Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(errorMessage, "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void btnSimulationTime_Click(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender == btnSimulationTimeMain ? tbSimulationTimeMain : tbSimulationTimeComp;
            UpdateSimulationValue(textBox, value => remainingSeconds = (int)value, "Please enter a valid time in seconds.");
        }

        public void btnSingularityTreshold_Click(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender == btnSingularityTresholdMain ? tbSingularityThresholdMain : tbSingularityThresholdComp;
            UpdateSimulationValue(textBox, value => singularityThreshold = (float)value, "Please enter a valid threshold value.");
        }

        public void btnUpdateDt_Click(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender == btnUpdateDtMain ? tbDtMain : tbDtComp;
            UpdateSimulationValue(textBox, value => deltaTime = value, "Please enter a valid Dt value.");
        }
        #endregion

        private void ClearTextBoxes(params TextBox[] textBoxes)
        {
            foreach (TextBox textBox in textBoxes)
            {
                textBox.Text = string.Empty;
            }
        }

        private void EnableOrDisableGroupBoxes(bool isEnable, params GroupBox[] groupBoxes)
        {
            foreach (GroupBox groupBox in groupBoxes)
            {
                groupBox.IsEnabled = isEnable;
            }
        }
        #endregion


    }
}
