using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "CosmoCast/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName = "Enemy";
    public int maxHP = 30;
    public int attackDamage = 5;
    public int coinReward = 100;

    // Sorts enemies ascending by maxHP using Array.Sort with a Comparison delegate.
    public static void SortByHP(EnemyData[] enemies)
    {
        Array.Sort(enemies, (a, b) => a.maxHP.CompareTo(b.maxHP));
    }

    // Sorts enemies alphabetically by name, then binary-searches for a match.
    // Returns the index if found, -1 otherwise.
    public static int FindByName(EnemyData[] enemies, string targetName)
    {
        Array.Sort(enemies, (a, b) =>
            string.Compare(a.enemyName, b.enemyName, StringComparison.OrdinalIgnoreCase));

        return BinarySearchByName(enemies, targetName, 0, enemies.Length - 1);
    }

    // Recursive binary search on a name-sorted EnemyData array.
    private static int BinarySearchByName(EnemyData[] enemies, string target, int low, int high)
    {
        if (low > high) return -1;

        int mid = (low + high) / 2;
        int cmp = string.Compare(enemies[mid].enemyName, target, StringComparison.OrdinalIgnoreCase);

        if (cmp == 0)  return mid;
        if (cmp < 0)   return BinarySearchByName(enemies, target, mid + 1, high);
                       return BinarySearchByName(enemies, target, low, mid - 1);
    }
}
