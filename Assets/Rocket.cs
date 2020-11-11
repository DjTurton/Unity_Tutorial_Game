using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    
    //this lets us modify the value of this float from the editor
    [SerializeField] float rcsThrust = 100f;

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
            rigidbody.AddRelativeForce(Vector3.up);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        } 
        else 
        {
            audioSource.Stop();
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
                state = State.Transcending;
                Invoke("LoadNextScene", 0.5f); //this gives us a short delay before loading the next level 
                break;
            default:
                //die
                state = State.Dying;
                Invoke("LoadFirstScene", 0.5f);
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
