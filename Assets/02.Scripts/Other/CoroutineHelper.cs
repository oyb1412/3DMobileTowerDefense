using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{
    public static CoroutineHelper Instance;
    private void Awake() {
        Instance = this;
    }
}
