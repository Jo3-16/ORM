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
         //   this.dataBridge = new DataBridgeDemo();
            this.dataBridge = new DataBridgeODIS();
        }

        public EdgeModel AddChild(string parentId)
        {
            var parent = GetOrCreateVertex(parentId);
            var childId = dataBridge.AddChild(parentId);
            var child = GetOrCreateVertex(childId);
            return new EdgeModel(parent, child) { SourceRole = "Gemeinde", TargetRole = "Techniker" };
        }

        public IEnumerable<EdgeModel> GetConnectedEdgesForVertex(string vertexId)
        {
            var vertex = GetVertex(vertexId);
            var children = dataBridge.GetConnectedVerticesForVertex(vertexId);
           
            foreach (var vertexData in children)
            {
                var childVertex = graph.Vertices.FirstOrDefault(v => v.VertexId.Equals(vertexData.VertexId))
                    ?? new VertexModel(VertexTypes.Person, vertexData.VertexId, vertexData.FullName, vertexData.AddressImage, vertexData.StandardPhone); 
               
                   yield return  new EdgeModel(vertex, childVertex)
                   {
                       SourceRole = vertexData.OtherRole,
                       TargetRole = vertexData.MyRole
                   };
      
            }
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