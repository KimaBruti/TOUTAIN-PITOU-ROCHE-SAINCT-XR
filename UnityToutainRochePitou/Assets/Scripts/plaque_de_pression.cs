using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plaque_de_pression : MonoBehaviour
{
    [SerializeField] private Material hit_color;
    [SerializeField] private Material no_hit_color;
    [SerializeField] private MeshRenderer indicateur;
    [SerializeField] private GameObject declencheur;
    [SerializeField] private bool valid=false;
    [SerializeField] private List<plaque_de_pression> others=new List<plaque_de_pression>();
    [SerializeField] private MeshRenderer image;
    [SerializeField] private int nb_porte;
    [SerializeField] private Color valid_image_color;
    [SerializeField] private Color non_valid_image_color;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("salut");
        if(other.gameObject==declencheur)
        {
            valid=true;
            open_door();
            indicateur.material=hit_color;
            Material material = declencheur.GetComponent<Renderer>().material;
            material.SetColor("_EmissionColor", Color.green);
            if(others.Count!=0)
            {
                Material material_image = image.material;
                material_image.SetColor("_Color", valid_image_color);
            }
            return;
        }
        indicateur.material=no_hit_color;
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject==declencheur)
        {
            indicateur.material=no_hit_color;
            Material material = declencheur.GetComponent<Renderer>().material;
            material.SetColor("_EmissionColor", Color.red);
            if(others.Count!=0)
            {
                Material material_image = image.material;
                material_image.SetColor("_Color", non_valid_image_color);
            }
        }
    }

    void open_door()
    {
        if(others.Count!=0)
        {
            for(int i=0;i<others.Count;i++)
            {
                if(others[i].valid==false)
                {
                    return;
                }
            }
        }
        room_manager.Instance.open_door(nb_porte);
    }
}
