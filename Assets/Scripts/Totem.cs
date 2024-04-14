using System;
using UnityEngine;

public class Totem : MonoBehaviour
{
    private float _health = 1f;
    public event Action TotemDestroyed;

    public void DamageIncoming()
    {
        _health -= 0.15f;
        if (_health < 0)
        {
            Destroy(gameObject);
            TotemDestroyed?.Invoke();
        }
        GetComponentInChildren<TotemHealth>().SetHealth(_health);
    }
}
