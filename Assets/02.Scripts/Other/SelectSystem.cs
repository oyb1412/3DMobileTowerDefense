using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 오브젝트 선택
/// </summary>
public class SelectSystem : MonoBehaviour
{
    [SerializeField]private LayerMask SelectLayer;  //선택할 오브젝트들의 레이어
    private ISelectedObject _lastSelectObject;  //선택한 오브젝트
    private float _touchTime;  
    private bool _isLongTouch;
    private bool _isTouch;

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

    /// <summary>
    /// 오브젝트 선택
    /// </summary>
    private void Select() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool col = Physics.Raycast(ray, out var hit, float.MaxValue, SelectLayer);
        DeSelect();  //선택 해제
        
        if (col) {  //감지된 오브젝트 선택
            Managers.Audio.PlaySfx(Define.SfxType.ObjectSelect);
            if (hit.collider.TryGetComponent<ISelectedObject>(out _lastSelectObject))
                _lastSelectObject.OnSelect();
        }
    }

    /// <summary>
    /// 선택 해제
    /// </summary>
    private void DeSelect() {
        if (_lastSelectObject == null)
            return;

        if (!_lastSelectObject.IsValid())
            return;

        _lastSelectObject.OnDeSelect();  //선택된 오브젝트가 존재하면, 선택 해제
        _lastSelectObject = null;
    }

}
