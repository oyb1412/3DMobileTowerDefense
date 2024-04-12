/// <summary>
/// ��ó Ÿ�� ����
/// </summary>
public class ArcherTowerController : TowerControllerBase {
    /// <summary>
    /// �ʱ�ȭ
    /// </summary>
    protected override void Init() => base.Init();  //Ÿ�� �⺻ ���� �ʱ�ȭ
   
    /// <summary>
    /// Ÿ�� ���� �� ȣ��
    /// �ִϸ��̼� �̺�Ʈ���� ȣ��
    /// </summary>
    protected override void OnAttackEvent() {
        base.OnAttackEvent(); //Ÿ�� �⺻ ���� �̺�Ʈ ȣ��

        if (!_targetEnemy) {  //���� ����� �������� ���� �� ���� ���� �� ���� ���
            ChangeState(Define.TowerState.Idle);  //IDLE���·� ����
            return;
        }
            
        ArcherProjectileController projectile = Managers.Resources.Instantiate(_projectilePath, null).GetComponent<ArcherProjectileController>();  //�߻�ü ����
        projectile.Init(_firePoint.position, _status.AttackDamage, _targetEnemy, _base);  //�߻�ü �ʱ�ȭ
        Managers.Audio.PlaySfx(Define.SfxType.BeamProjectile);  //�߻� ȿ���� ���
        _currentAttackDelay = 0;  //���� ������ �ʱ�ȭ
    }
}
