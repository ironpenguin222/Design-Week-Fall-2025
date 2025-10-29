using System.Collections;
using UnityEngine;

public class SprayHitbox : MonoBehaviour
{
    private float lifetime = 0.5f;
    public float shakeAmount = 50f;
    private float enemiesHit;
    public float pierce = 1;

    void Start()
    {
        float damageMultiplier = Mathf.Lerp(50f, 200f, shakeAmount);
        lifetime = Mathf.Lerp(0.1f, 1.5f, shakeAmount);
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(shakeAmount);
                enemiesHit++;
                Debug.Log(enemiesHit);
                if (enemiesHit > pierce)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
