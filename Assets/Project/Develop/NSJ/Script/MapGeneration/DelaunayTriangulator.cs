using System.Collections.Generic;
using UnityEngine;

namespace Procedural_Map_Generation
{
    public class DelaunayTriangulator
    {
        /// <summary>
        /// Delaunay �ﰢ������ �����մϴ�.(Room ����� �Է����� ����)
        /// </summary>
        /// <param name="rooms"></param>
        /// <returns></returns>
        public List<Triangle> Triangulate(List<Room> rooms)
        {
            // ���� ���� ��� ����
            List<Vertex> vertices = new List<Vertex>();
            foreach (Room room in rooms)
            {
                vertices.Add(room.Vertex);
            }
            // �ﰢ�� ����
            return Triangulate(vertices);
        }

        /// <summary>
        /// Delaunay �ﰢ������ �����մϴ�.(���� ����� �Է����� ����)
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns></returns>
        public List<Triangle> Triangulate(List<Vertex> vertices )
        {
            // �ﰢ�� ��� �ʱ�ȭ
            List<Triangle>  triangles = new List<Triangle>();
            // �Էµ� ������ 3�� �̸��� ��� �ﰢ���� ���� �� ����
            if (vertices.Count < 3)
            {
                Debug.LogWarning("Not enough vertices to form a triangle.");
                return triangles;
            }
            // ���� �ﰢ�� ���� �� �߰�
            Triangulate(vertices, triangles);
            return triangles;
        }

        /// <summary>
        /// Delaunay �ﰢ������ �����մϴ�. (���� ��ϰ� �ﰢ�� ����� �Է����� ����)
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="triangles"></param>
        private void Triangulate(List<Vertex> vertices, List<Triangle> triangles)
        {
            // ���� �ﰢ�� ����
            Triangle superTriangle = CreateSuperTriangle(vertices);

            // ���� �ﰢ���� �ﰢ�� ��Ͽ� �߰�
            triangles.Add(superTriangle);

            List<Triangle> badTriangles = new List<Triangle>();
            HashSet<Edge> polygon = new HashSet<Edge>();
            // ��� ������ ��ȸ�ϸ� �ﰢ���� ����
            for (int i = 0; i< vertices.Count; i++)
            {
                // badTriangles ��� �ʱ�ȭ
                badTriangles.Clear();
                // ���� ������ ������
                Vertex vertex = vertices[i];
                // ���� ������ ���Ե� �ﰢ���� ã��
                for(int j = 0; j< triangles.Count;j++) 
                {
                    Triangle triangle = triangles[j];
                    // ���� ������ �ﰢ���� ������ �ȿ� �ִ��� �˻�
                    if (triangle.IsPointInsideCirCle(vertex.Pos))
                    {
                        // badTriangles ��Ͽ� �߰�
                        badTriangles.Add(triangle);
                    }
                }
                // �ܰ����� �����
                polygon.Clear();
                foreach (Triangle triangle in badTriangles)
                {
                    // �ٸ� badTriangles�� �������� �ʴ� ������ polygon�� �߰�
                    foreach (Edge edge in triangle.Edges)
                    {
                        if (!polygon.Contains(edge))
                        {
                            polygon.Add(edge);
                        }
                        else
                        {
                            polygon.Remove(edge);
                        }
                    }
                }
                // ���� �ﰢ�� ���� 
                foreach (Triangle triangle in badTriangles)
                {
                    triangles.Remove(triangle);
                }
                // polygon�� �ִ� ������ ����Ͽ� ���ο� �ﰢ�� ����
                foreach (Edge edge in polygon)
                {
                    // ���ο� �ﰢ�� ����
                    Triangle newTriangle = new Triangle(edge.A, edge.B, vertex);
                    triangles.Add(newTriangle);
                }

            }
            // ���� �ﰢ���� ���Ե� �ﰢ������ ����
            for (int i = triangles.Count - 1; i >= 0; i--)
            {
                Triangle triangle = triangles[i];
                if (triangle.ContainVertex(superTriangle.A) ||
                    triangle.ContainVertex(superTriangle.B) ||
                    triangle.ContainVertex(superTriangle.C))
                {
                    triangles.RemoveAt(i);
                }
            }
        }

        private Triangle CreateSuperTriangle(List<Vertex> vertices)
        {
            // ���� �ﰢ���� �����ϱ� ���� �ּ�/�ִ� ��ǥ �ʱ�ȭ
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            // ��� ������ ��ǥ�� ��ȸ�Ͽ� �ּ�/�ִ� ��ǥ�� ã��

            for (int i = 0; i < vertices.Count; i++)
            {
                Vector2 pos = vertices[i].Pos;
                if (pos.x < minX)
                    minX = pos.x;
                if (pos.y < minY)
                    minY = pos.y;
                if (pos.x > maxX)
                    maxX = pos.x;
                if (pos.y > maxY)
                    maxY = pos.y;
            }

            // ���� �ﰢ���� ũ�⸦ ����
            float dx = maxX - minX;
            float dy = maxY - minY;
            float dmax = Mathf.Max(dx, dy) * 5f;

            // ���� �ﰢ���� �߽� ��ǥ ���

            Vector2 center = new Vector2((minX + maxX) / 2, (minY + maxY) / 2);

            // ���� �ﰢ���� ���� ��ǥ ���
            Vertex a = new Vertex(new Vector2(center.x - 2 * dmax, center.y - dmax));
            Vertex b = new Vertex(new Vector2(center.x + 2 * dmax, center.y - dmax));
            Vertex c = new Vertex(new Vector2(center.x, center.y + 2 * dmax));

            return new Triangle(a, b, c);
        }
    }
}