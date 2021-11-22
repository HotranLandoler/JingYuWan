using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

namespace JYW.UI
{
    public class UiManager : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField]
        private UiButton menuButton;
        public UiButton MenuButton => menuButton;

        [SerializeField]
        private UiButton playCardButton;
        public UiButton PlayCardButton => playCardButton;

        [SerializeField]
        private UiButton nextRoundButton;
        public UiButton NextRoundButton => nextRoundButton;

        public UnityEvent PlayButtonClicked;
        public UnityEvent NextButtonClicked;

        [Header("Effects")]
        [SerializeField]
        private Text cardNameText;

        [SerializeField]
        private float cardTextScale = 2f;

        [SerializeField]
        private float cardTextScaleTime = 0.4f;

        [SerializeField]
        private float cardTextShowTime = 0.5f;

        [Header("Tips")]
        [SerializeField]
        private Text warningText;

        private void Start()
        {
            cardNameText.gameObject.SetActive(false);
            playCardButton.button.onClick.AddListener(() => PlayButtonClicked?.Invoke());
            nextRoundButton.button.onClick.AddListener(() => NextButtonClicked?.Invoke());
        }

        public IEnumerator ShowCardText(CardData card)
        {
            cardNameText.text = card.Title;
            cardNameText.color = new Color(1f, 1f, 1f, 0f);
            cardNameText.transform.localScale = Vector3.one * cardTextScale;
            cardNameText.gameObject.SetActive(true);
            cardNameText.DOFade(1f, cardTextScaleTime);
            yield return cardNameText.transform.DOScale(Vector3.one, cardTextScaleTime);
            yield return new WaitForSeconds(cardTextShowTime);
            cardNameText.DOFade(0f, cardTextScaleTime);
            cardNameText.transform.DOScale(cardTextScale, cardTextScaleTime);
        }

        public void ShowWarning(string warning)
        {
            warningText.text = warning;
        }

    }
}