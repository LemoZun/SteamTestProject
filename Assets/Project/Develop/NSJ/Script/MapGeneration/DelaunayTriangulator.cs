using System.Collections.Generic;
using UnityEngine;

namespace Procedural_Map_Generation
{
    public class DelaunayTriangulator
    {
        /// <summary>
        /// Delaunay 삼각분할을 수행합니다.(Room 목록을 입력으로 받음)
        /// </summary>
        /// <param name="rooms"></param>
        /// <returns></returns>
        public List<Triangle> Triangulate(List<Room> rooms)
        {
            // 방의 정점 목록 생성
            List<Vertex> vertices = new List<Vertex>();
            foreach (Room room in rooms)
            {
                vertices.Add(room.Vertex);
            }
            // 삼각형 생성
            return Triangulate(vertices);
        }

        /// <summary>
        /// Delaunay 삼각분할을 수행합니다.(정점 목록을 입력으로 받음)
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns></returns>
        public List<Triangle> Triangulate(List<Vertex> vertices )
        {
            // 삼각형 목록 초기화
            List<Triangle>  triangles = new List<Triangle>();
            // 입력된 정점이 3개 미만인 경우 삼각형을 만들 수 없음
            if (vertices.Count < 3)
            {
                Debug.LogWarning("Not enough vertices to form a triangle.");
                return triangles;
            }
            // 슈퍼 삼각형 생성 및 추가
            Triangulate(vertices, triangles);
            return triangles;
        }

        /// <summary>
        /// Delaunay 삼각분할을 수행합니다. (정점 목록과 삼각형 목록을 입력으로 받음)
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="triangles"></param>
        private void Triangulate(List<Vertex> vertices, List<Triangle> triangles)
        {
            // 슈퍼 삼각형 생성
            Triangle superTriangle = CreateSuperTriangle(vertices);

            // 슈퍼 삼각형을 삼각형 목록에 추가
            triangles.Add(superTriangle);

            List<Triangle> badTriangles = new List<Triangle>();
            HashSet<Edge> polygon = new HashSet<Edge>();
            // 모든 정점을 순회하며 삼각형을 생성
            for (int i = 0; i< vertices.Count; i++)
            {
                // badTriangles 목록 초기화
                badTriangles.Clear();
                // 현재 정점을 가져옴
                Vertex vertex = vertices[i];
                // 현재 정점이 포함된 삼각형을 찾음
                for(int j = 0; j< triangles.Count;j++) 
                {
                    Triangle triangle = triangles[j];
                    // 현재 정점이 삼각형의 외접원 안에 있는지 검사
                    if (triangle.IsPointInsideCirCle(vertex.Pos))
                    {
                        // badTriangles 목록에 추가
                        badTriangles.Add(triangle);
                    }
                }
                // 외곽선만 남기기
                polygon.Clear();
                foreach (Triangle triangle in badTriangles)
                {
                    // 다른 badTriangles와 공유하지 않는 엣지를 polygon에 추가
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
                // 기존 삼각형 제거 
                foreach (Triangle triangle in badTriangles)
                {
                    triangles.Remove(triangle);
                }
                // polygon에 있는 엣지를 사용하여 새로운 삼각형 생성
                foreach (Edge edge in polygon)
                {
                    // 새로운 삼각형 생성
                    Triangle newTriangle = new Triangle(edge.A, edge.B, vertex);
                    triangles.Add(newTriangle);
                }

            }
            // 슈퍼 삼각형에 포함된 삼각형들을 제거
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
            // 슈퍼 삼각형을 생성하기 위한 최소/최대 좌표 초기화
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            // 모든 정점의 좌표를 순회하여 최소/최대 좌표를 찾음

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

            // 슈퍼 삼각형의 크기를 결정
            float dx = maxX - minX;
            float dy = maxY - minY;
            float dmax = Mathf.Max(dx, dy) * 5f;

            // 슈퍼 삼각형의 중심 좌표 계산

            Vector2 center = new Vector2((minX + maxX) / 2, (minY + maxY) / 2);

            // 슈퍼 삼각형의 정점 좌표 계산
            Vertex a = new Vertex(new Vector2(center.x - 2 * dmax, center.y - dmax));
            Vertex b = new Vertex(new Vector2(center.x + 2 * dmax, center.y - dmax));
            Vertex c = new Vertex(new Vector2(center.x, center.y + 2 * dmax));

            return new Triangle(a, b, c);
        }
    }
}