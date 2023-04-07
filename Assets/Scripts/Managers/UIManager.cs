using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : Singleton<UIManager>
{
    Transform uiRoot;
    Transform UIRoot
    {
        get
        {

            if (uiRoot == null)
            {
                if (GameObject.Find("UIRoot") == null)
                {
                    uiRoot = Instantiate(Resources.Load<GameObject>("UI/UIRoot").transform);
                }
                else
                {
                    uiRoot = GameObject.Find("UIRoot").transform;
                }
              
            }

            return uiRoot;
        }
    }

    public UIRoot GetUIRoot()
    {
        return UIRoot.GetComponent<UIRoot>();
    }
    
    
    // EventCamera 선정
    private Transform _eventCamera;
    public Transform EventCamera
    {
        get
        {
            if (_eventCamera == null)
                _eventCamera = Camera.main.transform;
            return _eventCamera;
        }
        set => _eventCamera = value;
    }

    Dictionary<string, GameObject> UIList = new Dictionary<string, GameObject>();

    // 켜져있는 모든 앱을 종료하고 초기화합니다.
    public void AppInitial()
    {
        Dictionary<string, GameObject>.ValueCollection val = UIList.Values;
        List<GameObject> valList = new List<GameObject>();
        valList.AddRange(val);

        for (int idx = 0; idx < valList.Count; ++idx)
            Destroy(valList[idx]);
        UIList.Clear();
    }

    // UIList에 등록된 모든 UI를 숨기거나 보여줄 수 있습니다.
    public void ShowUIList(bool isShow)
    {
        Dictionary<string, GameObject>.ValueCollection val = UIList.Values;
        List<GameObject> valList = new List<GameObject>();
        valList.AddRange(val);

        for (int idx = 0; idx < valList.Count; ++idx)
            valList[idx].SetActive(isShow);
    }
    
    /// <summary>
    /// UI를 리스트에서 제거하고 파괴합니다.
    /// </summary>
    /// <typeparam name="T">종료시킬 UI Class</typeparam>
    public void RemoveUI<T>()
    {
        string className = typeof(T).Name;
        if (UIList.ContainsKey(className))
        {
            if (UIList[className].gameObject != null)
            {
                Destroy(UIList[className]);
                UIList.Remove(className);
            }
        }
    }

    /// <summary>
    /// UI를 리스트에 등록합니다.
    /// </summary>
    /// <param name="go">추가할 UI의 Gameobject</param>
    /// <typeparam name="T">등록할 UI Class</typeparam>
    public void AddUI<T>(GameObject go)
    {
        string className = typeof(T).Name;
        if (UIList.ContainsKey(className) == false)
            UIList.Add(className, go);
    }


    /// <summary>
    /// UI를 생성합니다.
    /// 이전에 UI가 존재했다면 지우고 다시 생성합니다.
    /// </summary>
    /// <typeparam name="T">생성할 UI Class</typeparam>
    /// <param name="parent">부모가 필요하다면 선언해줍니다.</param>
    /// <param name="needCanvas"></param>
    /// <returns></returns>
    public T CreateUI<T>(Transform parent = null, bool needCanvas = false)
    {
        if (needCanvas)
        {
            if (parent == null)
                parent = UIRoot;
        }

        string className = typeof(T).Name;
        string path = GetPath<T>();

        try
        {
            if (IsUIExist<T>())
                UIList.Remove(className);
            
            GameObject go = Instantiate(Resources.Load<GameObject>(path), parent);

            if (go.GetComponent<Canvas>())
                go.GetComponent<Canvas>().worldCamera = Camera.main;

            if (needCanvas)
            {
                RectTransform rect = go.GetComponent<RectTransform>();
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;


                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
                rect.localScale = Vector3.one;
            }
            go.transform.SetAsLastSibling();

            T temp = go.GetComponent<T>();
            AddUI<T>(go);

            return temp;
        }
        catch (Exception e)
        {
            Debug.LogError("Create Error : " + e.Message);
            GameObject go = Instantiate(Resources.Load<GameObject>(path), parent);
            T temp = go.GetComponent<T>();
            if (UIList.ContainsKey(className))
                UIList.Remove(className);
            AddUI<T>(go);
            return temp;
        }
    }


    /// <summary>
    /// UI를 받아옵니다. 받아올 UI가 생성되있지않다면 생성 후 반환합니다.
    /// </summary>
    /// <typeparam name="T">받아올 UI Class</typeparam>
    /// <param name="parent">부모가 필요하다면 선언해줍니다.</param>
    /// <param name="needCanvas"></param>
    /// <returns></returns>
    public T GetUI<T>(Transform parent = null, bool needCanvas = false) where T : Component
    {

        if (UIList.ContainsKey(typeof(T).Name) && UIList[typeof(T).Name] != null)
            return UIList[typeof(T).Name].GetComponent<T>();
        else
            return CreateUI<T>(parent, needCanvas);
    }

    /// <summary>
    /// UI 존재 여부를 체크합니다.
    /// </summary>
    /// <typeparam name="T">체크할 UI Class</typeparam>
    /// <returns>UI가 존재하는지 여부</returns>
    public bool IsUIExist<T>()
    {
        if (UIList.ContainsKey(typeof(T).Name) && UIList[typeof(T).Name] != null)
            return true;
        else
            return false;
    }
    
    /// <summary>
    /// UI가 활성화 되어 있는지 체크합니다.
    /// 존재하지 않는 다면 false 입니다.
    /// 만약 UIBase의 Disable 함수가 실행중이면
    /// false를 리턴합니다.
    /// </summary>
    /// <typeparam name="T">체크할 UI Class</typeparam>
    /// <returns></returns>
    public bool IsUIActive<T>()
    {
        if (IsUIExist<T>() && UIList[typeof(T).Name].activeSelf)
        {
            bool isEnabled = UIList[typeof(T).Name].GetComponent<UIBase<T>>().IsEnabled;

            return isEnabled;
        }
        else
            return false;
    }
    
    private string GetPath<T>()
    {
        string className = typeof(T).Name;
        return "UI/" + className;
    }
}
