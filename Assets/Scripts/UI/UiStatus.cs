using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace JYW.UI
{
    public class UiStatus : MonoBehaviour
    {
        [SerializeField]
        private Character binding;

        [SerializeField]
        private Image IconImage;

        [Header("Health")]
        [SerializeField]
        private Image healthImage;

        [SerializeField]
        private TextMeshProUGUI healthText;

        [Header("Energy")]
        [SerializeField]
        private Image energyImage;

        [SerializeField]
        private TextMeshProUGUI energyText;

        private readonly string format = "0.#";

        private void OnEnable()
        {
            binding.HealthChanged += UpdateHealth;
            binding.EnergyChanged += UpdateEnergy;
            binding.Initialized += SetIcon;
        }

        private void OnDisable()
        {
            binding.HealthChanged -= UpdateHealth;
            binding.EnergyChanged -= UpdateEnergy;
            binding.Initialized -= SetIcon;
        }

        private void SetIcon()
        {
            IconImage.sprite = binding.Sect.Icon;
        }

        private void UpdateHealth()
        {
            //Debug.Log(binding.CurrentHealth);
            healthImage.fillAmount = binding.CurrentHealth / binding.MaxHealth;
            healthText.text = binding.CurrentHealth.ToString(format);
        }

        private void UpdateEnergy()
        {
            energyImage.fillAmount = binding.CurrentEnergy / binding.MaxEnergy;
            energyText.text = binding.CurrentEnergy.ToString(format);
        }
    }
}