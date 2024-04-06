using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Net;

public class CanonProjectileController : ProjectileControllerBase {
    private const float _limitYPos = -0.4f;
    private Vector3 _targetPos;
    [SerializeField]private float height;
    [SerializeField] private float duration;
    public override void Init(Vector3 pos, Vector3 dir, int damage, GameObject target, GameObject shooter) {
        base.Init(pos, dir, damage, target, shooter);
        _targetPos = target.transform.position;
        Vector3 controlPoint = (pos + _targetPos) / 2 + Vector3.up * height;
        Vector3[] path = new Vector3[] { pos, controlPoint, _targetPos };

        transform.DOPath(path, duration, PathType.Linear)
            .SetOptions(false)
            .SetEase(Ease.InOutQuad);
    }

    private void FixedUpdate() {
        if (transform.position.y < _limitYPos)
            Crash();
    }
}