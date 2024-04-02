using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectedObject
{
    ISelectedObject OnSelect();
    void OnDeSelect();

    Transform MyTransform { get; }
}
