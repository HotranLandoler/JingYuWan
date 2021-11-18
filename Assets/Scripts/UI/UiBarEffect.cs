using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JYW.UI
{
    public class UiBarEffect : MonoBehaviour
    {
        private Image image;

        [SerializeField]
        private Image target;

        [SerializeField]
        private float speed = 3f;

        // Start is called before the first frame update
        void Awake()
        {
            image = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            if (image.fillAmount > target.fillAmount)
            {
                image.fillAmount -= speed * Time.deltaTime;
            }
            else image.fillAmount = target.fillAmount;
        }
    }
}