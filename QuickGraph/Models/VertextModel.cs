using System.Diagnostics;

namespace ORM.QuickGraph.Models
{
    public enum VertexTypes
    {
        Person,
        Sport,
        Ausrüstung
    }

    [DebuggerDisplay("{Name}")]
    public class VertextModel
    {
        public VertextModel(VertexTypes type, string name)
        {
            Type = type;
            Name = name;
        }

        public VertexTypes Type { get; set; }
        public string Name { get; set; }

        public bool IsExpanded { get; set; }
    }
}