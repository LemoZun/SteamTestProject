using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace NSJTool
{
    public class ObjectPool : MonoBehaviour
    {
        public float MaxTimer = 600f; // 풀 오브젝트 최대 유지 시간



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
        /// <summary>
        /// 풀 정보 클래스입니다. 각 프리팹에 대한 풀 정보를 저장합니다.
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
        /// 풀 정보를 저장하는 딕셔너리입니다. 키는 프리팹의 인스턴스 ID입니다.
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
        /// 풀에서 오브젝트를 가져옵니다.
        /// </summary>
        public static GameObject Get(GameObject prefab)
        {
            GameObject instance = ProcessGet(prefab);
            return instance;
        }
        /// <summary>
        /// 풀에서 오브젝트를 가져오고, 지정된 Transform에 위치시킵니다.
        /// </summary>
        public static GameObject Get(GameObject prefab, Transform transform)
        {
            GameObject instance = ProcessGet(prefab, transform);
            return instance;
        }
        /// <summary>
        /// 풀에서 오브젝트를 가져오고, 지정된 Transform에 위치시키며, 월드 포지션을 유지할지 여부를 설정합니다.
        /// </summary>
        public static GameObject Get(GameObject prefab, Transform transform, bool worldPositionStay)
        {
            GameObject instance = ProcessGet(prefab, transform, worldPositionStay);
            return instance;
        }
        /// <summary>
        /// 풀에서 오브젝트를 가져오고, 지정된 위치와 회전을 설정합니다.
        /// </summary>
        public static GameObject Get(GameObject prefab, Vector3 pos, Quaternion rot)
        {
            GameObject instance = ProcessGet(prefab, pos, rot);
            return instance;
        }
        /// <summary>
        /// 풀에서 오브젝트를 가져오고, 지정된 프리팹을 반환 지연 시간 후에 반환합니다.
        /// </summary>
        public static GameObject Get(GameObject prefab, float returnDelay)
        {
            GameObject instance = ProcessGet(prefab);
            Return(instance, returnDelay);
            return instance;
        }
        /// <summary>
        /// 풀에서 오브젝트를 가져오고, 지정된 Transform에 위치시키며, 반환 지연 시간 후에 반환합니다.
        /// </summary>
        public static GameObject Get(GameObject prefab, Transform transform, float returnDelay)
        {
            GameObject instance = ProcessGet(prefab, transform);
            Return(instance, returnDelay);
            return instance;
        }
        /// <summary>
        /// 풀에서 오브젝트를 가져오고, 지정된 Transform에 위치시키며, 월드 포지션을 유지할지 여부를 설정하고, 반환 지연 시간 후에 반환합니다.
        /// </summary>
        public static GameObject Get(GameObject prefab, Transform transform, bool worldPositionStay, float returnDelay)
        {
            GameObject instance = ProcessGet(prefab, transform, worldPositionStay);
            Return(instance, returnDelay);
            return instance;
        }
        /// <summary>
        /// 풀에서 오브젝트를 가져오고, 지정된 위치와 회전을 설정하며, 반환 지연 시간 후에 반환합니다.
        /// </summary>
        public static GameObject Get(GameObject prefab, Vector3 pos, Quaternion rot, float returnDelay)
        {
            GameObject instance = ProcessGet(prefab, pos, rot);

            Return(instance, returnDelay);
            return instance;
        }
        /// <summary>
        /// 풀에서 오브젝트를 가져오고, 해당 컴포넌트를 반환합니다.
        /// </summary>
        public static T Get<T>(T prefab) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject);
            T component = instance.GetComponent<T>();
            return component;
        }

        /// <summary>
        /// 풀에서 오브젝트를 가져오고, 해당 컴포넌트를 지정된 Transform에 위치시킵니다.
        /// </summary>
        public static T Get<T>(T prefab, Transform transform) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject, transform);
            T component = instance.GetComponent<T>();
            return component;
        }

        /// <summary>
        /// 풀에서 오브젝트를 가져오고, 해당 컴포넌트를 지정된 Transform에 위치시키며, 월드 포지션을 유지할지 여부를 설정합니다.
        /// </summary>
        public static T Get<T>(T prefab, Transform transform, bool worldPositionStay) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject, transform, worldPositionStay);
            T component = instance.GetComponent<T>();
            return component;
        }

        /// <summary>
        /// 풀에서 오브젝트를 가져오고, 해당 컴포넌트를 지정된 위치와 회전을 설정합니다.
        /// </summary>
        public static T Get<T>(T prefab, Vector3 pos, Quaternion rot) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject, pos, rot);
            T component = instance.GetComponent<T>();
            return component;
        }

        /// <summary>
        /// 풀에서 오브젝트를 가져오고, 해당 컴포넌트를 반환 지연 시간 후에 반환합니다.
        /// </summary>
        public static T Get<T>(T prefab, float returnDelay) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject);
            T component = instance.GetComponent<T>();
            Return(component, returnDelay);
            return component;
        }
        /// <summary>
        /// 풀에서 오브젝트를 가져오고, 해당 컴포넌트를 지정된 Transform에 위치시키며, 반환 지연 시간 후에 반환합니다.
        /// </summary>
        public static T Get<T>(T prefab, Transform transform, float returnDelay) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject, transform);
            T component = instance.GetComponent<T>();
            Return(component, returnDelay);
            return component;
        }
        /// <summary>
        ///  풀에서 오브젝트를 가져오고, 해당 컴포넌트를 지정된 Transform에 위치시키며, 월드 포지션을 유지할지 여부를 설정하고, 반환 지연 시간 후에 반환합니다.
        /// </summary>
        public static T Get<T>(T prefab, Transform transform, bool worldPositionStay, float returnDelay) where T : Component
        {
            GameObject instance = ProcessGet(prefab.gameObject, transform, worldPositionStay);
            T component = instance.GetComponent<T>();
            Return(component, returnDelay);
            return component;
        }
        /// <summary>
        /// 풀에서 오브젝트를 가져오고, 해당 컴포넌트를 지정된 위치와 회전을 설정하며, 반환 지연 시간 후에 반환합니다.
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
        /// 풀에서 오브젝트를 반환합니다. 반환된 오브젝트는 비활성화되고, 풀에 다시 추가됩니다.
        /// </summary>
        public static void Return(GameObject instance)
        {
            ProcessReturn(instance.gameObject);
        }
        /// <summary>
        /// 풀에서 오브젝트를 반환합니다. 반환된 오브젝트는 비활성화되고, 풀에 다시 추가됩니다.
        /// </summary>
        public static void Return<T>(T instance) where T : Component
        {
            ProcessReturn(instance.gameObject);
        }
        /// <summary>
        /// 풀에서 오브젝트를 반환합니다. 반환된 오브젝트는 비활성화되고, 지정된 지연 시간 후에 풀에 다시 추가됩니다.
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
        /// 풀에서 오브젝트를 반환합니다. 반환된 오브젝트는 비활성화되고, 지정된 지연 시간 후에 풀에 다시 추가됩니다.
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
        /// 풀에서 오브젝트를 반환하는 코루틴입니다. 지정된 지연 시간 후에 오브젝트를 풀에 다시 추가합니다.
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
        /// 자동 풀링을 위한 코루틴을 시작합니다. 지정된 프리팹을 주기적으로 가져오고, 반환 지연 시간 후에 반환합니다.
        /// </summary>
        public static void Get(GameObject prefab, float intervalTime, float returnDelay, ref Coroutine coroutine)
        {
            if (coroutine == null)
            {
                coroutine = Instance.StartCoroutine(Instance.GetAutoPoolRoutine(prefab, intervalTime, returnDelay));
            }
        }
        /// <summary>
        /// 자동 풀링을 위한 코루틴을 시작합니다. 지정된 프리팹을 주기적으로 가져오고, Transform에 위치시키며, 반환 지연 시간 후에 반환합니다.
        /// </summary>
        public static void Get(GameObject prefab, Transform transform, float intervalTime, float returnDelay, ref Coroutine coroutine)
        {
            if (coroutine == null)
            {
                coroutine = Instance.StartCoroutine(Instance.GetAutoPoolRoutine(prefab, transform, false, intervalTime, returnDelay));
            }
        }
        /// <summary>
        /// 자동 풀링을 위한 코루틴을 시작합니다. 지정된 프리팹을 주기적으로 가져오고, Transform에 위치시키며, 월드 포지션을 유지할지 여부를 설정하고, 반환 지연 시간 후에 반환합니다.
        /// </summary>
        public static void Get(GameObject prefab, Transform transform, bool worldPositionStay, float intervalTime, float returnDelay, ref Coroutine coroutine)
        {
            if (coroutine == null)
            {
                coroutine = Instance.StartCoroutine(Instance.GetAutoPoolRoutine(prefab, transform, worldPositionStay, intervalTime, returnDelay));
            }
        }
        /// <summary>
        /// 자동 풀링을 위한 코루틴을 시작합니다. 지정된 프리팹을 주기적으로 가져오고, 위치와 회전을 설정하며, 반환 지연 시간 후에 반환합니다.
        /// </summary>
        public static void Get(GameObject prefab, Vector3 pos, Quaternion rot, float intervalTime, float returnDelay, ref Coroutine coroutine)
        {
            if (coroutine == null)
            {
                coroutine = Instance.StartCoroutine(Instance.GetAutoPoolRoutine(prefab, pos, rot, intervalTime, returnDelay));
            }
        }
        /// <summary>
        /// 자동 풀링을 위한 코루틴을 시작합니다. 지정된 프리팹을 주기적으로 가져오고, 반환 지연 시간 후에 반환합니다. 지정된 지속 시간 후에 자동 풀링을 중지합니다.
        /// </summary>
        public static void Get(GameObject prefab, float intervalTime, float returnDelay, float duration)
        {
            Coroutine coroutine = Instance.StartCoroutine(Instance.GetAutoPoolRoutine(prefab, intervalTime, returnDelay));
            Instance.StartCoroutine(Instance.GetAutoPoolDurationRoutine(coroutine, duration));
        }
        /// <summary>
        /// 자동 풀링을 위한 코루틴을 시작합니다. 지정된 프리팹을 주기적으로 가져오고, Transform에 위치시키며, 반환 지연 시간 후에 반환합니다. 지정된 지속 시간 후에 자동 풀링을 중지합니다.
        /// </summary>
        public static void Get(GameObject prefab, Vector3 pos, Quaternion rot, float intervalTime, float returnDelay, float duration)
        {
            Coroutine coroutine = Instance.StartCoroutine(Instance.GetAutoPoolRoutine(prefab, pos, rot, intervalTime, returnDelay));
            Instance.StartCoroutine(Instance.GetAutoPoolDurationRoutine(coroutine, duration));
        }
        /// <summary>
        /// 자동 풀링을 위한 코루틴을 시작합니다. 지정된 프리팹을 주기적으로 가져오고, Transform에 위치시키며, 월드 포지션을 유지할지 여부를 설정하고, 반환 지연 시간 후에 반환합니다. 지정된 지속 시간 후에 자동 풀링을 중지합니다.
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
        /// 자동 풀링을 위한 코루틴입니다. 지정된 프리팹을 주기적으로 가져오고, 반환 지연 시간 후에 반환합니다.
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
        /// 자동 풀링을 위한 코루틴입니다. 지정된 프리팹을 주기적으로 가져오고, Transform에 위치시키며, 반환 지연 시간 후에 반환합니다.
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
        /// 자동 풀링을 위한 코루틴입니다. 지정된 프리팹을 주기적으로 가져오고, 위치와 회전을 설정하며, 반환 지연 시간 후에 반환합니다.
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
        /// 자동 풀링을 위한 코루틴입니다. 지정된 프리팹을 주기적으로 가져오고, 반환 지연 시간 후에 반환합니다. 지정된 지속 시간 후에 자동 풀링을 중지합니다.
        /// </summary>
        IEnumerator GetAutoPoolDurationRoutine(Coroutine coroutine, float duration)
        {
            yield return GetDelay(duration);
            Return(ref coroutine);
        }
        #endregion
        /// <summary>
        /// 풀 오브젝트를 찾습니다. 해당 프리팹에 대한 풀 정보가 없으면 새로 생성합니다.
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

                // 풀 딕셔너리 추가
                Instance._poolDic.Add(prefabID, newPoolInfo);
                // 풀 액티브상태 감지
                Instance.StartCoroutine(Instance.IsActiveRoutine(prefabID));
            }

            pool = Instance._poolDic[prefabID];
            pool.IsUsed = true;
            Instance._poolDic[prefabID] = pool;

            return pool;
        }
        /// <summary>
        /// 풀 정보 생성
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
        /// 풀 오브젝트 컴포넌트 추가
        /// </summary>
        private static void AddPoolObjectComponent(GameObject instance, PoolInfo info)
        {
            PooledObject poolObject = instance.GetOrAddComponent<PooledObject>();
            poolObject.PoolInfo = info;
            poolObject.SubscribePoolDeactivateEvent();
        }

        /// <summary>
        /// 풀에서 오브젝트를 가져오는 메서드입니다. 해당 프리팹에 대한 풀 정보를 찾고, 활성화된 오브젝트가 있으면 반환하고, 없으면 새로 생성합니다.
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
        /// 풀에서 오브젝트를 가져오고, 지정된 Transform에 위치시킵니다. 해당 프리팹에 대한 풀 정보를 찾고, 활성화된 오브젝트가 있으면 반환하고, 없으면 새로 생성합니다.
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
        /// 풀에서 오브젝트를 가져오고, 지정된 Transform에 위치시키며, 월드 포지션을 유지할지 여부를 설정합니다. 해당 프리팹에 대한 풀 정보를 찾고, 활성화된 오브젝트가 있으면 반환하고, 없으면 새로 생성합니다.
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
        /// 풀에서 오브젝트를 가져오고, 지정된 위치와 회전을 설정합니다. 해당 프리팹에 대한 풀 정보를 찾고, 활성화된 오브젝트가 있으면 반환하고, 없으면 새로 생성합니다.
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
        /// 풀에서 오브젝트를 가져오고, 해당 컴포넌트를 반환합니다. 해당 프리팹에 대한 풀 정보를 찾고, 활성화된 오브젝트가 있으면 반환하고, 없으면 새로 생성합니다.
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
        /// 풀에서 오브젝트를 가져오는 메서드입니다. 해당 프리팹에 대한 풀 정보를 찾고, 활성화된 오브젝트가 있으면 반환하고, 없으면 새로 생성합니다.
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
        /// 풀 오브젝트가 활성화 상태인지 확인하는 코루틴입니다.
        /// </summary>
        private IEnumerator IsActiveRoutine(int id)
        {
            float delayTime = 10f;
            float timer = MaxTimer;
            while (true)
            {
                // 풀 사용했을때 시간 초기화
                if (Instance._poolDic[id].IsUsed == true)
                {
                    timer = MaxTimer;
                    PoolInfo pool = Instance._poolDic[id];
                    pool.IsUsed = false;
                    pool.IsActive = true;
                }

                // 타이머 종료 시 
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
        /// 풀 오브젝트를 비우고 초기화합니다.
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
        /// 지정된 시간만큼 대기하는 WaitForSeconds 객체를 반환합니다. 이미 생성된 객체가 있으면 재사용합니다.
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