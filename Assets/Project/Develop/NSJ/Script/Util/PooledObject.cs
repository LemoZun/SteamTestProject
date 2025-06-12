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
        /// Ǯ���� ������Ʈ�� Ȱ��ȭ�� �� ȣ��˴ϴ�.
        /// Ǯ���� ������Ʈ�� Ȱ��ȭ�� �� �ʱ�ȭ �۾��� �����մϴ�.
        /// </summary>
        public void InitPooledObject()
        {
            if (_poolObject != null)
            {
                _poolObject.InitPooledObject();
            }
        }

        /// <summary>
        /// Ǯ�� ��Ȱ��ȭ�� �� ȣ��Ǵ� �̺�Ʈ�� �����մϴ�.
        /// </summary>
        public void SubscribePoolDeactivateEvent()
        {
            PoolInfo.OnPoolDeactivate += DestroyObject;
        }
        /// <summary>
        /// Ǯ�� ��Ȱ��ȭ�� �� ȣ��˴ϴ�.
        /// </summary>
        private void DestroyObject()
        {
            Destroy(gameObject);
            PoolInfo.OnPoolDeactivate -= DestroyObject;
        } 
    }

    /// <summary>
    /// IPooledObject �������̽��� Ǯ���� ������Ʈ�� �ʱ�ȭ�� �� ȣ��Ǵ� �޼��带 �����մϴ�.
    /// </summary>
    public interface IPooledObject
    {
        void InitPooledObject();
    }
}
