using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
//Moving platforms
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);    
    [SerializeField] float period = 2f;

    float movementFactor; //0 for not moved 1 for fully moved
    Vector3 startingPos; // must be stored for absolute movement

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //set movement factor automatically
        
        //protect against NaN error
        if (period <= Mathf.Epsilon){ return; }
        float cycles = Time.time / period; //grows continually from 0

        const float tau = Mathf.PI * 2f; //6.28 (tau) aka 2PI
        float rawSinWave = Mathf.Sin(cycles * tau); //cycles between -0.5 and 0.5

        movementFactor = rawSinWave / 2f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
