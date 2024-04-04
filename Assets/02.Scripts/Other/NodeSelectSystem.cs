using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeSelectSystem : MonoBehaviour
{
    [SerializeField]private LayerMask SelectLayer;
    private ISelectedObject _lastSelectObject;
    private void Start() {

    }
    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool col = Physics.Raycast(ray, out var hit, float.MaxValue, SelectLayer);
            DeSelect();

            if (col) {
                if(hit.collider.TryGetComponent<ISelectedObject>(out _lastSelectObject))
                    _lastSelectObject.OnSelect();
            }
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
