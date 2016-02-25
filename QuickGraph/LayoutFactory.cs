using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GraphSharp.Algorithms.Layout.Compound;
using GraphSharp.Algorithms.Layout.Compound.FDP;
using GraphSharp.Algorithms.Layout.Simple.FDP;
using GraphSharp.Algorithms.Layout.Simple.Hierarchical;
using GraphSharp.Algorithms.OverlapRemoval;
using QuickGraph;

namespace QG2
{
    public class LayoutFactory<TVertex, TEdge, TGraph>
        where TVertex : class
        where TEdge : IEdge<TVertex>
        where TGraph : class, IBidirectionalGraph<TVertex, TEdge>
    {

        public IDictionary<TVertex, Point> ComputeLayout(TGraph graph, IDictionary<TVertex, Point> oldPositions)
        {
            IDictionary<TVertex, Size> oldSizes = graph.Vertices.ToDictionary(v => v,
                v => new Size(Constants.Width, Constants.Height));

            var layoutAlgorithm = GetCompoundFDPLayoutAlgorithm(graph, oldPositions, oldSizes);

            layoutAlgorithm.Compute();

            return layoutAlgorithm.VertexPositions;
        }

        private static CompoundFDPLayoutAlgorithm<TVertex, TEdge, TGraph> GetCompoundFDPLayoutAlgorithm(
            TGraph graph,
            IDictionary<TVertex, Point> oldPositions,
            IDictionary<TVertex, Size> oldSizes)
        {
            return new CompoundFDPLayoutAlgorithm<TVertex, TEdge, TGraph>(
                graph,
                oldSizes,
                new Dictionary<TVertex, Thickness>(),
                new Dictionary<TVertex, CompoundVertexInnerLayoutType>(),
                oldPositions,
                new CompoundFDPLayoutParameters());
        }

    }
}