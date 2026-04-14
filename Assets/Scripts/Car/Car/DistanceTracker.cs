using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistanceTracker : MonoBehaviour
{
    [SerializeField]
    Rigidbody rigidBody;

    public TMP_Text distanceText;
    public TMP_Text highScoreText;

    float totalDistance = 0f;
    float highScore = 0f;

    void Start()
    {
        highScore = PlayerPrefs.GetFloat("HighScore", 0f);

        if (highScoreText != null)
            highScoreText.text = "Best: " + Mathf.RoundToInt(highScore) + "m";



    }

    void Update()
    {
        // stop tracking if game is frozen
        if (Time.timeScale == 0) return;

        totalDistance += rigidBody.velocity.z * Time.deltaTime;
        distanceText.text = "Distance: " + Mathf.RoundToInt(totalDistance) + "m";
    }

    
    public void SaveScore()
    {
        if (totalDistance > highScore)
        {
            // new high score!
            highScore = totalDistance;
            PlayerPrefs.SetFloat("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    public float GetDistance()
    {
        return totalDistance;
    }
}