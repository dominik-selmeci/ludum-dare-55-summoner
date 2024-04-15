using System;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    Vector3 _direction;
    Rigidbody2D _rigidBody;

    public event Action GhostDeath;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        // if (agent.remainingDistance > agent.stoppingDistance)
        // {
        //     // // Apply wobbly or "ghostly" movement here.
        //     // float wobbleAmount = 0.03f;
        //     // transform.position = transform.position + new Vector3(Random.Range(-wobbleAmount, wobbleAmount), Random.Range(-wobbleAmount, wobbleAmount), 0);
        //     transform.position = Mathf.Lerp(transform.position, nearestotem, )T
        // }
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
