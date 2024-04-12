using UnityEngine;

public class MagicProjectileController : ProjectileControllerBase {
    [SerializeField] private EnemyController _target;
    private const float PROJECTILE_VELOCITY = 70f;  //�߻�ü �ӵ�


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
        _target = target.GetComponentInParent<EnemyController>();  //Ÿ�� ���ӿ�����Ʈ���� �ֳʹ���Ʈ�ѷ� ����

    }

    /// <summary>
    /// �߻�ü �̵�
    /// </summary>
    private void FixedUpdate() {
        if (!GameSystem.Instance.IsPlay())  //���� ���� �� return
            return;

        if (transform.position.y < LIMITPOS_Y)  //������ġ�� ��� �� �ı�
            Managers.Resources.Destroy(gameObject);

        if (!Util.NullCheck(_target.gameObject) && _target.Status.CurrentHp > 0)  //Ÿ���� NULL�� �ƴϰ� HP�� ����� ���¶��
            transform.LookAt(_target.transform);  //Ÿ�� ��ġ�� ȸ��

        _rigidbody.velocity = transform.forward * PROJECTILE_VELOCITY;  //�߻�ü �̵�
    }
    private void OnCollisionEnter(Collision collision) {
        if (!collision.collider.CompareTag(Define.TAG_ENEMY))  //�ֳʹ̰� �ƴ� �ݶ��̴����� �浹 ����
            return;

        Crash(collision);  //�ֳʹ̿��� �浹
    }
}