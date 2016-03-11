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

    [DebuggerDisplay("Vertex {Name}")]
    public class VertextModel
    {
        public VertextModel(VertexTypes type, string name)
        {
            Type = type;
            Name = name;
           // Id = Guid.NewGuid();
        }

   //     public Guid Id { get; }

        public VertexTypes Type { get; set; }
        public string Name { get; set; }

        public bool IsExpanded { get; set; }

        protected bool Equals(VertextModel other)
        {
            return Name.Equals(other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((VertextModel) obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}