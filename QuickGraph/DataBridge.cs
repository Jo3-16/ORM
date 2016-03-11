using System.Collections.Generic;
using System.Linq;
using ORM.RelationshipView.Models;

namespace ORM.RelationshipView
{
    public class DataBridge
    {
        private readonly RelationshipGraph graph;

        public DataBridge(RelationshipGraph graph)
        {
            this.graph = graph;
        }

        public EdgeModel AddChild(string parentId)
        {
            var parent = GetOrCreateVertex(parentId);
            var child = GetOrCreateVertex("New Child");
            return new EdgeModel(parent, child) { SourceRole = "Gemeinde", TargetRole = "Techniker" };
        }

        public IEnumerable<EdgeModel> GetConnectedEdgesForVertex(string vertexId)
        {
            var vertex = GetOrCreateVertex(vertexId);
            var children = GetConnectedVerticesForVertex(vertexId);
            return children.Select(child => new EdgeModel(vertex, child)
            {
                SourceRole = "Gemeinde",
                TargetRole = "Techniker"
            });
        }


        public IEnumerable<VertextModel> GetConnectedVerticesForVertex(string vertexId)
        {
            var vertex = GetOrCreateVertex(vertexId);

            if (vertex.Name == "Gerhard")
            {
                yield return GetOrCreateVertex("Fahrrad");
                yield return GetOrCreateVertex("Krafttraining");
            }

            if (vertex.Name == "Krafttraining")
            {
                yield return GetOrCreateVertex("Chris");
                yield return GetOrCreateVertex("LangHantel");
            }
            if (vertex.Name == "Chris")
            {
                yield return GetOrCreateVertex("Pflanzen");
                yield return GetOrCreateVertex("Gitarre");
                yield return GetOrCreateVertex("Basketball");
                yield return GetOrCreateVertex("Volleyball");
                yield return GetOrCreateVertex("Schlafen");
            }
            if (vertex.Name == "LangHantel")
            {
                yield return GetOrCreateVertex("Hantel-Scheibe");
                yield return GetOrCreateVertex("Verschlüsse");
            }
        }

        public VertextModel GetOrCreateVertex(string name)
        {
            var existingVertex = graph.Vertices.FirstOrDefault(v => v.Name.Equals(name));
            return existingVertex ?? new VertextModel(VertexTypes.Person, name);
        }
    }
}