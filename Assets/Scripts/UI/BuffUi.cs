using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Pool;
using JYW.Buffs;

namespace JYW.UI
{
    public class BuffUi : MonoBehaviour
    {        
        private static readonly float maskEffectTime = 0.5f;

        private Tweener maskEffectTween;

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Text levelText;

        [SerializeField]
        private Image maskImage;

        private Buff bindBuff;

        public void Set(Buff buff)
        {
            bindBuff = buff;
            iconImage.sprite = buff.Data.Icon;
            nameText.text = buff.Data.Name;
            levelText.text = buff.Level.ToString();
            maskImage.fillAmount = 0f;
            buff.ValueChanged += UpdateUI;
        }

        private void OnDisable()
        {
            maskEffectTween?.Kill();
            if (bindBuff != null)
                bindBuff.ValueChanged -= UpdateUI;
        }

        private void UpdateUI()
        {
            levelText.text = bindBuff.Level.ToString();
            var fill = 1 - ((float)bindBuff.DurationTimer) / bindBuff.Duration;
            if (!Mathf.Approximately(maskImage.fillAmount, fill))
                maskImage.DOFillAmount(fill, maskEffectTime);
        }

        public void Remove(ObjectPool<BuffUi> objectPool = null)
        {
            StartCoroutine(DoRemove());
        }

        private IEnumerator DoRemove()
        {
            if (maskEffectTween != null)
                yield return maskEffectTween.WaitForCompletion();
            yield return iconImage.DOFade(0f, maskEffectTime).WaitForCompletion();
            Destroy(gameObject);
        }
    }
}