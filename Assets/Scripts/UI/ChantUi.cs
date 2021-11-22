using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JYW.UI
{
    public class ChantUi : MonoBehaviour
    {
        [SerializeField]
        private float effectTime = 0.5f;

        [SerializeField]
        private Image barImage;

        [SerializeField]
        private Text chantText;

        [SerializeField]
        private Color stopColor;

        private Color defaultColor;

        private Animator animator;

        private Chant binding;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            defaultColor = barImage.color;
        }

        public void Toggle(bool show)
        {
            animator.SetBool("Show", show);
        }

        public void Set(Chant chant)
        {
            binding = chant;
            chantText.text = chant.TargetCard.Title;
            barImage.fillAmount = ((float)chant.Process) / chant.Duration;
            barImage.color = defaultColor;
            binding.ValueChanged += UpdateUI;
        }

        /// <summary>
        /// ¥Ú∂œ∂¡Ãı
        /// </summary>
        public void Drop()
        {
            barImage.color = stopColor;
            Toggle(false);
        }

        private void OnDisable()
        {
            if (binding != null)
                binding.ValueChanged -= UpdateUI;
        }

        private void UpdateUI()
        {
            //Debug.Log("!");
            //chantText.text = chant.TargetCard.Title;
            var fill = ((float)binding.Process) / binding.Duration;
            if (!Mathf.Approximately(barImage.fillAmount, fill))
                barImage.DOFillAmount(fill, effectTime);
        }
    }
}