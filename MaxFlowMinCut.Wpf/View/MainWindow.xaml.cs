// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="FH Wr. Neustadt">
//   Christoph Hauer / Markus Zytek. All rights reserved.
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MaxFlowMinCut.Wpf.View
{
    using System;
    using System.Windows;

    using MaxFlowMinCut.Lib;
    using MaxFlowMinCut.Wpf.ViewModel;
    using MaxFlowMinCut.Wpf.Visualizer;

    /// <summary>
    /// Interaction logic for MainWindow.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The view model.
        /// </summary>
        private readonly MainWindowViewModel viewModel;

        /// <summary>
        /// The visual flow graph.
        /// </summary>
        private VisualGraph visualFlowGraph;

        /// <summary>
        /// The visual residual graph.
        /// </summary>
        private VisualGraph visualResidualGraph;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            this.viewModel = (MainWindowViewModel)this.DataContext;
            this.viewModel.FlowGraphChanged += this.OnFlowGraphChanged;
            this.viewModel.ResidualGraphChanged += this.OnResidualGraphChanged;
            this.viewModel.ResetGraphLayout += this.OnResetGraphLayout;
        }

        /// <summary>
        /// The on flow graph changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="libGraph">
        /// The lib graph.
        /// </param>
        private void OnFlowGraphChanged(object sender, Graph libGraph)
        {
            // var oldGraph = this.GViewerFlow.Graph;

            ////oldGraph.FindNode("a").Attr.Pos;
            this.visualFlowGraph = new VisualGraph(libGraph);
            var msaglGraph = this.visualFlowGraph.CreateFlowGraph();

            // this.GViewerFlow.NeedToCalculateLayout = true;
            this.GViewerFlow.Graph = msaglGraph;

            // this.GViewerFlow.NeedToCalculateLayout = false;
        }

        /// <summary>
        /// The on residual graph changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="libGraph">
        /// The lib graph.
        /// </param>
        private void OnResidualGraphChanged(object sender, Graph libGraph)
        {
            if (libGraph != null)
            {
                this.visualResidualGraph = new VisualGraph(libGraph);
                var msaglGraph = this.visualResidualGraph.CreateResidualGraph();
                this.GViewerResidual.Graph = msaglGraph;
            }
            else
            {
                this.visualResidualGraph = null;
                this.GViewerResidual.Graph = null;
            }
        }

        /// <summary>
        /// Called when a reset layout is requested.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void OnResetGraphLayout(object sender, EventArgs e)
        {
            if (this.visualFlowGraph != null)
            {
                this.GViewerFlow.Graph = this.visualFlowGraph.CreateFlowGraph();
            }

            if (this.visualResidualGraph != null)
            {
                this.GViewerResidual.Graph = this.visualResidualGraph.CreateResidualGraph();
            }
        }
    }
}