using System.Windows.Controls;

namespace ORM.RelationshipView
{
    /// <summary>
    /// Interaction logic for RelationshipView.xaml
    /// </summary>
    public partial class RelationshipView : UserControl
    {
        public RelationshipView()
        {
            InitializeComponent();
        }

        private RelationshipViewModel ViewModel => this.DataContext as RelationshipViewModel;


        private void MyGraphCanvas_OnAddVertex(string obj)
        {
            ViewModel.AddVertex(obj);
        }

        private void MyGraphCanvas_OnToggleExpand(string arg1, bool arg2)
        {
            ViewModel.ToggleExpand(arg1, arg2);
        }
    }
}
