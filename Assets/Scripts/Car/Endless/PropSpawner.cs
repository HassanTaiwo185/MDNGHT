using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour
{
    public GameObject[] propPrefabs;

    // bigger pool to cover whole road
    GameObject[] propsPool = new GameObject[40];

    Transform playerCarTransform;

    // increase this to push props closer to houses
    public float sideOffset = 75f;

    // how often to place props along road
    public float propSpacing = 0.05f;

    void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // create pool
        for (int i = 0; i < propsPool.Length; i++)
        {
            GameObject randomProp = propPrefabs[Random.Range(0, propPrefabs.Length)];
            propsPool[i] = Instantiate(randomProp);
            propsPool[i].SetActive(false);
        }

        // fill both sides with props from start
        for (int i = 0; i < propsPool.Length / 2; i++)
        {
            float zPos = playerCarTransform.position.z + (i * propSpacing);
            PlaceOnLeft(propsPool[i], zPos);
        }

        for (int i = propsPool.Length / 2; i < propsPool.Length; i++)
        {
            float zPos = playerCarTransform.position.z + ((i - propsPool.Length / 2) * propSpacing);
            PlaceOnRight(propsPool[i], zPos);
        }

        InvokeRepeating("CheckProps", 0.1f, 0.1f);
    }

    void Update()
    {

    }

    void CheckProps()
    {
        for (int i = 0; i < propsPool.Length; i++)
        {
            if (propsPool[i].transform.position.z < playerCarTransform.position.z - 15f)
            {
                // find furthest prop
                float furthestZ = playerCarTransform.position.z;
                for (int j = 0; j < propsPool.Length; j++)
                {
                    if (propsPool[j].transform.position.z > furthestZ)
                        furthestZ = propsPool[j].transform.position.z;
                }

                float newZ = furthestZ + propSpacing;

                // first half of pool is always left side
                // second half is always right side
                if (i < propsPool.Length / 2)
                    PlaceOnLeft(propsPool[i], newZ);
                else
                    PlaceOnRight(propsPool[i], newZ);
            }
        }
    }

    void PlaceOnLeft(GameObject prop, float zPosition)
    {
        prop.transform.position = new Vector3(-sideOffset, 0f, zPosition);
        prop.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        prop.SetActive(true);
    }

    void PlaceOnRight(GameObject prop, float zPosition)
    {
        prop.transform.position = new Vector3(sideOffset, 0f, zPosition);
        prop.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        prop.SetActive(true);
    }
}