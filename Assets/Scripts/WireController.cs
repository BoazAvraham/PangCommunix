using System;
using System.Collections;
using UnityEngine;

public class WireController : MonoBehaviour
{
    [SerializeField] private WireView wireView;
    
    private float speed = 5f;
    public event Action OnWireDestroyed ;
    private Coroutine coroutine;

    private void Start()
    {
        wireView.OnStretchEnd += () => StartCoroutine(OnDestroyIE());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator OnDestroyIE()
    {
        yield return new WaitForSecondsRealtime(GameManager.Instance.WireTimeOut);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        
        OnWireDestroyed?.Invoke();
    }
}
