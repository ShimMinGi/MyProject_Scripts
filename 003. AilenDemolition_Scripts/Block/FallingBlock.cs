using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    private Rigidbody2D rigid;
    private BoxCollider2D boxCollider;

    public FallingBlockSpawn fallingBlockSpawn;

    public int maxHitCount; // 블록의 최대 체력
    public int hitCount; // 현재 체력

    public float boundForce = 5f; // 블록이 튕기는 힘
    public float fallingSpeed = -5f; // 블록의 하강 속도

    public int baseHealth = 100; // 기본 체력

    public int blockLayerMask; // 비트 연산을 위한 레이어 마스크

    private Collider2D playerCollider; // Player Collider
    private Collider2D floorCollider; // Floor Collider

    Player player;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Init(FallingBlockSpawn fallingBlockSpawn)
    {
        this.fallingBlockSpawn = fallingBlockSpawn;

        player = GameManager.Instance.Player;
        floorCollider = GameObject.FindGameObjectWithTag("Floor").GetComponent<Collider2D>(); // Floor Collider Find
    }

    private void Start()
    {
        blockLayerMask = 1 << 7;
        InitializeStats(GameManager.Instance.currentStage);
    }

    public void InitializeStats(int stage)
    {
        // 스테이지 난이도 설정
        switch (stage)
        {
            case 1:
                maxHitCount = baseHealth;
                fallingSpeed = -5f;
                boundForce = 15f;
                break;
            case 2:
                maxHitCount = baseHealth + 10;
                fallingSpeed = -6f;
                boundForce = 13f;
                break;
            case 3:
                maxHitCount = baseHealth + 20;
                fallingSpeed = -7f;
                boundForce = 11f;
                break;
            case 4:
                maxHitCount = baseHealth + 30;
                fallingSpeed = -8f;
                boundForce = 10f;
                break;
            case 5:
                maxHitCount = baseHealth + 40;
                fallingSpeed = -9f;
                boundForce = 8f;
                break;
            case 6:
                maxHitCount = baseHealth + 50;
                fallingSpeed = -10f;
                boundForce = 7f;
                break;
            case 7:
                maxHitCount = baseHealth + 60;
                fallingSpeed = -11f;
                boundForce = 6f;
                break;
            case 8:
                maxHitCount = baseHealth + 70;
                fallingSpeed = -12f;
                boundForce = 5f;
                break;
            case 9:
                maxHitCount = baseHealth + 80;
                fallingSpeed = -13f;
                boundForce = 4f;
                break;
            case 10:
                maxHitCount = baseHealth + 90;
                fallingSpeed = -14f;
                boundForce = 3f;
                break;
        }

        if (rigid != null)
        {
            rigid.velocity = new Vector2(0, fallingSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            if (IsPlayerOnFloor(player))
            {
                if (player.isGuarding)
                {
                    OnPlayerGuard(player); // 플레이어가 가드 중이면 블록 튕김
                }
                else
                {
                    boxCollider.isTrigger = true;
                    player.OnContactBlock(true);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        if (player)
        {
            boxCollider.isTrigger = false;
            player.OnContactBlock(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Floor"))
        {
            BounceOff(); // Floor에 닿으면 블록이 위로 튕김
            DamagePlayer(); // Floor에 닿을 때 플레이어 체력 감소
        }
    }

    public void OnPlayerGuard(Player player)
    {
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * (boundForce * 2), ForceMode2D.Impulse);
    }

    private void BounceOff()
    {
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * (boundForce * 2), ForceMode2D.Impulse);
    }

    private void DamagePlayer()
    {
        if (player != null)
        {
            player.Damage(1);  // 데미지를 입힘
        }
    }

    private bool IsPlayerOnFloor(Player player)
    {
        Collider2D playerCollider = player.GetComponent<Collider2D>(); // Player 객체의 Collider 가져오기
        if (playerCollider == null || floorCollider == null)
        {
            return false;
        }

        return playerCollider.IsTouching(floorCollider);
    }

    public void TakeDamage()
    {
        hitCount++;

        if (hitCount >= maxHitCount)
        {
            fallingBlockSpawn.RemoveBlock(this);
            OnDestroyBlock();
            Destroy(gameObject);
        }
    }

    private void OnDestroyBlock()
    {
        if (fallingBlockSpawn.GetRemainingBlocksCount() == 0)
        {
            GameManager.Instance.OnStageComplete();
        }
    }
}
