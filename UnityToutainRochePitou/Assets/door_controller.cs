using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door_controller : MonoBehaviour
{
    public List<GameObject> doors=new List<GameObject>();
    public static door_controller Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance=this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void open_door(int nb)
    {
        Animator door_anim=doors[nb].GetComponent<Animator>();
        door_anim.SetTrigger("open");
    }
}
