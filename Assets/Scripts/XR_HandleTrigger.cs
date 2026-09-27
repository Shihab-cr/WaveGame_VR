using UnityEngine;
using UnityEngine.InputSystem;

public class XR_HandleTrigger : MonoBehaviour
{
    [SerializeField] private InputActionReference actionValue;
    [SerializeField] private PlayerInventory playerInventory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (actionValue.action.WasPressedThisFrame())
        {
            HandleGunFire();
        }
        if(actionValue.action.ReadValue<float>() > 0.5f)
        {
            HandleGunFire();
        }
    }
    private void OnEnable()
    {
        if (actionValue != null) actionValue.action.Enable();
    }
    private void OnDisable()
    {
        if (actionValue != null) actionValue.action.Disable();
    }
    private void HandleGunFire()
    {
        if (playerInventory == null) return;
        else
        {
            if (playerInventory.GetCurrWeapon() == null) return;
            else
            {
                FireGun gun = playerInventory.GetCurrWeapon().GetComponent<FireGun>();
                gun.Fire();
            }
        }
    }
}
