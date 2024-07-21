using System;
using System.Collections.Generic;

namespace ProblemSolving.Templates.Graphs
{
    [IncludeIfReferenced]
    public static class SCC
    {
        public static (int[] sccIndices, int sccCount) TarjanToSccIndices(List<int>[] graph)
        {
            var n = graph.Length;

            var visStack = new Stack<int>();
            var sccIndices = new int[n];
            var nodes = new (bool instack, int relabel, int minreach)[n];

            var relabelCounter = 1;
            var sccCounter = 0;

            for (var idx = 0; idx < n; idx++)
                if (nodes[idx].relabel == 0)
                    TarjanToSccIndicesDFS(graph, sccIndices, nodes, visStack, ref relabelCounter, ref sccCounter, idx);

            return (sccIndices, sccCounter);
        }
        public static List<List<int>> TarjanToSccList(List<int>[] graph)
        {
            var n = graph.Length;

            var visStack = new Stack<int>();
            var sccs = new List<List<int>>();
            var nodes = new (bool instack, int relabel, int minreach)[n];

            var relabelCounter = 1;

            for (var idx = 0; idx < n; idx++)
                if (nodes[idx].relabel == 0)
                    TarjanToSccListDFS(graph, sccs, nodes, visStack, ref relabelCounter, idx);

            return sccs;
        }

        private static void TarjanToSccIndicesDFS(
            List<int>[] graph,
            int[] sccIndices,
            (bool instack, int relabel, int minreach)[] nodes,
            Stack<int> visStack,
            ref int relabelCounter,
            ref int sccCounter,
            int curr)
        {
            visStack.Push(curr);

            ref var info = ref nodes[curr];
            info.instack = true;
            info.relabel = relabelCounter++;
            info.minreach = info.relabel;

            foreach (var child in graph[curr])
            {
                ref var cinfo = ref nodes[child];

                if (cinfo.relabel == 0)
                {
                    // Forward edge
                    TarjanToSccIndicesDFS(graph, sccIndices, nodes, visStack, ref relabelCounter, ref sccCounter, child);
                    info.minreach = Math.Min(info.minreach, cinfo.minreach);
                }
                else if (cinfo.instack)
                {
                    // Backward edge
                    info.minreach = Math.Min(info.minreach, cinfo.minreach);
                }
            }

            if (info.relabel == info.minreach)
            {
                while (true)
                {
                    var top = visStack.Pop();
                    sccIndices[top] = sccCounter;
                    nodes[top].instack = false;

                    if (top == curr)
                        break;
                }

                sccCounter++;
            }
        }
        private static void TarjanToSccListDFS(
            List<int>[] graph,
            List<List<int>> sccs,
            (bool instack, int relabel, int minreach)[] nodes,
            Stack<int> visStack,
            ref int relabelCounter,
            int curr)
        {
            visStack.Push(curr);

            ref var info = ref nodes[curr];
            info.instack = true;
            info.relabel = relabelCounter++;
            info.minreach = info.relabel;

            foreach (var child in graph[curr])
            {
                ref var cinfo = ref nodes[child];

                if (cinfo.relabel == 0)
                {
                    // Forward edge
                    TarjanToSccListDFS(graph, sccs, nodes, visStack, ref relabelCounter, child);
                    info.minreach = Math.Min(info.minreach, cinfo.minreach);
                }
                else if (cinfo.instack)
                {
                    // Backward edge
                    info.minreach = Math.Min(info.minreach, cinfo.minreach);
                }
            }

            if (info.relabel == info.minreach)
            {
                var scc = new List<int>();

                while (true)
                {
                    var top = visStack.Pop();
                    scc.Add(top);
                    nodes[top].instack = false;

                    if (top == curr)
                        break;
                }

                sccs.Add(scc);
            }
        }
    }
}
