using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Procedural_Map_Generation
{
    public class Room : MonoBehaviour
    {
        public enum RoomType
        {
            UnSet,
            Normal,
            Start,
            Boss,
            Shop,
            Special,
            Secret,
            End,
        }

        public RoomType Type;


        public List<Room> ConnectedRooms;
        public Vertex Vertex;



        /// <summary>
        /// ���� �߽����� �����մϴ�. (Vertex�� �����ϰų� ������Ʈ)
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
        /// ���� �ʱ�ȭ�մϴ�.
        /// </summary>
        public void InitRoom()
        {
            switch (Type)
            {
                case RoomType.UnSet:
                    break;
                case RoomType.Normal:
                    // �Ϲ� �� �ʱ�ȭ ����
                    TestInit(Color.white);
                    break;
                case RoomType.Start:
                    // ���� �� �ʱ�ȭ ����
                    TestInit(Color.green);
                    break;
                case RoomType.Boss:
                    // ���� �� �ʱ�ȭ ����
                    TestInit(Color.red);
                    break;
                case RoomType.Shop:
                    // ���� �� �ʱ�ȭ ����
                    TestInit(Color.cyan);
                    break;
                case RoomType.Special:
                    // Ư�� �� �ʱ�ȭ ����
                    TestInit(Color.yellow);
                    break;
                case RoomType.Secret:
                    // ��� �� �ʱ�ȭ ����
                    TestInit(Color.magenta);
                    break;
                case RoomType.End:
                    // ���� �� �ʱ�ȭ ����    
                    break;
            }
        }

        public void TestInit(Color color)
        {
            GetComponent<SpriteRenderer>().material.color = color;
        }

        /// <summary>
        /// ���� �߽� �׸��� ��ġ�� ��ȯ
        /// </summary>
        /// <returns></returns>
        public Vector2Int GetCenterGridPosition()
        {
            return Vector2Int.RoundToInt(Vertex.Pos);
        }
    }
}