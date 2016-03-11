using System.Collections.Generic;
using System.Windows;
using ORM.RelationshipView.Models;

namespace ORM.RelationshipView
{
    public class RelationshipInfo
    {
        public RelationshipInfo(RelationshipGraph graph, IDictionary<VertexModel, Point> layout)
        {
            Graph = graph;
            Layout = layout;
        }

        public RelationshipGraph Graph { get; set; }

        public IDictionary<VertexModel,Point> Layout { get; set; }
    }
}
