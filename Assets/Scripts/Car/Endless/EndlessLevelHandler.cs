using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessLevelHandler : MonoBehaviour
{
    // Different road prefabs or type 
    [SerializeField]
    GameObject[] sectionPrefabs;

    // Pool of 20 road objects
    GameObject[] sectionsPool = new GameObject[20];

    // Only 10 road sections active and visble at a time
    GameObject[] sections = new GameObject[10];
    
    // Reference to player position
    Transform playerCarTransform;

    WaitForSeconds waitFor100MilliSeconds = new WaitForSeconds(0.1f);

    // Length of each road section 
    const float sectionLength = 26;

    void Start()
    {
        // Get player by tag
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;

        int prefabIndex = 0;

        // Create 20 roads objects, disable and hide them
        for (int i = 0; i < sectionsPool.Length; i++)
        {
            sectionsPool[i] = Instantiate(sectionPrefabs[prefabIndex]);
            sectionsPool[i].SetActive(false);

            prefabIndex++;

            // Ensure to stay within our road prefabs or type
            if (prefabIndex > sectionPrefabs.Length - 1)
            {
                prefabIndex = 0;
            }
        }

        // Activate first 10 roads and place them in order
        for (int i = 0; i < sections.Length; i++)
        {
            // Get random unused roads
            GameObject randomSection = GetRandomSectionFromPool();

            // place road one after another
            randomSection.transform.position = new Vector3(randomSection.transform.position.x, 0, i * sectionLength);
            randomSection.SetActive(true);

            // save active or visible road
            sections[i] = randomSection;
        }

        // Start checking road positions to update further
        StartCoroutine(UpdateLessOftenCo());
    }

    // Coroutine runs forever and checks roads every 0.1 seconds
    IEnumerator UpdateLessOftenCo()
    {
        while (true)
        {
            UpdateSectionPosition();
            yield return waitFor100MilliSeconds;
        }
    }

    // Check if any road went behind player, inactivate it, get random unused road and put it front of the furthest road 
    void UpdateSectionPosition()
    {
        for (int i = 0; i < sections.Length; i++)
        {
            // If road is behind player position
            if (sections[i].transform.position.z < playerCarTransform.position.z - sectionLength)
            {
                // Finding the furthest road section
                float furthestZ = sections[0].transform.position.z;

                for (int j = 1; j < sections.Length; j++)
                {
                    if (sections[j].transform.position.z > furthestZ)
                    {
                        furthestZ = sections[j].transform.position.z;
                    }
                }

                // inactivate old road behind the player
                sections[i].SetActive(false);

                // Get new random unused road
                sections[i] = GetRandomSectionFromPool();

                // Place it in front of the furthest road and activate it
                sections[i].transform.position = new Vector3(0, 0, furthestZ + sectionLength);
                sections[i].SetActive(true);
            }
        }
    }

    // Pick a random inactive  road from pool
    GameObject GetRandomSectionFromPool()
    {
        // Pick random index
        int randomIndex = Random.Range(0, sectionsPool.Length);

        // No new unused road found yet
        bool isNewSectionFound = false;

        while (!isNewSectionFound)
        {
            // If road section is not active, then unused road found
            if (!sectionsPool[randomIndex].activeInHierarchy)
            {
                isNewSectionFound = true;
            }
            else
            {
                // If active, try next index
                randomIndex++;

                // If reached end, go back to 0
                if (randomIndex > sectionsPool.Length - 1)
                {
                    randomIndex = 0;
                }
            }
        }

        return sectionsPool[randomIndex];
    }
}