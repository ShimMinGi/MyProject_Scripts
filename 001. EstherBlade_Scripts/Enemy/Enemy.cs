using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [field: Header("References")]
    [field: SerializeField] public EnemySO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    public ForceHandler ForceHandler { get; private set; }
    private EnemyStateMachine stateMachine;

    [field: SerializeField] public Weapon CurrentWeapon { get; private set; }

    public HealthSystem HealthSystem { get; private set; }
    [field: SerializeField] public LayerMask entityLayerMask { get; private set; }

    private void Awake()
    {
        Data.AttackData.Initialize();
        AnimationData.Initialize();

        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();
        ForceHandler = GetComponent<ForceHandler>();

        stateMachine = new EnemyStateMachine(this);

        HealthSystem = GetComponent<HealthSystem>();

        if (CurrentWeapon == null)
        {
            CurrentWeapon = GetComponentInChildren<Weapon>();
            CurrentWeapon.Initialize(entityLayerMask);
        }
    }

    private void Start()
    {
        HealthSystem.OnDamage += stateMachine.TakeDamage;
        HealthSystem.OnDie += stateMachine.Die;

        stateMachine.TransitState(stateMachine.IdleState);
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }
}