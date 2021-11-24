using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JYW.UI.ToolTip
{
    public class ToolTipTrigger : MonoBehaviour
    {
        private IToolTipable toolTipProvider;

        [SerializeField]
        private Button toolTipButton;

        private void Awake()
        {
            toolTipProvider = GetComponent<IToolTipable>();
            toolTipButton.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            ToolTip.Instance.ShowToolTip(toolTipProvider.Tips);
        }
    }
}