// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistoryView.xaml.cs" company="FH Wr. Neustadt">
//   Christoph Hauer / Markus Zytek. All rights reserved.
// </copyright>
// <summary>
//   Interaktionslogik für HistoryView.xaml.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MaxFlowMinCut.Wpf.View
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Forms.Integration;

    using MaxFlowMinCut.Lib.History;
    using MaxFlowMinCut.Wpf.Visualizer;

    using Microsoft.Msagl.Drawing;
    using Microsoft.Msagl.GraphViewerGdi;

    /// <summary>
    /// The history view.
    /// </summary>
    public partial class HistoryView : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryView"/> class.
        /// </summary>
        public HistoryView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Sets the graph history.
        /// </summary>
        /// <value>
        /// The graph history.
        /// </value>
        public GraphHistory GraphHistory
        {
            set
            {
                Application.Current.Dispatcher.Invoke(() => { this.CreateGraphHistoryStepsUserControls(value); });
            }
        }

        /// <summary>
        /// Creates the graph history steps user controls.
        /// </summary>
        /// <param name="history">
        /// The history.
        /// </param>
        private void CreateGraphHistoryStepsUserControls(GraphHistory history)
        {
            foreach (GraphHistoryStep step in history)
            {
                var stepGrid = new Grid();
                stepGrid.ColumnDefinitions.Add(new ColumnDefinition());
                stepGrid.ColumnDefinitions.Add(new ColumnDefinition());

                var viewerFlow = this.CreateGraphViewer(new VisualGraph(step.FlowGraph).CreateFlowGraph());
                stepGrid.Children.Add(viewerFlow);
                viewerFlow.SetValue(Grid.ColumnProperty, 0);

                if (step.ResidualGraph != null)
                {
                    var viewerResidual =
                        this.CreateGraphViewer(new VisualGraph(step.ResidualGraph).CreateResidualGraph());
                    stepGrid.Children.Add(viewerResidual);
                    viewerResidual.SetValue(Grid.ColumnProperty, 1);
                }

                this.StepsStackPanel.Children.Add(stepGrid);
            }
        }

        /// <summary>
        /// Creates the graph viewer.
        /// </summary>
        /// <param name="graph">
        /// The graph.
        /// </param>
        /// <returns>
        /// The created Graph Viewer host.
        /// </returns>
        private UIElement CreateGraphViewer(Graph graph)
        {
            var graphViewer = new GViewer();
            graphViewer.ToolBarIsVisible = false;
            var host = new WindowsFormsHost();
            host.Child = graphViewer;
            host.Height = 300;
            graphViewer.Graph = graph;

            return host;
        }
    }
}