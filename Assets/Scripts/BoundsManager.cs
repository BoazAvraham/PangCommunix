using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsManager : SingletonMonobehavior<BoundsManager>
{

    [SerializeField] private GameObject ceiling;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject ground;

    private float topOfScreenY;
    private void Start()
    {
        float screenMinX = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        float screenMaxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x;
        float screenMinY = Camera.main.ScreenToWorldPoint(Vector3.zero).y;
        float screenMaxY = Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y;
        
        ceiling.transform.position = Vector3.up * screenMaxY;
        leftWall.transform.position = Vector3.right * screenMinX;
        rightWall.transform.position = Vector3.right * screenMaxX;
        ground.transform.position = Vector3.up * screenMinY;

        var ceilingGap = screenMaxY - ceiling.GetComponent<Collider2D>().bounds.min.y;
        //0.1f for overlapping in wire - found it to be the easiest solution
        topOfScreenY = screenMaxY - ceilingGap + 0.1f;
    }

    public float GetTopOfScreen() => topOfScreenY;
}
