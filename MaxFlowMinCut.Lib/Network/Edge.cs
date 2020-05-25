// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Edge.cs" company="FH Wr. Neustadt">
//   Christoph Hauer / Markus Zytek. All rights reserved.
// </copyright>
// <summary>
//   The edge.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MaxFlowMinCut.Lib.Network
{
    using System;

    /// <summary>
    /// The graph edge.
    /// </summary>
    [Serializable]
    public class Edge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Edge"/> class.
        /// </summary>
        /// <param name="nodeFrom">
        /// The node from.
        /// </param>
        /// <param name="nodeTo">
        /// The node to.
        /// </param>
        /// <param name="capacity">
        /// The capacity.
        /// </param>
        public Edge(Node nodeFrom, Node nodeTo, int capacity)
        {
            this.ID = Guid.NewGuid();
            this.NodeFrom = nodeFrom;
            this.NodeTo = nodeTo;
            this.Capacity = capacity;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id of the edge.
        /// </value>
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the node from.
        /// </summary>
        /// <value>
        /// The node from.
        /// </value>
        public Node NodeFrom { get; set; }

        /// <summary>
        /// Gets or sets the node to.
        /// </summary>
        /// <value>
        /// The node to.
        /// </value>
        public Node NodeTo { get; set; }

        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        /// <value>
        /// The capacity.
        /// </value>
        public int Capacity { get; set; }

        /// <summary>
        /// Gets or sets the flow.
        /// </summary>
        /// <value>
        /// The flow of the edge.
        /// </value>
        public int Flow { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is path marked.
        /// </summary>
        /// <value>
        /// The is path marked.
        /// </value>
        public bool IsPathMarked { get; set; }

        /// <summary>
        /// Gets a value indicating whether is visited.
        /// </summary>
        /// <value>
        /// The is visited.
        /// </value>
        public bool IsVisited
        {
            get
            {
                return this.Flow > 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is full used.
        /// </summary>
        /// <value>
        /// The is full used.
        /// </value>
        public bool IsFullUsed
        {
            get
            {
                return this.Flow == this.Capacity;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is minimum cut.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is minimum cut; otherwise, <c>false</c>.
        /// </value>
        public bool IsMinCut { get; set; }

        /// <summary>
        /// Equals the specified compare edge.
        /// </summary>
        /// <param name="compareEdge">
        /// The compare edge.
        /// </param>
        /// <returns>
        /// The equality of the compared edges.
        /// </returns>
        public bool Equals(Edge compareEdge)
        {
            return this.ID.Equals(compareEdge.ID);
        }
    }
}