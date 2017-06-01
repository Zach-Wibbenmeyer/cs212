﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace prog4
{
    /// <summary>
    /// Represents a labeled, directed edge in a RelationshipGraph
    /// </summary>
    class GraphEdge
    {
        public string label { get; private set; }
        private GraphNode fromNode, toNode;

        // constructor
        public GraphEdge(GraphNode from, GraphNode to, string myLabel)
        {
            fromNode = from;
            toNode = to;
            label = myLabel;
        }

        // return label of edge
        public string Label()
        {
            return label;
        }


        // return the name of the "to" person in the relationship
        public string To()
        {
            return toNode.Name();
        }

        public GraphNode ToNode()
        {
            return toNode;
        }


        public String From()
        {
            return fromNode.Name();
        }

        // return string form of edge
        public override string ToString()
        {
            string result = fromNode.Name() + " --(" + label + ")--> " + toNode.Name();
            return result;
        }
    }
}

