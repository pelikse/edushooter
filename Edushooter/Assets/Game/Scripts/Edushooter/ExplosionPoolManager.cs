using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class ExplosionPoolManager : MMSingleton<ExplosionPoolManager>
{
    public MMSimpleObjectPooler standardPooler;
    public MMSimpleObjectPooler specialPooler;

    private float explosionTimer = 2f;
    private float currentTimer;

    void Start()
    {
        // Set timer ke nilai awal
        currentTimer = explosionTimer;
    }

    void Update()
    {
        // Hitung mundur timer setiap frame
        if (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime; // Kurangi timer setiap detik
        }
    }

    // Method untuk memunculkan explosion di lokasi tertentu
    public void SpawnExplosion(Vector3 position)
    {
        GameObject explosion;

        // kalau cd udh selesai ambil special, kalau blm ambil standard

        // standardPooler.GetPooledGameObject();
        // specialPooler.GetPooledGameObject();

        // buat logika timer utk ganti efek ledakan
        // munculin animasi, kemudian ilangin kalo durasi selesai

        // Jika timer belum mencapai 0, gunakan standard explosion
        if (currentTimer > 0)
        {
            explosion = standardPooler.GetPooledGameObject();
        }
        else // Jika timer mencapai 0, gunakan special explosion
        {
            explosion = specialPooler.GetPooledGameObject();

            // Reset timer setelah special explosion muncul
            currentTimer = explosionTimer;
        }

        // Pastikan explosion tidak null dan aktifkan di posisi tertentu
        if (explosion != null)
        {
            explosion.transform.position = position;
            explosion.SetActive(true);
        }
    }
}
