using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    protected AttackInfoData currentAttackInfo;
    protected bool alreadyAppliedForce;
    protected bool alreadyAppliedDamage;

    public EnemyAttackState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {

    }

    public override void Enter()
    {
        currentAttackInfo = stateMachine.CurrentAttackInfo;
        alreadyAppliedForce = false;
        alreadyAppliedDamage = false;

        if (currentAttackInfo == null)
        {
            stateMachine.TransitState(stateMachine.IdleState);
            return;
        }

        base.Enter();
        StartAnimation(stateMachine.Enemy.AnimationData.AttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.AttackParameterHash);
    }

    public override void Update()
    {
        ForceMove();
    }

    protected bool IsAttackComplete(float normalizedTime)
    {
        if (normalizedTime >= 1f)
        {
            stateMachine.TransitState(stateMachine.IdleState);

            Weapon weapon = stateMachine.Enemy.CurrentWeapon;
            weapon.DeactiveCollider();
            return true;
        }

        return false;
    }

    protected virtual bool ShouldApplyForce(float normalizedTime)
    {
        return !alreadyAppliedForce && normalizedTime >= currentAttackInfo.ForceTransitionTime;
    }

    protected virtual bool ShouldApplyDamage(float normalizedTime)
    {
        return !alreadyAppliedDamage && normalizedTime >= currentAttackInfo.ActivationTime;
    }

    protected void TryApplyForce()
    {
        alreadyAppliedForce = true;
        stateMachine.Enemy.ForceHandler.Reset();
        stateMachine.Enemy.ForceHandler.AddForce(stateMachine.Enemy.transform.forward * currentAttackInfo.Force);
    }

    protected void ApplyAttack()
    {
        switch (currentAttackInfo.DetectionType)
        {
            case DetectionType.WeaponCollider:
                ActivateWeaponCollider();
                break;
            case DetectionType.BoxCast:
                PerformBoxCase();
                break;
        }
    }
    private void ActivateWeaponCollider()
    {
        alreadyAppliedDamage = true;
        Weapon weapon = stateMachine.Enemy.CurrentWeapon;
        if (weapon != null)
        {
            weapon.OnTargetDetected -= ApplyDamage;
            weapon.OnTargetDetected += ApplyDamage;

            weapon.ActivateCollider();
        }
    }

    private void PerformBoxCase()
    {
        alreadyAppliedDamage = true;

        Transform enemyTransform = stateMachine.Enemy.transform;
        Vector3 boxCenter = enemyTransform.position +
            enemyTransform.forward * currentAttackInfo.BoxCastSize.z / 2f +
            enemyTransform.up * currentAttackInfo.BoxCastSize.y / 2f;
        Vector3 boxSize = currentAttackInfo.BoxCastSize;
        Quaternion rotation = enemyTransform.rotation;

        RaycastHit[] hits = Physics.BoxCastAll(
            boxCenter, boxSize / 2, enemyTransform.forward, rotation, stateMachine.Enemy.entityLayerMask);

        foreach (RaycastHit hit in hits)
        {
            ApplyDamage(hit.collider.GetComponent<HealthSystem>());
        }
    }

    private void ApplyDamage(HealthSystem healthSystem)
    {
        if (healthSystem != null
            && healthSystem.Faction != stateMachine.Enemy.HealthSystem.Faction)
        {
            Debug.Log($"{healthSystem.name} -> {currentAttackInfo.Damage} Damage");
            healthSystem.TakeDamage(currentAttackInfo.Damage);
        }
    }

}
