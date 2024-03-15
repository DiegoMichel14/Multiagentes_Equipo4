using UnityEngine;
using System.Collections;
public class CarMovement : MonoBehaviour
{
    // Variables públicas para almacenar la información del carro
    public string direction;
    public string initialPosition;
    public string movementSequence;
private bool isMoving = false; // Bandera para indicar si el carro está en movimiento
    public float moveSpeed = 1f; // Velocidad de movimiento del carro en metros por segundo
    //private string movementSequence; // Secuencia de movimientos del carro
    private int currentIndex = 0; // Índice actual en la secuencia

    private float elapsedTime = 0f;
    //private float timeBetweenMoves = 1f / 60f; // Tiempo entre movimientos para cumplir con aproximadamente 60 fps

    // Método para establecer la secuencia de movimientos del carro
    public void SetMovementSequence(string sequence)
    {
        movementSequence = sequence;
    }

    public void SetInitialPosition(string sequence)
    {
        initialPosition = sequence;
    }

    public void SetDirection(string sequence)
    {
        direction = sequence;
    }

    public void Update()
    {
        if (movementSequence != null && currentIndex < movementSequence.Length && !isMoving)
        {
            char move = movementSequence[currentIndex];

            // Mover el carro según el próximo movimiento
            if (move == '1')
            {
                MoveCarForward();
            }
            else if (move == '0')
            {
                StartCoroutine(IdleForOneSecond());
            }

            // Avanzar al siguiente movimiento
            currentIndex++;
        }
    }

    void MoveCarForward()
    {
        StartCoroutine(MoveToPosition(transform.position + transform.forward, 1f / moveSpeed));
    }

    IEnumerator IdleForOneSecond()
    {
        isMoving = true;
        yield return new WaitForSeconds(1f);
        isMoving = false;
    }

    IEnumerator MoveToPosition(Vector3 targetPosition, float timeToMove)
    {
        isMoving = true;
        float elapsedTime = 0f;
        Vector3 startingPos = transform.position;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(startingPos, targetPosition, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }

}