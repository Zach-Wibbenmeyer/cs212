using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace prog4
{
    /// <summary>
    /// Represents a directed labeled graph with a string name at each node
    /// and a string label for each edge.
    /// </summary>
    class RelationshipGraph
    {
        /*
         *  This data structure contains a list of nodes (each of which has
         *  an adjacency list) and a dictionary (hash table) for efficiently 
         *  finding nodes by name
         */
        public List<GraphNode> nodes { get; private set; }
        private Dictionary<String, GraphNode> nodeDict;

        // constructor builds empty relationship graph
        public RelationshipGraph()
        {
            nodes = new List<GraphNode>();
            nodeDict = new Dictionary<String, GraphNode>();
        }

        // AddNode creates and adds a new node if there isn't already one by that name
        public void AddNode(string name)
        {
            if (!nodeDict.ContainsKey(name))
            {
                GraphNode n = new GraphNode(name);
                nodes.Add(n);
                nodeDict.Add(name, n);
            }
        }

        // AddEdge adds the edge, creating endpoint nodes if necessary.
        // Edge is added to adjacency list of from edges.
        public void AddEdge(string name1, string name2, string relationship)
        {
            AddNode(name1);                     // create the node if it doesn't already exist
            GraphNode n1 = nodeDict[name1];     // now fetch a reference to the node
            AddNode(name2);
            GraphNode n2 = nodeDict[name2];
            GraphEdge e = new GraphEdge(n1, n2, relationship);
            n1.AddIncidentEdge(e);
        }

        // Get a node by name using dictionary
        public GraphNode GetNode(string name)
        {
            if (nodeDict.ContainsKey(name))
                return nodeDict[name];
            else
                return null;
        }

        // Print every name from the graph
        public void Dump()
        {
            foreach (var n in nodes)
            {
                Console.Write(n.ToString());
            }
        }

        // prints the orphans
        public void PrintOrphans()
        {
            Console.WriteLine("Orphans:");
            foreach (var n in nodes)
            {
                if (n.GetEdges("parent").Count == 0)
                    Console.WriteLine("\t" + n.Name());
            }
        }

        /*
         * FindShortestPath() - does a breadth first search to compute the shortest path
         * @param: F (type -> String), T (type -> String)
         */
        public List<GraphNode> FindShortestPath(String F, String T)
        {
            // do a breadth first search to find the shortest path.
            var path = new List<GraphNode>();
            var levels = new List<List<GraphNode>>();
            var level = new List<GraphNode>();
            var seen = new Dictionary<String, Boolean>();
            var levelCount = 0;
            var from = GetNode(F);
            var to = GetNode(T);

            if (from == null || to == null || from == to)
                return null;

            level.Add(from);
            levels.Add(level);
            while (true)
            {
                level = new List<GraphNode>();
                foreach (var n in levels[levels.Count - 1])
                {
                    foreach (var e in n.GetEdges())
                    {
                        if (e.To() == T) 
                            goto EndBuild;
                        if (seen.ContainsKey(e.To())) continue;
                        level.Add(e.ToNode());
                        seen.Add(e.To(), true);
                    }
                }
                levels.Add(level);
            }
        EndBuild:
            path.Add(to);
            levelCount = levels.Count - 1;
            try
            {
                while (path[path.Count - 1] != from && levelCount >= 0)
                {

                    
                    foreach (var e in path[path.Count - 1].GetEdges())
                    {
                        if (!levels[levelCount].Contains(e.ToNode())) continue;
                        path.Add(e.ToNode());
                        goto NextInWhile;
                    }
                    
                    foreach (var n in levels[levelCount])
                    {
                        foreach (var e in n.GetEdges())
                        {
                            if (e.ToNode() != path[path.Count - 1]) continue;
                            path.Add(n);
                            goto NextInWhile;
                        }
                    }
                NextInWhile:
                    levelCount--;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            path.Reverse();
            if (path.Count == 1)
                path = null;
            return path;
        }

        /*
         * FindDescendants() - computes descendents of a person
         * @param: name (type -> String)
         * @returns: descendents of a person
         */
        public void FindDescendants(String name)
        {
            GraphNode root = GetNode(name);
            var thisGeneration = new List<GraphNode>();
            var nextGeneration = new List<GraphNode>();
            thisGeneration.Add(root);
            Console.WriteLine("Descendant of " + root.Name() + ":");
            int generation = 0;
            var strings = new List<String>();

            while (thisGeneration.Count > 0)
            {


                if (generation == 1)
                    Console.Write("Children: ");
                else if (generation > 1)
                {
                    for (int i = generation; i > 2; i--)
                    {
                        Console.Write("Great ");
                    }
                    Console.Write("Grandchildren: ");
                }
                if (generation > 0)
                    Console.WriteLine(String.Join(", ", (String[])strings.ToArray()));

                generation++;

                strings = new List<string>();

                foreach (var n in thisGeneration)
                {
                    foreach (var e in n.GetEdges("child"))
                    {
                        nextGeneration.Add(e.ToNode());
                        strings.Add(e.To());
                    }
                }

                thisGeneration = nextGeneration;
                nextGeneration = new List<GraphNode>();
            }

        }

        /*FindCousins() - computes the FindCousins for a person
         * @param: person (type -> String), n (type -> int), k (type -> int)
         * @return: FindCousins 
         */
        public void FindCousins(String person, int n, int k)
        {
            n += 1;
            Dictionary<String, bool> seen = new Dictionary<String, bool>();
            Dictionary<GraphNode, bool> topLevel = new Dictionary<GraphNode, bool>();
            Dictionary<GraphNode, bool> endLevel = new Dictionary<GraphNode, bool>();
            if (GetNode(person) == null)
            {
                Console.WriteLine("No such person: " + person);
                return;
            }
            topLevel.Add(GetNode(person), true);
            seen.Add(person, true);
            for (var i = 0; i < n + k; i++)
            {// go up n generations
                var level = new Dictionary<GraphNode, bool>();
                foreach (GraphNode gn in topLevel.Keys)
                {
                    foreach (GraphEdge e in gn.GetEdges("parent"))
                    {
                        if (!level.ContainsKey(e.ToNode()))
                            level.Add(e.ToNode(), true);
                        if (i < n - 1)
                            seen.Add(e.To(), true);
                    }
                }
                if (i == n - 1 && k != 0)
                    endLevel = level;
                topLevel = level;
            }

            for (var i = 0; i < n; i++)
            {
                var level = new Dictionary<GraphNode, bool>();
                foreach (GraphNode gn in topLevel.Keys)
                {
                    if (seen.ContainsKey(gn.Name()))
                        continue;
                    if (!seen.ContainsKey(gn.Name()))
                        seen.Add(gn.Name(), true);
                    foreach (GraphEdge e in gn.GetEdges("child"))
                    {
                        if (!level.ContainsKey(e.ToNode()))
                            level.Add(e.ToNode(), true);
                    }
                }
                topLevel = level;
            }
            for (var i = 0; i < n + k; i++)
            {
                Dictionary<GraphNode, bool> level = new Dictionary<GraphNode, bool>();
                foreach (GraphNode gn in endLevel.Keys)
                {
                    if (seen.ContainsKey(gn.Name()))
                        continue;
                    if (!seen.ContainsKey(gn.Name()))
                        seen.Add(gn.Name(), true);
                    foreach (GraphEdge e in gn.GetEdges("child"))
                    {
                        if (!level.ContainsKey(e.ToNode()))
                            level.Add(e.ToNode(), true);
                    }
                }
                endLevel = level;
            }


            List<String> printArray = new List<String>();
            foreach (GraphNode gn in topLevel.Keys)
                if (!seen.ContainsKey(gn.Name()))
                    printArray.Add(gn.Name());
            foreach (GraphNode gn in endLevel.Keys)
                if (!seen.ContainsKey(gn.Name()))
                    printArray.Add(gn.Name());


            Console.WriteLine(String.Join(", ", (String[])printArray.ToArray()));
        }
    }
}
