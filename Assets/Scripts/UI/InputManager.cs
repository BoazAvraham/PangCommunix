using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonMonobehavior<InputManager>
{
    private bool isMobile;
    
    private void Start()
    {
        isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
        Debug.Log("Running on mobile : " + isMobile);
        if (isMobile)
            LevelUIManager.Instance.InitMobileInputUI();
    }

    public float GetHorizontalInput()
    {
        if (isMobile)
            return HandleMobileInput();
        else
            return HandlePCInput();
    }
    
    public bool GetShootInput()
    {
        if (isMobile)
            return HandleMobileShootInput();
        else
            return HandlePCShootInput();
    }

    private bool HandlePCShootInput() => Input.GetKeyDown(KeyCode.UpArrow);
    private bool HandlePCShootInputPlayer2() => false;
    private bool HandleMobileShootInput() => MobileInputManager.Instance.IsShooting;
    
    private float HandlePCInput()
    {
        return Input.GetAxis("Horizontal");
    }
    
    private float HandlePCInputPlayer2() => Input.GetAxis("Horizontal 2");
    

    private float HandleMobileInput() => MobileInputManager.Instance.HorizontalInput;

}
