using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "CosmoCast/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName = "Enemy";
    public int maxHP = 30;
    public int attackDamage = 5;
    public int coinReward = 100;
}
