using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ORM.QuickGraph.Models;

namespace ORM.QuickGraph.Controls
{
    public class VertexControl : Control
    {
        private readonly Action<VertextModel, bool> onToggleExpand;
        private readonly Action<VertextModel> onAddVertex;

        static VertexControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VertexControl), new FrameworkPropertyMetadata(typeof(VertexControl)));
        }

        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register(
            "Caption", typeof (string), typeof (VertexControl), new PropertyMetadata(default(string)));


        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
            "IsExpanded", typeof (bool), typeof (VertexControl), new PropertyMetadata(false));


        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof (bool), typeof (VertexControl), new PropertyMetadata(default(bool)));



        public bool IsSelected
        {
            get { return (bool) GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public bool IsExpanded
        {
            get { return (bool) GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public string Caption
        {
            get { return (string) GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public VertexControl(VertextModel vertex, Action<VertextModel,bool> onToggleExpand, Action<VertextModel> onAddVertex)
        {
            Vertex = vertex;
            this.onToggleExpand = onToggleExpand;
            this.onAddVertex = onAddVertex;
        }

        public VertextModel Vertex { get; set; }

        public void ToggleExpand()
        {
            IsExpanded = !IsExpanded;
            onToggleExpand(Vertex, IsExpanded);
        }

        public void AddVertex()
        {
            onAddVertex(Vertex);
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            return new PointHitTestResult(this,hitTestParameters.HitPoint);
        }
    }
}
