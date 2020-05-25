// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="FH Wr. Neustadt">
//   Christoph Hauer / Markus Zytek. All rights reserved.
// </copyright>
// <summary>
//   The main window view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MaxFlowMinCut.Wpf.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;
    using System.Xml.Serialization;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using MaxFlowMinCut.Lib;
    using MaxFlowMinCut.Lib.Algorithm;
    using MaxFlowMinCut.Lib.History;
    using MaxFlowMinCut.Lib.Network;
    using MaxFlowMinCut.Wpf.Model;
    using MaxFlowMinCut.Wpf.View;

    using Microsoft.Win32;

    using Mutzl.MvvmLight;

    /// <summary>
    /// The main window view model.
    /// </summary>
    internal class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// The current step.
        /// </summary>
        private int currentStep;

        /// <summary>
        /// The graph steps history.
        /// </summary>
        private GraphHistory graphSteps;

        /// <summary>
        /// The input edges.
        /// </summary>
        private ObservableCollection<InputEdge> inputEdges = new ObservableCollection<InputEdge>();

        /// <summary>
        /// The play steps dispatcher timer.
        /// </summary>
        private DispatcherTimer playStepsDispatcherTimer;

        /// <summary>
        /// The visualized graph.
        /// </summary>
        private Graph visualizedGraph;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            this.InitializeFields();
            this.InitializeGraph();
            this.InitializeCommands();
        }

        /// <summary>
        /// The flow graph changed.
        /// </summary>
        public event EventHandler<Graph> FlowGraphChanged;

        /// <summary>
        /// The residual graph changed.
        /// </summary>
        public event EventHandler<Graph> ResidualGraphChanged;

        /// <summary>
        /// Occurs when [reset graph layout].
        /// </summary>
        public event EventHandler ResetGraphLayout;

        /// <summary>
        /// Gets a value indicating whether is visualized.
        /// </summary>
        /// <value>
        /// The is visualized.
        /// </value>
        public bool IsVisualized { get; private set; }

        /// <summary>
        /// Gets a value indicating whether is calculated.
        /// </summary>
        /// <value>
        /// The is calculated.
        /// </value>
        public bool IsCalculated { get; private set; }

        /// <summary>
        /// Gets the play steps command.
        /// </summary>
        /// <value>
        /// The play steps command.
        /// </value>
        public RelayCommand PlayStepsCommand { get; private set; }

        /// <summary>
        /// Gets the stop command.
        /// </summary>
        /// <value>
        /// The stop command.
        /// </value>
        public RelayCommand StopCommand { get; private set; }

        /// <summary>
        /// Gets the step forward command.
        /// </summary>
        /// <value>
        /// The step forward command.
        /// </value>
        public RelayCommand StepForwardCommand { get; private set; }

        /// <summary>
        /// Gets the step backward command.
        /// </summary>
        /// <value>
        /// The step backward command.
        /// </value>
        public RelayCommand StepBackwardCommand { get; private set; }

        /// <summary>
        /// Gets the go to start command.
        /// </summary>
        /// <value>
        /// The go to start command.
        /// </value>
        public RelayCommand GoToStartCommand { get; private set; }

        /// <summary>
        /// Gets the go to end command.
        /// </summary>
        /// <value>
        /// The go to end command.
        /// </value>
        public RelayCommand GoToEndCommand { get; private set; }

        /// <summary>
        /// Gets the reset graph layout command.
        /// </summary>
        /// <value>
        /// The reset graph layout command.
        /// </value>
        public DependentRelayCommand ResetGraphLayoutCommand { get; private set; }

        /// <summary>
        /// Gets the clear graph command.
        /// </summary>
        /// <value>
        /// The clear graph command.
        /// </value>
        public RelayCommand ClearGraphCommand { get; private set; }

        /// <summary>
        /// Gets the calculate command.
        /// </summary>
        /// <value>
        /// The calculate command.
        /// </value>
        public DependentRelayCommand CalculateCommand { get; private set; }

        /// <summary>
        /// Gets the visualize command.
        /// </summary>
        /// <value>
        /// The visualize command.
        /// </value>
        public RelayCommand VisualizeCommand { get; private set; }

        /// <summary>
        /// Gets or sets the show graph history command.
        /// </summary>
        /// <value>
        /// The show graph history command.
        /// </value>
        public DependentRelayCommand ShowGraphHistoryCommand { get; set; }

        /// <summary>
        /// Gets or sets the input edges.
        /// </summary>
        /// <value>
        /// The input edges.
        /// </value>
        public ObservableCollection<InputEdge> InputEdges
        {
            get
            {
                return this.inputEdges;
            }

            set
            {
                this.inputEdges = value;
                this.IsVisualized = false;
                this.IsCalculated = false;
            }
        }

        /// <summary>
        /// Gets or sets the flow graph.
        /// </summary>
        /// <value>
        /// The flow graph.
        /// </value>
        public Graph FlowGraph { get; set; }

        /// <summary>
        /// Gets or sets the source node.
        /// </summary>
        /// <value>
        /// The source node.
        /// </value>
        public string SourceNode { get; set; }


        /// <summary>
        /// Gets or sets the target node.
        /// </summary>
        /// <value>
        /// The target node.
        /// </value>
        public string TargetNode { get; set; }

        /// <summary>
        /// Gets or sets the last step command.
        /// </summary>
        /// <value>
        /// The last step command.
        /// </value>
        public RelayCommand LastStepCommand { get; set; }

        /// <summary>
        /// Gets or sets the pause steps command.
        /// </summary>
        /// <value>
        /// The pause steps command.
        /// </value>
        public RelayCommand PauseStepsCommand { get; set; }

        /// <summary>
        /// Gets or sets the first step command.
        /// </summary>
        /// <value>
        /// The first step command.
        /// </value>
        public RelayCommand FirstStepCommand { get; set; }

        /// <summary>
        /// Gets the max flow.
        /// </summary>
        /// <value>
        /// The max flow.
        /// </value>
        public int MaxFlow { get; private set; }

        /// <summary>
        /// Gets the min cut.
        /// </summary>
        /// <value>
        /// The min cut.
        /// </value>
        public int MinCut { get; private set; }

        /// <summary>
        /// Gets or sets the load graph command.
        /// </summary>
        /// <value>
        /// The load graph command.
        /// </value>
        public RelayCommand LoadGraphCommand { get; set; }

        /// <summary>
        /// Gets or sets the save graph command.
        /// </summary>
        /// <value>
        /// The save graph command.
        /// </value>
        public RelayCommand SaveGraphCommand { get; set; }

        /// <summary>
        /// Called when reset graph layout reset was requested.
        /// </summary>
        private void RaiseResetGraphLayout()
        {
            var handler = this.ResetGraphLayout;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// The convert input edges to graph.
        /// </summary>
        /// <param name="inputEdges">
        /// The input edges.
        /// </param>
        /// <returns>
        /// The <see cref="Graph"/>.
        /// </returns>
        private Graph ConvertInputEdgesToGraph(IEnumerable<InputEdge> inputEdges)
        {
            var graph = new Graph();

            foreach (var inputEdge in inputEdges)
            {
                var nodeFrom = graph.Nodes.Find(n => n.Name.Equals(inputEdge.NodeFrom));
                if (nodeFrom == null)
                {
                    nodeFrom = new Node(inputEdge.NodeFrom);
                    graph.Nodes.Add(nodeFrom);
                }

                var nodeTo = graph.Nodes.Find(n => n.Name.Equals(inputEdge.NodeTo));
                if (nodeTo == null)
                {
                    nodeTo = new Node(inputEdge.NodeTo);
                    graph.Nodes.Add(nodeTo);
                }

                var edge = new Edge(nodeFrom, nodeTo, inputEdge.Capacity);
                nodeFrom.Edges.Add(edge);
                graph.Edges.Add(edge);
            }

            return graph;
        }

        /// <summary>
        /// The initialize fields.
        /// </summary>
        private void InitializeFields()
        {
            this.playStepsDispatcherTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 1) };
            this.playStepsDispatcherTimer.Tick += this.PlayStepsDispatcherTimerOnTick;
        }

        /// <summary>
        /// The initialize commands.
        /// </summary>
        private void InitializeCommands()
        {
            this.VisualizeCommand = new RelayCommand(this.ExecuteVisualizeGraph, this.CanExecuteVisualizeCommand);

            this.CalculateCommand = new DependentRelayCommand(
                this.ExecuteCalculateGraph,
                this.CanExecuteCalculateCommand,
                this, 
                () => this.IsVisualized, 
                () => this.SourceNode, 
                () => this.TargetNode);

            this.ClearGraphCommand = new RelayCommand(this.ExecuteClearGraph);

            this.LoadGraphCommand = new RelayCommand(this.ExecuteLoadGraph, this.CanExecuteLoadGraphCommand);

            this.SaveGraphCommand = new RelayCommand(this.ExecuteSaveGraph, this.CanExecuteSaveGraphCommand);

            this.FirstStepCommand = new RelayCommand(
                this.ExecuteVisualizeFirstGraphStep,
                this.CanExecuteFirstStepCommand);

            this.StepForwardCommand = new RelayCommand(
                this.ExecuteVisualizeNextGraphStep,
                this.CanExecuteStepForwardCommand);

            this.PlayStepsCommand = new RelayCommand(this.ExecutePlaySteps, this.CanExecutePlayStepsCommand);

            this.PauseStepsCommand = new RelayCommand(this.ExecutePauseSteps, this.CanExecutePauseStepsCommand);

            this.StepBackwardCommand = new RelayCommand(
                this.ExecuteVisualizePreviousGraphStep,
                this.CanExecuteStepBackwardCommand);

            this.LastStepCommand = new RelayCommand(this.ExecuteVisualizeLastGraphStep, this.CanExecuteLastStepCommand);

            this.ShowGraphHistoryCommand = new DependentRelayCommand(
                this.ExecuteShowGraphHistory,
                this.CanExecuteShowGraphHistoryCommand,
                this,
                () => this.IsCalculated);

            this.ResetGraphLayoutCommand = new DependentRelayCommand(
                this.RaiseResetGraphLayout,
                () => this.IsVisualized && !this.playStepsDispatcherTimer.IsEnabled,
                this,
                () => this.IsVisualized);
        }

        /// <summary>
        /// The execute save graph.
        /// </summary>
        private void ExecuteSaveGraph()
        {
            var saveFileDialog = new SaveFileDialog();

            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.Filter = "XML-Documents (.xml)|*.xml";

            saveFileDialog.ShowDialog();
            var fileName = saveFileDialog.FileName;

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return;
            }

            try
            {
                var xmlSerializer = new XmlSerializer(this.InputEdges.GetType());
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    xmlSerializer.Serialize(fileStream, this.InputEdges);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
        }

        /// <summary>
        /// The can execute save graph command.
        /// </summary>
        /// <returns>
        /// True, if can execute save graph command.
        /// </returns>
        private bool CanExecuteSaveGraphCommand()
        {
            return this.InputEdges.Count > 0;
        }

        /// <summary>
        /// The execute load graph.
        /// </summary>
        private void ExecuteLoadGraph()
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.Multiselect = false;
            openFileDialog.DefaultExt = ".xml";
            openFileDialog.Filter = "XML-Documents (.xml)|*.xml";

            openFileDialog.ShowDialog();
            var fileName = openFileDialog.FileName;

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return;
            }

            try
            {
                var xmlSerializer = new XmlSerializer(this.InputEdges.GetType());
                using (var fileStream = new FileStream(fileName, FileMode.Open))
                {
                    this.InputEdges = (ObservableCollection<InputEdge>)xmlSerializer.Deserialize(fileStream);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
        }

        /// <summary>
        /// The can execute load graph command.
        /// </summary>
        /// <returns>
        /// True, if can execute load graph.
        /// </returns>
        private bool CanExecuteLoadGraphCommand()
        {
            return true;
        }

        /// <summary>
        /// The can execute show graph history command.
        /// </summary>
        /// <returns>
        /// True, if can execute show history.
        /// </returns>
        private bool CanExecuteShowGraphHistoryCommand()
        {
            return this.IsVisualized && this.IsCalculated && !this.playStepsDispatcherTimer.IsEnabled;
        }

        /// <summary>
        /// The can execute calculate command.
        /// </summary>
        /// <returns>
        /// True, if can execute calculate.
        /// </returns>
        private bool CanExecuteCalculateCommand()
        {
            if (string.IsNullOrEmpty(this.SourceNode) || string.IsNullOrEmpty(this.TargetNode))
            {
                return false;
            }

            try
            {
                var inputEdgesContainsSource = this.InputEdges.Any(e => e.NodeFrom.Equals(this.SourceNode));
                var inputEdgesContainsTarget = this.InputEdges.Any(e => e.NodeTo.Equals(this.TargetNode));

                var sourceEdge = this.InputEdges.FirstOrDefault(e => e.NodeFrom.Equals(this.SourceNode));
                string sourceName = string.Empty;
                if (sourceEdge != null)
                {
                    sourceName = sourceEdge.NodeFrom;
                }

                var targetEdge = this.InputEdges.FirstOrDefault(e => e.NodeTo.Equals(this.TargetNode));
                string targetName = string.Empty;

                if (targetEdge != null)
                {
                    targetName = targetEdge.NodeTo;
                }

                var onlyOutgoingFromSource = this.inputEdges.All(e => !e.NodeTo.Equals(sourceName));
                var onlyInToTarget = this.inputEdges.All(e => !e.NodeFrom.Equals(targetName));

                return this.IsVisualized && !this.playStepsDispatcherTimer.IsEnabled && inputEdgesContainsSource
                       && inputEdgesContainsTarget && onlyOutgoingFromSource && onlyInToTarget;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// The can execute pause steps command.
        /// </summary>
        /// <returns>
        /// True, if can execute pause.
        /// </returns>
        private bool CanExecutePauseStepsCommand()
        {
            return this.IsVisualized && this.IsCalculated && this.playStepsDispatcherTimer.IsEnabled;
        }

        /// <summary>
        /// The execute pause steps.
        /// </summary>
        private void ExecutePauseSteps()
        {
            this.playStepsDispatcherTimer.Stop();
        }

        /// <summary>
        /// The can execute play steps command.
        /// </summary>
        /// <returns>
        /// True, if can execute play.
        /// </returns>
        private bool CanExecutePlayStepsCommand()
        {
            return this.IsVisualized && this.IsCalculated && !this.playStepsDispatcherTimer.IsEnabled
                   && this.currentStep < this.graphSteps.LastStep;
        }

        /// <summary>
        /// The execute play steps.
        /// </summary>
        private void ExecutePlaySteps()
        {
            this.playStepsDispatcherTimer.Start();
        }

        /// <summary>
        /// The play steps dispatcher timer on tick.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        private void PlayStepsDispatcherTimerOnTick(object sender, EventArgs eventArgs)
        {
            if (this.currentStep < this.graphSteps.LastStep)
            {
                this.currentStep++;
                this.RaiseFlowAndResidualGraphChanged(this);
            }
            else
            {
                this.playStepsDispatcherTimer.Stop();
                this.PauseStepsCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// The can execute last step command.
        /// </summary>
        /// <returns>
        /// True, if can execute last step.
        /// </returns>
        private bool CanExecuteLastStepCommand()
        {
            return this.IsCalculated && this.IsVisualized && this.currentStep < this.graphSteps.LastStep
                   && !this.playStepsDispatcherTimer.IsEnabled;
        }

        /// <summary>
        /// The execute visualize last graph step.
        /// </summary>
        private void ExecuteVisualizeLastGraphStep()
        {
            this.currentStep = this.graphSteps.LastStep;
            this.RaiseFlowAndResidualGraphChanged(this);
        }

        /// <summary>
        /// The can execute first step command.
        /// </summary>
        /// <returns>
        /// True, if can execute frist step.
        /// </returns>
        private bool CanExecuteFirstStepCommand()
        {
            return this.IsCalculated && this.IsVisualized && this.currentStep > this.graphSteps.FirstStep
                   && !this.playStepsDispatcherTimer.IsEnabled;
        }

        /// <summary>
        /// The execute visualize first graph step.
        /// </summary>
        private void ExecuteVisualizeFirstGraphStep()
        {
            this.currentStep = this.graphSteps.FirstStep;
            this.RaiseFlowAndResidualGraphChanged(this);
        }

        /// <summary>
        /// The can execute step forward command.
        /// </summary>
        /// <returns>
        /// True, if can execute forward.
        /// </returns>
        private bool CanExecuteStepForwardCommand()
        {
            return this.IsCalculated && this.IsVisualized && this.currentStep < this.graphSteps.LastStep
                   && !this.playStepsDispatcherTimer.IsEnabled;
        }

        /// <summary>
        /// The can execute step backward command.
        /// </summary>
        /// <returns>
        /// True, if can execute backward.
        /// </returns>
        private bool CanExecuteStepBackwardCommand()
        {
            return this.IsCalculated && this.IsVisualized && this.currentStep > this.graphSteps.FirstStep
                   && !this.playStepsDispatcherTimer.IsEnabled;
        }

        /// <summary>
        /// The execute visualize previous graph step.
        /// </summary>
        private void ExecuteVisualizePreviousGraphStep()
        {
            this.currentStep--;
            this.RaiseFlowAndResidualGraphChanged(this);
        }

        /// <summary>
        /// The raise flow and residual graph changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        private void RaiseFlowAndResidualGraphChanged(object sender)
        {
            this.RaiseFlowGraphChanged(sender, this.graphSteps[this.currentStep].FlowGraph);
            this.RaiseResidualGraphChanged(sender, this.graphSteps[this.currentStep].ResidualGraph);
        }

        /// <summary>
        /// The execute show graph history.
        /// </summary>
        private void ExecuteShowGraphHistory()
        {
            var view = new HistoryView { GraphHistory = this.graphSteps };

            view.Show();
        }

        /// <summary>
        /// Executes the visualize next graph step.
        /// </summary>
        private void ExecuteVisualizeNextGraphStep()
        {
            this.currentStep++;
            this.RaiseFlowAndResidualGraphChanged(this);
        }

        /// <summary>
        /// Clears the graph.
        /// </summary>
        private void ExecuteClearGraph()
        {
            this.InputEdges.Clear();
            this.IsVisualized = false;
        }

        /// <summary>
        /// The execute calculate graph.
        /// </summary>
        private void ExecuteCalculateGraph()
        {
            const string Source = "s";
            const string Target = "t";
            var fordFulkerson = new FordFulkerson(this.visualizedGraph, Source, Target);

            this.graphSteps = fordFulkerson.RunAlgorithm();
            this.IsCalculated = true;
            this.MaxFlow = fordFulkerson.MaxFlow;
            this.MinCut = fordFulkerson.MinCut;

            this.currentStep = 0;
            this.RaiseFlowAndResidualGraphChanged(this);
        }

        /// <summary>
        /// The initialize graph.
        /// </summary>
        private void InitializeGraph()
        {
            this.InputEdges = new ObservableCollection<InputEdge>();
            this.InputEdges.CollectionChanged += (sender, e) =>
                {
                    Application.Current.Dispatcher.Invoke(
                        () =>
                        {
                            this.IsVisualized = false;
                            this.IsCalculated = false;
                        });
                };

            // INITIAL-TEST-GRAPH
            this.InputEdges.Add(new InputEdge("s", "2", 10));
            this.InputEdges.Add(new InputEdge("s", "3", 5));
            this.InputEdges.Add(new InputEdge("s", "4", 15));

            this.InputEdges.Add(new InputEdge("2", "3", 4));
            this.InputEdges.Add(new InputEdge("2", "5", 9));
            this.InputEdges.Add(new InputEdge("2", "6", 15));

            this.InputEdges.Add(new InputEdge("3", "6", 8));
            this.InputEdges.Add(new InputEdge("3", "4", 4));

            this.InputEdges.Add(new InputEdge("4", "7", 30));

            this.InputEdges.Add(new InputEdge("5", "6", 15));
            this.InputEdges.Add(new InputEdge("5", "t", 10));

            this.InputEdges.Add(new InputEdge("6", "7", 15));
            this.InputEdges.Add(new InputEdge("6", "t", 10));

            this.InputEdges.Add(new InputEdge("7", "3", 6));
            this.InputEdges.Add(new InputEdge("7", "t", 10));
        }

        /// <summary>
        /// The can execute visualize command.
        /// </summary>
        /// <returns>
        /// True, if can execute visualize.
        /// </returns>
        private bool CanExecuteVisualizeCommand()
        {
            var allEdgesHaveNodes =
                this.InputEdges.All(edge => !string.IsNullOrEmpty(edge.NodeFrom) && !string.IsNullOrEmpty(edge.NodeTo));
            return this.InputEdges.Count > 0 && allEdgesHaveNodes && !this.playStepsDispatcherTimer.IsEnabled;
        }

        /// <summary>
        /// The execute visualize flow graph.
        /// </summary>
        private void ExecuteVisualizeGraph()
        {
            this.visualizedGraph = this.ConvertInputEdgesToGraph(this.InputEdges);
            this.RaiseFlowGraphChanged(this, this.visualizedGraph);

            this.IsVisualized = true;

            this.MinCut = 0;
            this.MaxFlow = 0;
        }

        /// <summary>
        /// The raise flow graph changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="graph">
        /// The graph.
        /// </param>
        private void RaiseFlowGraphChanged(object sender, Graph graph)
        {
            if (this.FlowGraphChanged != null)
            {
                Application.Current.Dispatcher.Invoke(() => { this.FlowGraphChanged(sender, graph); });
            }
        }

        /// <summary>
        /// The raise residual graph changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="graph">
        /// The graph.
        /// </param>
        private void RaiseResidualGraphChanged(object sender, Graph graph)
        {
            if (this.ResidualGraphChanged != null)
            {
                Application.Current.Dispatcher.Invoke(() => { this.ResidualGraphChanged(sender, graph); });
            }
        }
    }
}