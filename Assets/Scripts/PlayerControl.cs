using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //Input variables
    float yawIn = 0;
    float pitchIn = 0;
    float rollIn = 0;
    float thrust = 0;
    float climb = 0;
    Vector3 rotateInput;

    //Movement constants
    public float maxVelocity;
    public float rotationRate;
    public float accelerationRate;
    public float frictionRate;
    public float yawRate;
    public float pitchRate;
    public float rollRate;
    Vector3 rotationRates;

    //State variables
    float yaw;
    float pitch;
    float roll;
    int missileBank;

    //Vector3 friction;
    Rigidbody rigidbody;
    
    public Camera playerCamera;
    Quaternion cameraWorldRotation;

    //Weapons
    public Material missileMaterial;

    // Placeholder for magic values
    string ShootBulletKey = "k";
    string ShootMissileKey = "m";

    void Start()
    {
        rotateInput = new Vector3(0, 0, 0);
        rotationRates = new Vector3(pitchRate, yawRate, rollRate);
        rigidbody = GetComponent<Rigidbody>();
        //friction = new Vector3(frictionRate, frictionRate, frictionRate);
        cameraWorldRotation = playerCamera.transform.rotation;

        missileBank = 5;
    }

    void FixedUpdate()
    {
        yawIn = Input.GetAxis("Yaw");
        rollIn = Input.GetAxis("Roll");
        pitchIn = Input.GetAxis("Pitch");
        thrust = Input.GetAxis("Thrust");
        climb = Input.GetAxis("Ascend");

        /*
        Debug.Log("Climb: " + climb);
        Debug.Log("Climb vector: " + transform.up * climb);
        */
        
        rotateInput = new Vector3(pitchIn, yawIn, rollIn);
        rotateInput.Scale(rotationRates * Time.fixedDeltaTime);
        transform.localRotation *= Quaternion.Euler(rotateInput);

        float velocityMag = rigidbody.velocity.magnitude;

        Vector3 movement = transform.forward * accelerationRate * Time.fixedDeltaTime * thrust;
        movement += Vector3.up * climb * accelerationRate * Time.fixedDeltaTime;

        if (Mathf.Abs(velocityMag) < maxVelocity){
            rigidbody.AddForce(movement);
        }

        //friction
        if (thrust == 0)
        {
            Vector3 friction = rigidbody.velocity * -frictionRate * Time.deltaTime;
            rigidbody.AddForce(friction);
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    void Update(){
        //playerCamera.transform.rotation = cameraWorldRotation;
        CheckInput();
    }

    void CheckInput()
    {
        if(Input.GetKeyDown(ShootBulletKey))
        {
            Bullet.Create(transform, "enemy");
        }

        if(Input.GetKeyDown(ShootMissileKey) && missileBank > 0)
        {
            var a = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<HomingMissile>();
            a.GetComponent<HomingMissile>().parentTransform = transform;
            a.GetComponent<Renderer>().material = missileMaterial;
            missileBank--;
        }
    }
}
