// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyGraphWrapper.xaml.cs" company="FH Wr. Neustadt">
//   Christoph Hauer / Markus Zytek. All rights reserved.
// </copyright>
// <summary>
//   The dependency graph wrapper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MaxFlowMinCut.Wpf.UserControl
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    using MaxFlowMinCut.Lib;
    using MaxFlowMinCut.Wpf.Visualizer;

    using Microsoft.Msagl.GraphViewerGdi;

    /// <summary>
    /// The dependency graph wrapper.
    /// </summary>
    public partial class DependencyGraphWrapper : UserControl
    {
        /// <summary>
        /// The flow graph property.
        /// </summary>
        public static readonly DependencyProperty FlowGraphProperty = DependencyProperty.Register(
            "FlowGraph", 
            typeof(Graph), 
            typeof(DependencyGraphWrapper), 
            new PropertyMetadata());

        /// Using a DependencyProperty as the backing store for Viewer.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The residual graph property.
        /// </summary>
        public static readonly DependencyProperty ResidualGraphProperty = DependencyProperty.Register(
            "ResidualGraph", 
            typeof(Graph), 
            typeof(DependencyGraphWrapper), 
            new PropertyMetadata());

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyGraphWrapper"/> class.
        /// </summary>
        public DependencyGraphWrapper()
        {
            this.InitializeComponent();

            DependencyPropertyDescriptor.FromProperty(FlowGraphProperty, typeof(DependencyGraphWrapper))
                .AddValueChanged(
                    this, 
                    (s, e) =>
                        {
                            if (this.Viewer != null && this.Viewer.Child != null && this.Viewer.Child is GViewer)
                            {
                                Application.Current.Dispatcher.Invoke(
                                    () =>
                                        {
                                            (this.Viewer.Child as GViewer).Graph =
                                                new VisualGraph(this.FlowGraph).CreateFlowGraph();
                                        });
                            }
                        });

            DependencyPropertyDescriptor.FromProperty(ResidualGraphProperty, typeof(DependencyGraphWrapper))
                .AddValueChanged(
                    this, 
                    (s, e) =>
                        {
                            if (this.Viewer != null && this.Viewer.Child != null && this.Viewer.Child is GViewer)
                            {
                                Application.Current.Dispatcher.Invoke(
                                    () =>
                                        {
                                            (this.Viewer.Child as GViewer).Graph =
                                                new VisualGraph(this.ResidualGraph).CreateFlowGraph();
                                        });
                            }
                        });
        }

        /// <summary>
        /// Gets or sets the graph.
        /// </summary>
        /// <value>
        /// The graph.
        /// </value>
        public Graph FlowGraph
        {
            get
            {
                return (Graph)this.GetValue(FlowGraphProperty);
            }

            set
            {
                this.SetValue(FlowGraphProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the residual graph.
        /// </summary>
        /// <value>
        /// The residual graph.
        /// </value>
        public Graph ResidualGraph
        {
            get
            {
                return (Graph)this.GetValue(ResidualGraphProperty);
            }

            set
            {
                this.SetValue(ResidualGraphProperty, value);
            }
        }
    }
}