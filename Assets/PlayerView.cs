using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer renderer;


    private void Start()
    {
        GameManager.Instance.OnLevelCompleted += Victory;
    }

    public void UpdateView(float speed)
    {
        renderer.flipX = speed < 0;
        animator.SetFloat("Speed", Mathf.Abs(speed));
    }

    public void Victory()
    {
        animator.SetTrigger("Victory");
    }
}
