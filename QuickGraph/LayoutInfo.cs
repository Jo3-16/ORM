using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ORM.QuickGraph.Models;

namespace ORM.QuickGraph
{
    public class LayoutInfo
    {
        public LayoutInfo(RelationShipGraph graph, IDictionary<VertextModel, Point> layout)
        {
            Graph = graph;
            Layout = layout;
        }

        public RelationShipGraph Graph { get; set; }

        public IDictionary<VertextModel,Point> Layout { get; set; }
    }
}
