using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Objects & Prefabs")]
    [SerializeField] Ghost _ghostPrefab;
    [SerializeField] SummoningTimeBar _summoningTimeBar;
    [SerializeField] SummoningParticles _summoningParticles;
    [SerializeField] List<Totem> _totems;

    [Header("UI Canvases")]
    [SerializeField] Canvas _introCanvas;
    [SerializeField] Canvas _tutorialCanvas;
    [SerializeField] Canvas _winningCanvas;
    [SerializeField] Canvas _lostCanvas;
    [SerializeField] CameraShake _cameraShake;

    [Header("Audio SFX & Music")]
    [SerializeField] AudioClip _damageSfx;
    [SerializeField] AudioClip _ghostDeathSfx;
    [SerializeField] AudioClip _music;

    [Header("Colors")]
    [SerializeField] Color _white; // #FFFFFF
    [SerializeField] Color _pink; // #F384F8

    ParticleSystem _particleSystem;
    ParticleSystem.EmissionModule _particleSystemEmission;
    ParticleSystem.MainModule _particleSystemMain;
    AudioSource _audioSource;
    float _nextSpawn = 0;
    float _spawnTime = 3f;
    float _summoningTime = 0;
    float _startSummoningTime = 0;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _particleSystem = _summoningParticles.GetComponent<ParticleSystem>();
        _particleSystemEmission = _particleSystem.emission;
        _particleSystemMain = _particleSystem.main;

        _totems.ForEach(totem =>
        {
            totem.TotemDestroyed += GameOver;
            totem.TotemDamaged += TotemDamaged;
        });

        Time.timeScale = 0;
        _introCanvas.gameObject.SetActive(true);
        SetSpawnTime();
    }

    private void TotemDamaged()
    {
        StartCoroutine(_cameraShake.Shake(0.3f, 0.1f));
        _audioSource.PlayOneShot(_damageSfx);
    }

    void GameOver()
    {
        _lostCanvas.gameObject.SetActive(true);
        _audioSource.Stop();
        Time.timeScale = 0;
    }

    void Update()
    {
        SetSpawnTime();

        if (_startSummoningTime != 0)
        {
            float summoningTime = _summoningTime + (Time.time - _startSummoningTime);
            _summoningTimeBar.transform.localScale = new Vector3(summoningTime / 60f, 0.5f, 1f);

            _particleSystemEmission.rateOverTime = 600f * (summoningTime / 60f);

            if (summoningTime > 15) _spawnTime = 2f;
            if (summoningTime > 30) _spawnTime = 1.4f;
            if (summoningTime > 45) _spawnTime = 0.8f;

            _particleSystemMain.startLifetime = 1.5f + 3.5f * (summoningTime / 60f);
            _particleSystemMain.startColor = _pink;

            if (summoningTime > 60f)
            {
                _winningCanvas.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
        }
        else
        {
            _particleSystemMain.startColor = _white;
            _particleSystemEmission.rateOverTime = 10f;
        }
    }

    void SetSpawnTime()
    {
        if (_nextSpawn < Time.time || _nextSpawn == 0)
        {
            Spawn();
            _nextSpawn = Time.time + _spawnTime;
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
        newGhost.GhostDeath += GhostDeath;
        if (x < 0) newGhost.GetComponent<SpriteRenderer>().flipX = true;
        newGhost.SetDirection(GetNearestTotemDirection(newGhost));
    }

    void GhostDeath()
    {
        _audioSource.PlayOneShot(_ghostDeathSfx);
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

    public void StartSummoningTime()
    {
        _startSummoningTime = Time.time;
    }

    public void StopSummoningTime()
    {
        _summoningTime += Time.time - _startSummoningTime;
        _startSummoningTime = 0;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);

        Time.timeScale = 1;
        _introCanvas.gameObject.SetActive(false);
        _tutorialCanvas.gameObject.SetActive(false);
    }

    public void ShowTutorialCanvas()
    {
        _tutorialCanvas.gameObject.SetActive(true);
        _introCanvas.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        _tutorialCanvas.gameObject.SetActive(false);
        Time.timeScale = 1;
        _audioSource.clip = _music;
        _audioSource.Play();
    }
}
