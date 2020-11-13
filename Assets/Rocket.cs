using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    
    //this lets us modify the value of this float from the editor
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    //sound
    [SerializeField] AudioClip mainEngine; //our main engine thrust sound
    [SerializeField] AudioClip deathSound; // The sound that plays when we die
    [SerializeField] AudioClip winSound; // The sound that plays when we win a level
    //particles
    [SerializeField] ParticleSystem mainEngineParticles; //our main engine thrust particles
    [SerializeField] ParticleSystem deathParticles; // The effect that plays when we die
    [SerializeField] ParticleSystem winParticles; // The effect that plays when we win a level

    //variable to initialize rigidbody
    Rigidbody rigidbody;
    AudioSource audioSource;

    //player state
    enum State { Alive, Dying, Transcending };
    State state = State.Alive;


    // Start is called before the first frame update
    void Start()
    {
        //getting components to be used in ship
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    //processes the players input
    private void ProcessInput() 
    {
        if (state == State.Alive) 
        {
            processThrust();
            processRotation();
        } 
    }

    void processThrust()
    {
        //thrust
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up * rcsThrust * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
                mainEngineParticles.Play();
            }
        } 
        else 
        {
            audioSource.Stop();
            mainEngineParticles.Stop();

        }

    }

    void processRotation() 
    {
        //left and right rotation
        rigidbody.freezeRotation = true; // take manual control of rotation

        //used for modifying how far we rotate per frame
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidbody.freezeRotation = false; // resume physics control of rotation
    }

    //dealing with collisions
    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                break;
            case "Finish":
                //go to the next level
                audioSource.Stop();
                audioSource.PlayOneShot(winSound);
                winParticles.Play();
                state = State.Transcending;
                Invoke("LoadNextScene", levelLoadDelay); //this gives us a short delay before loading the next level 
                break;
            default:
                //die
                state = State.Dying;
                audioSource.Stop();
                audioSource.PlayOneShot(deathSound);
                deathParticles.Play();
                Invoke("LoadFirstScene", levelLoadDelay);
                break;
        }
    }

    //loads the next level
    void LoadNextScene() 
    {
        SceneManager.LoadScene(1); //TODO: allow more levels 
    }

    //loads the first level
    void LoadFirstScene() 
    {
        SceneManager.LoadScene(0); 
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }
}
