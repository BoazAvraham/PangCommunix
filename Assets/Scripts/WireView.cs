using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private BoxCollider2D collider;
    [SerializeField] private Transform head;

    public event Action OnStretchEnd;
        
    private Coroutine _coroutine;
    

    private void Start()
    {
        _coroutine = StartCoroutine(ShootWire());
    }

    private IEnumerator ShootWire()
    {
        yield return null;
        while (renderer.bounds.max.y < BoundsManager.Instance.GetTopOfScreen())
        {
            var size = renderer.size + Vector2.up * Time.deltaTime * GameManager.Instance.ShotSpeed;
            renderer.size = size;
            float tipOfWire = renderer.bounds.max.y;
            collider.size = size;//new Vector2(size.x,tipOfWire) ;
            collider.offset = new Vector2(collider.offset.x, size.y / 2);
            head.position = new Vector3(head.position.x, tipOfWire);
            yield return null;
        }
        _coroutine = null;
        OnStretchEnd?.Invoke();
    }

    private void OnDestroy()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }
}
