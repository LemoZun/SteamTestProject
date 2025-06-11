using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Procedural_Map_Generation
{
    /// <summary>
    /// Delaunay + MST + 루프 연결 수행
    /// </summary>
    public class RoomGraphBuilder
    {
        public List<Triangle> Triangles { get; private set; }
        public List<Edge> Edges { get; private set; }
        public List<Edge> MSTEdges { get; private set; }

        public Dictionary<Vertex, Room> VertexToRoom = new Dictionary<Vertex, Room>();

        /// <summary>
        /// 방들을 연결합니다. MST를 사용하여 방 간의 최적 경로를 생성합니다.
        /// </summary>
        public Dictionary<Vertex,Room> ConnectRooms(List<Room> rooms)
        {
            VertexToRoom.Clear();

            foreach (Room room in rooms)
            {
                if (VertexToRoom.ContainsKey(room.Vertex) == false)
                {
                    VertexToRoom[room.Vertex] = room;
                }
            }
            return VertexToRoom;
        }

        /// <summary>
        /// 방 목록을 입력으로 받아 Delaunay 삼각분할을 수행하고, MST를 생성합니다.
        /// </summary>
        public void Generate(List<Room> rooms, DelaunayTriangulator triangulator)
        {
            Triangles = triangulator.Triangulate(rooms);
            Edges = GetAllEdgeFromTriangle(Triangles);
            MSTEdges = MSTUtility.GetMST(Edges);
        }

        /// <summary>
        /// MST에 추가적인 엣지를 삽입합니다.
        /// </summary>
        /// <param name="edges"></param>
        public void AddAddtonalMSTEdge(int _maxAddtionalEdgeCount)
        {
            // 추가적인 MST 엣지 삽입 로직
            // 전체 엣지 중에서 MST에 포함되지 않은 엣지를 찾기
            List<Edge> extraEdge = GetExtraEdge(_maxAddtionalEdgeCount);

            // 엣지 n개 추가
            if (extraEdge.Count > 0)
            {
                int count = Random.Range(0, Mathf.Min(_maxAddtionalEdgeCount, extraEdge.Count));
                while (count > 0)
                {
                    int index = Random.Range(0, extraEdge.Count);
                    Edge edge = extraEdge[index];
                    // 방 간 연결
                    RoomConnector.ConnectRooms(edge, VertexToRoom);

                    // MST에 추가
                    MSTEdges.Add(edge);

                    edge.Type = Edge.EdgeType.Loop; // 추가로 생성된 엣지 표지

                    // 엣지 리스트에서 제거
                    extraEdge.RemoveAt(index);
                    count--;
                }
            }
        }

        /// <summary>
        /// 추가 엣지를 가져옵니다. MST에 포함되지 않은 엣지 중에서 랜덤하게 선택합니다.
        /// </summary>
        private List<Edge> GetExtraEdge(int connt)
        {
            List<Edge> extraEdge = Edges.Where(edge => MSTEdges.Contains(edge) == false).ToList();
            return extraEdge.OrderBy(edge => Random.value).Take(connt).ToList();
        }

        /// <summary>
        /// 모든 삼각형에서 엣지를 추출하여 리스트로 반환합니다. 중복된 엣지는 제거됩니다.
        /// </summary>
        private static List<Edge> GetAllEdgeFromTriangle(List<Triangle> triangles)
        {
            List<Edge> edges = new List<Edge>();
            foreach (Triangle triangle in triangles)
            {
                // 각 삼각형의 엣지를 가져와서 edges 리스트에 추가
                foreach (Edge edge in triangle.Edges)
                {
                    // 엣지가 이미 존재하지 않는 경우에만 추가
                    if (!edges.Contains(edge))
                    {
                        edges.Add(edge);
                    }
                }
            }
            return edges;
        }
    }
}