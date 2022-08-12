using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public event Action<Collectible> OnPickup;

    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            OnPickup?.Invoke(this);

            gameObject.SetActive(false);
        }
    }
}