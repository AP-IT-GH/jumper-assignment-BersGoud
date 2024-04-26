using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleSpawn : MonoBehaviour
{
    public GameObject rewardPrefab;
    public GameObject wallPrefab;
    public GameObject linePrefab;
    float episodeStartTime;
    float spawnInterval = 3f; // Spawn interval in seconds
    float nextSpawnTime;

    public cubeAgent cubeAgent;

    void Start()
    {
        episodeStartTime = Time.time;
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomItem();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnRandomItem()
    {
        float randomX = -5f;
        float randomZ = UnityEngine.Random.Range(-2.5f, 2.5f);
        float randomWidth = UnityEngine.Random.Range(0.5f, 3.5f);

        GameObject newItem;

        int randomItem = UnityEngine.Random.Range(0, 3); // 0: reward, 1: wall, 2: line
        this.cubeAgent.RandomItemType = randomItem;
        int minSpeed;
        int maxSpeed;
        string objectType;

        switch (randomItem)
        {
            case 0: // Reward
                newItem = Instantiate(rewardPrefab, transform);
                newItem.transform.localPosition = new Vector3(randomX, 0.5f, Mathf.Clamp(randomZ, -2.25f, 2.25f));
                minSpeed = 3;
                maxSpeed = 5;
                objectType = "reward";
                break;
            case 1: // Wall
                newItem = Instantiate(wallPrefab, transform);
                newItem.transform.localPosition = new Vector3(randomX, 0.5f, Mathf.Clamp(randomZ, ((randomWidth / 2) - 2.5f), (2.5f - (randomWidth / 2))));
                newItem.transform.localScale = new Vector3(0.1f, 1f, randomWidth);
                minSpeed = 1;
                maxSpeed = 3;
                objectType = "wall";
                break;
            case 2: // Line
                newItem = Instantiate(linePrefab, transform);
                newItem.transform.localPosition = new Vector3(randomX, 0.17f, 0f); // Always spawn at local z = 0
                minSpeed = 2;
                maxSpeed = 2;
                objectType = "line";
                break;
            default:
                Debug.LogError("Invalid random item index");
                return;
        }

        StartCoroutine(SlideAndDisappear(objectType, newItem, minSpeed, maxSpeed));
    }

    public void DestroySpawnedObjects()
    {
        foreach (Transform child in transform)
        {
            if (!child.CompareTag("Untagged")) // Check if the tag is not "Untagged"
            {
                Destroy(child.gameObject);
            }
        }
    }

    IEnumerator SlideAndDisappear(string objectType, GameObject item, int minSpeed, int maxSpeed)
    {

        float slideSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed); // Speed of sliding
        while (item.transform.localPosition.x <= 5f)
        {
            float step = slideSpeed * Time.deltaTime;
            item.transform.localPosition += new Vector3(step, 0f, 0f);
            yield return null;
        }

        if (objectType == "line") // this object is a beam that the ML agent needs to jump over it
        {
            Debug.Log("end reached by beam");
            FindObjectOfType<cubeAgent>().giveRewardExternally(3.0f);
        }
        else if (objectType == "wall")
        {
            Debug.Log("end reached by wall");
            FindObjectOfType<cubeAgent>().giveRewardExternally(0.8f);
        }
        else
        {
            Debug.Log("end reached by reward");
            FindObjectOfType<cubeAgent>().giveRewardExternally(-0.05f);
        }

        Destroy(item);
    }
}
