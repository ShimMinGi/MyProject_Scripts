using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    protected AttackInfoData currentAttackInfo;
    protected bool alreadyAppliedForce;
    protected bool alreadyAppliedDamage;

    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

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
        StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }

    public override void Update()
    {
        ForceMove();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }


    protected bool IsAttackComplete(float normalizedTime)
    {
        if (normalizedTime >= 1f)
        {
            stateMachine.TransitState(stateMachine.IdleState);

            stateMachine.Player.CurrentWeapon?.DeactiveCollider();

            return true;
        }

        return false;
    }

    protected virtual bool ShouldApplyForce(float normalizedTime)
    {
        return !alreadyAppliedForce && normalizedTime >= currentAttackInfo.ForceTransitionTime;
    }

    protected void TryApplyForce()
    {
        Player player = stateMachine.Player;
        alreadyAppliedForce = true;
        player.ForceHandler.Reset();
        player.ForceHandler.AddForce(player.transform.forward * currentAttackInfo.Force);
    }

    protected virtual bool ShouldApplyDamage(float normalizedTime)
    {
        return !alreadyAppliedDamage && normalizedTime >= currentAttackInfo.ActivationTime;
    }

    protected void ApplyAttack()
    {
        switch (currentAttackInfo.DetectionType)
        {
            case DetectionType.WeaponCollider:
                ActivateWeaponCollider();
                break;
            case DetectionType.BoxCast:
                PerformBoxCast();
                break;
        }
    }

    private void ActivateWeaponCollider()
    {
        alreadyAppliedDamage = true;

        Weapon weapon = stateMachine.Player.CurrentWeapon;
        if (weapon != null)
        {
            weapon.OnTargetDetected -= ApplyDamage;
            weapon.OnTargetDetected += ApplyDamage;
            weapon.ActivateCollider();
        }
    }

    private void PerformBoxCast()
    {
        alreadyAppliedDamage = true;
        Transform playerTransform = stateMachine.Player.transform;

        Vector3 boxCenter = playerTransform.position +
            playerTransform.forward * currentAttackInfo.BoxCastSize.z / 2f +
            playerTransform.up * currentAttackInfo.BoxCastSize.y / 2f;
        Vector3 boxSize = currentAttackInfo.BoxCastSize;
        Quaternion rotation = playerTransform.rotation;

        RaycastHit[] hits = Physics.BoxCastAll(
            boxCenter,
            boxSize / 2,
            playerTransform.forward,
            rotation,
            0f,
            stateMachine.Player.entityLayerMask);

        foreach (RaycastHit hit in hits)
        {
            ApplyDamage(hit.collider.GetComponent<HealthSystem>());
        }

        //stateMachine.Player.GizmoDrawer?.Initialize(boxCenter, boxSize, rotation, 0.5f);
    }

    private void ApplyDamage(HealthSystem healthSystem)
    {
        if (healthSystem != null &&
            healthSystem.Faction != stateMachine.Player.HealthSystem.Faction
            )
        {
            Debug.Log($"{healthSystem.name} -> Atk {currentAttackInfo.Damage}");
            healthSystem.TakeDamage(currentAttackInfo.Damage);
        }
    }

}
