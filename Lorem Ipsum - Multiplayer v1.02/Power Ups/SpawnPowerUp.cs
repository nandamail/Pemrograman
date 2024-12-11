using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class SpawnPowerUp : NetworkBehaviour
{
    [SerializeField] public GameObject powerUpPrefab; // Prefab untuk power-up
    [SerializeField] public Transform[] PowerUpSpawn; // Array lokasi respawn power-up
    [SerializeField] private float respawnDelay = 5f; // Waktu respawn setelah diambil
    [SerializeField] private GameObject pickupEffectPrefab; // Prefab untuk efek visual saat power-up diambil

    private List<GameObject> activePowerUps = new List<GameObject>(); // Daftar power-ups yang aktif

    private void Start()
    {
        if (IsServer)
        {
            InitializePowerUps();
        }
    }

    // Inisialisasi power-ups di semua spawn points saat game dimulai
    private void InitializePowerUps()
    {
        // Pastikan ada spawn points dan prefab yang diatur
        if (PowerUpSpawn.Length == 0 || powerUpPrefab == null)
        {
            Debug.LogError("[SpawnPowerUp] PowerUpSpawn atau prefab tidak tersedia!");
            return;
        }

        // Spawn power-up di setiap spawn point
        foreach (Transform spawnPoint in PowerUpSpawn)
        {
            SpawnPowerUpAtPoint(spawnPoint);
        }
    }

    // Spawn power-up pada titik tertentu
    private void SpawnPowerUpAtPoint(Transform spawnPoint)
    {
        if (powerUpPrefab == null)
        {
            Debug.LogError("[SpawnPowerUp] Prefab power-up belum diatur!");
            return;
        }

        // Spawn power-up di server
        GameObject powerUpInstance = Instantiate(powerUpPrefab, spawnPoint.position, spawnPoint.rotation);
        NetworkObject networkObject = powerUpInstance.GetComponent<NetworkObject>();

        if (networkObject != null)
        {
            networkObject.Spawn(); // Spawning dengan Netcode
            activePowerUps.Add(powerUpInstance); // Tambahkan ke daftar aktif
        }
        else
        {
            Debug.LogError("[SpawnPowerUp] Power-up prefab tidak memiliki NetworkObject!");
        }
    }

    // Fungsi dipanggil saat power-up diambil oleh pemain
    public void PowerUpTaken(GameObject powerUp)
    {
        if (!IsServer)
            return;

        Vector3 position = powerUp.transform.position;

        // Hapus power-up dari jaringan
        NetworkObject networkObject = powerUp.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.Despawn();
        }

        activePowerUps.Remove(powerUp); // Hapus dari daftar aktif
        Destroy(powerUp);

        // Kirim efek visual ke semua client
        PlayPickupEffectClientRpc(position);

        // Respawn setelah delay menggunakan Coroutine
        StartCoroutine(RespawnPowerUpAfterDelay());
    }

    // Coroutine untuk respawn power-up setelah delay
    private IEnumerator<WaitForSeconds> RespawnPowerUpAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);

        SpawnRandomPowerUp();
    }

    // Fungsi untuk memanggil power-up secara acak
    private void SpawnRandomPowerUp()
    {
        if (PowerUpSpawn.Length == 0 || powerUpPrefab == null)
        {
            Debug.LogError("[SpawnPowerUp] PowerUpSpawn atau prefab tidak tersedia!");
            return;
        }

        int randomIndex = Random.Range(0, PowerUpSpawn.Length);
        Transform spawnPoint = PowerUpSpawn[randomIndex];
        SpawnPowerUpAtPoint(spawnPoint);
    }

    // ClientRpc untuk efek visual di semua client
    [ClientRpc]
    private void PlayPickupEffectClientRpc(Vector3 position)
    {
        if (pickupEffectPrefab != null)
        {
            Instantiate(pickupEffectPrefab, position, Quaternion.identity);
        }
    }
}
