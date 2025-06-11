using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Procedural_Map_Generation
{
    /// <summary>
    /// ���� ��ǥ ���� �� ��ġ
    /// </summary>
    public class CorridorGenerator
    {
        /// <summary>
        /// ���� �����մϴ�.
        /// </summary>
        /// <param name="mstEdges"></param>
        /// <param name="vertexToRoom"></param>
        public void GenerateCorridors(List<Edge> mstEdges, Dictionary<Vertex, Room> vertexToRoom)
        {
            foreach (Edge edge in mstEdges)
            {
                // ���� A ����
                Room roomA = vertexToRoom[edge.A];
                // ���� B ����
                Room roomB = vertexToRoom[edge.B];

                List<Vector2Int> corridor = GenerateLShapeGenerator(roomA, roomB);

                foreach (var tilePos in corridor)
                {
                    // ���� ������Ʈ ���� ����(�ӽ� ����׷� ����)
                    Debug.DrawLine((Vector2)tilePos, (Vector2)tilePos + Vector2.one * 0.2f, Color.yellow, 5f);
                }

            }
        }

        /// <summary>
        /// L���� ���� ��ǥ�� �����մϴ�. (�� ��ü�� �Է����� ����)
        /// </summary>
        private List<Vector2Int> GenerateLShapeGenerator(Room roomA, Room roomB)
        {
            Vector2Int posA = roomA.GetCenterGridPosition();
            Vector2Int posB = roomB.GetCenterGridPosition();

            return GenerateLShapeGenerator(posA, posB);
        }

        /// <summary>
        /// L���� ���� ��ǥ�� �����մϴ�.
        /// </summary>
        public List<Vector2Int> GenerateLShapeGenerator(Vector2Int posA, Vector2Int posB)
        {
            // ���� ��ǥ�� ������ ����Ʈ
            List<Vector2Int> path = new List<Vector2Int>();

            // x ���� �̵�
            int x = posA.x;
            while( x != posB.x)
            {
                path.Add(new Vector2Int(x, posA.y));
                x += (posB.x > x) ? 1 : -1;
            }

            // y ���� �̵�
            int y = posA.y;
            while(y != posB.y)
            {
                path.Add(new Vector2Int(posB.x, y));
                y += (posB.y > y) ? 1 : -1;
            }

            path.Add(posB); // ������ ��ǥ �߰�

            return path;
        }
    }
}