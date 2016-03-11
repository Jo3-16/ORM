using System.Collections.Generic;
using System.Windows;
using ORM.RelationshipView.Models;

namespace ORM.RelationshipView
{
    public class RelationshipInfo
    {
        public RelationshipInfo(RelationshipGraph graph, IDictionary<VertextModel, Point> layout)
        {
            Graph = graph;
            Layout = layout;
        }

        public RelationshipGraph Graph { get; set; }

        public IDictionary<VertextModel,Point> Layout { get; set; }
    }
}
