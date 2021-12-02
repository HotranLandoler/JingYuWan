using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Pool;
using TMPro;

namespace JYW.UI
{
    public class CharacterUi : MonoBehaviour
    {
        private Character character;

        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private Ui rounderToken;

        [Header("DamageText")]
        [SerializeField]
        private TextMeshProUGUI damageTextPrefab;

        [SerializeField]
        private Color criticColor = Color.white;

        [SerializeField]
        private RectTransform damageTextStartPos;

        [Header("Chant")]
        [SerializeField]
        private ChantUi chantUi;

        private ObjectPool<TextMeshProUGUI> damageTextPool;

        private void Awake()
        {
            character = GetComponent<Character>();
            damageTextPool = new ObjectPool<TextMeshProUGUI>(
                createFunc: () => Instantiate(damageTextPrefab, canvas.transform),
                actionOnGet: text => text.gameObject.SetActive(true),
                actionOnRelease: text => text.gameObject.SetActive(false),
                actionOnDestroy: text => { if (text) Destroy(text.gameObject); },
                defaultCapacity: 5,
                maxSize: 10);
        }

        private void OnEnable()
        {
            character.DamageTaken += ShowDamageText;
            character.ChantStarted += ShowChant;
            character.ChantCompleted += HideChant;
            character.ChantStopped += StopChant;
            character.RoundStarted += ShowToken;
            character.RoundEnded += HideToken;
            character.Dodged += ShowDodge;
            //character.ChantProcessed += UpdateChant;
        }

        private void OnDisable()
        {
            character.DamageTaken -= ShowDamageText;
            character.ChantStarted -= ShowChant;
            character.ChantCompleted -= HideChant;
            character.ChantStopped -= StopChant;
            character.RoundStarted -= ShowToken;
            character.RoundEnded -= HideToken;
            character.Dodged -= ShowDodge;
            //character.ChantProcessed -= UpdateChant;
        }

        private void OnDestroy()
        {
            damageTextPool.Clear();
        }

        private void ShowDamageText(DamageInfo damage)
        {
            var color = damage.IsCritical ? criticColor : Color.white;
            ShowText(damage.ToString(), color);
        }

        private void ShowText(string text, Color color)
        {
            //var txt = Instantiate(damageTextPrefab, canvas.transform);
            var txt = damageTextPool.Get();
            txt.rectTransform.SetParent(canvas.transform);
            txt.rectTransform.anchoredPosition = damageTextStartPos.anchoredPosition;
            txt.text = text;
            color.a = 0f;
            txt.color = color;
            txt.rectTransform.localScale = 0.5f * Vector3.one;

            Sequence seq = DOTween.Sequence();
            txt.rectTransform.DOScale(1f, 0.2f);
            txt.DOFade(1f, 0.2f);
            seq.Append(txt.rectTransform.DOAnchorPosY(damageTextStartPos.anchoredPosition.y + 100, 0.2f)).SetEase(Ease.OutCirc);
            seq.Append(txt.rectTransform.DOAnchorPosY(damageTextStartPos.anchoredPosition.y + 125, 0.4f));
            seq.Append(txt.rectTransform.DOAnchorPosY(damageTextStartPos.anchoredPosition.y + 150, 0.3f)).SetEase(Ease.InQuad);
            seq.Insert(0.6f, txt.DOFade(0f, 0.3f).OnComplete(() => damageTextPool.Release(txt)));           
        }

        private void ShowChant()
        {
            chantUi.Set(character.CurrentChant);
            chantUi.Toggle(true);
        }

        private void HideChant(Character character) =>
            chantUi.Toggle(false);

        private void StopChant() =>
            chantUi.Drop();
       

        private void ShowDodge() => ShowText(Game.Dodge, Color.white);

        private void ShowToken() => rounderToken.FadeIn();

        private void HideToken() => rounderToken.FadeOut();

        //private void UpdateChant()
        //{
        //    chantUi.
        //}
    }
}