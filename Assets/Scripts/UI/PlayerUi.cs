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
            leftButton.button.onClick.AddListener(LeftButtonClicked);
            rightButton.button.onClick.AddListener(RightButtonClicked);
        }

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