using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject policeCar;

    Transform playerCarTransform;
    float roadWidth;
    float roadY;

    GameObject[] cars = new GameObject[3];

    void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;

        GameObject road = GameObject.FindGameObjectWithTag("Road");
        roadWidth = road.GetComponentInChildren<Renderer>().bounds.size.x / 2f;
        roadY = road.GetComponentInChildren<Renderer>().bounds.max.y + 0.05f;

        Debug.Log("Road Half Width: " + roadWidth);
        Debug.Log("Road Y: " + roadY);

        for (int i = 0; i < cars.Length; i++)
        {
            float[] lanes = { -roadWidth / 2f, roadWidth / 2f };
            float randomX = lanes[Random.Range(0, lanes.Length)];
            float randomZ = playerCarTransform.position.z + (i + 1) * 20f;
            Vector3 spawnPosition = new Vector3(randomX, roadY, randomZ);
            cars[i] = Instantiate(policeCar, spawnPosition, Quaternion.Euler(0f, 180f, 0f));
        }

        InvokeRepeating("CheckCars", 0.1f, 0.1f);
    }

    void Update()
    {

    }

    int lastLane = 0;

    void CheckCars()
    {
        for (int i = 0; i < cars.Length; i++)
        {
            if (cars[i].transform.position.z < playerCarTransform.position.z - 10f)
            {
                float furthestZ = playerCarTransform.position.z;
                for (int j = 0; j < cars.Length; j++)
                {
                    if (cars[j].transform.position.z > furthestZ)
                        furthestZ = cars[j].transform.position.z;
                }

                float[] lanes = { -roadWidth / 4f, roadWidth / 4f };

                // 70% chance to switch lane, 30% chance to stay
                if (Random.Range(0, 10) < 7)
                    lastLane = lastLane == 0 ? 1 : 0;

                float randomX = lanes[lastLane];
                float randomZ = furthestZ + Random.Range(15f, 30f);

                cars[i].transform.position = new Vector3(randomX, roadY, randomZ);
                cars[i].transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }
    }
}