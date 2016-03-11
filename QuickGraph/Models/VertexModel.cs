using System;
using System.Diagnostics;

namespace ORM.RelationshipView.Models
{
    public enum VertexTypes
    {
        Person,
        Sport,
        Ausrüstung
    }

    [DebuggerDisplay("Vertex {VertexId}")]
    public class VertexModel
    {
        public string FullName { get; }
        public string AddressImage { get; }
        public string StandardPhone { get; }

        public VertexTypes Type { get;  }
        public string VertexId { get;  }

        public VertexModel(VertexTypes type, string vertexId)
        {
            Type = type;
            VertexId = vertexId;
        }

        public VertexModel(VertexTypes type, string vertexId, string fullName, string addressImage, string standardPhone) : this(type, vertexId)
        {
            FullName = fullName;
            AddressImage = addressImage;
            StandardPhone = standardPhone;
        }

        public bool IsExpanded { get; set; }

        protected bool Equals(VertexModel other)
        {
            return VertexId.Equals(other.VertexId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((VertexModel) obj);
        }

        public override int GetHashCode()
        {
            return VertexId.GetHashCode();
        }
    }
}