using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "CosmoCast/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName = "Enemy";
    public Sprite sprite;
    public int maxHP = 30;
    public int attackDamage = 5;
}
