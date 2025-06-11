using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Procedural_Map_Generation
{
    /// <summary>
    /// Edge ��� Room ���� ����
    /// </summary>
    public static class RoomConnector
    {
        /// <summary>
        /// Edge�� ���� �� ���� �����մϴ�. (Edge�� ���� ���� ����)
        /// </summary>

        public static void ConnectRooms(Edge edge, Dictionary<Vertex, Room> vertexToRoom)
        {
            Room roomA = vertexToRoom[edge.A];
            Room roomB = vertexToRoom[edge.B];
            // �� ���� ����
            ConnectRooms(roomA, roomB);
        }

        /// <summary>
        /// ���� �����մϴ�. (Edge ����Ʈ�� ���� ���� ����)
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
    }
}