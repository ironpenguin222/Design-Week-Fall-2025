using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[SelectionBase]
public class CanMover : MonoBehaviour
{

    [Tooltip("Zero-based index of the Joycon in the list of connected controllers.")]
    public int index = 0;

    [Tooltip("Rotation rate around each local axis.")]
    public Vector3 gyro;

    [Tooltip("Direction of acceleration in local coordinates (points up when supported against gravity).")]
    public Vector3 accel;

    [Tooltip("Ready-to-use rotation combining accelerometer and gyroscope sensors.")]
    public Quaternion orientation;

    Transform model;
    private List<Joycon> joycons;

    [Header("Shake")]
    private Vector3 lastAccel;
    public float shakeThreshold = 2.0f;
    public float shakeMeter = 1.0f;
    public float timeBetween = 0.4f;

    public float shakeDecay = 0.0f;
    public bool isStart = true;
    public EnemyManager em;
    public GameObject menu;
    public GameObject UI;

    [Header("Spray")]
    public GameObject sprayHitbox;
    public Transform sprayPoint;
    public float sprayStrength = 10f;
    public bool shook;

    [ContextMenu("Recenter")]
    void Recenter()
    {
        if (joycons != null && joycons.Count > index)
        {
            joycons[index].Recenter();
        }
    }

    void Start()
    {
        gyro = new Vector3(0, 0, 0);
        accel = new Vector3(0, 0, 0);
        // get the public Joycon array attached to the JoyconManager in scene
        joycons = JoyconManager.Instance.j;
        if (joycons.Count <= index)
        {
            Destroy(gameObject);
        }
    }

    void ShootSpray()
    {
        float charge = Mathf.Clamp(shakeMeter / 100f, 0.1f, 1f);
        float sizeMultiplier = Mathf.Lerp(0.5f, 2.5f, charge);
        float speedMultiplier = Mathf.Lerp(5f, 25f, charge);
        Joycon j = joycons[index];
        j.SetRumble(160, 320, 0.6f, 200);
        GameObject hitbox = Instantiate(sprayHitbox, sprayPoint.position, sprayPoint.rotation);
        hitbox.transform.localScale *= sizeMultiplier;

        SprayHitbox hitboxScript = hitbox.GetComponent<SprayHitbox>();
            hitboxScript.SetCharge(charge);

        Rigidbody rb = hitbox.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = sprayPoint.forward * speedMultiplier * sprayStrength;
        }

       
    }

    // Update is called once per frame
    void Update()
    {
        // make sure the Joycon only gets checked if attached
        if (joycons.Count > index)
        {
            Joycon j = joycons[index];

            if (JoyconManager.Instance?.prefabLeft != null && model == null)
            {
                var prefab = j.isLeft ? JoyconManager.Instance.prefabLeft : JoyconManager.Instance.prefabRight;
                model = Instantiate(prefab, transform);
            }

            // GetButtonDown checks if a button has been pressed (not held)
            if (j.GetButtonDown(Joycon.Button.DPAD_UP) && shakeMeter >= 25)
            {
                if (PlayerHealth.isDead)
                {
                    string currentSceneName = SceneManager.GetActiveScene().name;
                    SceneManager.LoadScene(currentSceneName);
                    PlayerHealth.isDead = false;
                    j.SetRumble(0, 0, 0, 0);
                    return;
                }
                if (isStart)
                {
                    em.StartWaves();
                    menu.SetActive(false);
                    UI.SetActive(true);
                    isStart = false;
                    return;
                }
                if (!shook)
                {
                    shakeMeter = shakeMeter - 25f;
                    shook = true;
                    j.Recenter();
                    StartCoroutine(ShootDelay());
                }
            }

            IEnumerator ShootDelay(){
               
                yield return new WaitForSeconds(timeBetween);
                ShootSpray();
                shook = false;
            }

            
            // GetButtonDown checks if a button has been released
            if (j.GetButtonUp(Joycon.Button.SHOULDER_2))
            {
            }
            // GetButtonDown checks if a button is currently down (pressed or held)
            if (j.GetButton(Joycon.Button.SHOULDER_2))
            {
            }

            if (j.GetButtonDown(Joycon.Button.DPAD_DOWN))
            {
                Debug.Log("Rumble");

                // Rumble for 200 milliseconds, with low frequency rumble at 160 Hz and high frequency rumble at 320 Hz. For more information check:
                // https://github.com/dekuNukem/Nintendo_Switch_Reverse_Engineering/blob/master/rumble_data_table.md

                j.SetRumble(160, 320, 0.6f, 200);

                // The last argument (time) in SetRumble is optional. Call it with three arguments to turn it on without telling it when to turn off.
                // (Useful for dynamically changing rumble values.)
                // Then call SetRumble(0,0,0) when you want to turn it off.
            }

            // Gyro values: x, y, z axis values (in radians per second)
            gyro = j.GetGyro();

            // Accel values:  x, y, z axis values (in Gs)
            accel = j.GetAccel();
            Vector3 shake = accel - lastAccel;
            float shakeIntensity = shake.magnitude;
            if (shakeIntensity > shakeThreshold)
            {
                shakeMeter += shakeIntensity * Time.deltaTime * 150f;
            }

            shakeMeter = Mathf.Max(0, shakeMeter - shakeDecay * Time.deltaTime);
            shakeMeter = Mathf.Clamp(shakeMeter, 0, 100);

            lastAccel = accel;

            orientation = j.GetOrientation();

            gameObject.transform.rotation = orientation;
        }
    }
}

