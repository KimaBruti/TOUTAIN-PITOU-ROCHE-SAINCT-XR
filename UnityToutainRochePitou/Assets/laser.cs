using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    public Vector3 direction_laser;
    public Transform depart_laser;
    public Material hit_color;
    public Material no_hit_color;
    public MeshRenderer indicateur;
    public Transform end;
    public GameObject recepteur;
    public int nb_door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(depart_laser.position, direction_laser, Color.green);

        Ray ray =new Ray(depart_laser.position,direction_laser);
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
            door_controller.Instance.open_door(0);
            indicateur.material=no_hit_color;
            end.position=hit.point;
        }
        else
        {
            indicateur.material=no_hit_color;
        }
    }
}
