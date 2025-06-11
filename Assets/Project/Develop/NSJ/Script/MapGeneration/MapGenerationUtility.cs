using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Procedural_Map_Generation
{
    public static class MapGenerationUtility
    {
        /// <summary>
        /// ���� �����մϴ�. (Edge�� ���� ���� ����)
        /// </summary>
        public static void ConnectRooms(List<Edge> edges, Dictionary<Vertex, Room> vertexToRoom)
        {
            //edges�� ��ȸ�ϸ� ���� ����
            foreach (Edge edge in edges)
            {
                Room roomA = vertexToRoom[edge.A];
                Room roomB = vertexToRoom[edge.B];
                // �� ���� ����
                ConnectRooms(roomA, roomB);
            }
        }

        /// <summary>
        /// �� ���� �����մϴ�. (����� ����)
        /// </summary>
        private static void ConnectRooms(Room roomA, Room roomB)
        {
            // �� ���� ���� ����
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
        /// �� �� �Ÿ��� ����մϴ�. (BFS�� ����Ͽ� ���� �����κ����� �Ÿ� ���)
        /// </summary>
        public static Dictionary<Room, int> CalculateDistanceFrom(Room start)
        {
            // �� �� �Ÿ� ���(BFS) ���
            Dictionary<Room, int> distances = new Dictionary<Room, int>();
            Queue<Room> queue = new Queue<Room>();

            // ���� ���� �Ÿ��� 0���� ����
            distances.Add(start, 0);
            queue.Enqueue(start);

            // BFS�� �̿��Ͽ� �� �� �Ÿ� ���
            while (queue.Count > 0)
            {
                Room current = queue.Dequeue();
                // ���� ���� �̿� ����� ��ȸ
                foreach (Room neighbor in current.ConnectedRooms)
                {
                    // �̿� ���� ���� �湮���� �ʾҴٸ�
                    if (!distances.ContainsKey(neighbor))
                    {
                        // �Ÿ��� ���� ���� �Ÿ� + 1�� ����
                        distances.Add(neighbor, distances[current] + 1);
                        queue.Enqueue(neighbor);
                    }
                }
            }
            return distances;
        }
   }
}
