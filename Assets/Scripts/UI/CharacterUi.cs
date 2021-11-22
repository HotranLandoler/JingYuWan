using System.Collections;
using System.Collections.Generic;
using System.Text;
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

        [Header("DamageText")]
        [SerializeField]
        private Text damageTextPrefab;

        [SerializeField]
        private RectTransform damageTextStartPos;

        [Header("Chant")]
        [SerializeField]
        private ChantUi chantUi;

        [Header("Move")]
        [SerializeField]
        private UiButton leftButton;

        [SerializeField]
        private UiButton rightButton;


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
            character.RoundStarted += ShowButtons;
            //character.ChantProcessed += UpdateChant;
        }

        private void OnDisable()
        {
            character.DamageTaken -= ShowDamageText;
            character.ChantStarted -= ShowChant;
            character.ChantCompleted -= HideChant;
            character.ChantStopped -= StopChant;
            character.RoundStarted -= ShowButtons;
            //character.ChantProcessed -= UpdateChant;
        }

        private void Start()
        {
            leftButton.button.onClick.AddListener(LeftButtonClicked);
            rightButton.button.onClick.AddListener(RightButtonClicked);
        }

        private void ShowDamageText(DamageInfo damage)
        {
            StringBuilder sb = new StringBuilder();
            if (damage.IsCritical) sb.Append("»áÐÄ ");
            sb.Append(damage.Damage.ToString("0.0"));
            var text = Instantiate(damageTextPrefab, canvas.transform);
            text.rectTransform.anchoredPosition = damageTextStartPos.anchoredPosition;
            text.text = sb.ToString();
            text.rectTransform.DOAnchorPosY(damageTextStartPos.anchoredPosition.y + 100, 1f);
            text.DOFade(0f, 1f);
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

        private void LeftButtonClicked()
        {
            character.MoveRequest((int)Direction.L * Character.NormalMoveDist, MoveType.Normal);
            HideButtons();
        }


        private void RightButtonClicked()
        {
            character.MoveRequest((int)Direction.R * Character.NormalMoveDist, MoveType.Normal);
            HideButtons();
        }

        private void ShowButtons()
        {
            leftButton.FadeIn();
            rightButton.FadeIn();
        }

        private void HideButtons()
        {
            leftButton.FadeOut();
            rightButton.FadeOut();
        }



        //private void UpdateChant()
        //{
        //    chantUi.
        //}
    }
}