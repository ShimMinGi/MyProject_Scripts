using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBaseState : IState
{
    protected EnemyStateMachine stateMachine;
    protected readonly GroundData groundData;
    protected readonly AttackData attackData;

    public EnemyBaseState(EnemyStateMachine enemyStateMachine)
    {
        this.stateMachine = enemyStateMachine;
        groundData = stateMachine.Enemy.Data.GroundData;
        attackData = stateMachine.Enemy.Data.AttackData;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void Update()
    {
        Move();
    }

    public virtual void HandleInput()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Enemy.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.Enemy.Animator.SetBool(animationHash, false);
    }

    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();

        Rotate(movementDirection);
        Move(movementDirection);
    }

    private Vector3 GetMovementDirection()
    {
        Vector3 dir = stateMachine.Target.transform.position - stateMachine.Enemy.transform.position;
        dir.y = 0;
        return dir.normalized;
    }

    private void Move(Vector3 movementDirection)
    {
        float movementSpeed = GetMovementSpeed();
        stateMachine.Enemy.Controller.Move(
            ((movementDirection * movementSpeed)
            + stateMachine.Enemy.ForceHandler.Movement)
            * Time.deltaTime);
    }

    protected void ForceMove()
    {
        stateMachine.Enemy.Controller.Move(stateMachine.Enemy.ForceHandler.Movement * Time.deltaTime);
    }

    private void Rotate(Vector3 movementDirection)
    {
        if (movementDirection != Vector3.zero)
        {
            Transform enemyTransform = stateMachine.Enemy.transform;
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            enemyTransform.rotation = Quaternion.Slerp(enemyTransform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }

    private float GetMovementSpeed()
    {
        float movementSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return movementSpeed;
    }

    protected float GetNormalizedTime(Animator animator, string tag)
    {
        if (animator.IsInTransition(0))
        {
            var nextInfo = animator.GetNextAnimatorStateInfo(0);
            return nextInfo.IsTag(tag) ? nextInfo.normalizedTime : -1f;
        }
        else
        {
            var currtenInfo = animator.GetCurrentAnimatorStateInfo(0);
            return currtenInfo.IsTag(tag) ? currtenInfo.normalizedTime : -1f;
        }
    }

    protected bool SearchForTaget()
    {
        Collider[] hits = Physics.OverlapSphere(stateMachine.Enemy.transform.position,
            stateMachine.Enemy.Data.SearchingDistance, stateMachine.Enemy.entityLayerMask);

        foreach (Collider hit in hits)
        {
            HealthSystem healthSystem = hit.GetComponent<HealthSystem>();
            if (healthSystem != null
                && !healthSystem.IsDead
                && healthSystem.Faction != stateMachine.Enemy.HealthSystem.Faction)
            {
                Debug.Log($"타켓 발견 : {healthSystem.name}");
                stateMachine.Target = healthSystem;
                return true;
            }
        }

        stateMachine.Target = null;
        return false;
    }

    protected bool IsInAttackRange()
    {
        if (!stateMachine.Target || stateMachine.Target.IsDead)
        { return false; }

        if (stateMachine.CurrentAttackInfo == null)
            SelectAttack();

        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Enemy.transform.position).sqrMagnitude;

        switch (stateMachine.CurrentAttackInfo.DetectionType)
        {
            case DetectionType.WeaponCollider:
                return playerDistanceSqr <= 1.5f;

            case DetectionType.BoxCast:
                return playerDistanceSqr <= stateMachine.CurrentAttackInfo.BoxCastSize.z * stateMachine.CurrentAttackInfo.BoxCastSize.z;
        }

        return false;
    }

    protected virtual void SelectAttack()
    {
        stateMachine.CurrentAttackInfo = attackData.BaseAttackInfo;
    }


    protected virtual bool IsInSight()
    {
        if (!stateMachine.Target || stateMachine.Target.IsDead)
            return false;

        Vector3 directionToTarget = stateMachine.Target.transform.position - stateMachine.Enemy.transform.position;
        directionToTarget.y = 0;
        directionToTarget.Normalize();

        Vector3 forward = stateMachine.Enemy.transform.forward;
        forward.y = 0;
        forward.Normalize();

        float angleToTaget = Vector3.Angle(forward, directionToTarget);

        float fildOfView = 60f;
        if (angleToTaget <= fildOfView / 2f)
        {
            return true;
        }

        return false;
    }
}
