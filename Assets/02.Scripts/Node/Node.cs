using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : SelectedObject, ISelectedObject {
    private MeshRenderer _mesh;

    public Transform MyTransform { get {return transform; }  }

    private void Start() {
        _mesh = GetComponent<MeshRenderer>();
    }
    public ISelectedObject OnSelect() {
        Managers.Instance.creator.SelectNode(true, transform.position, this);
        _mesh.material = Managers.Data.GreenMaterial;
        IsSelected = true;
        return this;
    }

    public void OnDeSelect() {
        Managers.Instance.creator.SelectNode(false, transform.position);
        IsSelected = false;
        _mesh.material = Managers.Data.DefaultMaterial;
    }
}
