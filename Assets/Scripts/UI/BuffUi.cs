using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Pool;
using JYW.Buffs;
using JYW.UI.ToolTip;
using TMPro;

namespace JYW.UI
{
    public class BuffUi : MonoBehaviour, IToolTipable
    {
        private static readonly float maskEffectTime = 0.5f;

        private Tweener maskEffectTween;

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private TextMeshProUGUI levelText;

        [SerializeField]
        private Image maskImage;

        private Buff bindBuff;

        public ICollection<TipInfo> Tips => bindBuff.Data.toolTips;

        public void Set(Buff buff)
        {
            bindBuff = buff;
            iconImage.sprite = buff.Data.Icon;
            nameText.text = buff.Data.Name;
            UpdateLevel(buff);
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
            UpdateLevel(bindBuff);
            var fill = 1 - ((float)bindBuff.DurationTimer) / bindBuff.Duration;
            if (!Mathf.Approximately(maskImage.fillAmount, fill))
                maskImage.DOFillAmount(fill, maskEffectTime);
        }

        private void UpdateLevel(Buff buff)
        {
            if (buff.Level == 1)
                levelText.enabled = false;
            else
            {
                levelText.text = bindBuff.Level.ToString();
                levelText.enabled = true;
            }
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