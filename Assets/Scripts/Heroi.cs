using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Heroi : MonoBehaviour
{
    private NavMeshAgent Agente;
    private Vector3 Destino;
    public GameObject MeuAtaque;
    private Animator ControlAnim;
    private GerenciadorDeObjetos Inventario;

    private bool podePegar = false;
    public float hp = 50;
    public bool vivo = true;
    void Start()
    {
        Inventario = GameObject.FindGameObjectWithTag("Inventario").GetComponent<GerenciadorDeObjetos>();
        Destino = new Vector3(0, 0, 0);
        Agente = GetComponent<NavMeshAgent>();
        ControlAnim = GetComponent<Animator>();
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
                if(localTocou.collider.gameObject.tag == "Inimigo")
                {
                    Agente.stoppingDistance = 2;
                    Destino = localTocou.transform.position;
                    
                }
                else
                {
                    Agente.stoppingDistance = 0;
                    Destino = localTocou.point;
                }
                
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Destino = transform.position;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ControlAnim.SetTrigger("Pegar");
        }
            Agente.SetDestination(Destino);

        ControleAtaque();
        Mover();
    }


    void Mover()
    {
       
        if(Agente.velocity.magnitude > 1)
        {
            ControlAnim.SetBool("Andar", true);
        }
        else
        {
            ControlAnim.SetBool("Andar", false);
        }
    }

    void ControleAtaque()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.X))
        {
            ControlAnim.SetTrigger("Ataque");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ControlAnim.SetTrigger("Disparo");
        }





    }
    public void AtivarAtk()
    {
        MeuAtaque.SetActive(true);
    }

    public void DesativarAtk()
    {
        MeuAtaque.SetActive(false);
    }

    public void AtivarPegada()
    {
        podePegar = true;
    }

    public void DesativarPegada()
    {
        podePegar = false;
        
    }


    void AtkDistancia()
    {
        RaycastHit meuAtkD;
        if (Physics.Raycast(MeuAtaque.transform.position, transform.forward, out meuAtkD, 10f))
        {
            
            if(meuAtkD.collider.gameObject.tag == "Inimigo")
            {
                meuAtkD.collider.gameObject.GetComponent<Inimigo>().TomeiDano();
            }

        }
    }


    private void OnTriggerStay(Collider colidiu)
    {
        if (colidiu.gameObject.tag == "Pegavel")
        {
            if (podePegar == true)
            {
                string nomeObj = colidiu.gameObject.GetComponent<Item>().Nome;
                Sprite imgObj = colidiu.gameObject.GetComponent<Item>().imagemObj;
                if (Inventario.ReceberItem(nomeObj, imgObj) == true)
                {
                    Destroy(colidiu.gameObject);
                }
                else
                {
                    //faz nada
                }


                podePegar = false;
            }

        }

       
    }

    private void OnTriggerEnter(Collider colidiu)
    {
        if (colidiu.gameObject.tag == "AtkInimigo")
        {
            if(vivo == true)
            {
                hp--;
                ControlAnim.SetTrigger("TomouDano");
                if (hp <= 0)
                {

                    Morrer();
                }
            }
            
        }

    }


    public void Morrer()
    {
        vivo = false;
        ControlAnim.SetBool("Morreu", true);
    }

    public void AumentarHP(float vidaPlus)
    {
        hp = hp + vidaPlus;
    }

}
