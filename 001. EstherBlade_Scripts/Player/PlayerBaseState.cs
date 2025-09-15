using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly GroundData groundData;
    protected readonly AttackData attackData;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.groundData = stateMachine.Player.Data.GroundData;
        this.attackData = stateMachine.Player.Data.AttackData;
    }

    public virtual void Enter()
    {
        AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    public virtual void Update()
    {
        Move();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    private void ReadMovementInput()
    {
        stateMachine.MovementInput = stateMachine.Player.Input.PlayerActions.Move.ReadValue<Vector2>();
    }

    protected virtual void AddInputActionsCallbacks()
    {
        PlayerInput input = stateMachine.Player.Input;

        input.PlayerActions.Move.canceled += OnMoveCanceled;
        input.PlayerActions.Run.started += OnRunStarted;
        input.PlayerActions.Jump.started += OnJumpStarted;

        input.PlayerActions.Attack.started += OnAttackStarted;
        input.PlayerActions.Skill.started += OnSkillStarted;

        input.PlayerActions.Attack.canceled += OnAttackCanceled;
        input.PlayerActions.Skill.started += OnSkillCanceled;
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        PlayerInput input = stateMachine.Player.Input;

        input.PlayerActions.Move.canceled -= OnMoveCanceled;
        input.PlayerActions.Run.started -= OnRunStarted;
        input.PlayerActions.Jump.started -= OnJumpStarted;

        input.PlayerActions.Attack.started -= OnAttackStarted;
        input.PlayerActions.Skill.started -= OnSkillStarted;

        input.PlayerActions.Attack.canceled -= OnAttackCanceled;
        input.PlayerActions.Skill.canceled -= OnSkillCanceled;
    }

    protected virtual void OnMoveCanceled(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnRunStarted(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnAttackStarted(InputAction.CallbackContext context)
    {
        stateMachine.IsAttacking = true;
    }
    protected virtual void OnSkillStarted(InputAction.CallbackContext context)
    {
        stateMachine.IsAttacking = true;
    }

    protected virtual void OnAttackCanceled(InputAction.CallbackContext context)
    {
        stateMachine.IsAttacking = false;
    }
    protected virtual void OnSkillCanceled(InputAction.CallbackContext context)
    {
        stateMachine.IsAttacking = false;
    }

    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();
        Rotate(movementDirection);
        Move(movementDirection);
    }

    private Vector3 GetMovementDirection()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return (forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x).normalized;
    }

    private void Rotate(Vector3 movementDirection)
    {
        if (movementDirection != Vector3.zero)
        {
            Transform playerTransform = stateMachine.Player.transform;
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }

    private void Move(Vector3 movementDirection)
    {
        float movementSpeed = GetMovementSpeed();
        stateMachine.Player.Controller.Move(
            (
                (movementDirection * movementSpeed)
                + stateMachine.Player.ForceHandler.Movement
            )
            * Time.deltaTime
        );
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
            var currentInfo = animator.GetCurrentAnimatorStateInfo(0);
            return currentInfo.IsTag(tag) ? currentInfo.normalizedTime : -1f;
        }
    }

    public virtual void PhysicsUpdate()
    {

    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, false);
    }

    protected virtual void ChangeAttackState(AttackInfoData attackInfoData)
    {
        stateMachine.CurrentAttackInfo = attackInfoData;
        switch (attackInfoData.AttackType)
        {
            case AttackType.Base:
                stateMachine.TransitState(stateMachine.BaseAttackState); break;

            case AttackType.Combo:
                stateMachine.TransitState(stateMachine.ComboAttackState); break;
        }
    }

    protected void ForceMove()
    {
        stateMachine.Player.Controller.Move(
            (
                stateMachine.Player.ForceHandler.Movement
            )
            * Time.deltaTime
        );
    }
}