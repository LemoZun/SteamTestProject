using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Procedural_Map_Generation
{
    public class MapGenerator : MonoBehaviour
    {
        /// <summary>
        /// 방 목록입니다.
        /// </summary>
        [Header("Room")]
        [Tooltip("방 목록")]
        [SerializeField] private List<Room> _rooms;

        [Tooltip("방의 최대 개수")]
        [SerializeField] private int _roomCount = 10;

        /// <summary>
        /// Vertex(방의 중심점)과 Room(방) 간의 매핑을 위한 딕셔너리입니다.
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
            // 방의 개수 랜덤하게 삭제

            while (_rooms.Count > _roomCount)
            {
                int index = Random.Range(0, _rooms.Count);
                Destroy(_rooms[index].gameObject);
                _rooms.RemoveAt(index);
            }

            // 남은 방의 좌표값에 약간의 노이즈 적용

            foreach(Room room in _rooms)
            {
                Vector3 noise = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);
                room.transform.position += noise;
            }   

            //방의 중심점 지정
            foreach (Room room in _rooms)
            {
                room.SetVertex();
            }

            // 드로네 삼각분할 시행
            _triangles = _delaunayTriangulator.Triangulate(_rooms);

            // 최소 신장 트리(MST) 생성
            _mstEdges = MSTUtility.GetMST(_triangles);

            // 방 후보 찾기
            FindRoomCandidates();

        }

        private void FindRoomCandidates()
        {
            // 방 간 연결
            ConnectRooms(_rooms);

            // 시작 지점
            _startRoom = _rooms[0];
            _startRoom.Type = Room.RoomType.Start;

            // 방 간 거리 계산(DPS) 사용
            Dictionary<Room, int> distances = MapGenerationUtility.CalculateDistanceFrom(_startRoom);

            // 가장 먼 방을 찾아서 보스 방으로 지정   
            SetBossRoom(distances);

            // 루트 분기 끝단 특수방 지정
            SetSpecialRooms(_rooms);

            // 이 외의 방들은 일반방 처리(Type이 UnSet인 방)
            SetNormalRooms(_rooms);

            // 방을 초기화
            foreach (Room room in _rooms)
            {
                room.InitRoom();
            }

        }

        /// <summary>
        /// 방들을 연결합니다. MST를 사용하여 방 간의 최적 경로를 생성합니다.
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
        /// 보스 방을 선택합니다. 가장 먼 방을 보스 방으로 지정합니다.
        /// </summary>
        private void SetBossRoom(Dictionary<Room, int> distances)
        {
            // 가장 먼 방 찾기
            Room bossRoom = distances.OrderByDescending(x => x.Value).FirstOrDefault().Key;

            // 보스 방이 null이 아닐 경우 타입을 Boss로 설정
            if (bossRoom != null)
            {
                bossRoom.Type = Room.RoomType.Boss;
            }
        }

        /// <summary>
        /// 루트 분기 끝단 특수방 지정
        /// </summary>
        private void SetSpecialRooms(List<Room> rooms)
        {
            // 리프 노드(연결된 방이 1개인 방이며 타입이 정해지지 않은 방) 찾기
            List<Room> leafRooms = rooms.Where(room => (room.ConnectedRooms.Count == 1) && room.Type == Room.RoomType.UnSet).ToList();

            // 특수 방 타입 스택 생성
            Stack<Room.RoomType> specialRoomTypes = new Stack<Room.RoomType>();
            specialRoomTypes.Push(Room.RoomType.Special);
            specialRoomTypes.Push(Room.RoomType.Shop);
            specialRoomTypes.Push(Room.RoomType.Secret);

            List<Room> normalRooms = null;
            // 특수 방 타입의 개수가 리프 노드의 개수보다 많으면 일반 방을 찾기 위해 일반 방 리스트 생성
            if ( specialRoomTypes.Count > leafRooms.Count)
            {
                // 일반 방 찾기: Type이 UnSet인 방들 중에서 연결된 방이 2개 이상인 방들
                normalRooms = rooms.Where(room => (room.ConnectedRooms.Count > 1) && room.Type == Room.RoomType.UnSet).ToList();
            }

            // 특수 방 타입이 남아있을 때까지 반복
            while (specialRoomTypes.Count > 0)
            {
           
                if (leafRooms.Count == 0)
                {
                    // 리프 노드가 없으면 리프 노드 바로 옆에있는 일반 방 중에서 랜덤으로 방을 선택하고, 특수 방 타입 스택에서 타입을 꺼내서 설정
                    int index = Random.Range(0, normalRooms.Count);
                    normalRooms[index].Type = specialRoomTypes.Pop();
                    normalRooms.RemoveAt(index);
                }
                else
                {
                    // 리프 노드에서 랜덤으로 방을 선택하고, 특수 방 타입 스택에서 타입을 꺼내서 설정
                    int index = Random.Range(0, leafRooms.Count);
                    leafRooms[index].Type = specialRoomTypes.Pop();
                    leafRooms.RemoveAt(index);
                }

            }
        }

        /// <summary>
        /// 일반 방을 설정합니다. Type이 UnSet인 방들을 Normal로 설정합니다.
        /// </summary>
        private void SetNormalRooms(List<Room> rooms)
        {
            // 일반 방 처리: Type이 UnSet인 방들
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