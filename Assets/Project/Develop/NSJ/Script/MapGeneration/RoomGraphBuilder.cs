using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Procedural_Map_Generation
{
    /// <summary>
    /// Delaunay + MST + ���� ���� ����
    /// </summary>
    public class RoomGraphBuilder
    {
        public List<Triangle> Triangles { get; private set; }
        public List<Edge> Edges { get; private set; }
        public List<Edge> MSTEdges { get; private set; }

        public Dictionary<Vertex, Room> VertexToRoom = new Dictionary<Vertex, Room>();

        /// <summary>
        /// ����� �����մϴ�. MST�� ����Ͽ� �� ���� ���� ��θ� �����մϴ�.
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
        /// �� ����� �Է����� �޾� Delaunay �ﰢ������ �����ϰ�, MST�� �����մϴ�.
        /// </summary>
        public void Generate(List<Room> rooms, DelaunayTriangulator triangulator)
        {
            Triangles = triangulator.Triangulate(rooms);
            Edges = GetAllEdgeFromTriangle(Triangles);
            MSTEdges = MSTUtility.GetMST(Edges);
        }

        /// <summary>
        /// MST�� �߰����� ������ �����մϴ�.
        /// </summary>
        /// <param name="edges"></param>
        public void AddAddtonalMSTEdge(int _maxAddtionalEdgeCount)
        {
            // �߰����� MST ���� ���� ����
            // ��ü ���� �߿��� MST�� ���Ե��� ���� ������ ã��
            List<Edge> extraEdge = GetExtraEdge(_maxAddtionalEdgeCount);

            // ���� n�� �߰�
            if (extraEdge.Count > 0)
            {
                int count = Random.Range(0, Mathf.Min(_maxAddtionalEdgeCount, extraEdge.Count));
                while (count > 0)
                {
                    int index = Random.Range(0, extraEdge.Count);
                    Edge edge = extraEdge[index];
                    // �� �� ����
                    RoomConnector.ConnectRooms(edge, VertexToRoom);

                    // MST�� �߰�
                    MSTEdges.Add(edge);

                    edge.Type = Edge.EdgeType.Loop; // �߰��� ������ ���� ǥ��

                    // ���� ����Ʈ���� ����
                    extraEdge.RemoveAt(index);
                    count--;
                }
            }
        }

        /// <summary>
        /// �߰� ������ �����ɴϴ�. MST�� ���Ե��� ���� ���� �߿��� �����ϰ� �����մϴ�.
        /// </summary>
        private List<Edge> GetExtraEdge(int connt)
        {
            List<Edge> extraEdge = Edges.Where(edge => MSTEdges.Contains(edge) == false).ToList();
            return extraEdge.OrderBy(edge => Random.value).Take(connt).ToList();
        }

        /// <summary>
        /// ��� �ﰢ������ ������ �����Ͽ� ����Ʈ�� ��ȯ�մϴ�. �ߺ��� ������ ���ŵ˴ϴ�.
        /// </summary>
        private static List<Edge> GetAllEdgeFromTriangle(List<Triangle> triangles)
        {
            List<Edge> edges = new List<Edge>();
            foreach (Triangle triangle in triangles)
            {
                // �� �ﰢ���� ������ �����ͼ� edges ����Ʈ�� �߰�
                foreach (Edge edge in triangle.Edges)
                {
                    // ������ �̹� �������� �ʴ� ��쿡�� �߰�
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