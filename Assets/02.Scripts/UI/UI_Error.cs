using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Error : MonoBehaviour
{
    private Text _errorText;
    private const float DestroyTime = 0.8f;

    public void Init(string text) {
        _errorText = GetComponentInChildren<Text>();
        _errorText.text = text;
        StartCoroutine(CoMove());
        StartCoroutine(CoDestroy());
    }

    IEnumerator CoMove() {
        while (true) {
            transform.position += Vector3.up / 2f;
            yield return null;
        }
    }
    IEnumerator CoDestroy() {
        yield return new WaitForSeconds(DestroyTime);
        Managers.Resources.Destroy(gameObject);
    }
}
