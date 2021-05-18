using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmTarjan
{
    class Program
    {
        enum State
        {
            White, 
            Grey,
            Black
        }

        class Node
        {
            public List<Edge> Edges = new List<Edge>();
            public int Number { get; set; }
            public Node(int num)
            {
                Number = num;
            }

            public bool IsIncident(Node node)
            {
                foreach(var edge in Edges)
                {
                    if (edge.IsIncident(node)) return true;
                }
                return false;
            }

            public IEnumerable<Node> GetIncidentsNodes()
            {
                foreach(var edge in Edges)
                {
                    yield return edge.GetToNode();
                }
            }
        }

        class Edge
        {
            public Node FromNode;
            public Node ToNode;

            public Edge(Node from, Node to)
            {
                FromNode = from;
                ToNode = to;
            }

            public bool IsIncident(Node node)
            {
                return FromNode == node || ToNode == node;
            }

            public Node GetToNode()
            {
                return ToNode;
            }
        }

        class Graph
        {
            public readonly List<Node> Nodes = new List<Node>();
        }

        static List<Node> TarjanAlg(Graph graph)
        {
            //Final result
            List<Node> result = new List<Node>();

            //Mark all the nodes in white colour
            var colour = new Dictionary<Node, State>();
            //foreach(var node in graph.Nodes)
            //{
            //    colour.Add(node, State.White);
            //}
            
            //Above could be written in one line
            colour = graph.Nodes.ToDictionary(node => node, node => State.White); //Key, value

            while (true)
            {
                 Node nodeWhite = colour.Where(z => z.Value == State.White).Select(z => z.Key).FirstOrDefault();
                if (nodeWhite == null) break; //There's no white nodes anymore.

                //If there's a loop inside a graph, it means that it cannot be sorted
                if (!TarjanSearch(colour, nodeWhite, result)) return null;

            }

            result.Reverse();
            return result;          

        }

        static bool TarjanSearch(Dictionary<Node, State> colour, Node node, List<Node> result)
        {
            //We found a loop and it's impossible to sort this graph topologically
            if (colour[node] == State.Grey) return false;

            if (colour[node] == State.Black) return true;

            //Visited node we mark as grey
            colour[node] = State.Grey;

            foreach (var ongoingNode in node.GetIncidentsNodes())
            {
                if (!TarjanSearch(colour, ongoingNode, result)) return false;              
            }

                 //There's no ongoingNodes from the current node
                colour[node] = State.Black;
                result.Add(node);
                return true;
            }

        static void Main(string[] args)
        {
            var n0 = new Node(0);
            var n1 = new Node(1);
            var n2 = new Node(2);
            var n3 = new Node(3);
            var n4 = new Node(4);

            var e01 = new Edge(n0, n1);
            var e13 = new Edge(n1, n3);
            var e02 = new Edge(n0, n2);
            var e34 = new Edge(n3, n4);
            var e12 = new Edge(n1, n2);
            var e23 = new Edge(n2, n3);
            var e24 = new Edge(n2, n4);

            n0.Edges.Add(e01);
            n0.Edges.Add(e02);
            n1.Edges.Add(e13);
            n1.Edges.Add(e12);
            n2.Edges.Add(e23);
            n2.Edges.Add(e24);
            n3.Edges.Add(e34);

            var graph = new Graph();
            graph.Nodes.Add(n0);
            graph.Nodes.Add(n1);
            graph.Nodes.Add(n2);
            graph.Nodes.Add(n3);
            graph.Nodes.Add(n4);


            List<Node> result = TarjanAlg(graph);

            foreach (var n in result)
                Console.WriteLine(n.Number);

        }
    }
}
