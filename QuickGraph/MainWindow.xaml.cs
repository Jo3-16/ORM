using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ORM.QuickGraph.Properties;

namespace ORM.QuickGraph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private RelationShipGraph graph;

        public MainWindow()
        {
            this.DataContext = this;
          
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
           // var graph = GraphFactory.CreateGraph();
            var graph = GraphFactory.CreateSmallGraph();
            this.Graph = graph;

            base.OnSourceInitialized(e);
        }

        public RelationShipGraph Graph
        {
            get { return graph; }
            set
            {
                if (Equals(value, graph)) return;
                graph = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
          GraphFactory.AddSomething(GraphFactory.LastGraph);
        }
    }
 
}
