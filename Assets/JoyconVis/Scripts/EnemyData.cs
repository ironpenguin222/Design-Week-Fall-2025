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
}
