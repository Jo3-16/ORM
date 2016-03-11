using System.Collections.Generic;
using System.Windows;
using ORM.RelationshipView.Models;

namespace ORM.RelationshipView
{
    public class  ToggleUndoRedoStep
    {
        public ToggleUndoRedoStep(string vertexId, bool expand, IDictionary<VertextModel, Point> layout)
        {
            VertexId = vertexId;
            Expand = expand;
            Layout = layout;
        }

        public string VertexId { get; set; }
        public bool Expand { get; set; }
        public IDictionary<VertextModel,Point> Layout { get; set; }
    }
}
