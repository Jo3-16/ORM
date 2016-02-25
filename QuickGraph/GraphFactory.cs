using System.Collections.Generic;
using System.Linq;
using System.Windows;
using QuickGraph.Algorithms;


namespace QG2
{
    public static class GraphFactory
    {
        public static RelationShipGraph LastGraph { get; private set; }

        public static RelationShipGraph CreateGraph()
        {
            var graph = new RelationShipGraph();

            var gerhard = new VertextModel(VertexTypes.Person, "Gerhard");
            var flo = new VertextModel(VertexTypes.Person, "Flo");
            var chris = new VertextModel(VertexTypes.Person, "Chris");
          //  var karlHeinz = new VertextModel(VertexTypes.Person, "Karl Heinz");

            var fahrrad = new VertextModel(VertexTypes.Sport, "Fahrrad");
            var krafttraining = new VertextModel(VertexTypes.Sport, "Krafttraining");
            var kampfsport = new VertextModel(VertexTypes.Sport, "Kampfsport");
          
            var hantel = new VertextModel(VertexTypes.Ausrüstung, "LangHantel");
            var hantelScheibe = new VertextModel(VertexTypes.Ausrüstung, "Hantel-Scheibe");
            var verschluesse = new VertextModel(VertexTypes.Ausrüstung, "Verschlüsse");

            graph.AddVertex(gerhard);
            graph.AddVertex(flo);
            graph.AddVertex(chris);
        //    graph.AddVertex(karlHeinz);

            graph.AddVertex(fahrrad);
            graph.AddVertex(krafttraining);
            graph.AddVertex(kampfsport);

            graph.AddVertex(hantel);
            graph.AddVertex(hantelScheibe);
            graph.AddVertex(verschluesse);


            graph.AddEdge(new EdgeModel(flo, fahrrad));
            graph.AddEdge(new EdgeModel(gerhard, fahrrad));
            graph.AddEdge(new EdgeModel(gerhard, krafttraining));

            graph.AddEdge(new EdgeModel(gerhard, chris));
            graph.AddEdge(new EdgeModel(chris, gerhard));


            graph.AddEdge(new EdgeModel(krafttraining, kampfsport));
            graph.AddEdge(new EdgeModel(kampfsport, krafttraining));

            graph.AddEdge(new EdgeModel(krafttraining, hantel));
            graph.AddEdge(new EdgeModel(hantel, hantelScheibe));
            graph.AddEdge(new EdgeModel(hantel, verschluesse));

            LastGraph = graph;

            return graph;
        }

        public static void AddSomething(RelationShipGraph graph)
        {
            var chris = graph.Vertices.First(v => v.Name.Equals("Chris"));
            var krafttraining = graph.Vertices.First(v => v.Name.Equals("Krafttraining"));
            graph.AddEdge(new EdgeModel(chris, krafttraining));

            var pflanzen = new VertextModel(VertexTypes.Sport, "Pflanzen");
            var gitarre = new VertextModel(VertexTypes.Sport, "Gitarre");
            var basketball = new VertextModel(VertexTypes.Sport, "Basketball");
            var volleyball = new VertextModel(VertexTypes.Sport, "Volleyball");
            var schlafen = new VertextModel(VertexTypes.Sport, "Schlafen");

            graph.AddVerticesAndEdge(new EdgeModel(chris, basketball));
            graph.AddVerticesAndEdge(new EdgeModel(chris, pflanzen));
            graph.AddVerticesAndEdge(new EdgeModel(chris, gitarre));
            graph.AddVerticesAndEdge(new EdgeModel(chris, volleyball));
            graph.AddVerticesAndEdge(new EdgeModel(chris, schlafen));

          
        }

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
            graph.AddVerticesAndEdge(new EdgeModel(parent, child) { SourceRole = "SRole", TargetRole = "TRole" });
          
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
                            SourceRole = "SRole",
                            TargetRole = "TRole"
                        });
                    }
                }
                else
                {
                    foreach (var child in children)
                    {
                        graph.RemoveEdge(new EdgeModel(vertex, child) {SourceRole = "SRole", TargetRole = "TRole"});
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
