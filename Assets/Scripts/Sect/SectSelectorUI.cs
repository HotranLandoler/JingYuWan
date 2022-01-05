using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JYW.UI
{
    public class SectSelectorUI : MonoBehaviour
    {
        [SerializeField]
        private SectDatabase dataSource;

        [SerializeField]
        private Button previousButton;

        [SerializeField]
        private Button nextButton;

        [SerializeField]
        private Image sectImage;

        [SerializeField]
        private Text sectText;

        //private Sect selected;
        public Sect Selected => dataSource.Sects[selectedIndex];

        private int selectedIndex = 0;

        private void Start()
        {
            previousButton.onClick.AddListener(Prev);
            nextButton.onClick.AddListener(Next);
            UpdateUI();
        }

        private void Prev()
        {
            selectedIndex--;
            if (selectedIndex < 0)
                selectedIndex = dataSource.Sects.Length - 1;
            UpdateUI();
        }

        private void Next()
        {
            selectedIndex++;
            if (selectedIndex >= dataSource.Sects.Length)
                selectedIndex = 0;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (Selected == null) return;
            sectImage.sprite = Selected.Icon;
            sectText.text = Selected.Name;
        }
    }
}