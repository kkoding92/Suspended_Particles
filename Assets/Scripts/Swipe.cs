using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : TouchLogicV2
{
    public float rotateSpeed = 100.0f;
    public int invertPitch = 1;
    public Transform player;
    public Transform radar;
    private float pYaw = 0.0f;
    private Vector3 oRotation;

    void Start()
    {
        oRotation = player.eulerAngles;
        pYaw = oRotation.y;
    }

    public override void OnTouchBegan()
    {
        touch2Watch = TouchLogicV2.currTouch;
    }
    public override void OnTouchMoved()
    {
        pYaw += Input.GetTouch(touch2Watch).deltaPosition.x * rotateSpeed * invertPitch * Time.deltaTime;
        //do the rotations of our camera
        player.eulerAngles = new Vector3(0.0f, pYaw, 0.0f);
        //radar.eulerAngles = new Vector3(0.0f, 0.0f, -pYaw);
    }

    public override void OnTouchEndedAnywhere()
    {
        //the || condition is a failsafe
        if (TouchLogicV2.currTouch == touch2Watch || Input.touches.Length <= 0)
            touch2Watch = 64;
    }
}
