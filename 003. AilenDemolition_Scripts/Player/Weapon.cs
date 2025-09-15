using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    BoxCollider2D boxCollider;
    Player player;
    Animator animator;

    public Player Player { get { return player; } }

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    public void Initialize(Player _player)
    {
        player = _player;
        EndAttack(); // �ʱ⿡�� ������ ����� ����
    }

    private void Update()
    {
        if (Input.GetButtonDown("Attack"))
        {
            StartAttack();
        }
        if (Input.GetButtonUp("Attack"))
        {
            EndAttack();
        }
    }

    public void StartAttack()
    {
        boxCollider.enabled = true;
        animator.SetTrigger("WeaponAttack");
    }

    public void EndAttack()
    {
        boxCollider.enabled = false;
        animator.ResetTrigger("WeaponAttack");
    }

    public void StartGuard()
    {
        animator.SetTrigger("WeaponGuard");
    }

    public void EndGuard()
    {
        animator.ResetTrigger("WeaponGuard");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (boxCollider.enabled && other.CompareTag("Block"))
        {
            FallingBlock block = other.GetComponent<FallingBlock>();
            if (block != null)
            {
                // ����� ü���� ���ҽ�ŵ�ϴ�.
                block.TakeDamage();
            }
        }
    }
}
