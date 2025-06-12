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

        public void InitPooledObject()
        {
            if (_poolObject != null)
            {
                _poolObject.InitPooledObject();
            }
        }
        public void SubscribePoolDeactivateEvent()
        {
            PoolInfo.OnPoolDeactivate += DestroyObject;
        }
        private void DestroyObject()
        {
            Destroy(gameObject);
            PoolInfo.OnPoolDeactivate -= DestroyObject;
        } 
    }


    public interface IPooledObject
    {
        void InitPooledObject();
    }
}
