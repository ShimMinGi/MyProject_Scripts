using System;
using UnityEngine;

[Serializable]
public class PlayerAnimationData
{
    [SerializeField] string groundedParameterName = "@Grounded";
    [SerializeField] string idleParameterName = "isIdling";
    [SerializeField] string walkParameterName = "isWalking";
    [SerializeField] string runParameterName = "isRunning";

    [SerializeField] string airParameterName = "@Airborne";
    [SerializeField] string jumpParameterName = "isJumping";
    [SerializeField] string fallParameterName = "isFalling";

    [SerializeField] string attackParameterName = "@Attack";
    [SerializeField] string baseAttackParameterName = "BaseAttack";

    [SerializeField] string hitParameterName = "isHit";
    [SerializeField] string dieParameterName = "isDie";

    public int GroundedParameterHash { get; private set; }
    public int IdleParameterHash { get; private set; }
    public int WalkParameterHash { get; private set; }
    public int RunParameterHash { get; private set; }


    public int AirParameterHash { get; private set; }
    public int JumpParameterHash { get; private set; }
    public int FallParameterHash { get; private set; }


    public int AttackParameterHash { get; private set; }
    public int BaseAttackParameterHash { get; private set; }


    public int HitParameterHash { get; private set; }
    public int DieParameterHash { get; private set; }

    public void Initialize()
    {
        GroundedParameterHash = Animator.StringToHash(groundedParameterName);
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        WalkParameterHash = Animator.StringToHash(walkParameterName);
        RunParameterHash = Animator.StringToHash(runParameterName);

        FallParameterHash = Animator.StringToHash(fallParameterName);
        AirParameterHash = Animator.StringToHash(airParameterName);
        JumpParameterHash = Animator.StringToHash(jumpParameterName);

        AttackParameterHash = Animator.StringToHash(attackParameterName);
        BaseAttackParameterHash = Animator.StringToHash(baseAttackParameterName);

        HitParameterHash = Animator.StringToHash(hitParameterName);
        DieParameterHash = Animator.StringToHash(dieParameterName);

        Debug.Log("JumpParameterHash: " + JumpParameterHash);
        Debug.Log("FallParameterHash: " + FallParameterHash);
    }
}