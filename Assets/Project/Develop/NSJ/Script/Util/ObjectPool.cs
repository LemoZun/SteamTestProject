using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

namespace NSJTool
{
    public class ObjectPool : MonoBehaviour
    {
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
                    // 새롭게 풀 오브젝트 생성
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
        public struct PoolInfo
        {
            public Stack<GameObject> Pool;
            public GameObject Prefab;
            public Transform Parent;
            public bool IsActive;
        }

        /// <summary>
        /// 프리팹용
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
        public static GameObject Get(GameObject prefab)
        {
            GameObject instance = ProcessGet(prefab);
            return instance;
        }
        public static GameObject Get(GameObject prefab, Transform transform)
        {
            GameObject instance = ProcessGet(prefab, transform);
            return instance;
        }
        public static GameObject Get(GameObject prefab, Transform transform, bool worldPositionStay)
        {
            GameObject instance = ProcessGet(prefab, transform, worldPositionStay);
            return instance;
        }
        public static GameObject Get(GameObject prefab, Vector3 pos, Quaternion rot)
        {
            GameObject instance = ProcessGet(prefab, pos, rot);
            return instance;
        }
        public static GameObject Get(GameObject prefab, float returnDelay)
        {
            GameObject instance = ProcessGet(prefab);
            Return(instance, returnDelay);
            return instance;
        }
        public static GameObject Get(GameObject prefab, Transform transform, float returnDelay)
        {
            GameObject instance = ProcessGet(prefab, transform);
            Return(instance, returnDelay);
            return instance;
        }
        public static GameObject Get(GameObject prefab, Transform transform, bool worldPositionStay, float returnDelay)
        {
            GameObject instance = ProcessGet(prefab, transform, worldPositionStay);
            Return(instance, returnDelay);
            return instance;
        }
        public static GameObject Get(GameObject prefab, Vector3 pos, Quaternion rot, float returnDelay)
        {
            GameObject instance = ProcessGet(prefab, pos, rot);

            Return(instance, returnDelay);
            return instance;
        }
        public static T Get<T>(T prefab) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject);
            T component = instance.GetComponent<T>();
            return component;
        }

        public static T Get<T>(T prefab, Transform transform) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject, transform);
            T component = instance.GetComponent<T>();
            return component;
        }

        public static T Get<T>(T prefab, Transform transform, bool worldPositionStay) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject, transform, worldPositionStay);
            T component = instance.GetComponent<T>();
            return component;
        }

        public static T Get<T>(T prefab, Vector3 pos, Quaternion rot) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject, pos, rot);
            T component = instance.GetComponent<T>();
            return component;
        }

        public static T Get<T>(T prefab, float returnDelay) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject);
            T component = instance.GetComponent<T>();
            Return(component, returnDelay);
            return component;
        }
        public static T Get<T>(T prefab, Transform transform, float returnDelay) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject, transform);
            T component = instance.GetComponent<T>();
            Return(component, returnDelay);
            return component;
        }
        public static T Get<T>(T prefab, Transform transform, bool worldPositionStay, float returnDelay) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject, transform, worldPositionStay);
            T component = instance.GetComponent<T>();
            Return(component, returnDelay);
            return component;
        }
        public static T Get<T>(T prefab, Vector3 pos, Quaternion rot, float returnDelay) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject, pos, rot);
            T component = instance.GetComponent<T>();
            Return(component, returnDelay);
            return component;
        }
        #endregion
        #region ReturnPool
        public static void Return(GameObject instance)
        {
            ProcessReturn(instance.gameObject);
        }
        public static void Return<T>(T instance) where T : Component
        {
            ProcessReturn(instance.gameObject);
        }
        public static void Return(GameObject instance, float delay)
        {
            if (instance == null)
                return;
            if (instance.activeSelf == false)
                return;
            Instance.StartCoroutine(Instance.ReturnRoutine(instance, delay));
        }
        public static void Return<T>(T instance, float delay) where T : Component
        {
            if (instance == null)
                return;

            if (instance.gameObject.activeSelf == false)
                return;
            Instance.StartCoroutine(Instance.ReturnRoutine(instance.gameObject, delay));
        }
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
        public static void Get(GameObject prefab, float intervalTime, float returnDelay, ref Coroutine coroutine)
        {
            if (coroutine == null)
            {
                coroutine = Instance.StartCoroutine(Instance.GetAutoPoolRoutine(prefab, intervalTime, returnDelay));
            }
        }
        public static void Get(GameObject prefab, Transform transform, float intervalTime, float returnDelay, ref Coroutine coroutine)
        {
            if (coroutine == null)
            {
                coroutine = Instance.StartCoroutine(Instance.GetAutoPoolRoutine(prefab, transform, false, intervalTime, returnDelay));
            }
        }
        public static void Get(GameObject prefab, Transform transform, bool worldPositionStay, float intervalTime, float returnDelay, ref Coroutine coroutine)
        {
            if (coroutine == null)
            {
                coroutine = Instance.StartCoroutine(Instance.GetAutoPoolRoutine(prefab, transform, worldPositionStay, intervalTime, returnDelay));
            }
        }
        public static void Get(GameObject prefab, Vector3 pos, Quaternion rot, float intervalTime, float returnDelay, ref Coroutine coroutine)
        {
            if (coroutine == null)
            {
                coroutine = Instance.StartCoroutine(Instance.GetAutoPoolRoutine(prefab, pos, rot, intervalTime, returnDelay));
            }
        }
        public static void Get(GameObject prefab, float intervalTime, float returnDelay, float duration)
        {
            Coroutine coroutine = Instance.StartCoroutine(Instance.GetAutoPoolRoutine(prefab, intervalTime, returnDelay));
            Instance.StartCoroutine(Instance.GetAutoPoolDurationRoutine(coroutine, duration));
        }
        public static void Get(GameObject prefab, Vector3 pos, Quaternion rot, float intervalTime, float returnDelay, float duration)
        {
            Coroutine coroutine = Instance.StartCoroutine(Instance.GetAutoPoolRoutine(prefab, pos, rot, intervalTime, returnDelay));
            Instance.StartCoroutine(Instance.GetAutoPoolDurationRoutine(coroutine, duration));
        }
        public static void Return(ref Coroutine coroutine)
        {
            if (coroutine != null)
            {
                Instance.StopCoroutine(coroutine);
                coroutine = null;
            }
        }
        IEnumerator GetAutoPoolRoutine(GameObject prefab, float intervalTime, float returnDelay)
        {
            while (true)
            {
                Get(prefab, returnDelay);
                yield return GetDelay(intervalTime);
            }
        }
        IEnumerator GetAutoPoolRoutine(GameObject prefab, Transform transform, bool worldPositionStay, float intervalTime, float returnDelay)
        {
            while (true)
            {
                Get(prefab, transform, worldPositionStay, returnDelay);
                yield return GetDelay(intervalTime);
            }
        }
        IEnumerator GetAutoPoolRoutine(GameObject prefab, Vector3 pos, Quaternion rot, float intervalTime, float returnDelay)
        {
            while (true)
            {
                Get(prefab, pos, rot, returnDelay);
                yield return GetDelay(intervalTime);
            }
        }
        IEnumerator GetAutoPoolDurationRoutine(Coroutine coroutine, float duration)
        {
            yield return GetDelay(duration);
            Return(ref coroutine);
        }
        #endregion

        private static PoolInfo FindPool(GameObject poolPrefab)
        {
            //CreateObjectPool();

            int prefabID = poolPrefab.GetInstanceID();

            PoolInfo pool = default;
            if (Instance._poolDic.ContainsKey(prefabID) == false)
            {
                Transform newParent = new GameObject(poolPrefab.name).transform;
                newParent.SetParent(Instance.transform, true); // parent
                Stack<GameObject> newPool = new Stack<GameObject>(); // pool
                PoolInfo newPoolInfo = GetPoolInfo(newPool, poolPrefab, newParent);

                // 풀 딕셔너리 추가
                Instance._poolDic.Add(prefabID, newPoolInfo);
                // 풀 액티브상태 감지
                Instance.StartCoroutine(Instance.IsActiveRoutine(prefabID));
            }

            pool = Instance._poolDic[prefabID];
            pool.IsActive = true;
            Instance._poolDic[prefabID] = pool;

            return pool;
        }
        private static PoolInfo GetPoolInfo(Stack<GameObject> pool, GameObject prefab, Transform parent)
        {
            PoolInfo info = new PoolInfo();
            info.Pool = pool;
            info.Parent = parent;
            info.Prefab = prefab;
            return info;
        }
        private static void AddPoolObjectComponent(GameObject instance, PoolInfo info)
        {
            PooledObject poolObject = instance.GetOrAddComponent<PooledObject>();
            poolObject.PoolInfo.Prefab = info.Prefab;
        }
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
        private IEnumerator IsActiveRoutine(int id)
        {
            float maxTimer = 300;
            float delayTime = 10;
            float timer = maxTimer;
            while (true)
            {
                // 풀 사용했을때 시간 초기화
                if (Instance._poolDic[id].IsActive == true)
                {
                    timer = maxTimer;
                    PoolInfo pool = Instance._poolDic[id];
                    pool.IsActive = false;
                    Instance._poolDic[id] = pool;
                }

                // 타이머 종료 시 
                if (timer <= 0)
                {
                    if (Instance._poolDic[id].Pool == null)
                        continue;

                    // 풀(큐) 내부 오브젝트 파괴
                    int poolCount = Instance._poolDic[id].Pool.Count;
                    for (int i = 0; i < poolCount; i++)
                    {
                        GameObject pooledObject = Instance._poolDic[id].Pool.Pop();
                        if (pooledObject != null)
                        {
                            Destroy(pooledObject);
                        }
                    }
                }
                else
                {
                    timer -= delayTime;
                }
                yield return GetDelay(delayTime);
            }
        }

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