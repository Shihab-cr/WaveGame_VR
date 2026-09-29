using UnityEngine;
using UnityEngine.InputSystem;

public class HandlePlayerMovementInput : MonoBehaviour
{
    [SerializeField] private InputActionReference movementInput;
    private float timer = 0f;
    private float baseCoolDown = 0.3f;
    private FootStepsSFX_Handler footStepsHandler;
    private float inputMag;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        footStepsHandler = GetComponent<FootStepsSFX_Handler>();
        inputMag = movementInput.action.ReadValue<Vector2>().magnitude;
        
    }

    // Update is called once per frame
    void Update()
    {
        inputMag = movementInput.action.ReadValue<Vector2>().magnitude;

        if (inputMag> 0.1f)
        {
            if (timer < Time.time)
            {
                timer = Time.time + (baseCoolDown / inputMag);
                footStepsHandler.PlayFootSteps();
            }
            
          
        }
    }
}
