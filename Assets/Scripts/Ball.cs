using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private static readonly float MODIFIER = 30f;
    
    [SerializeField] private float horizontalForceModifier = 10f;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float shrinkFactor = 0.5f;
    [SerializeField] private int numOfSplits;
    
    
    private float scale = 1;
    private float height;
    private float horizontalSpeed;
    private Rigidbody2D rb;
    private bool firstTimeGroundHit = true;
    private SpriteRenderer renderer;
    
    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        renderer = GetComponentInChildren <SpriteRenderer>();
    }

    private void Start()
    {
        GameManager.Instance.RegisterBall();
        rb.velocity = new Vector2(verticalSpeed, 0);
    }

    public void Init(float speed, float scale, float shrinkFactor, int splits, Color color)
    {
        numOfSplits = splits - 1;
        this.shrinkFactor = shrinkFactor;
        verticalSpeed = speed;
        GetComponentInChildren<SpriteRenderer>().color = GetColorByScale(color);
        this.scale = scale * shrinkFactor;
        transform.localScale *= scale;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Set initial height of jump, no matter the initial spawn height
        if (other.gameObject.tag == "Ground")
        {
            if (firstTimeGroundHit)
            {
                firstTimeGroundHit = false;
                horizontalSpeed = GetSpeedFromScale();
                Debug.Log("Set Horizontal Speed to " + horizontalSpeed);
                Debug.Log("Set VERT Speed to " + verticalSpeed);
                rb.velocity = new Vector2(rb.velocity.x, horizontalSpeed);
            }
        }
    }

    private float GetSpeedFromScale()
    {
        return Mathf.Sqrt(scale) * horizontalForceModifier;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Wire")
        {
            float nextScale = scale * shrinkFactor;
            //To prevent too small balls
            if (numOfSplits > 0 && nextScale > GameManager.Instance.MinBallSize)
                SplitBall();

            DestroyFromCollision();
        }
    }

    //split to 2 balls, shrink by factor, color same but brighter
    private void SplitBall()
    {
        GameObject newBall = Instantiate(GameManager.Instance.GetBallPrefab(), transform.position, Quaternion.identity);
        var leftBall = newBall.GetComponent<Ball>();
        leftBall.Init(-verticalSpeed, scale , shrinkFactor, numOfSplits, renderer.color );
        newBall = Instantiate(GameManager.Instance.GetBallPrefab(), transform.position, Quaternion.identity);
        var rightBall = newBall.GetComponent<Ball>();
        rightBall.Init(verticalSpeed, scale , shrinkFactor, numOfSplits, renderer.color);   
    }

    private void DestroyFromCollision()
    {
        SoundManager.Instance.SoundPop();
        GameManager.Instance.AddScore();
        GameManager.Instance.UnregisterBall();
        Destroy(gameObject);
    }
    
    private Color GetColorByScale(Color color)
    {
        Color c = (Color.white + color) /2f;
        c.a = 1;
        return c; 
    }
}
