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

        private int distance;

        private void UpdateText()
        {
            text.text = distance.ToString();
        }

        private void Awake()
        {
            text = GetComponent<Text>();
        }

        private void Start()
        {
            UpdateText();
        }

        private void Update()
        {
            var dist = Game.GetDistance(gameManager.Player.transform, gameManager.Enemy.transform);
            if (distance != dist)
            {
                distance = dist;
                UpdateText();
            }
        }
    }
}