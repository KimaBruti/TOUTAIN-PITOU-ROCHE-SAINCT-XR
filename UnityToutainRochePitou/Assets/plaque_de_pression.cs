using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plaque_de_pression : MonoBehaviour
{
    public Material hit_color;
    public Material no_hit_color;
    public MeshRenderer indicateur;
    public GameObject declencheur;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject==declencheur)
        {
            indicateur.material=hit_color;
            return;
        }
        indicateur.material=no_hit_color;
    }

    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject==declencheur)
        {
            indicateur.material=no_hit_color;
        }
    }
}
