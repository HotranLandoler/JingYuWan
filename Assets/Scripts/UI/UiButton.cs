using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JYW.UI
{
    public class UiButton : MonoBehaviour
    {
        public Button button { get; private set; }

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            button = GetComponent<Button>();
        }

        public void FadeIn()
        {
            animator.ResetTrigger("FadeOut");
            animator.SetTrigger("FadeIn");
        }

        public void FadeOut()
        {
            animator.ResetTrigger("FadeIn");
            animator.SetTrigger("FadeOut");
        }
    }
}