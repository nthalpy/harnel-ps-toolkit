using System;
using System.Collections.Generic;

namespace ProblemSolving.Templates.MaxFlow
{
    /// <summary>
    /// Dinic's Algorithm
    /// </summary>
    [IncludeIfReferenced]
    public class MaxFlowHelper<TNode>
        where TNode : struct
    {
        private struct FlowEdge
        {
            public int Dst;
            public int Opp;
            public long Flow;
            public long Capacity;

            public FlowEdge(int dst, int opp, long capacity)
            {
                this.Dst = dst;
                this.Opp = opp;
                this.Flow = 0;
                this.Capacity = capacity;
            }

            public bool CanFlow => Flow < Capacity;
        }

        private Dictionary<TNode, int> _map;
        private List<RefableList<FlowEdge>> _graph;

        public MaxFlowHelper()
        {
            _map = new Dictionary<TNode, int>();
            _graph = new List<RefableList<FlowEdge>>();
        }

        private int GetId(TNode node)
        {
            if (!_map.TryGetValue(node, out var v))
            {
                v = _map.Count;
                _map[node] = v;
                _graph.Add(new RefableList<FlowEdge>());
            }

            return v;
        }
        public void AddEdge(TNode src, TNode dst, long forwardCap, long backwardCap)
        {
            var srcId = GetId(src);
            var dstId = GetId(dst);

            var srcopp = _graph[dstId].Count;
            var dstopp = _graph[srcId].Count;

            _graph[srcId].Add(new(dstId, srcopp, forwardCap));
            _graph[dstId].Add(new(srcId, dstopp, backwardCap));
        }

        public long FindMaxFlow(TNode source, TNode destination)
        {
            var sourceId = GetId(source);
            var destinationId = GetId(destination);
            var flow = 0L;

            var n = _graph.Count;
            var levelGraph = new int[n];

            while (true)
            {
                var flowOccured = false;
                RebuildLevelGraph(sourceId, destinationId, levelGraph);

                while (true)
                {
                    var f = MakeFlow(sourceId, destinationId, levelGraph, Int64.MaxValue);
                    if (f == 0)
                        break;

                    flow += f;
                    flowOccured = true;
                }

                if (!flowOccured)
                    break;
            }

            return flow;
        }

        private long MakeFlow(int pos, int sink, int[] levelGraph, long currFlow)
        {
            if (currFlow == 0)
            {
                levelGraph[pos] = -1;
                return 0;
            }

            if (pos == sink)
                return currFlow;

            var currlevel = levelGraph[pos];
            var g = _graph[pos];
            var count = g.Count;
            for (var idx = 0; idx < count; idx++)
            {
                ref var forward = ref g[idx];

                if (!forward.CanFlow)
                    continue;

                if (currlevel >= levelGraph[forward.Dst])
                    continue;

                var flow = MakeFlow(forward.Dst, sink, levelGraph, Math.Min(forward.Capacity - forward.Flow, currFlow));
                if (flow == 0)
                    continue;

                ref var backward = ref _graph[forward.Dst][forward.Opp];
                forward.Flow += flow;
                backward.Flow -= flow;

                return flow;
            }

            levelGraph[pos] = -1;
            return 0;
        }

        private void RebuildLevelGraph(int source, int destination, int[] levelGraph)
        {
            var q = new Queue<(int pos, int level)>();
            q.Enqueue((source, 0));

            Array.Fill(levelGraph, -1);
            levelGraph[source] = 0;

            while (q.TryDequeue(out var state))
            {
                var (pos, level) = state;

                if (pos == destination)
                    continue;

                var g = _graph[pos];
                var count = g.Count;
                for (var idx = 0; idx < count; idx++)
                {
                    ref var e = ref g[idx];
                    if (e.CanFlow && levelGraph[e.Dst] == -1)
                    {
                        levelGraph[e.Dst] = level + 1;
                        q.Enqueue((e.Dst, level + 1));
                    }
                }
            }
        }
    }
}
