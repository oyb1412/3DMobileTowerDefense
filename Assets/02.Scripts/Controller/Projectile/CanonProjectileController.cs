using UnityEngine;
using DG.Tweening;

/// <summary>
/// ĳ��Ÿ�� �߻�ü ����
/// </summary>
public class CanonProjectileController : ProjectileControllerBase {
    private Vector3 _targetPos;  //���� Ÿ���� ��ġ
    private const float PROJECTILE_HEIGHT_POS = 10f;  //�߻�ü�� �̵��� ����
    private const float PROJECTILE_DURATION = 0.5f;  //�߻�ü �̵��Ϸ���� �ɸ��� �ð�

    /// <summary>
    /// �߻�ü �ʱ�ȭ
    /// </summary>
    /// <param name="pos">�߻�ü �ʱ� ��ġ</param>
    /// <param name="dir">�߻�ü �ʱ� ȸ����</param>
    /// <param name="damage">�߻�ü ������</param>
    /// <param name="target">Ÿ�� �ֳʹ� ���ӿ�����Ʈ</param>
    /// <param name="shooter">�߻��� Ÿ�� ���ӿ�����Ʈ</param>
    public override void Init(Vector3 pos, int damage, GameObject target, GameObject shooter) {
        base.Init(pos, damage, target, shooter);  //�߻�ü �⺻ ���� �ʱ�ȭ
        _targetPos = target.transform.position;  //Ÿ���� ��ġ ����

        Vector3 controlPoint = (pos + _targetPos) / 2 + Vector3.up * PROJECTILE_HEIGHT_POS;  //�߻�ü�� ����� �ִ���ġ ���
        Vector3[] path = new Vector3[] { pos, controlPoint, _targetPos };  //�߻�ü�� ���� ��ġ, �ִ� ��ġ, Ÿ���� ��ġ�� ������� path ���

        transform.DOPath(path, PROJECTILE_DURATION, PathType.Linear)  //���� path��� �߻�ü �̵�
            .SetOptions(false)
            .SetEase(Ease.InOutQuad);
    }

    private void FixedUpdate() {
        if (!GameSystem.Instance.IsPlay())  //������ ����Ǹ� return
            return;

        if (transform.position.y <= LIMITPOS_Y ||  //�߻�ü�� ������ġ�� ����� ����
            transform.position.y <= _targetPos.y)
            Crash();
    }


}