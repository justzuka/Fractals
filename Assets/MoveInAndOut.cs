using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class MoveInAndOut : MonoBehaviour
{
    public float duration = .6f;
    private RectTransform rTra;
    bool onScreen = true;
    private void Start()
    {
        rTra = GetComponent<RectTransform>();
    }
    public void Toggle()
    {
        onScreen = !onScreen;
        if (onScreen)
        {
            rTra.DOAnchorPosX(0, duration);
        }
        else
        {
            rTra.DOAnchorPosX(-rTra.rect.x, duration);
        }
    }
}
