using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour
{
    private float _health = 1f;

    private void OnMouseDown()
    {
        _health -= 0.15f;
        if (_health < 0) Destroy(gameObject);
        Debug.Log("clicked Totem");
        GetComponentInChildren<TotemHealth>().SetHealth(_health);
    }

    public void DamageIncoming()
    {
        _health -= 0.15f;
        if (_health < 0) Destroy(gameObject);
        GetComponentInChildren<TotemHealth>().SetHealth(_health);
    }
}
