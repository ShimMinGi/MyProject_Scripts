using System;
using UnityEngine;

public enum Faction
{
    Player,
    Enemy,
    Nature,
    None
}

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private Faction faction;
    public Faction Faction => faction;

    [SerializeField] private int maxHealth = 100;
    private int health;

    public event Action OnDamage;
    public event Action OnDie;

    public int CurrentHealth => health;
    public int MaxHealth => maxHealth;

    public bool IsDead => gameObject.activeSelf ? (health == 0) : true;

    private void Awake()
    {
        health = maxHealth;
        Debug.Log($"{name} HP : {health} / {maxHealth}");
    }

    public void TakeDamage(int damage)
    {
        if (health == 0) return;

        health = (int)MathF.Max(health - damage, 0);

        if (health == 0)
            OnDie?.Invoke();
        else
            OnDamage?.Invoke();

        Debug.Log($"{name} HP : {health} / {maxHealth}");
    }

    public void Heal(int healAmount)
    {
        if (health == 0)
        {
            return;
        }

        int prevHealth = health;
        health = Mathf.Min(health + healAmount, maxHealth);
        OnDamage?.Invoke();
    }
}