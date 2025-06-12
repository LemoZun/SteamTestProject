using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Procedural_Map_Generation
{
    /// <summary>
    /// �� Ÿ�� �з�
    /// </summary>
    public class RoomClassifier
    {
        public void ClassifyRooms(List<Room> rooms, Room startRoom)
        {
            startRoom.Type = RoomType.Start;
            Dictionary<Room, int> distances = MapGenerationUtility.CalculateDistanceFrom(startRoom);

            SetBossRoom(distances);
            SetSpecialRooms(rooms);
            SetNormalRooms(rooms);
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
                bossRoom.Type = RoomType.Boss;
            }
        }

        /// <summary>
        /// ��Ʈ �б� ���� Ư���� ����
        /// </summary>
        private void SetSpecialRooms(List<Room> rooms)
        {
            // ���� ���(����� ���� 1���� ���̸� Ÿ���� �������� ���� ��) ã��
            List<Room> leafRooms = rooms.Where(room => (room.ConnectedRooms.Count == 1) && room.Type == RoomType.UnSet).ToList();

            // Ư�� �� Ÿ�� ���� ����
            Stack<RoomType> specialRoomTypes = new Stack<RoomType>();
            specialRoomTypes.Push(RoomType.Special);
            specialRoomTypes.Push(RoomType.Shop);

            List<Room> normalRooms = null;
            // Ư�� �� Ÿ���� ������ ���� ����� �������� ������ �Ϲ� ���� ã�� ���� �Ϲ� �� ����Ʈ ����
            if (specialRoomTypes.Count > leafRooms.Count)
            {
                // �Ϲ� �� ã��: Type�� UnSet�� ��� �߿��� ����� ���� 2�� �̻��� ���
                normalRooms = rooms.Where(room => (room.ConnectedRooms.Count > 1) && room.Type == RoomType.UnSet).ToList();
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
                if (room.Type == RoomType.UnSet)
                {
                    room.Type = RoomType.Normal;
                }
            }
        }
    }
}