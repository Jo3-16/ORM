using System;
using ORM.RelationshipView.Models;
using QuickGraph;

namespace ORM.RelationshipView
{
    public class RelationshipGraph : BidirectionalGraph<VertextModel, EdgeModel>
    {
        public event Action<RelationshipGraph> Updated = g => { };

        public void OnUpdated()
        {
            if (ignoreEvents)
            {
                eventsOccuredWhileIgnored = true;
            }
            else
            {
                Updated(this);
            }
        }

        public IDisposable SupressEvents()
        {
            return new EventSupressor(this);
        }

        private bool eventsOccuredWhileIgnored;
        private bool ignoreEvents;

        public bool IgnoreEvents
        {
            get { return ignoreEvents; }
            set
            {
                ignoreEvents = value;
                if (eventsOccuredWhileIgnored)
                {
                    eventsOccuredWhileIgnored = false;
                    OnUpdated();
                }
            }
        }


        protected override void OnEdgeAdded(EdgeModel args)
        {
            OnUpdated();
            base.OnEdgeAdded(args);
        }

        protected override void OnEdgeRemoved(EdgeModel args)
        {
            OnUpdated();
            base.OnEdgeRemoved(args);
        }

        protected override void OnVertexAdded(VertextModel args)
        {
            OnUpdated();
            base.OnVertexAdded(args);
        }

        protected override void OnVertexRemoved(VertextModel args)
        {
            OnUpdated();
            base.OnVertexRemoved(args);
        }

        public class EventSupressor : IDisposable
        {
            private readonly RelationshipGraph graph;
            private bool disposed;


            public EventSupressor(RelationshipGraph graph)
            {
                this.graph = graph;
                this.graph.IgnoreEvents = true;
            }

            public void Dispose()
            {
                if (disposed)
                    return;

                graph.IgnoreEvents = false;
                disposed = true;
            }
        }
    }
}