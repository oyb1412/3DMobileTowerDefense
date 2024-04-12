/// <summary>
/// ����Ÿ�� ����
/// </summary>
public class MagicTowerController : TowerControllerBase{
    /// <summary>
    /// �ʱ�ȭ
    /// </summary>
    protected override void Init() {
        base.Init();  //Ÿ�� �⺻ ���� �ʱ�ȭ
    }

    /// <summary>
    /// Ÿ�� ���� �� ȣ��
    /// �ִϸ��̼� �̺�Ʈ���� ȣ��
    /// </summary>
    protected override void OnAttackEvent() {
        base.OnAttackEvent();  //Ÿ�� �⺻ ���� �̺�Ʈ ȣ��
        if (!_targetEnemy) {  //Ÿ���� �������� ���� �� ���� ���� �� ���� ���
            ChangeState(Define.TowerState.Idle);  //Idle���·� ����
            return;
        }

        MagicProjectileController projectile = Managers.Resources.Instantiate(_projectilePath, null).GetComponent<MagicProjectileController>();  //�߻�ü ����
        projectile.Init(_firePoint.position, _status.AttackDamage, _targetEnemy, _base);  //�߻�ü �ʱ�ȭ
        Managers.Audio.PlaySfx(Define.SfxType.MagicProjectile);  //�߻� ȿ���� ���
        _currentAttackDelay = 0;  //�߻� ������ �ʱ�ȭ
    }
}
