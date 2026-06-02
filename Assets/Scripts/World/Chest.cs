using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] public GameObject chestPrefab;
    

    void Start()
    {
        
    }

    void Update()
    {

    }

    // 0 for common 1 for rare 2 for epic 3 for legendary
    int rarity(int chance)
    {
        if (chance == 1)
        {
            return 0;
        }
        // if (1/random thing)
        // {
        //     return chance/10;
        // }
        // else
        // {
        //     return rarity(chance/10);
        // }
        return 0;
    }
}
