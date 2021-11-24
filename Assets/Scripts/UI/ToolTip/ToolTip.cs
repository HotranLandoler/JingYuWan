using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Pool;

namespace JYW.UI.ToolTip
{
    public class ToolTip : MonoBehaviour
    {
        public static ToolTip Instance { get; private set; }

        [SerializeField]
        private Text textPrefab;

        private ObjectPool<Text> textPool;

        private RectTransform rectTransform;

        private CanvasGroup canvasGroup;

        private List<Text> toolTips = new List<Text>();

        private ICollection<TipInfo> lastTips;

        private Tween hideTween;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            textPool = new ObjectPool<Text>(
                createFunc: () => Instantiate(textPrefab, transform),
                actionOnGet: text => text.gameObject.SetActive(true),
                actionOnRelease: text => text.gameObject.SetActive(false),
                actionOnDestroy: text => Destroy(text.gameObject),
                defaultCapacity: 3,
                maxSize: 10);
            Instance = this;
            //gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            textPool.Clear();
        }

        public void ShowToolTip(ICollection<TipInfo> tips)
        {
            if (tips == null) return;
            if (tips.Count == 0) return;
            if (lastTips != tips)
            {
                foreach (var tip in toolTips)
                {
                    textPool.Release(tip);
                }
                toolTips.Clear();
                foreach (TipInfo t in tips)
                {
                    var text = textPool.Get();
                    text.text = $"<b>{t.TipName}</b>: {t.Desc}";
                    toolTips.Add(text);
                }
            }

            Vector2 pos = Input.mousePosition;
            float pivotX = pos.x / Screen.width;
            float pivotY = pos.y / Screen.height;
            rectTransform.pivot = new Vector2(pivotX, pivotY); 
            transform.position = Input.mousePosition;

            hideTween?.Kill();
            if(canvasGroup.alpha != 1f) canvasGroup.DOFade(1f, 0.5f);
            hideTween = canvasGroup.DOFade(0f, 0.5f).SetDelay(1.5f);
            lastTips = tips;
            //gameObject.SetActive(true);
        }
    }
}