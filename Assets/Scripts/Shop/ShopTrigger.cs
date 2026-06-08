using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private ShopItem[] stock;
    [SerializeField] private ShopUI shopUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            shopUI.OpenShop(stock);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            shopUI.CloseShop();
    }
}