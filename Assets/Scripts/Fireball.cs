using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    Vector3 _direction;
    Rigidbody2D _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _rigidBody.velocity = _direction * _speed;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Ghost ghost = collision.collider.GetComponent<Ghost>();
        if (ghost != null)
        {
            Destroy(ghost.gameObject);
            Destroy(gameObject);
            return;
        }

        Totem totem = collision.collider.GetComponent<Totem>();
        if (totem != null)
        {
            totem.DamageIncoming();
            Destroy(gameObject);
        }
    }
}
