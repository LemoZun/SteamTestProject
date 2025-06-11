using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Procedural_Map_Generation
{
    public static class MSTUtility
    {
        public static List<Edge> GetMST(List<Triangle> triangles)
        {
            List<Edge> edges = new List<Edge>();
            foreach (Triangle triangle in triangles)
            {
                // 각 삼각형의 엣지를 가져와서 edges 리스트에 추가
                foreach (Edge edge in triangle.Edges)
                {
                    // 엣지가 이미 존재하지 않는 경우에만 추가
                    if (!edges.Contains(edge))
                    {
                        edges.Add(edge);
                    }
                }
            }
            return GetMST(edges);
        }

        public static List<Edge> GetMST(List<Edge> edges)
        {
            List<Edge> mst = new List<Edge>();

            // 간선이 없는 경우 빈 MST 반환
            if (edges.Count == 0)
                return mst;

            // 간선을 길이 기준으로 정렬
            edges.Sort((a, b) => a.LengthSquared.CompareTo(b.LengthSquared));

            // Union-Find 자료구조 초기화
            Dictionary<Vertex, Vertex> parent = new Dictionary<Vertex, Vertex>();
            foreach (Edge edge in edges)
            {
                if (parent.ContainsKey(edge.A) == false)
                    parent[edge.A] = edge.A;
                if (parent.ContainsKey(edge.B) == false)
                    parent[edge.B] = edge.B;
            }

            // Find 함수
            Vertex Find(Vertex v)
            {
                if (parent[v] != v)
                {
                    parent[v] = Find(parent[v]); // 경로 압축
                }
                return parent[v];
            }

            // Union 함수
            bool Union(Vertex a, Vertex b)
            {
                Vertex rootA = Find(a);
                Vertex rootB = Find(b);
                if (rootA == rootB)
                    return false;

                parent[rootB] = rootA; // rootB를 rootA의 자식으로 설정
                return true;
            }

            // 간선을 순회하며 MST 생성

            foreach (Edge edge in edges)
            {
                if (Union(edge.A, edge.B))
                {
                    mst.Add(edge); // MST에 간선 추가
                }
            }

            return mst;
        }
    }
}