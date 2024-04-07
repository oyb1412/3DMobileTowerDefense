using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action<PointerEventData> OnEnterHandler;
    public Action<PointerEventData> OnExitHandler;
    public Action<PointerEventData> OnClickHandler;
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickHandler?.Invoke(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEnterHandler?.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData) {
        OnExitHandler?.Invoke(eventData);
    }
}
