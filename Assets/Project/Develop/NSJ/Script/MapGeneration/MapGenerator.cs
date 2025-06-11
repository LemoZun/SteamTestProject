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
        [Tooltip("추가 길 개수")]
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
            // 방 목록 초기화
            SetRoomCoordinate();

            // 방 목록을 입력으로 받아 Delaunay 삼각분할을 수행하고, MST를 생성합니다.
            _roomGraphBuilder.Generate(_rooms, _delaunayTriangulator);

            // 방 후보 찾기
            FindRoomCandidates();

            // 추가적인 연결 삽입 (루프)
            _roomGraphBuilder.AddAddtonalMSTEdge(_maxAddtionalEdgeCount);

            // 복도 생성
            GenerateCorridors();
        }

        private void SetRoomCoordinate()
        {
            // 방의 개수 랜덤하게 삭제
            while (_rooms.Count > _roomCount)
            {
                int index = Random.Range(0, _rooms.Count);
                Destroy(_rooms[index].gameObject);
                _rooms.RemoveAt(index);
            }

            // 남은 방의 좌표값에 약간의 노이즈 적용
            foreach (Room room in _rooms)
            {
                Vector3 noise = new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), 0);
                room.transform.position += noise;
            }

            //방의 중심점 지정
            foreach (Room room in _rooms)
            {
                room.SetVertex();
            }
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

            // 방 분류
            _roomClassifier.ClassifyRooms(_rooms, _startRoom);

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
            Dictionary<Vertex,Room> vertexToRoom = _roomGraphBuilder.ConnectRooms(rooms);

            RoomConnector.ConnectRooms(_roomGraphBuilder.MSTEdges,_roomGraphBuilder.VertexToRoom);
        }

        /// <summary>
        /// 복도를 생성합니다
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