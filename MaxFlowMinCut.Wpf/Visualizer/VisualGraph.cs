// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VisualGraph.cs" company="FH Wr. Neustadt">
//   Christoph Hauer / Markus Zytek. All rights reserved.
// </copyright>
// <summary>
//   The visual graph.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MaxFlowMinCut.Wpf.Visualizer
{
    using System.Linq;

    using Microsoft.Msagl.Drawing;

    using Graph = MaxFlowMinCut.Lib.Graph;

    /// <summary>
    /// The visual graph.
    /// </summary>
    public class VisualGraph
    {
        /// <summary>
        /// The lib graph.
        /// </summary>
        private readonly Graph libGraph;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisualGraph"/> class.
        /// </summary>
        /// <param name="libGraph">
        /// The lib graph.
        /// </param>
        public VisualGraph(Graph libGraph)
        {
            this.libGraph = libGraph;
        }

        /// <summary>
        /// The create flow graph.
        /// </summary>
        /// <returns>
        /// The <see cref="Graph"/>.
        /// </returns>
        public Microsoft.Msagl.Drawing.Graph CreateFlowGraph()
        {
            var msaglGraph = this.CreateGraph();

            foreach (var edge in this.libGraph.Edges)
            {
                var addedge = msaglGraph.AddEdge(
                    edge.NodeFrom.Name, 
                    string.Format("{0}/{1}", edge.Flow, edge.Capacity), 
                    edge.NodeTo.Name);
                addedge.Attr.LineWidth = 1;

                if (edge.IsVisited)
                {
                    addedge.Attr.Color = Color.Blue;
                    if (edge.IsFullUsed)
                    {
                        addedge.Attr.LineWidth = 3;
                    }
                }

                if (edge.IsMinCut)
                {
                    addedge.Attr.Color = Color.Red;
                    addedge.Attr.AddStyle(Style.Dashed);
                    addedge.SourceNode.Attr.Color = Color.Red;
                }

                addedge.SourceNode.Attr.Shape = Shape.Circle;
                addedge.TargetNode.Attr.Shape = Shape.Circle;
            }

            return msaglGraph;
        }

        /// <summary>
        /// The create residual graph.
        /// </summary>
        /// <returns>
        /// The <see cref="Graph"/>.
        /// </returns>
        public Microsoft.Msagl.Drawing.Graph CreateResidualGraph()
        {
            var msaglGraph = this.CreateGraph();

            foreach (var edge in this.libGraph.Edges.Where(e => e.Capacity > 0))
            {
                var addedge = msaglGraph.AddEdge(
                    edge.NodeFrom.Name, 
                    string.Format("{0}", edge.Capacity), 
                    edge.NodeTo.Name);

                addedge.Attr.Color = edge.IsPathMarked ? Color.Orange : Color.Black;

                addedge.SourceNode.Attr.Shape = Shape.Circle;
                addedge.TargetNode.Attr.Shape = Shape.Circle;
            }

            return msaglGraph;
        }

        /// <summary>
        /// The create graph.
        /// </summary>
        /// <returns>
        /// The <see cref="Graph"/>.
        /// </returns>
        private Microsoft.Msagl.Drawing.Graph CreateGraph()
        {
            var msaglGraph = new Microsoft.Msagl.Drawing.Graph();
            msaglGraph.Attr.OptimizeLabelPositions = true;
            msaglGraph.Attr.LayerDirection = LayerDirection.LR;

            return msaglGraph;
        }
    }
}