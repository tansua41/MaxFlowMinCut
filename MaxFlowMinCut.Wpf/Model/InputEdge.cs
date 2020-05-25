// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputEdge.cs" company="FH Wr. Neustadt">
//   Christoph Hauer / Markus Zytek. All rights reserved.
// </copyright>
// <summary>
//   The input edge.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MaxFlowMinCut.Wpf.Model
{
    using System;

    /// <summary>
    /// The input edge.
    /// </summary>
    public class InputEdge
    {
        /// <summary>
        /// The capacity.
        /// </summary>
        private int capacity;

        /// <summary>
        /// The node from.
        /// </summary>
        private string nodeFrom;

        /// <summary>
        /// The node to.
        /// </summary>
        private string nodeTo;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputEdge"/> class.
        /// </summary>
        public InputEdge()
        {
            this.Capacity = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputEdge"/> class.
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
        public InputEdge(string nodeFrom, string nodeTo, int capacity)
        {
            this.NodeFrom = nodeFrom;
            this.NodeTo = nodeTo;
            this.Capacity = capacity;
        }

        /// <summary>
        /// Gets or sets the node from.
        /// </summary>
        /// <value>
        /// The node from.
        /// </value>
        public string NodeFrom
        {
            get
            {
                return this.nodeFrom;
            }

            set
            {
                this.nodeFrom = value.ToLower();
            }
        }

        /// <summary>
        /// Gets or sets the node to.
        /// </summary>
        /// <value>
        /// The node to.
        /// </value>
        public string NodeTo
        {
            get
            {
                return this.nodeTo;
            }

            set
            {
                this.nodeTo = value.ToLower();
            }
        }

        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        /// <value>
        /// The capacity.
        /// </value>
        public int Capacity
        {
            get
            {
                return this.capacity;
            }

            set
            {
                this.capacity = Math.Max(value, 1);
            }
        }
    }
}