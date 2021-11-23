using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
        private Text damageTextPrefab;

        [SerializeField]
        private RectTransform damageTextStartPos;

        [Header("Chant")]
        [SerializeField]
        private ChantUi chantUi;


        private void Awake()
        {
            character = GetComponent<Character>();            
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

        

        private void ShowDamageText(DamageInfo damage)
        {
            //StringBuilder sb = new StringBuilder();
            //if (damage.IsCritical) sb.Append("»áÐÄ ");
            //sb.Append(damage.Damage.ToString());
            ShowText(damage.ToString());
        }

        private void ShowText(string text)
        {
            var txt = Instantiate(damageTextPrefab, canvas.transform);
            txt.rectTransform.anchoredPosition = damageTextStartPos.anchoredPosition;
            txt.text = text;
            txt.rectTransform.DOAnchorPosY(damageTextStartPos.anchoredPosition.y + 100, 1f);
            txt.DOFade(0f, 1f);
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
       

        private void ShowDodge() => ShowText(Game.Dodge);

        private void ShowToken() => rounderToken.FadeIn();

        private void HideToken() => rounderToken.FadeOut();

        //private void UpdateChant()
        //{
        //    chantUi.
        //}
    }
}