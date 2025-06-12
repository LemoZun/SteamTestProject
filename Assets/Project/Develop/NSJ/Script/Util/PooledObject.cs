using UnityEngine;

namespace NSJTool
{
    public class PooledObject : MonoBehaviour
    {
        public ObjectPool.PoolInfo PoolInfo;

        IPooledObject _poolObject;

        private void Awake()
        {
            _poolObject = GetComponent<IPooledObject>();
        }

        /// <summary>
        /// 풀링된 오브젝트가 활성화될 때 호출됩니다.
        /// 풀링된 오브젝트가 활성화될 때 초기화 작업을 수행합니다.
        /// </summary>
        public void InitPooledObject()
        {
            if (_poolObject != null)
            {
                _poolObject.InitPooledObject();
            }
        }

        /// <summary>
        /// 풀이 비활성화될 때 호출되는 이벤트를 구독합니다.
        /// </summary>
        public void SubscribePoolDeactivateEvent()
        {
            PoolInfo.OnPoolDeactivate += DestroyObject;
        }
        /// <summary>
        /// 풀이 비활성화될 때 호출됩니다.
        /// </summary>
        private void DestroyObject()
        {
            Destroy(gameObject);
            PoolInfo.OnPoolDeactivate -= DestroyObject;
        } 
    }

    /// <summary>
    /// IPooledObject 인터페이스는 풀링된 오브젝트가 초기화될 때 호출되는 메서드를 정의합니다.
    /// </summary>
    public interface IPooledObject
    {
        void InitPooledObject();
    }
}
