// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphHistoryStep.cs" company="FH Wr. Neustadt">
//   Christoph Hauer / Markus Zytek. All rights reserved.
// </copyright>
// <summary>
//   The graph history step.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MaxFlowMinCut.Lib.History
{
    /// <summary>
    /// The graph history step.
    /// </summary>
    public class GraphHistoryStep
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphHistoryStep"/> class.
        /// </summary>
        /// <param name="flowGraph">
        /// The flow graph.
        /// </param>
        public GraphHistoryStep(Graph flowGraph)
            : this(flowGraph, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphHistoryStep"/> class.
        /// </summary>
        /// <param name="flowGraph">
        /// The flow graph.
        /// </param>
        /// <param name="residualGraph">
        /// The residual graph.
        /// </param>
        public GraphHistoryStep(Graph flowGraph, Graph residualGraph)
        {
            this.FlowGraph = flowGraph;
            this.ResidualGraph = residualGraph;
        }

        /// <summary>
        /// Gets the flow graph.
        /// </summary>
        /// <value>
        /// The flow graph.
        /// </value>
        public Graph FlowGraph { get; private set; }

        /// <summary>
        /// Gets the residual graph.
        /// </summary>
        /// <value>
        /// The residual graph.
        /// </value>
        public Graph ResidualGraph { get; private set; }
    }
}