using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Fireball _spellPrefab;
    [SerializeField] float _movementSpeed = 4;
    Rigidbody2D _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            Vector3 spellDirection = (mouseWorldPos - transform.position).normalized;
            float angle = Mathf.Atan2(spellDirection.y, spellDirection.x) * Mathf.Rad2Deg - 180;
            Fireball fireball = Instantiate(_spellPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));
            fireball.SetDirection(spellDirection);
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(horizontal, vertical);
        _rigidBody.velocity = move * _movementSpeed;
    }
}