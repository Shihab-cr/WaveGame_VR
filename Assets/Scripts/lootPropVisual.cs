using UnityEngine;

public class lootPropVisual : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 20f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Time.deltaTime * rotationSpeed * Vector3.up);
    }
}
