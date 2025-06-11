using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Procedural_Map_Generation
{
    public class MapGenerator : MonoBehaviour
    {
        /// <summary>
        /// �� ����Դϴ�.
        /// </summary>
        [Header("Room")]
        [Tooltip("�� ���")]
        [SerializeField] private List<Room> _rooms;

        [Tooltip("���� �ִ� ����")]
        [SerializeField] private int _roomCount = 10;
        [Tooltip("�߰� �� ����")]
        [SerializeField] private int _maxAddtionalEdgeCount = 2;

        DelaunayTriangulator _delaunayTriangulator = new DelaunayTriangulator();
        RoomGraphBuilder _roomGraphBuilder = new RoomGraphBuilder();    
        RoomClassifier _roomClassifier = new RoomClassifier();
        CorridorGenerator _corridorGenerator = new CorridorGenerator();

        private Room _startRoom;

        private void Start()
        {
            GeneratorMap();
        }

        private void GeneratorMap()
        {
            // �� ��� �ʱ�ȭ
            SetRoomCoordinate();

            // �� ����� �Է����� �޾� Delaunay �ﰢ������ �����ϰ�, MST�� �����մϴ�.
            _roomGraphBuilder.Generate(_rooms, _delaunayTriangulator);

            // �� �ĺ� ã��
            FindRoomCandidates();

            // �߰����� ���� ���� (����)
            _roomGraphBuilder.AddAddtonalMSTEdge(_maxAddtionalEdgeCount);

            // ���� ����
            GenerateCorridors();
        }

        private void SetRoomCoordinate()
        {
            // ���� ���� �����ϰ� ����
            while (_rooms.Count > _roomCount)
            {
                int index = Random.Range(0, _rooms.Count);
                Destroy(_rooms[index].gameObject);
                _rooms.RemoveAt(index);
            }

            // ���� ���� ��ǥ���� �ణ�� ������ ����
            foreach (Room room in _rooms)
            {
                Vector3 noise = new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), 0);
                room.transform.position += noise;
            }

            //���� �߽��� ����
            foreach (Room room in _rooms)
            {
                room.SetVertex();
            }
        }

        private void FindRoomCandidates()
        {
            // �� �� ����
            ConnectRooms(_rooms);

            // ���� ����
            _startRoom = _rooms[0];
            _startRoom.Type = Room.RoomType.Start;

            // �� �� �Ÿ� ���(DPS) ���
            Dictionary<Room, int> distances = MapGenerationUtility.CalculateDistanceFrom(_startRoom);

            // �� �з�
            _roomClassifier.ClassifyRooms(_rooms, _startRoom);

            // ���� �ʱ�ȭ
            foreach (Room room in _rooms)
            {
                room.InitRoom();
            }

        }

        /// <summary>
        /// ����� �����մϴ�. MST�� ����Ͽ� �� ���� ���� ��θ� �����մϴ�.
        /// </summary>
        private void ConnectRooms(List<Room> rooms)
        {
            Dictionary<Vertex,Room> vertexToRoom = _roomGraphBuilder.ConnectRooms(rooms);

            RoomConnector.ConnectRooms(_roomGraphBuilder.MSTEdges,_roomGraphBuilder.VertexToRoom);
        }

        /// <summary>
        /// ������ �����մϴ�
        /// </summary>
        private void GenerateCorridors()
        {
            _corridorGenerator.GenerateCorridors(_roomGraphBuilder.MSTEdges, _roomGraphBuilder.VertexToRoom);
        }

        private void OnDrawGizmos()
        {
            if (_roomGraphBuilder.Triangles == null)
                return;

            //Gizmos.color = Color.cyan;

            //foreach (Triangle triangle in _triangles)
            //{
            //    Gizmos.DrawLine(triangle.A.Pos, triangle.B.Pos);
            //    Gizmos.DrawLine(triangle.B.Pos, triangle.C.Pos);
            //    Gizmos.DrawLine(triangle.C.Pos, triangle.A.Pos);
            //}

       

            //foreach (Edge edge in _roomGraphBuilder.MSTEdges)
            //{
            //    if(edge.Type == Edge.EdgeType.MST)
            //    {
            //        Gizmos.color = Color.red;
            //        Gizmos.DrawLine(edge.A.Pos, edge.B.Pos);
            //    }
            //    else
            //    {
            //        Gizmos.color = Color.magenta;
            //        Gizmos.DrawLine(edge.A.Pos, edge.B.Pos);
            //    }
            //}

        }
    }
}