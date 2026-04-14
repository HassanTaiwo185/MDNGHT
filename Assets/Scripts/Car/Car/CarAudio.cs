using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudio : MonoBehaviour
{

    [SerializeField]
    Rigidbody rigidBody;

    // Engine sound here in Inspector
    public AudioSource engineSound;
    public AudioSource crashSound;


    // min and max pitch based on speed
    float minPitch = 0.5f;
    float maxPitch = 2f;

    // max speed reference
    float maxSpeed = 30f;

    // the exact time in seconds where crash sound starts
    public float crashSoundStartTime = 0.561f;
    public float crashSoundEndTime = 5.563f;




    bool crashSoundPlayed = false;


    void Update()
    {
        // stop engine sound if crashed
        if (GetComponent<CarCollisionHandler>().hasCrashed)
        {
            // stop engine
            engineSound.Stop();

            // play crash sound once
            if (!crashSoundPlayed)
            {
                crashSoundPlayed = true;

                // start playing from exact timestamp
                crashSound.time = crashSoundStartTime;
                crashSound.Play();




                // calculate exactly how long to play
                float duration = crashSoundEndTime - crashSoundStartTime;
                Invoke("StopCrashSound", duration);

            }

       

            return;
        }

        // current speed
        float currentSpeed = rigidBody.velocity.z;

        // map speed to pitch range
        float pitch = Mathf.Lerp(minPitch, maxPitch, currentSpeed / maxSpeed);
        engineSound.pitch = pitch;
    }



    void StopCrashSound()
    {
        crashSound.Stop();
    }
}
