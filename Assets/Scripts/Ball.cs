using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private static readonly float MODIFIER = 30f;
    
    [SerializeField] private float horizontalForceModifier = 10f;
    [Range(0,1)]
    [SerializeField] private float splitForceReductionModifier = 0.6f;
    [SerializeField] private float verticalSpeed;

    private float scale = 1;
    private float height;
    private float horizontalSpeed;
    private Rigidbody2D rb;
    private bool firstTimeGroundHit = true;
    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
    }

    private void Start()
    {
        rb.velocity = new Vector2(verticalSpeed, 0);
    }

    public void Init(float speed, float scale)
    {
        verticalSpeed = speed;
        GetComponentInChildren<SpriteRenderer>().color = GetColorByScale();
        this.scale = scale;
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
                Debug.Log("Set Speed to " + horizontalSpeed);
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
            float nextScale = scale * splitForceReductionModifier;
            if (nextScale > GameManager.Instance.MinBallSize)
            {
                GameObject newBall = Instantiate(GameManager.Instance.GetBallPrefab(), transform.position, Quaternion.identity);
                var leftBall = newBall.GetComponent<Ball>();
                leftBall.Init(-5f,scale * 0.5f);
                newBall = Instantiate(GameManager.Instance.GetBallPrefab(), transform.position, Quaternion.identity);
                var rightBall = newBall.GetComponent<Ball>();
                rightBall.Init(5f,scale * 0.5f);    
            }
            Destroy(gameObject);
        }
    }

    private Color GetColorByScale()
    {
        Color c = Color.white * scale / 100f;
        c.a = 1;
        return c; 
    }
    
    private void OnDestroy()
    {
        SoundManager.Instance.SoundPop();
        GameManager.Instance.AddScore();
    }
}
