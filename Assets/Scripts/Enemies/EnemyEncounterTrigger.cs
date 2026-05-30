using UnityEngine;

// Attach to any enemy GameObject with a 3D Trigger Collider.
// Assign the matching EnemyData ScriptableObject in the Inspector.
public class EnemyEncounterTrigger : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (BattleManager.Instance == null) return;

        BattleManager.Instance.StartBattle(enemyData, gameObject);
        GetComponent<Collider>().enabled = false;
    }
}
