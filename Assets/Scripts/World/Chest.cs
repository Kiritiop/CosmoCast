using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private Item[] possibleItems;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Item template = possibleItems[Random.Range(0, possibleItems.Length)];
            Item newItem = Instantiate(template);
            newItem.Rarity = GetRarity(1000);
            InventoryManager.Instance.AddItem(newItem);
            Destroy(gameObject);
        }
    }

    private string GetRarity(int chance)
    {
        if (chance == 1)
            return "Common";

        if (Random.Range(0, chance) == 0)
        {
            if (chance == 1000) return "Legendary";
            if (chance == 100)  return "Epic";
            if (chance == 10)   return "Rare";
        }

        return GetRarity(chance / 10);
    }
}