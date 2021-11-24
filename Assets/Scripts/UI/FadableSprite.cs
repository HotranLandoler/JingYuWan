using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace JYW.UI
{
    public class FadableSprite : MonoBehaviour
    {
        [SerializeField]
        private float fadeTime = 0.8f;

        private SpriteRenderer spriteRenderer;

        private Tween fadeTween;

        public bool Display { get; private set; } = false;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Show()
        {
            fadeTween?.Kill();
            fadeTween = spriteRenderer.DOFade(1f, fadeTime);
            Display = true;
        }

        public void Hide()
        {
            fadeTween?.Kill();
            fadeTween = spriteRenderer.DOFade(0f, fadeTime);
            Display = false;
        }
    }
}