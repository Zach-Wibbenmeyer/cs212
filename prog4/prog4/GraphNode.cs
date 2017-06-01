using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace prog4
{
    /// <summary>
    /// Represents a node in a RelationshipGraph
    /// </summary>
    class GraphNode
    {
        public string name { get; private set; }

        public List<GraphEdge> incidentEdges { get; private set; }

        // constructor
        public GraphNode(string v)
        {
            name = v;
            incidentEdges = new List<GraphEdge>();
        }

        // Add an edge (but don't add duplicate edges)
        public void AddIncidentEdge(GraphEdge e)
        {
            foreach (GraphEdge edge in incidentEdges)
            {
                if (edge.ToString() == e.ToString())
                    return;
            }
            incidentEdges.Add(e);
        }

        // return a list of all outgoing edges
        public List<GraphEdge> GetEdges()
        {
            return incidentEdges;
        }

        // return a list of all nodes connected to this node
        public List<GraphNode> ToNodes()
        {
            List<GraphNode> ret = new List<GraphNode>();
            foreach (GraphEdge e in incidentEdges)
                ret.Add(e.ToNode());

            return ret;
        }

        // return a list of outgoing edges of specified label
        public List<GraphEdge> GetEdges(string label)
        {
            List<GraphEdge> list = new List<GraphEdge>();
            foreach (GraphEdge e in incidentEdges)
                if (e.label == label)
                    list.Add(e);
            return list;
        }

        // return name of node
        public string Name()
        {
            return name;
        }

        // return text form of node, including outgoing edges
        public override string ToString()
        {
            string result = name + "\n";
            foreach (GraphEdge e in incidentEdges)
            {
                result = result + "  " + e.ToString() + "\n";
            }
            return result;
        }
    }
}
