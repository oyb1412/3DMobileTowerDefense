using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    protected void BindEvent(Action<PointerEventData> action, Define.MouseEventType type)
    {
        UI_EventHandler evt = Util.GetorAddComponent<UI_EventHandler>(gameObject);

        switch (type)
        {
            case Define.MouseEventType.Enter:
                evt.OnEnterHandler += action;
                break;
            case Define.MouseEventType.LeftMouseDown:
                evt.OnClickHandler += action;
                break;
            case Define.MouseEventType.Exit:
                evt.OnExitHandler += action;
                break;
        }
    }
}
