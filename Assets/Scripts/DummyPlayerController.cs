using UnityEngine;

public class DummyPlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private Camera playerCam;
    [SerializeField] private LayerMask weaponLayer;
    [SerializeField] private Transform weaponOffset;
    private PlayerInventory playerInventory;
    private GunVisual gunVisual;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
        gunVisual = GetComponent<GunVisual>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleInteraction();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            GameObject gun = playerInventory.GetCurrWeapon();
            if (gun != null)
            {
                gun.GetComponent<FireGun>()?.Fire();
            }
        }
    }
    private void HandleInteraction()
    {
        Collider[] weapons = Physics.OverlapSphere(transform.position, 5, weaponLayer);
        float minDist = Mathf.Infinity;
        GameObject closestWeapon = null;
        foreach(Collider weapon in weapons)
        {
            float dist = Vector3.Distance(transform.position, weapon.gameObject.transform.position);
            if (minDist > dist)
            {
                minDist = dist;
                closestWeapon = weapon.gameObject;
            }
        }
        if(closestWeapon != null)
        {
            playerInventory.AssignWeapon(closestWeapon);
            gunVisual.SetGun(closestWeapon);
        }
    }
    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // 1. Get the camera's directional vectors
        Vector3 camForward = playerCam.transform.forward;
        Vector3 camRight = playerCam.transform.right;

        // 2. Flatten the vectors on the XZ plane to prevent flying/sinking
        camForward.y = 0;
        camRight.y = 0;

        // 3. Normalize to ensure consistent speed regardless of camera angle
        camForward.Normalize();
        camRight.Normalize();

        // 4. Calculate the final movement direction based on player input
        Vector3 moveDirection = (camForward * verticalInput + camRight * horizontalInput).normalized;

        // 5. Apply movement directly to the world position
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // 6. CRITICAL FIX: Always rotate the player to face the camera's forward direction,
        // ignoring which movement keys are currently being pressed.
        if (camForward != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(camForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }
}

