using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public event Action<HealthSystem> OnTargetDetected;

    private Collider weaponCollider;
    private bool isAttackActive = false;
    private List<HealthSystem> detectedTargets = new List<HealthSystem>();

    private LayerMask layerMask;

    public ItemInstance ItemInstance { get; private set; }

    public void Initialize(LayerMask layerMask, ItemInstance itemInstance = null)
    {
        this.layerMask = layerMask;
        this.ItemInstance = itemInstance;

        weaponCollider = GetComponent<Collider>();
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
        else
        {
            Debug.LogWarning("Weapon Collider?? ???????? ??????.");
        }
    }

    public void ActivateCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            isAttackActive = true;
            detectedTargets.Clear();
        }
    }

    public void DeactiveCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
            isAttackActive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAttackActive) return;

        if ((layerMask.value & (1 << other.gameObject.layer)) == 0) return;

        HealthSystem targetHealth = other.GetComponent<HealthSystem>();
        if (targetHealth != null &&
            !detectedTargets.Contains(targetHealth)
            )
        {
            detectedTargets.Add(targetHealth);
            OnTargetDetected?.Invoke(targetHealth);
        }
    }
}
