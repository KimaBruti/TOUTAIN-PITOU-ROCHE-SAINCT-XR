using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    [SerializeField] private Vector3 direction_laser;
    [SerializeField] private Transform depart_laser;
    [SerializeField] private Material hit_color;
    [SerializeField] private Material no_hit_color;
    [SerializeField] private MeshRenderer indicateur;
    [SerializeField] private Transform end;
    [SerializeField] private GameObject recepteur;
    [SerializeField] private int nb_door;

    void Update()
    {
		Ray ray=new Ray(depart_laser.position,direction_laser);
		RaycastHit hit;
		if(Physics.Raycast(ray,out hit, 100))
		{
            if(hit.transform.gameObject.tag=="Player")
            {
                return;
            }
            if(hit.transform.gameObject==recepteur)
            {
                indicateur.material=hit_color;
                end.position=hit.point;
                return;
            }
            room_manager.Instance.open_door(nb_door);
            indicateur.material=no_hit_color;
            end.position=hit.point;
        }
        else
        {
            indicateur.material=no_hit_color;
        }
    }
}
