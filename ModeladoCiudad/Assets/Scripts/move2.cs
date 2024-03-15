using UnityEngine;
using System.Collections.Generic;

public class Move2 : MonoBehaviour
{
    public GameObject carPrefab; // Prefab del carro
    public GameObject[] carPrefabs;
    private int maxCarInstances = 100; // Máximo número de instancias de carros permitidas

    // Lista de carros instanciados
    private List<GameObject> carInstances = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        string data = TCPIPServerAsync.receivedData;
        if (data != null)
        {
            string[] agentDataList = data.Split('|');

            // Iterar sobre cada cadena de datos de agente en agentDataList
            foreach (string agentData in agentDataList)
            {
                // Dividir la cadena de datos del agente en sus componentes
                string[] agentComponents = agentData.Split(',');

                // Verificar si aún se pueden instanciar más carros
                if (carInstances.Count < maxCarInstances)
                {
                    // Obtener la dirección del agente
                    string direction = agentComponents[0];

                    // Obtener la posición inicial del agente
                    string initialPosition = agentComponents[1];

                    // Obtener la secuencia de movimientos del agente
                    string movementSequence = agentComponents[2];

                    // Crear una nueva instancia de carro con los valores obtenidos
                    InstantiateCar(direction, initialPosition, movementSequence);
                }
                else
                {
                    Debug.Log("Se ha alcanzado el límite máximo de instancias de carros.");
                }
            }
        }

        // Actualizar el movimiento de los carros en cada frame
    }

    // Función para instanciar un vehículo con la dirección, posición inicial y secuencia de movimientos dados
    private void InstantiateCar(string direction, string initialPosition, string movementSequence)
    {
        // Determinar la posición inicial según la dirección
        Vector3 position;
        Quaternion rotation = Quaternion.identity;
        if (direction == "1")
        {
            position = new Vector3(19/2, 0, - (int.Parse(initialPosition)) + 20) ;
            rotation = Quaternion.Euler(0, 180, 0); // Rotar 90 grados en el eje Y
        }
        else if (direction == "2")
        {
            position = new Vector3(- (int.Parse(initialPosition)) + 20, 0, 19/2);
            rotation = Quaternion.Euler(0, 270, 0); // Rotar 90 grados en el eje Y
        }
        else
        {
            position = Vector3.zero;
        }
        int randomIndex = Random.Range(0, carPrefabs.Length);
        // Instanciar el carro en la posición inicial con la rotación adecuada
        GameObject car = Instantiate(carPrefabs[randomIndex], position, rotation);

        // Asignar la secuencia de movimientos al componente adecuado del carro (si tienes uno)
        // Por ejemplo, si tienes un componente llamado CarMovement que maneja el movimiento del carro, podrías asignarle la secuencia de movimientos aquí
        if (car.GetComponent<CarMovement>() != null)
        {
            car.GetComponent<CarMovement>().SetMovementSequence(movementSequence);
            car.GetComponent<CarMovement>().SetInitialPosition(initialPosition);
            car.GetComponent<CarMovement>().SetDirection(direction);
            UpdateCarMovement();
        }

        // Agregar el carro a la lista de carros instanciados
        carInstances.Add(car);
    }

    // Función para actualizar el movimiento de los carros en cada frame
    private void UpdateCarMovement()
    {
        foreach (GameObject car in carInstances)
        {
            // Obtener el componente de movimiento del carro
            CarMovement carMovement = car.GetComponent<CarMovement>();
            if (carMovement != null)
            {
                // Actualizar el movimiento del carro
                carMovement.Update();
            }
        }
    }
}
