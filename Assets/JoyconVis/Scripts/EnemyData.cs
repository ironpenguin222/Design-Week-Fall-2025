using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemies/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int tier = 1;
    public float health = 100f;
    public float speed = 2f;
    public float size = 1f;
    public Sprite sprite;
    public AudioClip entranceClip;
    public bool isBoss;
    public bool zigzag;
    public float zigzagAmplitude = 1f;
    public float zigzagFrequency = 2f;
}
