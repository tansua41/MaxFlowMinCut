// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Node.cs" company="FH Wr. Neustadt">
//   Christoph Hauer / Markus Zytek. All rights reserved.
// </copyright>
// <summary>
//   The graph node.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MaxFlowMinCut.Lib.Network
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The graph node.
    /// </summary>
    [Serializable]
    public class Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="name">
        /// The node name.
        /// </param>
        public Node(string name)
        {
            this.ID = Guid.NewGuid();
            this.Name = name;
            this.Edges = new List<Edge>();
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The node id.
        /// </value>
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the edges.
        /// </summary>
        /// <value>
        /// The edges.
        /// </value>
        public List<Edge> Edges { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The node name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        /// <value>
        /// The parent node.
        /// </value>
        public Node ParentNode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is minimum cut.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is minimum cut; otherwise, <c>false</c>.
        /// </value>
        public bool IsMinCut { get; set; }

        /// <summary>
        /// Equals the specified compare node.
        /// </summary>
        /// <param name="compareNode">
        /// The compare node.
        /// </param>
        /// <returns>
        /// The equality of the compared nodes.
        /// </returns>
        public bool Equals(Node compareNode)
        {
            return this.ID.Equals(compareNode.ID);
        }
    }
}