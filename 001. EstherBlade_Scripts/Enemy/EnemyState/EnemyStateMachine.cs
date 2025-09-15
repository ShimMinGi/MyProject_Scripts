using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    public Enemy Enemy { get; }


    public EnemyIdleState IdleState { get; }
    public EnemyChaseState ChaseState { get; }
    public EnemyBaseAttackState BaseAttackState { get; }
    public EnemyFallState FallState { get; }




    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;

    public HealthSystem Target { get; set; }
    public AttackInfoData CurrentAttackInfo { get; set; }

    public EnemyStateMachine(Enemy enemy)
    {
        this.Enemy = enemy;

        IdleState = new EnemyIdleState(this);
        ChaseState = new EnemyChaseState(this);
        BaseAttackState = new EnemyBaseAttackState(this);
        FallState = new EnemyFallState(this);

        MovementSpeed = enemy.Data.GroundData.BaseSpeed;
        RotationDamping = enemy.Data.GroundData.BaseRotationDamping;
    }

    public void TakeDamage()
    {
        Enemy.Animator.SetTrigger(Enemy.AnimationData.HitParameterHash);
    }

    public void Die()
    {
        Enemy.Animator.SetTrigger(Enemy.AnimationData.DieParameterHash);
        currentState = null;
    }

}
