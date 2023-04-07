using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBase<T> : MonoBehaviour
{
    public bool IsEnabled { get; private set; }

    protected CanvasScaler CanvasScaler { get; private set; }
    
    public virtual void Awake()
    {
        IsEnabled = true;

        CanvasScaler = GetComponent<CanvasScaler>();
        
        OpenUI();
    }

    public virtual void OpenUI()
    {
        IsEnabled = true;
        
        if (UIManager.Instance.IsUIExist<T>() == false)
            UIManager.Instance.AddUI<T>(gameObject);
    }

    public virtual void CloseUI()
    {
        Destroy(gameObject);
    }

    public void OnDestroy()
    {
        UIManager.Instance.RemoveUI<T>();
    }
    
    /// <summary>
    /// UI의 알파값을 0으로 바꾸고 클릭을 막음
    /// 실제로 SetActive를 바꾸지는 않지만 화면에 출력되지 않음
    /// </summary>
    public virtual void Disable()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
        IsEnabled = false;
    }


    /// <summary>
    /// Disable로 없어진 UI를 다시 출력함 
    /// </summary>
    public virtual void Enable()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            return;
        }
        
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        IsEnabled = true;
    }
}