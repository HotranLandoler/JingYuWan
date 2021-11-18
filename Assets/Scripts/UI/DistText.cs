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

        private int GetDistance(Transform a, Transform b)
        {
            return GameManager.ChiPerUnit * (int)Mathf.Abs(a.position.x - b.position.x);
        }

        private void UpdateText()
        {
            text.text = GetDistance(gameManager.Player.transform, gameManager.Enemy.transform).ToString();
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