using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
 
public class ButtonPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool ButtonPressed => buttonPressed;
    private bool buttonPressed;
 
    public void OnPointerDown(PointerEventData eventData){
        buttonPressed = true;
        Debug.Log(gameObject.name + " Dwon");
    }
 
    public void OnPointerUp(PointerEventData eventData){
        Debug.Log(gameObject.name + " Up");
        buttonPressed = false;
    }

    private void Start()
    { }
}