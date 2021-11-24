using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JYW.UI.ToolTip
{
    public interface IToolTipable
    {
        ICollection<TipInfo> Tips { get; }
    }
}