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

    //Key controls
    public string shootBullets;
    public string shootMissiles;

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

    float lastYaw;
    float lastThrust;

    bool yawChange;
    bool thrustChange;

    //Vector3 friction;
    Rigidbody rigidbody;
    
    public Camera playerCamera;
    Quaternion cameraWorldRotation;

    //Weapons
    public Material missileMaterial;
    public Material bulletMaterial;
    public int currentBullets;
    public int maxBullets;
    public int currentMissiles;
    public int maxMissiles;
    public int currentHealth;
    public int maxHealth;
    public int currentMines;
    public int maxMines;
    
    //Weapon states
    bool firingBullets;
    bool firingMissiles;
    bool missileLoaded;
    float bulletReloadRate = 30f;

    //Timers
    public float missileReloadTime;
    public float missileReloadTimer;

    //UI
    public UIManager UI;

    //VFX
    public PlayerVFX playerVFX;


    void Start()
    {
        rotateInput = new Vector3(0, 0, 0);
        rotationRates = new Vector3(pitchRate, yawRate, rollRate);
        rigidbody = GetComponent<Rigidbody>();
        //friction = new Vector3(frictionRate, frictionRate, frictionRate);
        cameraWorldRotation = playerCamera.transform.rotation;
        firingBullets = false;
        firingMissiles = false;
        missileLoaded = true;
        currentBullets = maxBullets;
        currentMissiles = maxMissiles;
        currentHealth = maxHealth;
        currentMines = maxMines;
        missileReloadTimer = missileReloadTime;
        lastYaw = 0;
        lastThrust = 0;
        yawChange = false;
        thrustChange = false;
    }

    void FixedUpdate()
    {
        yawIn = Input.GetAxis("Yaw");
        rollIn = Input.GetAxis("Roll");
        pitchIn = Input.GetAxis("Pitch");
        thrust = Input.GetAxis("Thrust");
        climb = Input.GetAxis("Ascend");

        if(lastThrust != thrust)
        {
            //Remove old thrust fx
            if(lastThrust > 0){
                playerVFX.Thrust(PlayerVFX.Thruster.stern, false);
            }
            else if(lastThrust < 0){
                playerVFX.Thrust(PlayerVFX.Thruster.fore, false);
            }

            //New thrust fx
            if (thrust > 0)
            {
                playerVFX.Thrust(PlayerVFX.Thruster.stern, true);
            }
            else if(thrust < 0)
            {
                playerVFX.Thrust(PlayerVFX.Thruster.fore, true);
            }
        }

        if (lastYaw != yawIn)
        {
            //Remove old thrust fx
            if (lastYaw < 0){
                playerVFX.Thrust(PlayerVFX.Thruster.starboardFore, false);
                playerVFX.Thrust(PlayerVFX.Thruster.portStern, false);
            }
            else if(lastYaw > 0){
                playerVFX.Thrust(PlayerVFX.Thruster.starboardStern, false);
                playerVFX.Thrust(PlayerVFX.Thruster.portFore, false);
            }

            //New thrust fx
            if (yawIn > 0)
            {
                playerVFX.Thrust(PlayerVFX.Thruster.starboardStern, true);
                playerVFX.Thrust(PlayerVFX.Thruster.portFore, true);
            }
            else if(yawIn < 0)
            {
                playerVFX.Thrust(PlayerVFX.Thruster.starboardFore, true);
                playerVFX.Thrust(PlayerVFX.Thruster.portStern, true);
            }
        }

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

        lastThrust = thrust;
        lastYaw = yawIn;
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    void Update(){
        CheckInput();
    }

    IEnumerator FireBullets(){
        while (firingBullets && currentBullets > 0) {
            var a = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<Bullet>();
            a.GetComponent<Bullet>().parentTransform = transform;
            a.GetComponent<Renderer>().material = bulletMaterial;
            currentBullets--;
            yield return null;
        }
    }

    IEnumerator ReloadMissile(){
        missileReloadTimer = 0.0f;
        StartCoroutine(UI.ReloadProgress(UIManager.WeaponType.homingMissile, missileReloadTime));
        while (missileReloadTimer < missileReloadTime)
        {
            missileReloadTimer += Time.deltaTime;
            yield return null;
        }
        missileLoaded = true;
    }

    IEnumerator ReloadBullets(){
        while (!firingBullets && currentBullets < maxBullets)
        {
            currentBullets++;
            yield return new WaitForSeconds(1f / bulletReloadRate);
        }
    }

    void CheckInput()
    {
        if(Input.GetKeyDown(shootBullets) && !firingBullets){
            firingBullets = true;
            StartCoroutine(FireBullets());
        }
        else if(Input.GetKeyUp(shootBullets) && firingBullets){
            firingBullets = false;
            StartCoroutine(ReloadBullets());
        }

        //Fire homing missile
        if(Input.GetKey(shootMissiles) && missileLoaded){
            missileLoaded = false;
            var a = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<HomingMissile>();
            a.GetComponent<HomingMissile>().parentTransform = transform;
            a.GetComponent<Renderer>().material = missileMaterial;
            StartCoroutine(ReloadMissile());
        }
    }
}
