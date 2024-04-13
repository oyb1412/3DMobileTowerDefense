using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CouroutineHelper : MonoBehaviour
{
    public static CouroutineHelper Instance;
    private void Awake() {
        Instance = this;
    }
}
