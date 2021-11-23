using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JYW.UI
{
    public class EnemyUi : MonoBehaviour
    {
        private Character character;

        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private Canvas canvas;

        private void Awake()
        {
            character = GetComponent<Character>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            character.InvisibleSet += SetInvisible;
        }

        private void OnDisable()
        {
            character.InvisibleSet -= SetInvisible;
        }

        private void SetInvisible(bool invisible)
        {
            if (invisible)
            {
                spriteRenderer.enabled = false;
                canvas.enabled = false;
            }
            else
            {
                spriteRenderer.enabled = true;
                canvas.enabled = true;
            }
        }
    }
}