using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUI : MonoBehaviour
{
    private Dictionary<string, GameObject> gameObjectDic;
    private Dictionary<(string, System.Type), Component> componentDic;

    void Awake()
    {
        Bind();
    }

    // 빠른 시간에 게임오브젝트만 바인딩
    protected void Bind()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>(true);
        gameObjectDic = new Dictionary<string, GameObject>(transforms.Length << 2);
        foreach (Transform child in transforms)
        {
            gameObjectDic.TryAdd(child.gameObject.name, child.gameObject);
        }

        componentDic = new Dictionary<(string, System.Type), Component>();
    }

    // 비교적 오랜 시간에 게임오브젝트와 모든 컴포넌트 바인딩
    protected void BindAll()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>(true);
        gameObjectDic = new Dictionary<string, GameObject>(transforms.Length << 2);

        foreach (Transform child in transforms)
        {
            gameObjectDic.TryAdd(child.gameObject.name, child.gameObject);
        }

        Component[] components = GetComponentsInChildren<Component>(true);
        componentDic = new Dictionary<(string, System.Type), Component>(components.Length << 4);
        foreach (Component child in components)
        {
            componentDic.TryAdd((child.gameObject.name, components.GetType()), child);
        }
    }

    // 이름이 name인 UI 게임오브젝트 가져오기
    // GetUI("Key") : Key 이름의 게임오브젝트 가져오기
    public GameObject GetUI(in string name)
    {
        gameObjectDic.TryGetValue(name, out GameObject gameObject);
        return gameObject;
    }

    // 이름이 name인 UI에서 컴포넌트 T 가져오기
    // GetUI<Image>("Key") : Key 이름의 게임오브젝트에서 Image 컴포넌트 가져오기
    public T GetUI<T>(in string name) where T : Component
    {
        (string, System.Type) key = (name, typeof(T));

        componentDic.TryGetValue(key, out Component component);
        if (component != null)
            return component as T;

        gameObjectDic.TryGetValue(name, out GameObject gameObject);
        if (gameObject == null)
            return null;

        component = gameObject.GetComponent<T>();
        if (component == null)
            return null;

        componentDic.TryAdd(key, component);
        return component as T;
    }
}