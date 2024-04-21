using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Fireball _spellPrefab;
    [SerializeField] float _movementSpeed = 4;
    [SerializeField] GameManager gameManager;
    Rigidbody2D _rigidBody;
    AudioSource _audioSource;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        if (Input.GetMouseButtonDown(0)) MouseDown();

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(horizontal, vertical);
        _rigidBody.velocity = move * _movementSpeed;
    }

    void MouseDown()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3 spellDirection = (mouseWorldPos - transform.position).normalized;
        float angle = Mathf.Atan2(spellDirection.y, spellDirection.x) * Mathf.Rad2Deg - 180;

        Fireball fireball = Instantiate(_spellPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));
        fireball.SetDirection(spellDirection);

        _audioSource.Play();
    }

    public void StartSummoningTime()
    {
        gameManager.StartSummoningTime();
    }

    public void StopSummoningTime()
    {
        gameManager.StopSummoningTime();
    }
}