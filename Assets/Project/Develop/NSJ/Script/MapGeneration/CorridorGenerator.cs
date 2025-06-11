using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Procedural_Map_Generation
{
    /// <summary>
    /// 복도 좌표 생성 및 배치
    /// </summary>
    public class CorridorGenerator
    {
        /// <summary>
        /// 복도 생성합니다.
        /// </summary>
        /// <param name="mstEdges"></param>
        /// <param name="vertexToRoom"></param>
        public void GenerateCorridors(List<Edge> mstEdges, Dictionary<Vertex, Room> vertexToRoom)
        {
            foreach (Edge edge in mstEdges)
            {
                // 복도 A 지정
                Room roomA = vertexToRoom[edge.A];
                // 복도 B 지정
                Room roomB = vertexToRoom[edge.B];

                List<Vector2Int> corridor = GenerateLShapeGenerator(roomA, roomB);

                foreach (var tilePos in corridor)
                {
                    // 복도 오브젝트 생성 로직(임시 디버그로 찍음)
                    Debug.DrawLine((Vector2)tilePos, (Vector2)tilePos + Vector2.one * 0.2f, Color.yellow, 5f);
                }

            }
        }

        /// <summary>
        /// L자형 복도 좌표를 생성합니다. (방 객체를 입력으로 받음)
        /// </summary>
        private List<Vector2Int> GenerateLShapeGenerator(Room roomA, Room roomB)
        {
            Vector2Int posA = roomA.GetCenterGridPosition();
            Vector2Int posB = roomB.GetCenterGridPosition();

            return GenerateLShapeGenerator(posA, posB);
        }

        /// <summary>
        /// L자형 복도 좌표를 생성합니다.
        /// </summary>
        public List<Vector2Int> GenerateLShapeGenerator(Vector2Int posA, Vector2Int posB)
        {
            // 복도 좌표를 저장할 리스트
            List<Vector2Int> path = new List<Vector2Int>();

            // x 방향 이동
            int x = posA.x;
            while( x != posB.x)
            {
                path.Add(new Vector2Int(x, posA.y));
                x += (posB.x > x) ? 1 : -1;
            }

            // y 방향 이동
            int y = posA.y;
            while(y != posB.y)
            {
                path.Add(new Vector2Int(posB.x, y));
                y += (posB.y > y) ? 1 : -1;
            }

            path.Add(posB); // 마지막 좌표 추가

            return path;
        }
    }
}