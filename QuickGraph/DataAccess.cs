using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using DevDataBridge;
using ORM.RelationshipView.Models;

namespace ORM.RelationshipView
{
    public class DataAccess
    {
        private readonly RelationshipGraph graph;
        private readonly IDataBridge dataBridge;

        public DataAccess(RelationshipGraph graph)
        {
            this.graph = graph;
            this.dataBridge = new DataBridgeDev();
        }

        public EdgeModel AddChild(string parentId)
        {
            var parent = GetOrCreateVertex(parentId);
            var childId = dataBridge.AddChild(parentId);
            var child = GetOrCreateVertex(childId);
            return new EdgeModel(parent, child) { SourceRole = "Gemeinde", TargetRole = "Techniker" };
        }

        public IEnumerable<VertextModel> GetConnectedVerticesForVertex(string vertexId)
        {
            var childrenIds = dataBridge.GetConnectedVerticesForVertex(vertexId);
            var children = childrenIds.Select(GetOrCreateVertex);
            return children;
        }

        public IEnumerable<EdgeModel> GetConnectedEdgesForVertex(string vertexId)
        {
            var vertex = GetVertex(vertexId);
            var children = GetConnectedVerticesForVertex(vertexId);
            return children.Select(child => new EdgeModel(vertex, child)
            {
                SourceRole = "Gemeinde",
                TargetRole = "Techniker"
            });
        }

        private VertextModel CreateVertex(string vertexId)
        {
            var vertexData = dataBridge.GetVertexData(vertexId);
            return new VertextModel(VertexTypes.Person, vertexData.VertexId);
        }

        public VertextModel GetOrCreateVertex(string vertexId)
        {
            var existingVertex = graph.Vertices.FirstOrDefault(v => v.Name.Equals(vertexId));
            return existingVertex ?? CreateVertex(vertexId);
        }

        public VertextModel GetVertex(string vertexId)
        {
            var existingVertex = graph.Vertices.FirstOrDefault(v => v.Name.Equals(vertexId));
            if (existingVertex == null)
            {
                throw new ArgumentOutOfRangeException(nameof(vertexId),$"No vertex for name {vertexId}");
            }
            return existingVertex;
        }
    }
}