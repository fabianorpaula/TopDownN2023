using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Heroi : MonoBehaviour
{
    private NavMeshAgent Agente;
    private Vector3 Destino;
    
    void Start()
    {
        Destino = new Vector3(0, 0, 0);
        Agente = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousepoint = Input.mousePosition;
            Ray pontodesaida = Camera.main.ScreenPointToRay(mousepoint);
            RaycastHit localTocou;
            if (Physics.Raycast(pontodesaida, out localTocou, Mathf.Infinity))
            {
                Destino = localTocou.point;
            }
        }

        Agente.SetDestination(Destino);
    }
}
