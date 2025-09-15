using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public enum PlayerState
{
    Idle = 0,
    Attack = 1,
    Guard = 2,
    Jump = 3
}

public class Player : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Rigidbody2D rigid;
    private Animator anim;
    public Weapon weapon;
    public MainUiManager uimanager;

    public int PlayerHealth = 3;
    public float jumpPower;
    public bool isGuarding;

    private PlayerState state = PlayerState.Idle;
    private bool isContactBlock = false;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        weapon = GetComponentInChildren<Weapon>();
    }

    private void Start()
    {
        if (weapon != null)
        {
            weapon.Initialize(this);
        }
        else
        {
            Debug.LogError("Weapon is missing");
        }

        ResetPlayer();
    }

    private void Update()
    {
        if (isContactBlock)
            return;

        AnimInput();
        UpdateState();
    }

    public void Damage(int damage)
    {
        PlayerHealth -= damage;
        if (PlayerHealth <= 0)
        {
            PlayerHealth = 0;
            Die();
        }

        uimanager.UpdatePlayerHealth(PlayerHealth);
    }

    private void Die()
    {
        Debug.Log("Player Dead");
        GameManager.Instance.PlayerDead();
        Destroy(gameObject);
    }

    public void ResetPlayer()
    {
        PlayerHealth = 3;
        transform.position = new Vector2(0, 1); // 초기 위치로 설정
        ChangeState(PlayerState.Idle);

        uimanager.UpdatePlayerHealth(PlayerHealth);
    }

    private void ChangeState(PlayerState newState)
    {
        switch (state)
        {
            case PlayerState.Guard:
                isGuarding = false;
                break;
        }

        switch (newState)
        {
            case PlayerState.Guard:
                isGuarding = true;
                break;
            case PlayerState.Jump:
                break;
            case PlayerState.Idle:
            case PlayerState.Attack:
                isGuarding = false;
                break;
        }

        state = newState;
        anim.SetInteger("state", (int)state);
    }

    private void AnimInput()
    {
        if (Input.GetButtonDown("Attack") && IsAttackable())
        {
            ChangeState(PlayerState.Attack);
        }
        else if (Input.GetButtonUp("Attack") && state == PlayerState.Attack)
        {
            ChangeState(PlayerState.Idle);
        }

        if (Input.GetButtonDown("Guard") && (state == PlayerState.Idle || state == PlayerState.Jump))
        {
            ChangeState(PlayerState.Guard);
        }
        else if (Input.GetButtonUp("Guard") && state == PlayerState.Guard)
        {
            ChangeState(PlayerState.Idle);
        }

        if (Input.GetButtonDown("Jump") && (state == PlayerState.Idle || state == PlayerState.Guard))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            ChangeState(PlayerState.Jump);
        }
    }

    private void UpdateState()
    {
        if (state == PlayerState.Jump && rigid.velocity.y == 0)
        {
            ChangeState(isGuarding ? PlayerState.Guard : PlayerState.Idle);
        }

        // 입력에 대한 처리 , (현) AnimInput
        switch (state)
        {
            case PlayerState.Guard:
            {
                if (Input.GetButtonDown("Guard"))
                {
                    
                }
            }
                break;
        }
    }

    bool IsAttackable()
    {
        return (state == PlayerState.Idle || state == PlayerState.Jump);
    }

    public void OnAttackHit()
    {
        Debug.Log("Attack");
        if (weapon != null)
        {
            weapon.StartAttack();
        }
    }
    public void OnGuarding()
    {
        Debug.Log("Guard");
        if (weapon != null)
        {
            weapon.StartGuard();
        }
    }

    //public void OnJumpEnd()
    //{
    //    Debug.Log("Jump End");
    //    ChangeState(PlayerState.Idle);
    //}

    public void OnContactBlock(bool isContacting)
    {
        isContactBlock = isContacting;
    }

    //public void OnAttackAnimationEnd()
    //{
    //    ChangeState(PlayerState.Idle);
    //}

    //public void OnJumpAnimationEnd()
    //{
    //    ChangeState(PlayerState.Idle);
    //}

    //// 애니메이션을 시작하는 메서드들
    //public void StartAttack()
    //{
    //    ChangeState(PlayerState.Attack);

    //public void StartJump()
    //{
    //    ChangeState(PlayerState.Jump);
    //    rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    //}

    //private void UpdateState()
    //{
    //    if (state == PlayerState.Jump && rigid.velocity.y == 0)
    //    {
    //        ChangeState(isGuarding ? PlayerState.Guard : PlayerState.Idle);
    //    }
    //}

    //public void OnContactBlock(bool isContacting)
    //{
    //    isContactBlock = isContacting;
    //}
}
