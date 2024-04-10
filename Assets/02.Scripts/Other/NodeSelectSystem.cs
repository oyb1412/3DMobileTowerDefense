using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeSelectSystem : MonoBehaviour
{
    [SerializeField]private LayerMask SelectLayer;
    private ISelectedObject _lastSelectObject;
    private float _touchTime;
    private bool _isLongTouch;
    private bool _isTouch;
    private void Start() {

    }
    private void Update() {

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (Input.GetMouseButtonDown(0)) {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Select();
        }
#elif UNITY_ANDROID
        if (Input.touchCount == 1 ) {
            Touch touch = Input.GetTouch(0);

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;

            switch (touch.phase) {
                case TouchPhase.Began:
                    if(!_isTouch) {
                        _touchTime = Time.time;
                        _isLongTouch = false;
                        _isTouch = true;
                    }
                    
                    break;

                case TouchPhase.Stationary:
                    if (!_isTouch) return;

                    if(Time.time - _touchTime > 0.1f && !_isLongTouch) {
                        _isLongTouch = true;
                    }
                    break;

                case TouchPhase.Ended:
                    if(!_isTouch) return;

                    if (!_isLongTouch) {
                        Select();
                    }

                    _isTouch = false;
                break;
            }
        }

#endif

    }

    private void Select() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool col = Physics.Raycast(ray, out var hit, float.MaxValue, SelectLayer);
        DeSelect();
        
        if (col) {
            Managers.Audio.PlaySfx(Define.SfxType.ObjectSelect);
            if (hit.collider.TryGetComponent<ISelectedObject>(out _lastSelectObject))
                _lastSelectObject.OnSelect();
        }
    }

    private void DeSelect() {
        if (_lastSelectObject == null)
            return;

        if (!_lastSelectObject.IsValid())
            return;

        _lastSelectObject.OnDeSelect();
        _lastSelectObject = null;
    }

}
