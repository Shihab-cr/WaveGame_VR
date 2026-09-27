using UnityEngine;

public class CheckCurrentHeldWeapon : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Weapon"))
        {
            playerInventory.AssignWeapon(col.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            playerInventory.UnAssignWeapon();
        }
    }
}
