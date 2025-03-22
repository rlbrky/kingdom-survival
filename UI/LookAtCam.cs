using UnityEngine;
using UnityEngine.UI;

public class LookAtCam : MonoBehaviour
{
    private Transform target; // The character this health bar follows

    void Start()
    {
        target = transform.parent; // Assuming the health bar is a child of the target
    }

    void Update()
    {
        // Always face the camera
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

        // Optional: Adjust position slightly above the character's head
        transform.position = target.position + new Vector3(0, 2f, 0);
    }
}
