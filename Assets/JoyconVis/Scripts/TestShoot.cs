using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[SelectionBase]
public class TestShoot : MonoBehaviour
{
    public float shakeMeter = 50.0f;

    public GameObject sprayHitbox;
    public Transform sprayPoint;
    public float sprayStrength = 10f;

    void ShootSpray()
    {
        float charge = Mathf.Clamp(shakeMeter / 100f, 0.1f, 1f);
        float sizeMultiplier = Mathf.Lerp(0.5f, 2.5f, charge);
        float speedMultiplier = Mathf.Lerp(5f, 25f, charge);

        GameObject hitbox = Instantiate(sprayHitbox, sprayPoint.position, sprayPoint.rotation);
        hitbox.transform.localScale *= sizeMultiplier;


        Rigidbody rb = hitbox.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = sprayPoint.forward * speedMultiplier * sprayStrength;
        }

        shakeMeter = Mathf.Max(0, shakeMeter - 100f * charge);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ShootSpray();
    }
}
