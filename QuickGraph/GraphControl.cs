using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ORM.QuickGraph.Controls;
using ORM.QuickGraph.Models;

namespace ORM.QuickGraph
{
    public class GraphControl : Grid
    {
        public static readonly DependencyProperty GraphProperty = DependencyProperty.Register(
            "Graph", typeof (RelationShipGraph), typeof (GraphControl), new PropertyMetadata(default(RelationShipGraph),(s,e)=> ((GraphControl)s).GraphChanged() ));

        public RelationShipGraph Graph
        {
            get { return (RelationShipGraph) GetValue(GraphProperty); }
            set { SetValue(GraphProperty, value); }
        }

        private void GraphChanged()
        {
            Graph.Updated += rg =>
            {
            //   var oldPositions = this.Children.OfType<VertexControl>().ToDictionary(g => g.Vertex, GetPosition);
                var verticesWithPositions2 = GraphFactory.CreateLayout(Graph, new Dictionary<VertextModel, Point>(0));
                CreateChildren(verticesWithPositions2);

                if (expandedVertex != null)
                {
                    var vertexControl = GetVertexControlFromVertexModel(expandedVertex);
                    if (vertexControl != null)
                    {
                        vertexControl.IsSelected = true;
                        vertexControl.BringIntoView();
                    }
                }
            };

            Refresh();
        }

        public void Refresh()
        {
            var verticesWithPositions = GraphFactory.CreateLayout(Graph, new Dictionary<VertextModel, Point>(0));
            CreateChildren(verticesWithPositions);
        }

        private Point offsetVector;

        private void CreateChildren(IDictionary<VertextModel, Point> verticesWithPositions)
        {
            if (verticesWithPositions == null) return;

            var minX = verticesWithPositions.Values.Min(p => p.X) - 10;
            var minY = verticesWithPositions.Values.Min(p => p.Y) -10 ;

            offsetVector = new Point(-minX, -minY);

            Children.Clear();
            verticesWithPositions.
                ToList()
                .Select(CreateVertex)
                .ToList()
                .ForEach(c => this.Children.Add(c));

            Graph.Edges
                .Select(CreateEdge)
                .ToList()
                .ForEach(e => this.Children.Add(e));
        }


        //private Point GetPosition(FrameworkElement element)
        //{
        //    //Canvas var pointD = new Point(GetLeft(element), GetTop(element));
        //    var pointD = new Point(element.Margin.Left, element.Margin.Top);
        //    return DtoW(pointD);
        //}

        private void SetPosition(FrameworkElement element, Point position)
        {
            var newPos = WtoD(position);
            //Canvas   SetTop(guiElement, postion.Y);SetLeft(guiElement, postion.X);
            element.Margin = new Thickness(newPos.X, newPos.Y, 0, 0);
        }

        private VertextModel expandedVertex;

        private void ToggleExpand(VertextModel vertex, bool expand)
        {
            expandedVertex = vertex;
            vertex.IsExpanded = expand;
            GraphFactory.ToggleExpandVertex(Graph,vertex,expand);
        }

        private void AddVertex(VertextModel parent)
        {
            GraphFactory.AddVertexTo(parent, Graph);
        }

        private FrameworkElement CreateVertex(KeyValuePair<VertextModel, Point> kvp)
        {
            var myVertex = kvp.Key;

            var vertexControl = new VertexControl(myVertex,ToggleExpand,AddVertex)
            {
                Caption = myVertex.Name,
                Background = Brushes.AliceBlue,
                Width = Constants.Width,
                Height = Constants.Height,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                IsExpanded = myVertex.IsExpanded
            };

            SetZIndex(vertexControl, 10000);

            SetPosition(vertexControl, kvp.Value);

            return vertexControl;
        }

        private VertexControl GetVertexControlFromVertexModel(VertextModel vertextModel)
        {
            return this.Children.OfType<VertexControl>().SingleOrDefault(c => Equals(vertextModel, c.Vertex));
        }

        private FrameworkElement CreateEdge(EdgeModel edgeModel)
        {
            var edgeControl = new EdgeControl
            {
                Source = GetVertexControlFromVertexModel(edgeModel.Source),
                Target = GetVertexControlFromVertexModel(edgeModel.Target),
                SourceRole =  edgeModel.SourceRole,
                TargetRole = edgeModel.TargetRole,
                Foreground = Brushes.IndianRed,
                ToolTip = $"SourceRole {edgeModel.SourceRole}, TargetRole {edgeModel.TargetRole}",
            };

           
            SetZIndex(edgeControl, 10);

            return edgeControl;
        }

        // Transform a point from world to device coordinates.
        private Point WtoD(Point point)
        {
            return new Point(point.X + offsetVector.X, point.Y + offsetVector.Y);
         //   return wtoDMatrix.Transform(point);
        }

        // Transform a point from device to world coordinates.
        private Point DtoW(Point point)
        {
            return new Point(point.X - offsetVector.X, point.Y - offsetVector.Y);
          //  return dtoWMatrix.Transform(point);
        }



        private VertexControl draggingVertex;


        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {

            //foreach (var result in Children.OfType<VertexControl>())
            //{

            //      if(  VisualTreeHelper.HI(result, e.GetPosition(this)) ==  

            //}

            //   VisualTreeHelper.

            //  VisualTreeHelper.HitTest(this,FilterCallback,ResultCallback, new PointHitTestParameters(e.GetPosition(this))  );



            //      var hitTest = InputHitTest(e.GetPosition(this)) as FrameworkElement;
            //if ((hitTest is VertexControl) == false)
            //{
            //    base.OnMouseLeftButtonDown(e);
            //    return;
            //}

          //  draggingVertex = (VertexControl)hitTest;
          //  draggingVertex.IsSelected = true;
        }

        private HitTestFilterBehavior FilterCallback(DependencyObject potentialHitTestTarget)
        {
            if (potentialHitTestTarget is VertexControl)
            {
                return HitTestFilterBehavior.Continue;
            }

            else
            {
               return HitTestFilterBehavior.Continue;
            }
        }

        private HitTestResultBehavior ResultCallback(HitTestResult result)
        {
            if (result.VisualHit is VertexControl)
            {
                draggingVertex = (VertexControl)result.VisualHit;
                draggingVertex.IsSelected = true;

                return HitTestResultBehavior.Stop;
            }
            else
            {
                return HitTestResultBehavior.Continue;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (draggingVertex != null)
            {
                SetPosition(draggingVertex, e.GetPosition(this));
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
        }
    }
}
