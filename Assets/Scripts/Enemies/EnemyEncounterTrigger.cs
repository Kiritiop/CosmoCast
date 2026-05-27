using UnityEngine;

// Attach this to any enemy GameObject that has a 3D Collider set to "Is Trigger"
// (Use BoxCollider or SphereCollider — NOT Collider2D, the player uses CharacterController which is 3D)
public class EnemyEncounterTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        BattleManager.Instance?.StartBattle();

        // Disable this trigger so it doesn't fire again mid-battle
        GetComponent<Collider>().enabled = false;
    }
}
