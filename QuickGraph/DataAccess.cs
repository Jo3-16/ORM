using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using DevDataBridge;
using ODISDataBridge;
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
            //TODO Andreas - hier kannst du umschalten auf Demo-Daten
           // this.dataBridge = new DataBridgeDev();
            this.dataBridge = new DataBridgeODIS();
        }

        public EdgeModel AddChild(string parentId)
        {
            var parent = GetOrCreateVertex(parentId);
            var childId = dataBridge.AddChild(parentId);
            var child = GetOrCreateVertex(childId);
            return new EdgeModel(parent, child) { SourceRole = "Gemeinde", TargetRole = "Techniker" };
        }

        public IEnumerable<VertexModel> GetConnectedVerticesForVertex(string vertexId)
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

        public VertexModel CreateVertex(string vertexId)
        {
            var vertexData = dataBridge.GetVertexData(vertexId);
            return new VertexModel(VertexTypes.Person, vertexData.VertexId, vertexData.FullName, vertexData.AddressImage, vertexData.StandardPhone);
        }

        public VertexModel GetOrCreateVertex(string vertexId)
        {
            var existingVertex = graph.Vertices.FirstOrDefault(v => v.VertexId.Equals(vertexId));
            return existingVertex ?? CreateVertex(vertexId);
        }

        public VertexModel GetVertex(string vertexId)
        {
            var existingVertex = graph.Vertices.FirstOrDefault(v => v.VertexId.Equals(vertexId));
            if (existingVertex == null)
            {
                throw new ArgumentOutOfRangeException(nameof(vertexId),$"No vertex for name {vertexId}");
            }
            return existingVertex;
        }
    }
}