using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JYW.UI
{
    public class Ui : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
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