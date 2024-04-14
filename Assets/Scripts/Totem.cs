using System;
using UnityEngine;

public class Totem : MonoBehaviour
{
    private float _health = 1f;
    public event Action TotemDestroyed;
    public event Action TotemDamaged;

    public void DamageIncoming()
    {
        _health -= 0.15f;
        if (_health < 0)
        {
            Destroy(gameObject);
            TotemDestroyed?.Invoke();
            return;
        }

        TotemDamaged?.Invoke();
        GetComponentInChildren<TotemHealth>().SetHealth(_health);
    }
}
