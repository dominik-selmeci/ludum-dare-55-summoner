using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Ghost _ghostPrefab;
    [SerializeField] List<Totem> _totems;
    [SerializeField] SummoningTimeBar _summoningTimeBar;
    [SerializeField] SummoningParticles _summoningParticles;
    [SerializeField] Canvas _winningCanvas;
    [SerializeField] Canvas _lostCanvas;
    [SerializeField] CameraShake _cameraShake;
    float _nextSpawn;
    float spawnTime = 3f;
    float _summoningTime = 0;
    float _startSummoningTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        _nextSpawn = Time.time + spawnTime;

        _totems.ForEach(totem =>
        {
            totem.TotemDestroyed += GameOver;
            totem.TotemDamaged += TotemDamaged;
        });
    }

    private void TotemDamaged()
    {
        StartCoroutine(_cameraShake.Shake(0.3f, 0.1f));
    }

    void GameOver()
    {
        _lostCanvas.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_nextSpawn < Time.time)
        {
            Spawn();
            _nextSpawn = Time.time + spawnTime;
        }

        if (_startSummoningTime != 0)
        {
            float summoningTime = _summoningTime + (Time.time - _startSummoningTime);
            _summoningTimeBar.transform.localScale = new Vector3(summoningTime / 60f, 0.5f, 1f);
            var emission = _summoningParticles.GetComponent<ParticleSystem>().emission;
            emission.rateOverTime = 600f * (summoningTime / 60f);

            if (summoningTime > 15) spawnTime = 2f;
            if (summoningTime > 30) spawnTime = 1.4f;
            if (summoningTime > 45) spawnTime = 0.8f;


            var main = _summoningParticles.GetComponent<ParticleSystem>().main;
            main.startLifetime = 1.5f + 3.5f * (summoningTime / 60f);
            ColorUtility.TryParseHtmlString("#F384F8", out Color myColor);
            main.startColor = myColor;


            if (summoningTime > 60f)
            {
                _winningCanvas.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
        }
        else
        {
            ColorUtility.TryParseHtmlString("#FFFFFF", out Color myColor);
            var main = _summoningParticles.GetComponent<ParticleSystem>().main;
            main.startColor = myColor;

            var emission = _summoningParticles.GetComponent<ParticleSystem>().emission;
            emission.rateOverTime = 10f;
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

        return (nearestTotem - ghost.transform.position).normalized;
    }

    internal void StartSummoningTime()
    {
        _startSummoningTime = Time.time;
    }

    internal void StopSummoningTime()
    {
        _summoningTime += Time.time - _startSummoningTime;
        _startSummoningTime = 0;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
