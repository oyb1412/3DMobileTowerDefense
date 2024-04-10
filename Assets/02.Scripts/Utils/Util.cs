using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class Util : MonoBehaviour
{
    private static float _touchTime;
    private static bool _isLongTouch;
    public static GameObject FindChild(GameObject go, string name, bool recursion) 
    {
        if (go == null || string.IsNullOrEmpty(name))
            return null;

        if (recursion) {
            foreach (Transform child in go.transform) {
                if (child.name == name)
                    return child.gameObject;

                GameObject found = FindChild(child.gameObject, name, recursion);
                if (found != null) return found;
            }
        } else {
            foreach (Transform child in go.transform) {
                if (child.name == name)
                    return child.gameObject;
            }
        }

        return null;
    }

    
 
    public static void SetOutLine(GameObject go, bool trigger) {

        Outline outline = go.GetComponent<Outline>();
        if (trigger) {
            outline.effectColor = Color.red;
            outline.effectDistance = new Vector2(10f, -10f);
        } else {
            outline.effectColor = Color.black;
            outline.effectDistance = new Vector2(2f, -2f);
        }

    }

    public static IEnumerator CoDestroy(GameObject go, float time) {
        yield return new WaitForSeconds(time);
        Managers.Resources.Destroy(go);
    }

    public static IEnumerator CoActive(GameObject go, float time, bool trigger = false) {
        yield return new WaitForSeconds(time);
        go.SetActive(trigger);
    }

    public static bool NullCheck(GameObject go) {
        if (go == null) return true;
        if (!go.activeInHierarchy) return true;
        return false;
    }
    
    public static void SetButtonEvent(Button btn,  UnityAction[] actions = null, UnityAction action = null, Define.SfxType sfxType = Define.SfxType.BtnSelect) {
        if(actions == null && action == null) {
            Debug.Log("버튼에 액션이 추가되지 않았습니다");
            return;
        }
        
        if(actions != null) {
            for(int i = 0; i< actions.Length; i++) {
                btn.onClick.AddListener(actions[i]);
            }
        }
        else if(action != null) {
            btn.onClick.AddListener(action);
        }

        btn.onClick.AddListener(() => Managers.Audio.PlaySfx(sfxType));
    }

    public static void ExitGame() {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public static void RectToWorldPosition(Vector3 pos, RectTransform rect) {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(new Vector3(pos.x, pos.y, pos.z));
        rect.position = screenPoint;
    }
    public static T GetorAddComponent<T>(GameObject go) where T : Component
    {
        var component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    public static void CreateErrorMessage(string message) {
        UI_Error error = Managers.Resources.Instantiate("UI/UI_Error", null).GetComponent<UI_Error>();
        error.Init(message);
    }
}
