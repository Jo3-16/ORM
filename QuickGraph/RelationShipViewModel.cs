using System.ComponentModel;
using System.Runtime.CompilerServices;
using ORM.RelationshipView.Properties;

namespace ORM.RelationshipView
{
    public class RelationshipViewModel : INotifyPropertyChanged
    {
        private readonly GraphFactory graphFactory;
        private string initialVertex;

        public RelationshipViewModel()
        {
            graphFactory = new GraphFactory
            {
                RelationshipInfoUpdated = () => OnPropertyChanged(nameof(RelationshipInfo))
            };
        }

        public RelationshipInfo RelationshipInfo => graphFactory.RelationshipInfo;
    
        public void Init(string vertexName)
        {
            initialVertex = vertexName;
            graphFactory.InitGraph(vertexName);
        }

        public void Home()
        {
            graphFactory.InitGraph(initialVertex);
        }

        public void AddVertex(string vertexName)
        {
            graphFactory.AddVertexTo(vertexName);
        }

        public void Refresh()
        {
            graphFactory.UpdateRelationshipInfo();
        }

        public bool CanUndoToggle => graphFactory.CanUndoToggle;
        public bool CanRedoToggle => graphFactory.CanRedoToggle;

        public void ToggleExpand(string vertexName, bool expand)
        {
            graphFactory.ToggleExpandVertex(vertexName, expand);

            OnPropertyChanged(nameof(CanUndoToggle));
            OnPropertyChanged(nameof(CanRedoToggle));
        }

        public void UndoToggle()
        {
            graphFactory.UndoToggle();

            OnPropertyChanged(nameof(CanUndoToggle));
            OnPropertyChanged(nameof(CanRedoToggle));
        }

        public void RedoToggle()
        {
            graphFactory.RedoToggle();

            OnPropertyChanged(nameof(CanUndoToggle));
            OnPropertyChanged(nameof(CanRedoToggle));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
