using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private int ballsLayer;
    private Rigidbody2D rb;
    private Collider2D collider2D;

    private int shotCounter = 0;
    [SerializeField] private PlayerView playerView;
    [SerializeField] private float speed;
    void Start()
    {
        speed = GameManager.Instance.IsMobile ? speed / 1.75f : speed; //from trial and error 
        rb = GetComponentInChildren<Rigidbody2D>();
        ballsLayer = LayerMask.NameToLayer("Balls");
        collider2D = GetComponent<Collider2D>();
    }
    
    void Update()
    {
        float horizontalInput = InputManager.Instance.GetHorizontalInput(); ;
        bool shootInput = InputManager.Instance.GetShootInput();

        Vector2 movement =  speed * horizontalInput * Time.deltaTime * transform.right;
        rb.MovePosition(rb.position + movement);
    
        if (shootInput && shotCounter < GameManager.Instance.ShotsLimit)
        {
            ShootWire();
        }

        playerView.UpdateView(horizontalInput);
    }

    private void ShootWire()
    {
        Vector3 pos = new Vector3(rb.position.x, collider2D.bounds.min.y);
        var wire =Instantiate(GameManager.Instance.GetWirePrefab(),pos,Quaternion.identity);
        wire.GetComponent<WireController>().OnWireDestroyed += () => shotCounter--;
        shotCounter++;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == ballsLayer)
        {
            Destroy(gameObject);
            GameManager.Instance.GameOver();
        }
    }
}
