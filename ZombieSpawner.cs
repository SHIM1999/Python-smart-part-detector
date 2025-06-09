using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform spawnPoint;

    public Transform mainCharacter;  // 메인 캐릭터 Transform 필드 추가

    public int maxZombies = 5;
    public float spawnInterval = 5f;

    private int spawnedCount = 0;

    void Start()
    {
        StartCoroutine(SpawnZombiesRoutine());
    }

    IEnumerator SpawnZombiesRoutine()
    {
        while (spawnedCount < maxZombies)
        {
            SpawnZombie();
            spawnedCount++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnZombie()
    {
        GameObject zombie = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);

        // Instantiate한 좀비의 ZombieMove 컴포넌트에 메인 캐릭터 할당
        ZombieMove moveScript = zombie.GetComponent<ZombieMove>();
        if (moveScript != null && mainCharacter != null)
        {
            moveScript.target = mainCharacter;
        }
    }
}
