using System.Collections;
using UnityEngine;

public class SprayHitbox : MonoBehaviour
{
    private float lifetime = 0.5f;
    public float shakeAmount;
    private float enemiesHit;
    public float pierce = 1;
    float damageMultiplier;

    public void SetCharge(float charge)
    {
        float normalized = Mathf.Clamp01(charge);
        damageMultiplier = Mathf.Lerp(35f, 150f, normalized);
        lifetime = Mathf.Lerp(0.1f, 1.5f, normalized);
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && !enemy.isHit)
            {
                enemy.TakeDamage(30);
                enemy.isHit = true;
                enemiesHit++;
                Debug.Log(enemiesHit);
                if (enemiesHit > pierce)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy.isHit)
            {
                enemy.isHit = false;
            }
        }
    }
}
