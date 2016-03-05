using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ORM.QuickGraph.Models;
using ORM.QuickGraph.Properties;

namespace ORM.QuickGraph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public GraphFactory GraphFactory { get; } = new GraphFactory();

        public MainWindow()
        {
            this.DataContext = this;
          
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
          GraphFactory.CreateSmallGraph();

            base.OnSourceInitialized(e);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MyGraphCanvas_OnAddVertex(string obj)
        {
            GraphFactory.AddVertexTo(obj);
        }

        private void MyGraphCanvas_OnToggleExpand(string arg1, bool arg2)
        {
            GraphFactory.ToggleExpandVertex(arg1,arg2);
        }
    }
 
}
