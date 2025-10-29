using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    public float fadeDuration = 1f;
    public GameObject DeathScreen;
    public Image deathImage;

    void Start()
    {
            deathImage.color = new Color(1f, 1f, 1f, 0f);
    }
    public void DeathTime()
    {
        StartCoroutine(DeathSequence());
    }
    public IEnumerator DeathSequence()
    {
        DeathScreen.SetActive(true);
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
                deathImage.color = new Color(1f, 1f, 1f, t);
            yield return null;
        }

        yield return new WaitForSeconds(2);

        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = 1f - Mathf.Clamp01(elapsed / fadeDuration);
                deathImage.color = new Color(1f, 1f, 1f, t);
            yield return null;
        }
    }
}
