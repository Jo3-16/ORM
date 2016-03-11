using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ORM.RelationshipView.Models;
using QuickGraph.Algorithms;

namespace ORM.RelationshipView
{
    public class GraphFactory
    {
        private RelationshipInfo relationshipInfo;
        private readonly RelationshipGraph graph;
        private readonly UndRedo<ToggleUndoRedoStep> undoRedo;
        private readonly DataAccess dataAccess;
        private readonly LayoutFactory<VertexModel, EdgeModel, RelationshipGraph> layoutFactory;

        public Action RelationshipInfoUpdated = () => { };
      
        public GraphFactory()
        {
            graph = new RelationshipGraph();
            layoutFactory = new LayoutFactory<VertexModel, EdgeModel, RelationshipGraph>();
            undoRedo = new UndRedo<ToggleUndoRedoStep>();
            UpdateRelationshipInfo();
            dataAccess = new DataAccess(graph);
        }

        public RelationshipInfo RelationshipInfo
        {
            get { return relationshipInfo; }
            private set
            {
                relationshipInfo = value;
                RelationshipInfoUpdated();
            }
        }

        private void InvokeOnGraph(Action<RelationshipGraph> action, IDictionary<VertexModel,Point> layout=null )
        {
            action(graph);
            UpdateRelationshipInfo(layout);
        }

        public void UpdateRelationshipInfo(IDictionary<VertexModel, Point> layout = null)
        {
            if (layout != null && layout.Keys.OrderBy(_ => _.VertexId).SequenceEqual(graph.Vertices.OrderBy(_ => _.VertexId)))
            {
                RelationshipInfo = new RelationshipInfo(graph, layout);
            }
            else
            {
                RelationshipInfo = new RelationshipInfo(graph, layoutFactory.ComputeLayout(graph, new Dictionary<VertexModel, Point>(0)));
            }
        }

        public void InitGraph(string name)
        {
            var firstVertex = dataAccess.CreateVertex(name); //  new VertexModel(VertexTypes.Person, name) {IsExpanded = true};

            InvokeOnGraph(graph =>
            {
                graph.Clear();
                graph.AddVertex(firstVertex);
              //  ToggleExpandInternal(firstVertex.VertexId, true);
            });
        }

        public void AddVertexTo(string parentId)
        { 
            var edgeModel = dataAccess.AddChild(parentId);

            InvokeOnGraph(graph =>
            {
                
                graph.AddVerticesAndEdge(edgeModel);
            });
        }

        public void ToggleExpandVertex(string vertexId, bool expand)
        {
            undoRedo.ClearRedoStack();
            undoRedo.SetUndoStep(new ToggleUndoRedoStep(vertexId,expand,RelationshipInfo.Layout));

            InvokeOnGraph(graph => ToggleExpandInternal(vertexId, expand));

        }

        public bool CanUndoToggle => undoRedo.CanUndoToggle;
        public bool CanRedoToggle => undoRedo.CanRedoToggle;

        public void UndoToggle()
        {
            var step = undoRedo.GetUndoStep();

            var vertexId = step.VertexId;
            var expand = step.Expand;
            var layout = step.Layout;

           undoRedo.SetRedoStep(new ToggleUndoRedoStep(vertexId,expand, RelationshipInfo.Layout));

            UndoRedoToggle(vertexId, !expand, layout);
        }

        public void RedoToggle()
        {
            var step = undoRedo.GetRedoStep();

            var vertexId = step.VertexId;
            var expand = step.Expand;
            var layout = step.Layout;

            undoRedo.SetUndoStep( new ToggleUndoRedoStep(vertexId, expand, RelationshipInfo.Layout));

            UndoRedoToggle(vertexId, expand, layout);
        }


        private void UndoRedoToggle(string vertexId, bool expand, IDictionary<VertexModel, Point> layout)
        {
            InvokeOnGraph(graph => ToggleExpandInternal(vertexId, expand), layout);
        }

       
        private void ToggleExpandInternal(string vertexId, bool expand)
        {
            var vertex = dataAccess.GetVertex(vertexId);
            vertex.IsExpanded = expand;

            using (graph.SupressEvents())
            {
                if (expand)
                {
                    var edges = dataAccess.GetConnectedEdgesForVertex(vertexId).ToList();

                    var egdgesToAdd = edges.Where(e => graph.Edges.Contains(e) == false).ToList();

                    egdgesToAdd.ForEach( e=> graph.AddVerticesAndEdge(e));
                }
                else
                {
                    dataAccess.GetConnectedEdgesForVertex(vertexId)
                        .ToList()
                        .ForEach(e =>
                    {
                        graph.RemoveEdge(e);
                    });

       
                    foreach (var emptyVertex in graph.IsolatedVertices().ToArray())
                    {
                        graph.RemoveVertex(emptyVertex);
                    }

                    if (!graph.Vertices.Any())
                    {
                        //TODO Hack
                       this.InitGraph("165378");
                    }
                }
            }
        }
    }
}