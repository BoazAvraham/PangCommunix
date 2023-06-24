using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileInputManager : SingletonMonobehavior<MobileInputManager>
{
    [SerializeField] private ButtonPress left;
    [SerializeField] private ButtonPress right;
    [SerializeField] private ButtonPress shoot;

    private float horizontalInput;
    
    private void Update()
    {
        float input = 0;
        if (left.ButtonPressed)
            input -= 1;
        if (right.ButtonPressed)
            input += 1;
        horizontalInput = input;
    }

    public float HorizontalInput => horizontalInput;
    public bool IsShooting => shoot.ButtonPressed;
}
