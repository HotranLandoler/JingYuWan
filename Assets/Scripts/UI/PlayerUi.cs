using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JYW.UI
{
    public class PlayerUi : MonoBehaviour
    {
        private Character character;

        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private UiManager uiManager;

        [SerializeField]
        private UiButton leftButton;

        [SerializeField]
        private UiButton rightButton;

        private void Awake()
        {
            character = GetComponent<Character>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            character.RoundStarted += ShowButtons;
            character.RoundEnded += HideButtons;
            character.InvisibleSet += SetInvisible;
        }

        private void OnDisable()
        {
            character.RoundStarted -= ShowButtons;
            character.RoundEnded -= HideButtons;
            character.InvisibleSet -= SetInvisible;
        }

        private void Start()
        {
            leftButton.button.onClick.AddListener(() => MoveButtonClicked(Direction.L));
            rightButton.button.onClick.AddListener(() => MoveButtonClicked(Direction.R));
        }

        private void MoveButtonClicked(Direction dir)
        {
            if (!character.CanMove())
            {
                uiManager.ShowWarning(Game.CantMove);
                return;
            }
            character.MoveRequest((int)dir * Character.NormalMoveDist, MoveType.Normal);
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

        private void SetInvisible(bool invisible)
        {
            if (invisible)
            {
                Color color = Color.white;
                color.a = 0.5f;
                spriteRenderer.color = color;
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }

    }
}