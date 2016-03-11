using System;
using System.Windows;
using ORM.RelationshipView;

namespace DemoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
           
            this.DataContext = this;
            this.RelationshipViewModel = new RelationshipViewModel();

            InitializeComponent();
        }

        public RelationshipViewModel RelationshipViewModel { get; set; }

        protected override void OnSourceInitialized(EventArgs e)
        {
            RelationshipViewModel.Init("Gerhard");
            base.OnSourceInitialized(e);
        }
    }
}
