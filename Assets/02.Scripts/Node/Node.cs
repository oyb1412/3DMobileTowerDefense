using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour, ISelectedObject {
    private MeshRenderer _mesh;

    public Transform MyTransform { get {return transform; }  }

    private void Start() {
        _mesh = GetComponent<MeshRenderer>();
    }
    public ISelectedObject OnSelect() {
        Managers.Instance.creator.SelectNode(true, transform.position, this);
        _mesh.material = Managers.Data.GreenMaterial;
        return this;
    }

    public void OnDeSelect() {
        Managers.Instance.creator.SelectNode(false, transform.position);
        _mesh.material = Managers.Data.DefaultMaterial;
    }

    public bool IsValid() {
        return this;
    }
}
