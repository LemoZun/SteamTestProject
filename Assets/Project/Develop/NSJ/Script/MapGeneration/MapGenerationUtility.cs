using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Procedural_Map_Generation
{
    public static class MapGenerationUtility
    {
        /// <summary>
        /// 방을 연결합니다. (Edge를 통해 방을 연결)
        /// </summary>
        public static void ConnectRooms(List<Edge> edges, Dictionary<Vertex, Room> vertexToRoom)
        {
            //edges를 순회하며 방을 연결
            foreach (Edge edge in edges)
            {
                Room roomA = vertexToRoom[edge.A];
                Room roomB = vertexToRoom[edge.B];
                // 두 방을 연결
                ConnectRooms(roomA, roomB);
            }
        }

        /// <summary>
        /// 두 방을 연결합니다. (양방향 연결)
        /// </summary>
        private static void ConnectRooms(Room roomA, Room roomB)
        {
            // 두 방을 서로 연결
            if (!roomA.ConnectedRooms.Contains(roomB))
            {
                roomA.ConnectedRooms.Add(roomB);
            }
            if (!roomB.ConnectedRooms.Contains(roomA))
            {
                roomB.ConnectedRooms.Add(roomA);
            }
        }

        /// <summary>
        /// 방 간 거리를 계산합니다. (BFS를 사용하여 시작 방으로부터의 거리 계산)
        /// </summary>
        public static Dictionary<Room, int> CalculateDistanceFrom(Room start)
        {
            // 방 간 거리 계산(BFS) 사용
            Dictionary<Room, int> distances = new Dictionary<Room, int>();
            Queue<Room> queue = new Queue<Room>();

            // 시작 방의 거리는 0으로 설정
            distances.Add(start, 0);
            queue.Enqueue(start);

            // BFS를 이용하여 방 간 거리 계산
            while (queue.Count > 0)
            {
                Room current = queue.Dequeue();
                // 현재 방의 이웃 방들을 순회
                foreach (Room neighbor in current.ConnectedRooms)
                {
                    // 이웃 방이 아직 방문되지 않았다면
                    if (!distances.ContainsKey(neighbor))
                    {
                        // 거리를 현재 방의 거리 + 1로 설정
                        distances.Add(neighbor, distances[current] + 1);
                        queue.Enqueue(neighbor);
                    }
                }
            }
            return distances;
        }
   }
}
