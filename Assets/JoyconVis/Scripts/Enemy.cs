using System.Collections;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData data;
    private Transform player;
    private float currentHealth;
    public bool isHit;
    private Renderer rend;
    private Color originalColor;

    public TextMeshProUGUI nameText;
    private float bobSpeed = 2f;
    private float bobHeight = 0.2f;
    private Vector3 startPos;
    private bool stealing;
    public AudioSource audios;
    public void Initialize(EnemyData enemyData, Transform playerTransform)
    {
        data = enemyData;
        player = playerTransform;
        nameText.text = data.enemyName;
        audios.clip = data.entranceClip;
        
        currentHealth = data.health;

        Debug.Log(data.enemyName + currentHealth);

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

        if (stealing)
        {
            Vector3 newDir = new Vector3(-8f, 0f, -3f);
            transform.position += newDir * (data.speed * 0.5f) * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.TakeDamage(1);
            stealing = true;
            StartCoroutine(StealPizza());
            
        }
    }

    IEnumerator StealPizza()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (rend != null)
            StartCoroutine(FlashRed());
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
            
        StartCoroutine(Dying());
    }

    private IEnumerator Dying()
    {
        rend.enabled = false;
        transform.position = new Vector3 (50f, 50f, 0f);
        audios.Play(0);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private IEnumerator FlashRed()
    {
        rend.material.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        rend.material.color = originalColor;
    }
}
