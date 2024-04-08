using UnityEngine;

public class Bilboard : MonoBehaviour {
    private Camera _mainCamera;

    private void Start() {
        _mainCamera = Managers.MainCamera.Camera;
    }

    private void LateUpdate() {
        transform.LookAt(_mainCamera.transform.position);
    }
}