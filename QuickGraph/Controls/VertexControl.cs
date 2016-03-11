using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ORM.RelationshipView.Models;

namespace ORM.RelationshipView.Controls
{
    public class VertexControl : Control
    {
        private readonly Action<VertexModel, bool> onToggleExpand;
        private readonly Action<VertexModel> onAddVertex;
        private VertexModel vertex;

        static VertexControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VertexControl), new FrameworkPropertyMetadata(typeof(VertexControl)));
        }

        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register(
            "Caption", typeof (string), typeof (VertexControl), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty StandardPhoneProperty = DependencyProperty.Register(
            "StandardPhone", typeof (string), typeof (VertexControl), new PropertyMetadata(default(string)));

        public string StandardPhone
        {
            get { return (string) GetValue(StandardPhoneProperty); }
            set { SetValue(StandardPhoneProperty, value); }
        }

        public static readonly DependencyProperty AddressProperty = DependencyProperty.Register(
            "Address", typeof (string), typeof (VertexControl), new PropertyMetadata(default(string)));

        public string Address
        {
            get { return (string) GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value); }
        }


        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
            "IsExpanded", typeof (bool), typeof (VertexControl), new PropertyMetadata(false));


        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof (bool), typeof (VertexControl), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty VertexIdProperty = DependencyProperty.Register(
            "VertexId", typeof (string), typeof (VertexControl), new PropertyMetadata(default(string)));

        public string VertexId
        {
            get { return (string) GetValue(VertexIdProperty); }
            set { SetValue(VertexIdProperty, value); }
        }



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


        public VertexControl(VertexModel vertex, Action<VertexModel,bool> onToggleExpand, Action<VertexModel> onAddVertex)
        {
            Vertex = vertex;
            this.onToggleExpand = onToggleExpand;
            this.onAddVertex = onAddVertex;
        }

        public VertexModel Vertex
        {
            get { return vertex; }
            set
            {
                vertex        = value;
                StandardPhone = vertex.StandardPhone;
                Caption       = vertex.FullName;
                IsExpanded    = vertex.IsExpanded;
                VertexId      = vertex.VertexId;
                Address       = vertex.AddressImage +"r\n"+ vertex.StandardPhone;
            }
        }

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
