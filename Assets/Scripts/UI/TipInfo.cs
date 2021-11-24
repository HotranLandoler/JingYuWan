using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JYW.UI.ToolTip
{
    [CreateAssetMenu]
    public class TipInfo : ScriptableObject
    {
        [SerializeField]
        private string tipName;
        public string TipName => tipName;

        [TextArea(2,4)]
        [SerializeField]
        private string desc;
        public string Desc => desc;
    }
}