using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace NSJTool
{
    public class ObjectPool : MonoBehaviour
    {
        public float MaxTimer = 600f; // Ǯ ������Ʈ �ִ� ���� �ð�



        private static ObjectPool _instance;
        public static ObjectPool Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                else
                {
                    // ���Ӱ� Ǯ ������Ʈ ����
                    GameObject newPool = new GameObject("ObjectPool");
                    ObjectPool pool = newPool.AddComponent<ObjectPool>();
                    return pool;
                }
            }
            set
            {
                _instance = value;
            }
        }
        /// <summary>
        /// Ǯ ���� Ŭ�����Դϴ�. �� �����տ� ���� Ǯ ������ �����մϴ�.
        /// </summary>
        public class PoolInfo
        {
            public Stack<GameObject> Pool;
            public GameObject Prefab;
            public Transform Parent;
            public bool IsActive;
            public bool IsUsed;
            public UnityAction OnPoolDeactivate;
        }

        /// <summary>
        /// Ǯ ������ �����ϴ� ��ųʸ��Դϴ�. Ű�� �������� �ν��Ͻ� ID�Դϴ�.
        /// </summary>
        private Dictionary<int, PoolInfo> _poolDic = new Dictionary<int, PoolInfo>();

        private Dictionary<float, WaitForSeconds> _delayDic = new Dictionary<float, WaitForSeconds>();

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);

        }

        #region GetPool
        /// <summary>
        /// Ǯ���� ������Ʈ�� �����ɴϴ�.
        /// </summary>
        public static GameObject Get(GameObject prefab)
        {
            GameObject instance = ProcessGet(prefab);
            return instance;
        }
        /// <summary>
        /// Ǯ���� ������Ʈ�� ��������, ������ Transform�� ��ġ��ŵ�ϴ�.
        /// </summary>
        public static GameObject Get(GameObject prefab, Transform transform)
        {
            GameObject instance = ProcessGet(prefab, transform);
            return instance;
        }
        /// <summary>
        /// Ǯ���� ������Ʈ�� ��������, ������ Transform�� ��ġ��Ű��, ���� �������� �������� ���θ� �����մϴ�.
        /// </summary>
        public static GameObject Get(GameObject prefab, Transform transform, bool worldPositionStay)
        {
            GameObject instance = ProcessGet(prefab, transform, worldPositionStay);
            return instance;
        }
        /// <summary>
        /// Ǯ���� ������Ʈ�� ��������, ������ ��ġ�� ȸ���� �����մϴ�.
        /// </summary>
        public static GameObject Get(GameObject prefab, Vector3 pos, Quaternion rot)
        {
            GameObject instance = ProcessGet(prefab, pos, rot);
            return instance;
        }
        /// <summary>
        /// Ǯ���� ������Ʈ�� ��������, ������ �������� ��ȯ ���� �ð� �Ŀ� ��ȯ�մϴ�.
        /// </summary>
        public static GameObject Get(GameObject prefab, float returnDelay)
        {
            GameObject instance = ProcessGet(prefab);
            Return(instance, returnDelay);
            return instance;
        }
        /// <summary>
        /// Ǯ���� ������Ʈ�� ��������, ������ Transform�� ��ġ��Ű��, ��ȯ ���� �ð� �Ŀ� ��ȯ�մϴ�.
        /// </summary>
        public static GameObject Get(GameObject prefab, Transform transform, float returnDelay)
        {
            GameObject instance = ProcessGet(prefab, transform);
            Return(instance, returnDelay);
            return instance;
        }
        /// <summary>
        /// Ǯ���� ������Ʈ�� ��������, ������ Transform�� ��ġ��Ű��, ���� �������� �������� ���θ� �����ϰ�, ��ȯ ���� �ð� �Ŀ� ��ȯ�մϴ�.
        /// </summary>
        public static GameObject Get(GameObject prefab, Transform transform, bool worldPositionStay, float returnDelay)
        {
            GameObject instance = ProcessGet(prefab, transform, worldPositionStay);
            Return(instance, returnDelay);
            return instance;
        }
        /// <summary>
        /// Ǯ���� ������Ʈ�� ��������, ������ ��ġ�� ȸ���� �����ϸ�, ��ȯ ���� �ð� �Ŀ� ��ȯ�մϴ�.
        /// </summary>
        public static GameObject Get(GameObject prefab, Vector3 pos, Quaternion rot, float returnDelay)
        {
            GameObject instance = ProcessGet(prefab, pos, rot);

            Return(instance, returnDelay);
            return instance;
        }
        /// <summary>
        /// Ǯ���� ������Ʈ�� ��������, �ش� ������Ʈ�� ��ȯ�մϴ�.
        /// </summary>
        public static T Get<T>(T prefab) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject);
            T component = instance.GetComponent<T>();
            return component;
        }

        /// <summary>
        /// Ǯ���� ������Ʈ�� ��������, �ش� ������Ʈ�� ������ Transform�� ��ġ��ŵ�ϴ�.
        /// </summary>
        public static T Get<T>(T prefab, Transform transform) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject, transform);
            T component = instance.GetComponent<T>();
            return component;
        }

        /// <summary>
        /// Ǯ���� ������Ʈ�� ��������, �ش� ������Ʈ�� ������ Transform�� ��ġ��Ű��, ���� �������� �������� ���θ� �����մϴ�.
        /// </summary>
        public static T Get<T>(T prefab, Transform transform, bool worldPositionStay) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject, transform, worldPositionStay);
            T component = instance.GetComponent<T>();
            return component;
        }

        /// <summary>
        /// Ǯ���� ������Ʈ�� ��������, �ش� ������Ʈ�� ������ ��ġ�� ȸ���� �����մϴ�.
        /// </summary>
        public static T Get<T>(T prefab, Vector3 pos, Quaternion rot) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject, pos, rot);
            T component = instance.GetComponent<T>();
            return component;
        }

        /// <summary>
        /// Ǯ���� ������Ʈ�� ��������, �ش� ������Ʈ�� ��ȯ ���� �ð� �Ŀ� ��ȯ�մϴ�.
        /// </summary>
        public static T Get<T>(T prefab, float returnDelay) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject);
            T component = instance.GetComponent<T>();
            Return(component, returnDelay);
            return component;
        }
        /// <summary>
        /// Ǯ���� ������Ʈ�� ��������, �ش� ������Ʈ�� ������ Transform�� ��ġ��Ű��, ��ȯ ���� �ð� �Ŀ� ��ȯ�մϴ�.
        /// </summary>
        public static T Get<T>(T prefab, Transform transform, float returnDelay) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject, transform);
            T component = instance.GetComponent<T>();
            Return(component, returnDelay);
            return component;
        }
        /// <summary>
        ///  Ǯ���� ������Ʈ�� ��������, �ش� ������Ʈ�� ������ Transform�� ��ġ��Ű��, ���� �������� �������� ���θ� �����ϰ�, ��ȯ ���� �ð� �Ŀ� ��ȯ�մϴ�.
        /// </summary>
        public static T Get<T>(T prefab, Transform transform, bool worldPositionStay, float returnDelay) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject, transform, worldPositionStay);
            T component = instance.GetComponent<T>();
            Return(component, returnDelay);
            return component;
        }
        /// <summary>
        /// Ǯ���� ������Ʈ�� ��������, �ش� ������Ʈ�� ������ ��ġ�� ȸ���� �����ϸ�, ��ȯ ���� �ð� �Ŀ� ��ȯ�մϴ�.
        /// </summary>
        public static T Get<T>(T prefab, Vector3 pos, Quaternion rot, float returnDelay) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject, pos, rot);
            T component = instance.GetComponent<T>();
            Return(component, returnDelay);
            return component;
        }
        #endregion
        #region ReturnPool
        /// <summary>
        /// Ǯ���� ������Ʈ�� ��ȯ�մϴ�. ��ȯ�� ������Ʈ�� ��Ȱ��ȭ�ǰ�, Ǯ�� �ٽ� �߰��˴ϴ�.
        /// </summary>
        public static void Return(GameObject instance)
        {
            ProcessReturn(instance.gameObject);
        }
        /// <summary>
        /// Ǯ���� ������Ʈ�� ��ȯ�մϴ�. ��ȯ�� ������Ʈ�� ��Ȱ��ȭ�ǰ�, Ǯ�� �ٽ� �߰��˴ϴ�.
        /// </summary>
        public static void Return<T>(T instance) where T : Component
        {
            ProcessReturn(instance.gameObject);
        }
        /// <summary>
        /// Ǯ���� ������Ʈ�� ��ȯ�մϴ�. ��ȯ�� ������Ʈ�� ��Ȱ��ȭ�ǰ�, ������ ���� �ð� �Ŀ� Ǯ�� �ٽ� �߰��˴ϴ�.
        /// </summary>
        public static void Return(GameObject instance, float delay)
        {
            if (instance == null)
                return;
            if (instance.activeSelf == false)
                return;
            Instance.StartCoroutine(Instance.ReturnRoutine(instance, delay));
        }
        /// <summary>
        /// Ǯ���� ������Ʈ�� ��ȯ�մϴ�. ��ȯ�� ������Ʈ�� ��Ȱ��ȭ�ǰ�, ������ ���� �ð� �Ŀ� Ǯ�� �ٽ� �߰��˴ϴ�.
        /// </summary>
        public static void Return<T>(T instance, float delay) where T : Component
        {
            if (instance == null)
                return;

            if (instance.gameObject.activeSelf == false)
                return;
            Instance.StartCoroutine(Instance.ReturnRoutine(instance.gameObject, delay));
        }
        /// <summary>
        /// Ǯ���� ������Ʈ�� ��ȯ�ϴ� �ڷ�ƾ�Դϴ�. ������ ���� �ð� �Ŀ� ������Ʈ�� Ǯ�� �ٽ� �߰��մϴ�.
        /// </summary>
        IEnumerator ReturnRoutine(GameObject instance, float delay)
        {
            yield return GetDelay(delay);
            if (instance == null)
                yield break;

            if (instance.activeSelf == false)
                yield break;

            Return(instance);
        }
        #endregion
        #region GetAutoPool
        /// <summary>
        /// �ڵ� Ǯ���� ���� �ڷ�ƾ�� �����մϴ�. ������ �������� �ֱ������� ��������, ��ȯ ���� �ð� �Ŀ� ��ȯ�մϴ�.
        /// </summary>
        public static void Get(GameObject prefab, float intervalTime, float returnDelay, ref Coroutine coroutine)
        {
            if (coroutine == null)
            {
                coroutine = Instance.StartCoroutine(Instance.GetAutoPoolRoutine(prefab, intervalTime, returnDelay));
            }
        }
        /// <summary>
        /// �ڵ� Ǯ���� ���� �ڷ�ƾ�� �����մϴ�. ������ �������� �ֱ������� ��������, Transform�� ��ġ��Ű��, ��ȯ ���� �ð� �Ŀ� ��ȯ�մϴ�.
        /// </summary>
        public static void Get(GameObject prefab, Transform transform, float intervalTime, float returnDelay, ref Coroutine coroutine)
        {
            if (coroutine == null)
            {
                coroutine = Instance.StartCoroutine(Instance.GetAutoPoolRoutine(prefab, transform, false, intervalTime, returnDelay));
            }
        }
        /// <summary>
        /// �ڵ� Ǯ���� ���� �ڷ�ƾ�� �����մϴ�. ������ �������� �ֱ������� ��������, Transform�� ��ġ��Ű��, ���� �������� �������� ���θ� �����ϰ�, ��ȯ ���� �ð� �Ŀ� ��ȯ�մϴ�.
        /// </summary>
        public static void Get(GameObject prefab, Transform transform, bool worldPositionStay, float intervalTime, float returnDelay, ref Coroutine coroutine)
        {
            if (coroutine == null)
            {
                coroutine = Instance.StartCoroutine(Instance.GetAutoPoolRoutine(prefab, transform, worldPositionStay, intervalTime, returnDelay));
            }
        }
        /// <summary>
        /// �ڵ� Ǯ���� ���� �ڷ�ƾ�� �����մϴ�. ������ �������� �ֱ������� ��������, ��ġ�� ȸ���� �����ϸ�, ��ȯ ���� �ð� �Ŀ� ��ȯ�մϴ�.
        /// </summary>
        public static void Get(GameObject prefab, Vector3 pos, Quaternion rot, float intervalTime, float returnDelay, ref Coroutine coroutine)
        {
            if (coroutine == null)
            {
                coroutine = Instance.StartCoroutine(Instance.GetAutoPoolRoutine(prefab, pos, rot, intervalTime, returnDelay));
            }
        }
        /// <summary>
        /// �ڵ� Ǯ���� ���� �ڷ�ƾ�� �����մϴ�. ������ �������� �ֱ������� ��������, ��ȯ ���� �ð� �Ŀ� ��ȯ�մϴ�. ������ ���� �ð� �Ŀ� �ڵ� Ǯ���� �����մϴ�.
        /// </summary>
        public static void Get(GameObject prefab, float intervalTime, float returnDelay, float duration)
        {
            Coroutine coroutine = Instance.StartCoroutine(Instance.GetAutoPoolRoutine(prefab, intervalTime, returnDelay));
            Instance.StartCoroutine(Instance.GetAutoPoolDurationRoutine(coroutine, duration));
        }
        /// <summary>
        /// �ڵ� Ǯ���� ���� �ڷ�ƾ�� �����մϴ�. ������ �������� �ֱ������� ��������, Transform�� ��ġ��Ű��, ��ȯ ���� �ð� �Ŀ� ��ȯ�մϴ�. ������ ���� �ð� �Ŀ� �ڵ� Ǯ���� �����մϴ�.
        /// </summary>
        public static void Get(GameObject prefab, Vector3 pos, Quaternion rot, float intervalTime, float returnDelay, float duration)
        {
            Coroutine coroutine = Instance.StartCoroutine(Instance.GetAutoPoolRoutine(prefab, pos, rot, intervalTime, returnDelay));
            Instance.StartCoroutine(Instance.GetAutoPoolDurationRoutine(coroutine, duration));
        }
        /// <summary>
        /// �ڵ� Ǯ���� ���� �ڷ�ƾ�� �����մϴ�. ������ �������� �ֱ������� ��������, Transform�� ��ġ��Ű��, ���� �������� �������� ���θ� �����ϰ�, ��ȯ ���� �ð� �Ŀ� ��ȯ�մϴ�. ������ ���� �ð� �Ŀ� �ڵ� Ǯ���� �����մϴ�.
        /// </summary>
        public static void Return(ref Coroutine coroutine)
        {
            if (coroutine != null)
            {
                Instance.StopCoroutine(coroutine);
                coroutine = null;
            }
        }
        /// <summary>
        /// �ڵ� Ǯ���� ���� �ڷ�ƾ�Դϴ�. ������ �������� �ֱ������� ��������, ��ȯ ���� �ð� �Ŀ� ��ȯ�մϴ�.
        /// </summary>
        IEnumerator GetAutoPoolRoutine(GameObject prefab, float intervalTime, float returnDelay)
        {
            while (true)
            {
                Get(prefab, returnDelay);
                yield return GetDelay(intervalTime);
            }
        }
        /// <summary>
        /// �ڵ� Ǯ���� ���� �ڷ�ƾ�Դϴ�. ������ �������� �ֱ������� ��������, Transform�� ��ġ��Ű��, ��ȯ ���� �ð� �Ŀ� ��ȯ�մϴ�.
        /// </summary>
        IEnumerator GetAutoPoolRoutine(GameObject prefab, Transform transform, bool worldPositionStay, float intervalTime, float returnDelay)
        {
            while (true)
            {
                Get(prefab, transform, worldPositionStay, returnDelay);
                yield return GetDelay(intervalTime);
            }
        }
        /// <summary>
        /// �ڵ� Ǯ���� ���� �ڷ�ƾ�Դϴ�. ������ �������� �ֱ������� ��������, ��ġ�� ȸ���� �����ϸ�, ��ȯ ���� �ð� �Ŀ� ��ȯ�մϴ�.
        /// </summary>
        IEnumerator GetAutoPoolRoutine(GameObject prefab, Vector3 pos, Quaternion rot, float intervalTime, float returnDelay)
        {
            while (true)
            {
                Get(prefab, pos, rot, returnDelay);
                yield return GetDelay(intervalTime);
            }
        }
        /// <summary>
        /// �ڵ� Ǯ���� ���� �ڷ�ƾ�Դϴ�. ������ �������� �ֱ������� ��������, ��ȯ ���� �ð� �Ŀ� ��ȯ�մϴ�. ������ ���� �ð� �Ŀ� �ڵ� Ǯ���� �����մϴ�.
        /// </summary>
        IEnumerator GetAutoPoolDurationRoutine(Coroutine coroutine, float duration)
        {
            yield return GetDelay(duration);
            Return(ref coroutine);
        }
        #endregion
        /// <summary>
        /// Ǯ ������Ʈ�� ã���ϴ�. �ش� �����տ� ���� Ǯ ������ ������ ���� �����մϴ�.
        /// </summary>
        private static PoolInfo FindPool(GameObject poolPrefab)
        {
            int prefabID = poolPrefab.GetInstanceID();

            PoolInfo pool = default;
            if (Instance._poolDic.ContainsKey(prefabID) == false)
            {
                Transform newParent = new GameObject(poolPrefab.name).transform;
                newParent.SetParent(Instance.transform, true); // parent
                Stack<GameObject> newPool = new Stack<GameObject>(); // pool
                PoolInfo newPoolInfo = GetPoolInfo(newPool, poolPrefab, newParent);

                // Ǯ ��ųʸ� �߰�
                Instance._poolDic.Add(prefabID, newPoolInfo);
                // Ǯ ��Ƽ����� ����
                Instance.StartCoroutine(Instance.IsActiveRoutine(prefabID));
            }

            pool = Instance._poolDic[prefabID];
            pool.IsUsed = true;
            Instance._poolDic[prefabID] = pool;

            return pool;
        }
        /// <summary>
        /// Ǯ ���� ����
        /// </summary>
        private static PoolInfo GetPoolInfo(Stack<GameObject> pool, GameObject prefab, Transform parent)
        {
            PoolInfo info = new PoolInfo();
            info.Pool = pool;
            info.Parent = parent;
            info.Prefab = prefab;
            return info;
        }
        /// <summary>
        /// Ǯ ������Ʈ ������Ʈ �߰�
        /// </summary>
        private static void AddPoolObjectComponent(GameObject instance, PoolInfo info)
        {
            PooledObject poolObject = instance.GetOrAddComponent<PooledObject>();
            poolObject.PoolInfo = info;
            poolObject.SubscribePoolDeactivateEvent();
        }

        /// <summary>
        /// Ǯ���� ������Ʈ�� �������� �޼����Դϴ�. �ش� �����տ� ���� Ǯ ������ ã��, Ȱ��ȭ�� ������Ʈ�� ������ ��ȯ�ϰ�, ������ ���� �����մϴ�.
        /// </summary>
        private static GameObject ProcessGet(GameObject prefab)
        {
            GameObject instance = null;
            PoolInfo info = FindPool(prefab);
            if (FindObject(info))
            {
                instance = info.Pool.Pop();
                instance.transform.position = Vector3.zero;
                instance.transform.rotation = Quaternion.identity;
                instance.transform.SetParent(null);
                instance.gameObject.SetActive(true);
            }
            else
            {
                instance = Instantiate(info.Prefab);
                AddPoolObjectComponent(instance, info);
            }
            return instance;
        }
        /// <summary>
        /// Ǯ���� ������Ʈ�� ��������, ������ Transform�� ��ġ��ŵ�ϴ�. �ش� �����տ� ���� Ǯ ������ ã��, Ȱ��ȭ�� ������Ʈ�� ������ ��ȯ�ϰ�, ������ ���� �����մϴ�.
        /// </summary>
        private static GameObject ProcessGet(GameObject prefab, Transform transform)
        {
            GameObject instance = null;
            PoolInfo info = FindPool(prefab);
            
            if (FindObject(info))
            {
                instance = info.Pool.Pop();
                instance.transform.SetParent(transform);
                instance.transform.position = transform.position;
                instance.transform.rotation = transform.rotation;
                instance.gameObject.SetActive(true);
            }
            else
            {
                instance = Instantiate(info.Prefab, transform);
                AddPoolObjectComponent(instance, info);
            }

            return instance;
        }
        /// <summary>
        /// Ǯ���� ������Ʈ�� ��������, ������ Transform�� ��ġ��Ű��, ���� �������� �������� ���θ� �����մϴ�. �ش� �����տ� ���� Ǯ ������ ã��, Ȱ��ȭ�� ������Ʈ�� ������ ��ȯ�ϰ�, ������ ���� �����մϴ�.
        /// </summary>
        private static GameObject ProcessGet(GameObject prefab, Transform transform, bool worldPositionStay)
        {
            GameObject instance = null;
            PoolInfo info = FindPool(prefab);
            if (FindObject(info))
            {
                instance = info.Pool.Pop();
                instance.transform.SetParent(transform);
                if (worldPositionStay == true)
                {
                    instance.transform.position = prefab.transform.position;
                    instance.transform.rotation = prefab.transform.rotation;
                }
                else
                {
                    instance.transform.position = transform.position;
                    instance.transform.rotation = transform.rotation;
                }
                instance.gameObject.SetActive(true);
            }
            else
            {
                instance = Instantiate(info.Prefab, transform, worldPositionStay);
                AddPoolObjectComponent(instance, info);
            }

            return instance;
        }
        /// <summary>
        /// Ǯ���� ������Ʈ�� ��������, ������ ��ġ�� ȸ���� �����մϴ�. �ش� �����տ� ���� Ǯ ������ ã��, Ȱ��ȭ�� ������Ʈ�� ������ ��ȯ�ϰ�, ������ ���� �����մϴ�.
        /// </summary>
        private static GameObject ProcessGet(GameObject prefab, Vector3 pos, Quaternion rot)
        {
            GameObject instance = null;
            PoolInfo info = FindPool(prefab);
            if (FindObject(info))
            {
                instance = info.Pool.Pop();
                instance.transform.position = pos;
                instance.transform.rotation = rot;
                instance.transform.SetParent(null);
                instance.gameObject.SetActive(true);
            }
            else
            {
                instance = Instantiate(info.Prefab, pos, rot);
                AddPoolObjectComponent(instance, info);
            }
            return instance;
        }
        /// <summary>
        /// Ǯ���� ������Ʈ�� ��������, �ش� ������Ʈ�� ��ȯ�մϴ�. �ش� �����տ� ���� Ǯ ������ ã��, Ȱ��ȭ�� ������Ʈ�� ������ ��ȯ�ϰ�, ������ ���� �����մϴ�.
        /// </summary>
        private static void ProcessReturn(GameObject instance)
        {
            //CreateObjectPool();
            if (instance == null)
                return;

            if (instance.activeSelf == false)
                return;

            PooledObject poolObject = instance.GetComponent<PooledObject>();
            PoolInfo info = FindPool(poolObject.PoolInfo.Prefab);

            instance.transform.position = info.Prefab.transform.position;
            instance.transform.rotation = info.Prefab.transform.rotation;
            instance.transform.localScale = info.Prefab.transform.localScale;
            instance.transform.SetParent(info.Parent);

            poolObject.InitPooledObject();

            instance.gameObject.SetActive(false);
            info.Pool.Push(instance.gameObject);
        }
        /// <summary>
        /// Ǯ���� ������Ʈ�� �������� �޼����Դϴ�. �ش� �����տ� ���� Ǯ ������ ã��, Ȱ��ȭ�� ������Ʈ�� ������ ��ȯ�ϰ�, ������ ���� �����մϴ�.
        /// </summary>
        private static bool FindObject(PoolInfo info)
        {
            GameObject instance = null;
            while (true)
            {
                if (info.Pool.Count <= 0)
                    return false;

                instance = info.Pool.Peek();
                if (instance != null)
                    break;

                info.Pool.Pop();
            }

            return true;

        }
        /// <summary>
        /// Ǯ ������Ʈ�� Ȱ��ȭ �������� Ȯ���ϴ� �ڷ�ƾ�Դϴ�.
        /// </summary>
        private IEnumerator IsActiveRoutine(int id)
        {
            float delayTime = 10f;
            float timer = MaxTimer;
            while (true)
            {
                // Ǯ ��������� �ð� �ʱ�ȭ
                if (Instance._poolDic[id].IsUsed == true)
                {
                    timer = MaxTimer;
                    PoolInfo pool = Instance._poolDic[id];
                    pool.IsUsed = false;
                    pool.IsActive = true;
                }

                // Ÿ�̸� ���� �� 
                if (timer <= 0)
                {
                    ClearPool(id);
                }
                else
                {
                    timer -= delayTime;
                }
                yield return GetDelay(delayTime);
            }
        }

        /// <summary>
        /// Ǯ ������Ʈ�� ���� �ʱ�ȭ�մϴ�.
        /// </summary>
        private void ClearPool(int id)
        {
            PoolInfo info = Instance._poolDic[id];

            if (info.IsActive == true)
                return;

            info.OnPoolDeactivate?.Invoke();


            info.Pool = new Stack<GameObject>();
            info.IsActive = false;
        }

        /// <summary>
        /// ������ �ð���ŭ ����ϴ� WaitForSeconds ��ü�� ��ȯ�մϴ�. �̹� ������ ��ü�� ������ �����մϴ�.
        /// </summary>
        private WaitForSeconds GetDelay(float time)
        {
            if (_delayDic.ContainsKey(time) == false)
            {
                _delayDic.Add(time, new WaitForSeconds(time));
            }
            return _delayDic[time];
        }
    }
}