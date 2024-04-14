using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Ghost _ghostPrefab;
    [SerializeField] List<Totem> _totems;
    float _nextSpawn;
    int spawnTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        _nextSpawn = Time.time + spawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (_nextSpawn < Time.time)
        {
            Spawn();
            _nextSpawn = Time.time + spawnTime;
        }
    }

    void Spawn()
    {
        string[] edges = { "top", "right", "bottom", "left" };
        int index = Random.Range(0, 4);
        string edge = edges[index];
        float x = Random.Range(-10f, 10f);
        float y = Random.Range(-6f, 6f);


        if (edge == "top") y = 6f;
        if (edge == "right") x = 10f;
        if (edge == "bottom") y = -6f;
        if (edge == "left") x = -10f;

        Ghost newGhost = Instantiate(_ghostPrefab, new Vector3(x, y, 0f), Quaternion.identity);
        if (x < 0) newGhost.GetComponent<SpriteRenderer>().flipX = true;
        newGhost.SetDirection(GetNearestTotemDirection(newGhost));
    }

    Vector3 GetNearestTotemDirection(Ghost ghost)
    {
        float minDistance = Mathf.Infinity;
        Vector3 nearestTotem = Vector3.zero;

        _totems.ForEach(totem =>
        {
            float distance = Vector3.Distance(ghost.transform.position, totem.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTotem = totem.transform.position;
            }
        });

        Debug.Log(nearestTotem);

        return (nearestTotem - ghost.transform.position).normalized;
    }
}
