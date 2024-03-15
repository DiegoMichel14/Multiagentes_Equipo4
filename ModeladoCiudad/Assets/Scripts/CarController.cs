using UnityEngine;

public class CarController : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verificar si la colisión es con otro carro
        if (collision.gameObject.CompareTag("Car"))
        {
            // Detener el movimiento del carro
            rb.velocity = Vector3.zero;
            // También puedes desactivar el Rigidbody para detener completamente el movimiento
            // rb.isKinematic = true;
        }
    }
}
