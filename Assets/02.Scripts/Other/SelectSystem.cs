using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ������Ʈ ����
/// </summary>
public class SelectSystem : MonoBehaviour
{
    [SerializeField]private LayerMask SelectLayer;  //������ ������Ʈ���� ���̾�
    private ISelectedObject _lastSelectObject;  //������ ������Ʈ
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
    /// ������Ʈ ����
    /// </summary>
    private void Select() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool col = Physics.Raycast(ray, out var hit, float.MaxValue, SelectLayer);
        DeSelect();  //���� ����
        
        if (col) {  //������ ������Ʈ ����
            Managers.Audio.PlaySfx(Define.SfxType.ObjectSelect);
            if (hit.collider.TryGetComponent<ISelectedObject>(out _lastSelectObject))
                _lastSelectObject.OnSelect();
        }
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    private void DeSelect() {
        if (_lastSelectObject == null)
            return;

        if (!_lastSelectObject.IsValid())
            return;

        _lastSelectObject.OnDeSelect();  //���õ� ������Ʈ�� �����ϸ�, ���� ����
        _lastSelectObject = null;
    }

}
