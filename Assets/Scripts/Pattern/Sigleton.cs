using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : Component
{
    public static GameObject ParentObj
    {
        get
        {
            if (Parent == null)
            {
                Parent = GameObject.Find("ParentObj");
                if (Parent == null)
                {
                    Parent = new GameObject("ParentObj");
                }
            }
            return Parent;
        }
    }
    static GameObject Parent = null;
    // Destroy 여부 확인용
    private static bool _ShuttingDown = false;
    private static T _Instance;

    public static T Instance
    {
        get
        {
            if (_ShuttingDown)
            {
                Debug.Log("[Singleton] Instance '" + typeof(T) + "' already destroyed. Returning null.");
                return null;
            }

            if (_Instance == null)
            {
                // 인스턴스 존재 여부 확인
                _Instance = (T)FindObjectOfType(typeof(T));

                // 아직 생성되지 않았다면 인스턴스 생성
                if (_Instance == null)
                {
                    // 새로운 게임오브젝트를 만들어서 싱글톤 Attach
                    var singletonObject = new GameObject();
                    _Instance = singletonObject.AddComponent<T>();
                    singletonObject.name = $"[{typeof(T).ToString()}]";
                    singletonObject.transform.SetParent(ParentObj.transform);
                }
            }
            if (_Instance.transform.parent == null)
            {
                _Instance.transform.SetParent(ParentObj.transform);
            }

            return _Instance;
        }
    }

    public static void Init()
    {
        Debug.Log($"Init : {Instance.name}");
    }
    
    private void OnApplicationQuit()
    {
        _ShuttingDown = true;
    }

    private void OnDestroy()
    {
        // _ShuttingDown = true;
    }

    public void Release()
    {
        Destroy(_Instance.gameObject);
        _Instance = null;
    }
}