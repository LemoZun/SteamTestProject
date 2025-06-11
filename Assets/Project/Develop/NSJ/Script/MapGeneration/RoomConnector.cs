using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Procedural_Map_Generation
{
    /// <summary>
    /// Edge 기반 Room 연결 수행
    /// </summary>
    public static class RoomConnector
    {
        /// <summary>
        /// Edge를 통해 두 방을 연결합니다. (Edge를 통해 방을 연결)
        /// </summary>

        public static void ConnectRooms(Edge edge, Dictionary<Vertex, Room> vertexToRoom)
        {
            Room roomA = vertexToRoom[edge.A];
            Room roomB = vertexToRoom[edge.B];
            // 두 방을 연결
            ConnectRooms(roomA, roomB);
        }

        /// <summary>
        /// 방을 연결합니다. (Edge 리스트를 통해 방을 연결)
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
    }
}