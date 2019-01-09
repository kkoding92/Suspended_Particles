using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeRadarObject : MonoBehaviour
{
    public Image image;
    public Radar radar;

    void Start()
    {
        radar.RegisterRadarObject(this.gameObject, image);
    }

    private void OnDestroy()
    {
        radar.RemoveRadarObject(this.gameObject);
    }
}
