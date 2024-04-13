using UnityEngine;

public class Totem : MonoBehaviour
{
    private float _health = 1f;

    public void DamageIncoming()
    {
        _health -= 0.15f;
        if (_health < 0) Destroy(gameObject);
        GetComponentInChildren<TotemHealth>().SetHealth(_health);
    }
}
