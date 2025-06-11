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

        /// <summary>
        /// Vertex(���� �߽���)�� Room(��) ���� ������ ���� ��ųʸ��Դϴ�.
        /// </summary>

        DelaunayTriangulator _delaunayTriangulator = new DelaunayTriangulator();

        List<Triangle> _triangles;

        List<Edge> _mstEdges;

        private Room _startRoom;

        private void Start()
        {
            GeneratorMap();
        }

        private void GeneratorMap()
        {
            // ���� ���� �����ϰ� ����

            while (_rooms.Count > _roomCount)
            {
                int index = Random.Range(0, _rooms.Count);
                Destroy(_rooms[index].gameObject);
                _rooms.RemoveAt(index);
            }

            // ���� ���� ��ǥ���� �ణ�� ������ ����

            foreach(Room room in _rooms)
            {
                Vector3 noise = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);
                room.transform.position += noise;
            }   

            //���� �߽��� ����
            foreach (Room room in _rooms)
            {
                room.SetVertex();
            }

            // ��γ� �ﰢ���� ����
            _triangles = _delaunayTriangulator.Triangulate(_rooms);

            // �ּ� ���� Ʈ��(MST) ����
            _mstEdges = MSTUtility.GetMST(_triangles);

            // �� �ĺ� ã��
            FindRoomCandidates();

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

            // ���� �� ���� ã�Ƽ� ���� ������ ����   
            SetBossRoom(distances);

            // ��Ʈ �б� ���� Ư���� ����
            SetSpecialRooms(_rooms);

            // �� ���� ����� �Ϲݹ� ó��(Type�� UnSet�� ��)
            SetNormalRooms(_rooms);

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
            Dictionary<Vertex, Room> vertexToRoom = new Dictionary<Vertex, Room>();

            foreach (Room room in rooms)
            {
                if (!vertexToRoom.ContainsKey(room.Vertex))
                {
                    vertexToRoom[room.Vertex] = room;
                }
            }

            MapGenerationUtility.ConnectRooms(_mstEdges, vertexToRoom);
        }

        /// <summary>
        /// ���� ���� �����մϴ�. ���� �� ���� ���� ������ �����մϴ�.
        /// </summary>
        private void SetBossRoom(Dictionary<Room, int> distances)
        {
            // ���� �� �� ã��
            Room bossRoom = distances.OrderByDescending(x => x.Value).FirstOrDefault().Key;

            // ���� ���� null�� �ƴ� ��� Ÿ���� Boss�� ����
            if (bossRoom != null)
            {
                bossRoom.Type = Room.RoomType.Boss;
            }
        }

        /// <summary>
        /// ��Ʈ �б� ���� Ư���� ����
        /// </summary>
        private void SetSpecialRooms(List<Room> rooms)
        {
            // ���� ���(����� ���� 1���� ���̸� Ÿ���� �������� ���� ��) ã��
            List<Room> leafRooms = rooms.Where(room => (room.ConnectedRooms.Count == 1) && room.Type == Room.RoomType.UnSet).ToList();

            // Ư�� �� Ÿ�� ���� ����
            Stack<Room.RoomType> specialRoomTypes = new Stack<Room.RoomType>();
            specialRoomTypes.Push(Room.RoomType.Special);
            specialRoomTypes.Push(Room.RoomType.Shop);
            specialRoomTypes.Push(Room.RoomType.Secret);

            List<Room> normalRooms = null;
            // Ư�� �� Ÿ���� ������ ���� ����� �������� ������ �Ϲ� ���� ã�� ���� �Ϲ� �� ����Ʈ ����
            if ( specialRoomTypes.Count > leafRooms.Count)
            {
                // �Ϲ� �� ã��: Type�� UnSet�� ��� �߿��� ����� ���� 2�� �̻��� ���
                normalRooms = rooms.Where(room => (room.ConnectedRooms.Count > 1) && room.Type == Room.RoomType.UnSet).ToList();
            }

            // Ư�� �� Ÿ���� �������� ������ �ݺ�
            while (specialRoomTypes.Count > 0)
            {
           
                if (leafRooms.Count == 0)
                {
                    // ���� ��尡 ������ ���� ��� �ٷ� �����ִ� �Ϲ� �� �߿��� �������� ���� �����ϰ�, Ư�� �� Ÿ�� ���ÿ��� Ÿ���� ������ ����
                    int index = Random.Range(0, normalRooms.Count);
                    normalRooms[index].Type = specialRoomTypes.Pop();
                    normalRooms.RemoveAt(index);
                }
                else
                {
                    // ���� ��忡�� �������� ���� �����ϰ�, Ư�� �� Ÿ�� ���ÿ��� Ÿ���� ������ ����
                    int index = Random.Range(0, leafRooms.Count);
                    leafRooms[index].Type = specialRoomTypes.Pop();
                    leafRooms.RemoveAt(index);
                }

            }
        }

        /// <summary>
        /// �Ϲ� ���� �����մϴ�. Type�� UnSet�� ����� Normal�� �����մϴ�.
        /// </summary>
        private void SetNormalRooms(List<Room> rooms)
        {
            // �Ϲ� �� ó��: Type�� UnSet�� ���
            foreach (Room room in rooms)
            {
                if (room.Type == Room.RoomType.UnSet)
                {
                    room.Type = Room.RoomType.Normal;
                }
            }
        }


        private void OnDrawGizmos()
        {
            if (_triangles == null)
                return;

            //Gizmos.color = Color.cyan;

            //foreach (Triangle triangle in _triangles)
            //{
            //    Gizmos.DrawLine(triangle.A.Pos, triangle.B.Pos);
            //    Gizmos.DrawLine(triangle.B.Pos, triangle.C.Pos);
            //    Gizmos.DrawLine(triangle.C.Pos, triangle.A.Pos);
            //}

            Gizmos.color = Color.red;

            foreach (Edge edge in _mstEdges)
            {
                Gizmos.DrawLine(edge.A.Pos, edge.B.Pos);
            }
        }
    }
}