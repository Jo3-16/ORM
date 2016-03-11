using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ORM.RelationshipView.Controls;
using ORM.RelationshipView.Models;

namespace ORM.RelationshipView
{
    public class GraphControl : Grid
    {
        public static readonly DependencyProperty RelationshipInfoProperty = DependencyProperty.Register(
            "RelationshipInfo", typeof (RelationshipInfo), typeof (GraphControl),
            new PropertyMetadata(default(RelationshipInfo), (s, e) => ((GraphControl) s).GraphChanged()));

        private VertexControl draggingVertex;
        private Point dragMouseOffset;
        private VertextModel expandedVertex;
        private Point offsetVector;

        public RelationshipInfo RelationshipInfo
        {
            get { return (RelationshipInfo) GetValue(RelationshipInfoProperty); }
            set { SetValue(RelationshipInfoProperty, value); }
        }

        private void GraphChanged()
        {
            CreateChildren(RelationshipInfo);

            if (expandedVertex == null) return;
            var vertexControl = GetVertexControlFromVertexModel(expandedVertex);

            if (vertexControl == null) return;
            SetSelection(vertexControl);
            vertexControl.BringIntoView();
        }

        private void CreateChildren(RelationshipInfo relationshipInfo)
        {

          var  verticesWithPositions = relationshipInfo.Layout;
            if (verticesWithPositions.Any() == false)
            {
                return;
            }

            if (verticesWithPositions == null) return;

            var minX = verticesWithPositions.Values.Min(p => p.X) - 10;
            var minY = verticesWithPositions.Values.Min(p => p.Y) - 10;

            offsetVector = new Point(-minX, -minY);

            Children.Clear();
            verticesWithPositions.
                ToList()
                .Select(CreateVertex)
                .ToList()
                .ForEach(c => Children.Add(c));

            RelationshipInfo.Graph.Edges
                .Select(CreateEdge)
                .ToList()
                .ForEach(e => Children.Add(e));
        }

        private void SetPosition(FrameworkElement element, Point position)
        {
            var newPos = WtoD(position);
            element.Margin = new Thickness(newPos.X, newPos.Y, 0, 0);
        }


        public event Action<string, bool> ToggleExpand = (model, b) => { }; 
        private void OnToggleExpand(VertextModel vertex, bool expand)
        {
            expandedVertex = vertex;
            ToggleExpand(vertex.Name, expand);
        }

        public event Action<string> AddVertex =model => {};
        private void OnAddVertex(VertextModel parent)
        {
            AddVertex(parent.Name);
        }

        private FrameworkElement CreateVertex(KeyValuePair<VertextModel, Point> kvp)
        {
            var myVertex = kvp.Key;

            var vertexControl = new VertexControl(myVertex, OnToggleExpand, OnAddVertex)
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
            return Children.OfType<VertexControl>().SingleOrDefault(c => Equals(vertextModel, c.Vertex));
        }

        private FrameworkElement CreateEdge(EdgeModel edgeModel)
        {
            var source = GetVertexControlFromVertexModel(edgeModel.Source);
            var target = GetVertexControlFromVertexModel(edgeModel.Target);
            var edgeControl = new EdgeControl
            {
                Source = source,
                Target = target,
                SourceRole = edgeModel.SourceRole,
                TargetRole = edgeModel.TargetRole,
                Foreground = Brushes.DarkRed,
                ToolTip =
                    $"{target.Caption} ist {edgeModel.SourceRole} für {source.Caption} \n{source.Caption} ist {edgeModel.TargetRole} für {target.Caption}"
            };

            SetZIndex(edgeControl, 10);

            return edgeControl;
        }

        private Point WtoD(Point point)
        {
            return new Point(point.X + offsetVector.X, point.Y + offsetVector.Y);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            var position = e.GetPosition(this);

            VisualTreeHelper.HitTest(this, FilterCallback, ResultCallback, new PointHitTestParameters(position));

            if (draggingVertex == null)
            {
                SetSelection();
            }
            else
            {
                SetSelection(draggingVertex);
                dragMouseOffset = e.GetPosition(draggingVertex);
            }
        }

        private HitTestFilterBehavior FilterCallback(DependencyObject potentialHitTestTarget)
        {
            return potentialHitTestTarget is VertexControl
                ? HitTestFilterBehavior.ContinueSkipChildren
                : HitTestFilterBehavior.ContinueSkipSelf;
        }


        private HitTestResultBehavior ResultCallback(HitTestResult result)
        {
            var hit = result.VisualHit as VertexControl;
            if (hit == null)
            {
                return HitTestResultBehavior.Continue;
            }

            draggingVertex = hit;

            return HitTestResultBehavior.Stop;
        }

        private void SetSelection(params VertexControl[] vertices)
        {
            foreach (var vertex in Children.OfType<VertexControl>())
            {
                vertex.IsSelected = vertices.Contains(vertex);
            }
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (draggingVertex != null)
            {
                var position = e.GetPosition(this);
                draggingVertex.Margin = new Thickness(position.X - dragMouseOffset.X, position.Y - dragMouseOffset.Y, 0,
                    0);
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            draggingVertex = null;
            base.OnMouseLeftButtonUp(e);
        }
    }
}