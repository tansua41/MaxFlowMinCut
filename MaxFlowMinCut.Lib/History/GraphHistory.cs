// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphHistory.cs" company="FH Wr. Neustadt">
//   Christoph Hauer / Markus Zytek. All rights reserved.
// </copyright>
// <summary>
//   The graph history.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MaxFlowMinCut.Lib.History
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// The graph history.
    /// </summary>
    public class GraphHistory : IEnumerable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphHistory"/> class.
        /// </summary>
        public GraphHistory()
        {
            this.Steps = new List<GraphHistoryStep>();
        }

        /// <summary>
        /// Gets the steps.
        /// </summary>
        /// <value>
        /// The steps.
        /// </value>
        public List<GraphHistoryStep> Steps { get; private set; }

        /// <summary>
        /// Gets the last step.
        /// </summary>
        /// <value>
        /// The last step.
        /// </value>
        public int LastStep
        {
            get
            {
                return this.Steps.Count - 1;
            }
        }

        /// <summary>
        /// Gets the first step.
        /// </summary>
        /// <value>
        /// The first step.
        /// </value>
        public int FirstStep
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the <see cref="Graph"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="Graph"/>.
        /// </value>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The <see cref="GraphHistoryStep"/>.
        /// </returns>
        public GraphHistoryStep this[int index]
        {
            get
            {
                return this.Steps[index];
            }
        }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return this.Steps.GetEnumerator();
        }

        /// <summary>
        /// Adds the graph step.
        /// </summary>
        /// <param name="flowGraph">
        /// The flow graph.
        /// </param>
        public void AddGraphStep(Graph flowGraph)
        {
            this.AddGraphStep(flowGraph, null);
        }

        /// <summary>
        /// Adds the graph step.
        /// </summary>
        /// <param name="flowGraph">
        /// The flow graph.
        /// </param>
        /// <param name="residualGraph">
        /// The residual graph.
        /// </param>
        public void AddGraphStep(Graph flowGraph, Graph residualGraph)
        {
            if (residualGraph != null)
            {
                this.Steps.Add(new GraphHistoryStep((Graph)flowGraph.Clone(), (Graph)residualGraph.Clone()));
            }
            else
            {
                this.Steps.Add(new GraphHistoryStep((Graph)flowGraph.Clone()));
            }
        }
    }
}