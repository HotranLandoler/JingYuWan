using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JYW.UI
{
    public class UiStatus : MonoBehaviour
    {
        [SerializeField]
        private Character binding;

        [Header("Health")]
        [SerializeField]
        private Image healthImage;

        [SerializeField]
        private Text healthText;

        [Header("Energy")]
        [SerializeField]
        private Image energyImage;

        [SerializeField]
        private Text energyText;

        private void OnEnable()
        {
            binding.HealthChanged += UpdateHealth;
            binding.EnergyChanged += UpdateEnergy;
        }

        private void OnDisable()
        {
            binding.HealthChanged -= UpdateHealth;
            binding.EnergyChanged -= UpdateEnergy;
        }

        private void UpdateHealth()
        {
            Debug.Log(binding.CurrentHealth);
            healthImage.fillAmount = binding.CurrentHealth / binding.MaxHealth;
            healthText.text = binding.CurrentHealth.ToString();
        }

        private void UpdateEnergy()
        {
            energyImage.fillAmount = binding.CurrentEnergy / binding.MaxEnergy;
            energyText.text = binding.CurrentEnergy.ToString();
        }
    }
}