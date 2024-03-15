using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semaforo : MonoBehaviour
{
    public GameObject luz;

    public Transform posVerde;
    public Transform posAmarilla;
    public Transform posRoja;

    private bool verde;
    private bool amarilloDesdeVerde;
    private bool amarilloDesdeRoja;
    private bool roja;


    private void Start()
    {
        roja = true;
    }

    void Update()
    {
        if(verde == true)
        {
            luz.transform.position = posVerde.position;
            luz.GetComponent<Light>().color = new Color32(61, 161, 27, 255);
            StartCoroutine(luzVerde());
            amarilloDesdeRoja = false;
        }

        if(amarilloDesdeVerde == true)
        {
            luz.transform.position = posAmarilla.position;
            luz.GetComponent<Light>().color = Color.yellow;
            StartCoroutine(luzAmarillaV());
            verde = false;
        }

        if(amarilloDesdeRoja == true)
        {
            luz.transform.position = posAmarilla.position;
            luz.GetComponent<Light>().color = Color.yellow;
            StartCoroutine(luzAmarillaR());
            roja = false;
        }

        if(roja == true)
        {
            luz.transform.position = posRoja.position;
            luz.GetComponent<Light>().color = Color.red;
            StartCoroutine(luzRoja());
            amarilloDesdeVerde = false;
        }
    }

    IEnumerator luzVerde()
    {
        yield return new WaitForSeconds(10);
        amarilloDesdeVerde = true;
    }

    IEnumerator luzAmarillaV()
    {
        yield return new WaitForSeconds(1);
        roja = true;
    }

    IEnumerator luzAmarillaR()
    {
        yield return new WaitForSeconds(1);
        verde = true;
    }

    IEnumerator luzRoja()
    {
        yield return new WaitForSeconds(10);
        amarilloDesdeRoja = true;
    }
}
