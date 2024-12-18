using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Slider HealthBar;
    public float MaxHealth;
    private float _currentHealth;
    public float HealthDecreaseRate;
    public float HealthIncreaseAmount;
    public Gradient Gradient;
    public Image Fill;

    public PlayerManager PlayerManager;
    public ScoreSystem ScoreSystem;

    void Start()
    {
        _currentHealth = MaxHealth;
        HealthBar.maxValue = MaxHealth;
        HealthBar.value = _currentHealth;
        PlayerManager = FindObjectOfType<PlayerManager>();
        UpdateHealthBarColor();
    }

    void Update()
    {
        if (PlayerManager.IsGameStarted && !PlayerManager.GameOver)
        {
            DecreaseHealth(); // Kurangi health saat game sudah dimulai dan belum game over
        }
    }

    //Mengurangi tenaga perdetik
    void DecreaseHealth()
    {
        _currentHealth -= HealthDecreaseRate * Time.deltaTime;
        if (_currentHealth < 0)
        {
            _currentHealth = 0;
            PlayerManager.GameOver = true;
        }
        HealthBar.value = _currentHealth;
        UpdateHealthBarColor();
    }

    public void IncreaseHealth()
    {
        // Hanya izinkan peningkatan kesehatan jika game belum berakhir
        if (!PlayerManager.GameOver)
        {
            _currentHealth += HealthIncreaseAmount;
            if (_currentHealth > MaxHealth)
            {
                _currentHealth = MaxHealth;
            }
            HealthBar.value = _currentHealth;
            UpdateHealthBarColor();
        }
    }


    // Mengurangi tenaga sebesar 10 ketika terkena obstacle
    public void ObstacleHit()
    {
        _currentHealth -= 10f;
        SoundManager.Instance.PlaySound3D("Die", transform.position);
        Debug.Log("nabrak anjay!");
        if (_currentHealth < 0)
        {
            _currentHealth = 0;
            PlayerManager.GameOver = true;
        }
        HealthBar.value = _currentHealth;
        UpdateHealthBarColor();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Medicine"))
        {
            IncreaseHealth();
            SoundManager.Instance.PlaySound3D("Poin", transform.position);
            ScoreSystem.TerasiHit();
            Destroy(other.gameObject);
        }
    }

    // Perbarui warna bar
    void UpdateHealthBarColor()
    {
        // Gunakan nilai normalisasi untuk mengevaluasi gradient
        Fill.color = Gradient.Evaluate(HealthBar.normalizedValue);
    }


    //public void EnemyHit()
    //{
    //    _currentHealth -= 50f; 
    //    SoundManager.Instance.PlaySound3D("Die", transform.position);
    //    if (_currentHealth < 0)
    //    {
    //        _currentHealth = 0; 
    //        PlayerManager.GameOver = true; 
    //    }
    //    HealthBar.value = _currentHealth; 
    //    UpdateHealthBarColor(); 
    //}
}
