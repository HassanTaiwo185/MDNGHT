using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CarCollisionHandler : MonoBehaviour
{
    [SerializeField]
    Rigidbody rigidBody;

    public bool hasCrashed = false;

    public TMP_Text gameOverDistanceText;
    public TMP_Text gameOverHighScoreText;

    
    public GameObject gameOverPanel;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("NPC") && !hasCrashed)
        {
            hasCrashed = true;
            CrashCar(collision);
        }
    }

    void CrashCar(Collision collision)
    {
        var vcam = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        if (vcam != null)
            vcam.enabled = false;

        GetComponent<CarHandler>().enabled = false;

        NPCCar npcScript = collision.gameObject.GetComponent<NPCCar>();
        if (npcScript != null)
            npcScript.enabled = false;

        Rigidbody policeRb = collision.gameObject.GetComponent<Rigidbody>();
        if (policeRb == null)
            policeRb = collision.gameObject.AddComponent<Rigidbody>();
        policeRb.velocity = Vector3.zero;
        policeRb.isKinematic = true;

        rigidBody.drag = 0f;
        rigidBody.angularDrag = 0f;
        rigidBody.constraints = RigidbodyConstraints.None;
        rigidBody.isKinematic = false;

        StartCoroutine(ApplyFlipForce());
        Invoke("EndGame", 3f);
    }

    IEnumerator ApplyFlipForce()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        rigidBody.AddForce(Vector3.up * 8f, ForceMode.Impulse);
        rigidBody.AddForce(Vector3.forward * 5f, ForceMode.Impulse);
        rigidBody.AddTorque(transform.right * 15f, ForceMode.Impulse);
    }

    void EndGame()
    {
        Time.timeScale = 0f;

        DistanceTracker tracker = FindObjectOfType<DistanceTracker>();
        if (tracker != null)
        {
            tracker.SaveScore();

            // hide HUD distance text
            tracker.distanceText.gameObject.SetActive(false);


            // show distance and high score on game over screen
            if (gameOverDistanceText != null)
                gameOverDistanceText.text = "Distance: " + Mathf.RoundToInt(tracker.GetDistance()) + "m";

            if (gameOverHighScoreText != null)
                gameOverHighScoreText.text = "Best: " + Mathf.RoundToInt(PlayerPrefs.GetFloat("HighScore", 0f)) + "m";
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    // call this from Restart button
    public void RestartGame()
    {
        // unfreeze time
        Time.timeScale = 1f;

        // reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


  

    public void GoToMainMenu()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }


}