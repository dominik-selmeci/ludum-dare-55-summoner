using System;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    Vector3 _direction;
    Rigidbody2D _rigidBody;

    public event Action GhostDeath;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_direction != null)
            _rigidBody.velocity = _direction;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Totem totem = collision.collider.GetComponent<Totem>();
        if (totem != null)
        {
            totem.DamageIncoming();
            Destroy(gameObject);
            return;
        }

        Fireball fireball = collision.collider.GetComponent<Fireball>();
        if (fireball != null)
        {
            GhostDeath?.Invoke();
        }
    }
}
