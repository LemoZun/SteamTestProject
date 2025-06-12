using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Procedural_Map_Generation
{
    public class Room : MonoBehaviour
    {
        public RoomType Type { get { return _model.Type; } set { _model.Type = value; } }

        public List<Room> ConnectedRooms;
        public Vertex Vertex;

        [SerializeField]private RoomModel _model;

        private void Awake()
        {
            _model = ModelFactory.CreateModel<RoomModel, RoomViewModel>();
        }

        /// <summary>
        /// 방의 중심점을 설정합니다. (Vertex를 생성하거나 업데이트)
        /// </summary>
        public void SetVertex()
        {
            if (Vertex == null)
            {
                Vertex = new Vertex(transform.position);
            }
            else
            {
                Vertex.Pos = transform.position;
            }
        }

        /// <summary>
        /// 방을 초기화합니다.
        /// </summary>
        public void InitRoom()
        {
            switch (Type)
            {
                case RoomType.UnSet:
                    break;
                case RoomType.Normal:
                    // 일반 방 초기화 로직
                    TestInit(Color.white);
                    break;
                case RoomType.Start:
                    // 시작 방 초기화 로직
                    TestInit(Color.green);
                    break;
                case RoomType.Boss:
                    // 보스 방 초기화 로직
                    TestInit(Color.red);
                    break;
                case RoomType.Shop:
                    // 상점 방 초기화 로직
                    TestInit(Color.cyan);
                    break;
                case RoomType.Special:
                    // 특수 방 초기화 로직
                    TestInit(Color.yellow);
                    break;
                case RoomType.Secret:
                    // 비밀 방 초기화 로직
                    TestInit(Color.magenta);
                    break;
                case RoomType.End:
                    // 엔드 방 초기화 로직    
                    break;
            }
        }

        public void TestInit(Color color)
        {
            GetComponent<SpriteRenderer>().material.color = color;
        }

        /// <summary>
        /// 방의 중심 그리드 위치를 반환
        /// </summary>
        /// <returns></returns>
        public Vector2Int GetCenterGridPosition()
        {
            return Vector2Int.RoundToInt(Vertex.Pos);
        }
    }
}