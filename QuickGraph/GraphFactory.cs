using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using ORM.QuickGraph.Annotations;
using ORM.QuickGraph.Models;
using QuickGraph.Algorithms;

namespace ORM.QuickGraph
{
    public class GraphFactory : INotifyPropertyChanged
    {
        private LayoutInfo layoutInfo;
        private readonly RelationShipGraph graph;
        private readonly UndRedo<ToggleUndoRedoStep> undoRedo;

        public GraphFactory()
        {
            graph = new RelationShipGraph();
            undoRedo = new UndRedo<ToggleUndoRedoStep>();
            UpdateLayoutInfo();
        }

        public LayoutInfo LayoutInfo
        {
            get { return layoutInfo; }
            private set
            {
                layoutInfo = value;
                OnPropertyChanged();
            }
        }

        public void CreateSmallGraph()
        {
            var gerhard = new VertextModel(VertexTypes.Person, "Gerhard") {IsExpanded = true};
            graph.AddVertex(gerhard);
            ToggleExpandInternal(gerhard.Name, true);
            UpdateLayoutInfo();
        }

        public void AddVertexTo(string parentId)
        {
            var parent = GetOrCreateVertex(parentId);
            var child = GetOrCreateVertex("New Child");
            graph.AddVerticesAndEdge(new EdgeModel(parent, child) {SourceRole = "Gemeinde", TargetRole = "Techniker"});
            UpdateLayoutInfo();
        }

        public void UpdateLayoutInfo()
        {
            LayoutInfo = new LayoutInfo(graph, CreateLayout(graph, new Dictionary<VertextModel, Point>(0)));
        }

        public void UpdateLayoutInfo(IDictionary<VertextModel, Point> layout)
        {
            if (layout.Keys.OrderBy(_ => _.Name).SequenceEqual(graph.Vertices.OrderBy(_=>_.Name)))
            {
                LayoutInfo = new LayoutInfo(graph, layout);
            }
            else
            {
                UpdateLayoutInfo();
            }
        }
     

        public void ToggleExpandVertex(string vertexId, bool expand)
        {
            undoRedo.ClearRedoStack();
            undoRedo.SetUndoStep(new ToggleUndoRedoStep(vertexId,expand,LayoutInfo.Layout));
            ToggleExpandInternal(vertexId, expand);
            UpdateLayoutInfo();

            OnPropertyChanged(nameof(CanUndoToggle));
            OnPropertyChanged(nameof(CanRedoToggle));
        }

        public bool CanUndoToggle => undoRedo.CanUndoToggle;
        public bool CanRedoToggle => undoRedo.CanRedoToggle;

        public void UndoToggle()
        {
            var step = undoRedo.GetUndoStep();

            var vertexId = step.VertexId;
            var expand = step.Expand;
            var layout = step.Layout;

           undoRedo.SetRedoStep(new ToggleUndoRedoStep(vertexId,expand, LayoutInfo.Layout));

            UndoRedoToggle(vertexId, !expand, layout);
        }

        public void RedoToggle()
        {
            var step = undoRedo.GetRedoStep();

            var vertexId = step.VertexId;
            var expand = step.Expand;
            var layout = step.Layout;

            undoRedo.SetUndoStep( new ToggleUndoRedoStep(vertexId, expand, LayoutInfo.Layout));

            UndoRedoToggle(vertexId, expand, layout);
        }


        private void UndoRedoToggle(string vertexId, bool expand, IDictionary<VertextModel, Point> layout)
        {
            ToggleExpandInternal(vertexId, expand);
            UpdateLayoutInfo(layout);
            OnPropertyChanged(nameof(CanUndoToggle));
            OnPropertyChanged(nameof(CanRedoToggle));
        }

       
        private void ToggleExpandInternal(string vertexId, bool expand)
        {
            var vertex = GetOrCreateVertex(vertexId);
            vertex.IsExpanded = expand;

            using (graph.SupressEvents())
            {
                var children = GetConnectedVerticesForVertex(graph, vertex);

                if (expand)
                {
                    foreach (var child in children)
                    {
                        graph.AddVerticesAndEdge(new EdgeModel(vertex, child)
                        {
                            SourceRole = "Gemeinde",
                            TargetRole = "Techniker"
                        });
                    }
                }
                else
                {
                    foreach (var child in children)
                    {
                        graph.Edges
                            .Where(e => e.Source.Equals(vertex) && e.Target.Equals(child))
                            .ToList()
                            .ForEach(e => graph.RemoveEdge(e));
                    }

                    foreach (var emptyVertex in graph.IsolatedVertices().ToArray())
                    {
                        graph.RemoveVertex(emptyVertex);
                    }
                }
            }
        }

        private IEnumerable<VertextModel> GetConnectedVerticesForVertex(RelationShipGraph graph, VertextModel vertex)
        {
            if (vertex.Name == "Gerhard")
            {
                yield return GetOrCreateVertex("Fahrrad");
                yield return GetOrCreateVertex("Krafttraining");
            }

            if (vertex.Name == "Krafttraining")
            {
                yield return GetOrCreateVertex("Chris");
                yield return GetOrCreateVertex("LangHantel");
            }
            if (vertex.Name == "Chris")
            {
                yield return GetOrCreateVertex("Pflanzen");
                yield return GetOrCreateVertex("Gitarre");
                yield return GetOrCreateVertex("Basketball");
                yield return GetOrCreateVertex("Volleyball");
                yield return GetOrCreateVertex("Schlafen");
            }
            if (vertex.Name == "LangHantel")
            {
                yield return GetOrCreateVertex("Hantel-Scheibe");
                yield return GetOrCreateVertex("Verschlüsse");
            }
        }

        private VertextModel GetOrCreateVertex(string name)
        {
            var existingVertex = graph.Vertices.FirstOrDefault(v => v.Name.Equals(name));
            return existingVertex ?? new VertextModel(VertexTypes.Person, name);
        }

        private IDictionary<VertextModel, Point> CreateLayout(RelationShipGraph graph,
            IDictionary<VertextModel, Point> oldPositions)
        {
            var specialGraphFactory = new LayoutFactory<VertextModel, EdgeModel, RelationShipGraph>();
            return specialGraphFactory.ComputeLayout(graph, oldPositions);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}