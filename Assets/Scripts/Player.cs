using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Fireball _spellPrefab;
    // Start is called before the first frame update
    void Start()
    {

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
            Debug.Log("mouse down " + mouseWorldPos);
            Fireball fireball = Instantiate(_spellPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));
            fireball.SetDirection(spellDirection);
        }
    }
}


/*
// 13.04.2024 AI-Tag
// This was created with assistance from Muse, a Unity Artificial Intelligence product

public class RotateSprite : MonoBehaviour
{
    public Vector2 spellDirection;

    void Update()
    {
        float angle = Mathf.Atan2(spellDirection.y, spellDirection.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}*/