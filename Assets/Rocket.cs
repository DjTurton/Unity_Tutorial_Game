using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    
    //this lets us modify the value of this float from the editor
    [SerializeField] float rcsThrust = 100f;

    //variable to initialize rigidbody
    Rigidbody rigidbody;
    AudioSource audioSource;
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
        processThrust();
        processRotation();
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
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                break;
            case "Fuek":
                //refuel
                break;
            default:
                //die
                print("oh no"); //todo remove me!!!
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        
    }
}
