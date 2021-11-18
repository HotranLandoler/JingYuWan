using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UITween : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().DOFade(1, 2).From(0);
        GetComponent<RectTransform>().DOAnchorPosX(250, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
