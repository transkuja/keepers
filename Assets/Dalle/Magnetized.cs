using UnityEngine;
using System.Collections;

public class Magnetized : MonoBehaviour {

    /* Attraction drag must be set so the magnetized object will stop the closer from the target object. */
    [SerializeField]
    private short attractionDrag = 6;

    /* Magnetized object speed. */
    [SerializeField]
    private float magnetizedVelocity = 1.0f;

    private Vector3 storedrotation;

    private float storedmass;


    /* Magnetized object rigidbody. */
    private Rigidbody magnetizedRigidbody;

    void Start()
    {
        magnetizedRigidbody = GetComponent<Rigidbody>();
        magnetizedRigidbody.useGravity = true;
        magnetizedVelocity *= 50;
		storedmass = magnetizedRigidbody.mass;
		storedrotation = magnetizedRigidbody.transform.localEulerAngles;
    }
    
    void onMouseDown () {

		magnetizedRigidbody.useGravity = false;
		magnetizedRigidbody.drag = attractionDrag;
        // had to add this for a heavy object.
        magnetizedRigidbody.mass = 1;

        magnetizedRigidbody.AddForce (AttractForce (transform) * Time.deltaTime * magnetizedVelocity);
		magnetizedRigidbody.transform.localEulerAngles= storedrotation;
        

    }

    /* Computes the attract force vector. The vector points to target. */
    private Vector3 AttractForce(Transform target)
    {
        Vector3 force = Vector3.zero;
        force.x = target.position.x - transform.position.x;
        force.y = target.position.y - transform.position.y;
        //force.z = target.position.z - transform.position.z;
        return force;
    }

}
