using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponentInParent<HealthSystem>();

        if (healthSystem == null)
        {
            Debug.LogError($"HealthUI ({gameObject.name})에서 HealthSystem을 찾지 못했습니다!");
        }
        else
        {
            Debug.Log("HealthUI 자동 할당 성공: " + healthSystem.gameObject.name);
        }
    }

    private void Start()
    {
        if (healthSystem != null)
        {
            healthSlider.maxValue = healthSystem.MaxHealth;
            healthSlider.value = healthSystem.CurrentHealth;
            UpdateUI();

            healthSystem.OnDamage += OnHealthChanged;
            healthSystem.OnDie += OnHealthChanged;
        }
    }

    private void OnHealthChanged()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (healthSystem != null)
        {
            int current = healthSystem.CurrentHealth;
            int max = healthSystem.MaxHealth;
            healthSlider.value = current;
            healthText.text = $"{current} / {max}";
        }
    }

    private void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180f, 0);
        }
    }

    private void OnDestroy()
    {
        if (healthSystem != null)
        {
            healthSystem.OnDamage -= OnHealthChanged;
            healthSystem.OnDie -= OnHealthChanged;
        }
    }
}