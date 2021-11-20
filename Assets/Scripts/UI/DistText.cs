using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JYW.UI
{
    public class DistText : MonoBehaviour
    {
        [SerializeField]
        private GameManager gameManager;

        private Text text;

        private void UpdateText()
        {
            text.text = Game.GetDistance(gameManager.Player.transform, gameManager.Enemy.transform).ToString();
        }

        private void Awake()
        {
            text = GetComponent<Text>();
        }

        private void Start()
        {
            UpdateText();
        }
    }
}