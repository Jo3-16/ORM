using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ORM.QuickGraph.Controls
{
    public class EdgeControl : Control
    {
        static EdgeControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EdgeControl), new FrameworkPropertyMetadata(typeof(EdgeControl)));
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source",
                                                                                           typeof(VertexControl),
                                                                                           typeof(EdgeControl),
                                                                                           new UIPropertyMetadata(null));
        public VertexControl Source
        {
            get { return (VertexControl)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register("Target",
                                                                                               typeof(VertexControl),
                                                                                               typeof(EdgeControl),
                                                                                               new UIPropertyMetadata(null));
        public VertexControl Target
        {
            get { return (VertexControl)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        public static readonly DependencyProperty SourceRoleProperty = DependencyProperty.Register(
            "SourceRole", typeof (string), typeof (EdgeControl), new PropertyMetadata(default(string)));

        public string SourceRole
        {
            get { return (string) GetValue(SourceRoleProperty); }
            set { SetValue(SourceRoleProperty, value); }
        }

        public static readonly DependencyProperty TargetRoleProperty = DependencyProperty.Register(
            "TargetRole", typeof (string), typeof (EdgeControl), new PropertyMetadata(default(string)));

        public string TargetRole
        {
            get { return (string) GetValue(TargetRoleProperty); }
            set { SetValue(TargetRoleProperty, value); }
        }

        public static readonly DependencyProperty SelectionBrushProperty = DependencyProperty.Register(
    "SelectionBrush", typeof(Brush), typeof(EdgeControl), new PropertyMetadata(Brushes.Blue));

        public Brush SelectionBrush
        {
            get { return (Brush)GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }
    }
}
