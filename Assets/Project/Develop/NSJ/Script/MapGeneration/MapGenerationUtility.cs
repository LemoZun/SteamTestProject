using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Procedural_Map_Generation
{
    public static class MapGenerationUtility
    {

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
