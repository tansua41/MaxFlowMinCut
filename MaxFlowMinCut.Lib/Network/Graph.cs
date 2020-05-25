// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Graph.cs" company="FH Wr. Neustadt">
//   Christoph Hauer / Markus Zytek. All rights reserved.
// </copyright>
// <summary>
//   The graph.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MaxFlowMinCut.Lib
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    using MaxFlowMinCut.Lib.Network;

    /// <summary>
    /// The graph.
    /// </summary>
    [Serializable]
    public class Graph : ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Graph"/> class.
        /// </summary>
        public Graph()
        {
            this.Nodes = new List<Node>();
            this.Edges = new List<Edge>();
        }

        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        public List<Node> Nodes { get; set; }

        /// <summary>
        /// Gets or sets the edges.
        /// </summary>
        /// <value>
        /// The edges.
        /// </value>
        public List<Edge> Edges { get; set; }

        /// <summary>
        /// The clone.
        /// </summary>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object Clone()
        {
            var binaryFormatter = new BinaryFormatter();
            var memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, this);
            memoryStream.Seek(0, SeekOrigin.Begin);
            var graph = (Graph)binaryFormatter.Deserialize(memoryStream);

            return graph;
        }
    }
}