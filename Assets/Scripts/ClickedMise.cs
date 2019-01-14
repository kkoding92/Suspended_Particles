using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickedMise : MonoBehaviour
{
    public Animation ani;

    public void OnMouseDown()
    {
        ani.enabled = true;
        ani.Play();
    }
}