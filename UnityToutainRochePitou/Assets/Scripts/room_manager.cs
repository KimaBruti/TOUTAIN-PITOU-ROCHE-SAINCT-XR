using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class room_manager : MonoBehaviour
{
    [SerializeField] private List<GameObject> doors=new List<GameObject>();
    [SerializeField] private List<GameObject> sols=new List<GameObject>();
    public static room_manager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance=this;
    }

    public void open_door(int nb)
    {
        Animator door_anim=doors[nb].GetComponent<Animator>();
        if(nb!=0)
        {
            //sols[nb].AddComponent<>();
        }
        door_anim.SetTrigger("open");
    }
}
