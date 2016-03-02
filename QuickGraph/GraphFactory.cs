using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ORM.QuickGraph.Models;
using QuickGraph.Algorithms;

namespace ORM.QuickGraph
{
    public static class GraphFactory
    {
        public static RelationShipGraph CreateSmallGraph()
        {
            var graph = new RelationShipGraph();
            var gerhard = new VertextModel(VertexTypes.Person, "Gerhard") { IsExpanded = true};
            graph.AddVertex(gerhard);
            ToggleExpandVertex(graph, gerhard, true);
            return graph;
        }

        public static void AddVertexTo(VertextModel parent, RelationShipGraph graph)
        {
            var child = GetOrCreateVertex(graph, "New Child");
            graph.AddVerticesAndEdge(new EdgeModel(parent, child) { SourceRole = "Gemeinde", TargetRole = "Techniker" });
        }


        public static void ToggleExpandVertex(RelationShipGraph graph, VertextModel vertex, bool expand)
        {
            using (graph.SupressEvents())
            {
                var children = GetConnectedVerticesForVertex(graph, vertex);

                if (expand)
                {
                    foreach (var child in children)
                    {
                        graph.AddVerticesAndEdge(new EdgeModel(vertex, child)
                        {
                            SourceRole = "Gemeinde",
                            TargetRole = "Techniker"
                        });
                    }
                }
                else
                {
                    foreach (var child in children)
                    {
                        graph.Edges
                            .Where(e => e.Source.Equals(vertex) && e.Target.Equals(child))
                            .ToList()
                            .ForEach(e => graph.RemoveEdge(e));
                    }

                    foreach (var emptyVertex in graph.IsolatedVertices().ToArray())
                    {
                        graph.RemoveVertex(emptyVertex);
                    }
                }
            }
        }

        private static IEnumerable<VertextModel> GetConnectedVerticesForVertex(RelationShipGraph graph, VertextModel vertex)
        {
            if (vertex.Name == "Gerhard")
            {
                yield return GetOrCreateVertex(graph, "Fahrrad");
                yield return GetOrCreateVertex(graph, "Krafttraining");
            }

            if (vertex.Name == "Krafttraining")
            {
                yield return GetOrCreateVertex(graph,  "Chris");
                yield return GetOrCreateVertex(graph,  "LangHantel");
            }
            if (vertex.Name == "Chris")
            {
                yield return GetOrCreateVertex( graph,  "Pflanzen");
                yield return GetOrCreateVertex( graph,  "Gitarre");
                yield return GetOrCreateVertex( graph,  "Basketball");
                yield return GetOrCreateVertex( graph,  "Volleyball");
                yield return GetOrCreateVertex( graph,  "Schlafen");
            }
            if (vertex.Name == "LangHantel")
            {
                yield return GetOrCreateVertex(graph,  "Hantel-Scheibe");
                yield return GetOrCreateVertex(graph,  "Verschlüsse");
            }
        }

        private static VertextModel GetOrCreateVertex(RelationShipGraph graph, string name)
        {
            var existingVertex = graph.Vertices.FirstOrDefault(v => v.Name.Equals(name));
            return existingVertex ?? new VertextModel(VertexTypes.Person, name);
        }


        public static IDictionary<VertextModel, Point> CreateLayout(RelationShipGraph graph,
            IDictionary<VertextModel, Point> oldPositions)
        {
            var specialGraphFactory = new LayoutFactory<VertextModel, EdgeModel, RelationShipGraph>();
           return specialGraphFactory.ComputeLayout(graph, oldPositions);
        }
    }
}
