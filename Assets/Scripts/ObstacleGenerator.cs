using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    // Structure to define a spawnable object
    [System.Serializable]
    public struct SpawnableObject
    {
        public GameObject prefab;
        [Range(0f, 1f)]
        public float spawnChance;
    }

    // Array of spawnable objects
    public SpawnableObject[] objects;

    // Minimum and maximum spawn rates
    public float minSpawnRate = 1f;
    public float maxSpawnRate = 2f;

    // Variable to store the last spawned object
    private GameObject lastSpawnedObject;

    // Called when the script instance is being loaded and enabled
    private void OnEnable()
    {
        // Invoke the Spawn method after a random delay within the specified range
        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }

    // Called when the script instance is being disabled
    private void OnDisable()
    {
        // Cancel any ongoing Invoke calls to prevent unwanted spawns
        CancelInvoke();
    }

    // Method to spawn obstacles based on their spawn chances
    private void Spawn()
    {
        // Generate a random spawn chance value
        float spawnChance = Random.value;

        // Iterate through the array of spawnable objects
        foreach (var obj in objects)
        {
            // Check if the generated spawn chance is less than the object's spawn chance
            if (spawnChance < obj.spawnChance)
            {
                // Check if the selected object is the same as the last spawned object
                if (lastSpawnedObject != null && obj.prefab == lastSpawnedObject)
                {
                    // If it's the same, skip this iteration and generate a new random spawn chance
                    Spawn();
                    return;
                }
                
                // Instantiate the chosen obstacle prefab and position it based on the generator's position
                GameObject obstacle = Instantiate(obj.prefab);
                obstacle.transform.position += transform.position;

                // Update the last spawned object
                lastSpawnedObject = obj.prefab;

                // Exit the loop after spawning one object
                break;
            }
            else
            {
                // If the spawn chance is not low enough, subtract the object's spawn chance from it
                spawnChance -= obj.spawnChance;
            }
        }

        // Invoke the Spawn method again after a new random delay within the specified range
        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }
}
