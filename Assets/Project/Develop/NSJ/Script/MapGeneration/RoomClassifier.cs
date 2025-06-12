using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Procedural_Map_Generation
{
    /// <summary>
    /// 방 타입 분류
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
        /// 보스 방을 선택합니다. 가장 먼 방을 보스 방으로 지정합니다.
        /// </summary>
        private void SetBossRoom(Dictionary<Room, int> distances)
        {
            // 가장 먼 방 찾기
            Room bossRoom = distances.OrderByDescending(x => x.Value).FirstOrDefault().Key;

            // 보스 방이 null이 아닐 경우 타입을 Boss로 설정
            if (bossRoom != null)
            {
                bossRoom.Type = RoomType.Boss;
            }
        }

        /// <summary>
        /// 루트 분기 끝단 특수방 지정
        /// </summary>
        private void SetSpecialRooms(List<Room> rooms)
        {
            // 리프 노드(연결된 방이 1개인 방이며 타입이 정해지지 않은 방) 찾기
            List<Room> leafRooms = rooms.Where(room => (room.ConnectedRooms.Count == 1) && room.Type == RoomType.UnSet).ToList();

            // 특수 방 타입 스택 생성
            Stack<RoomType> specialRoomTypes = new Stack<RoomType>();
            specialRoomTypes.Push(RoomType.Special);
            specialRoomTypes.Push(RoomType.Shop);

            List<Room> normalRooms = null;
            // 특수 방 타입의 개수가 리프 노드의 개수보다 많으면 일반 방을 찾기 위해 일반 방 리스트 생성
            if (specialRoomTypes.Count > leafRooms.Count)
            {
                // 일반 방 찾기: Type이 UnSet인 방들 중에서 연결된 방이 2개 이상인 방들
                normalRooms = rooms.Where(room => (room.ConnectedRooms.Count > 1) && room.Type == RoomType.UnSet).ToList();
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
                if (room.Type == RoomType.UnSet)
                {
                    room.Type = RoomType.Normal;
                }
            }
        }
    }
}