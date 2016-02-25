using System;
using QuickGraph;

namespace QG2
{
    public class RelationShipGraph : BidirectionalGraph<VertextModel, EdgeModel>
    {
        public event Action<RelationShipGraph> Updated = g => { };

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
    }

    public class EventSupressor : IDisposable
    {
        private readonly RelationShipGraph graph;
        private bool disposed;
  

        public EventSupressor(RelationShipGraph graph)
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