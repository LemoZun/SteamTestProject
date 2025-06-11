using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Procedural_Map_Generation
{
    public static class MapGenerationUtility
    {

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
