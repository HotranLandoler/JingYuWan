using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JYW.UI
{
    public class BuffUi : MonoBehaviour
    {
        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Text levelText;

        private Buff bindBuff;

        public void Set(Buff buff)
        {
            bindBuff = buff;
            iconImage.sprite = buff.Data.Icon;
            nameText.text = buff.Data.Name;
        }

        
    }
}