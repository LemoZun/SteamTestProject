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
                // �� �ﰢ���� ������ �����ͼ� edges ����Ʈ�� �߰�
                foreach (Edge edge in triangle.Edges)
                {
                    // ������ �̹� �������� �ʴ� ��쿡�� �߰�
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

            // ������ ���� ��� �� MST ��ȯ
            if (edges.Count == 0)
                return mst;

            // ������ ���� �������� ����
            edges.Sort((a, b) => a.LengthSquared.CompareTo(b.LengthSquared));

            // Union-Find �ڷᱸ�� �ʱ�ȭ
            Dictionary<Vertex, Vertex> parent = new Dictionary<Vertex, Vertex>();
            foreach (Edge edge in edges)
            {
                if (parent.ContainsKey(edge.A) == false)
                    parent[edge.A] = edge.A;
                if (parent.ContainsKey(edge.B) == false)
                    parent[edge.B] = edge.B;
            }

            // Find �Լ�
            Vertex Find(Vertex v)
            {
                if (parent[v] != v)
                {
                    parent[v] = Find(parent[v]); // ��� ����
                }
                return parent[v];
            }

            // Union �Լ�
            bool Union(Vertex a, Vertex b)
            {
                Vertex rootA = Find(a);
                Vertex rootB = Find(b);
                if (rootA == rootB)
                    return false;

                parent[rootB] = rootA; // rootB�� rootA�� �ڽ����� ����
                return true;
            }

            // ������ ��ȸ�ϸ� MST ����

            foreach (Edge edge in edges)
            {
                if (Union(edge.A, edge.B))
                {
                    mst.Add(edge); // MST�� ���� �߰�
                }
            }

            return mst;
        }
    }
}