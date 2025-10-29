using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData data;
    private Transform player;
    private float currentHealth;

    private Renderer rend;
    private Color originalColor;

    private float bobSpeed = 2f;
    private float bobHeight = 0.2f;
    private Vector3 startPos;

    public void Initialize(EnemyData enemyData, Transform playerTransform)
    {
        data = enemyData;
        player = playerTransform;

        currentHealth = data.health;

        transform.localScale = transform.localScale * data.size;

        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;

        startPos = transform.position;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = data.sprite;
    }

    void Update()
    {
        if (player == null) return;
        if (PlayerHealth.isDead)
        {
            Destroy(gameObject);
        }

        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * data.speed * Time.deltaTime;

        transform.position = new Vector3(transform.position.x,
            startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight,
            transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.TakeDamage(1);

            Destroy(gameObject);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (rend != null)
            StartCoroutine(FlashRed());

        if (currentHealth <= 0)
            Destroy(gameObject);
    }

    private IEnumerator FlashRed()
    {
        rend.material.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        rend.material.color = originalColor;
    }
}
