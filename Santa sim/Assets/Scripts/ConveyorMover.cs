using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ConveyorMover : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.forward; // local direction for belt movement
    public float speed = 1f;
    public bool move = true;

    Rigidbody rb;
    Present present;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        present = GetComponent<Present>();
        // Use kinematic movement to avoid physics instability
        rb.isKinematic = true;
    }

    void Update()
    {
        if (present != null && present.isBeingDragged)
            return;

        if (!move) return;

        transform.position += transform.TransformDirection(moveDirection.normalized) * speed * Time.deltaTime;
    }
}
