using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generation_laby : MonoBehaviour
{
    [System.Serializable]
    public struct Pair
    {
        public bool activate;
        public int nombre;

        public Pair(bool state, int nb)
        {
            this.activate = state;
            this.nombre = nb;
        }
    }
    [System.Serializable]
    public struct portail_paire
    {
        public int pos1;
        public int pos2;
        public bool is_horizontal1;
        public bool is_horizontal2;

        public portail_paire(int pos1, int pos2,bool is_horizontal1,bool is_horizontal2)
        {
            this.pos1 = pos1;
            this.pos2 = pos2;
            this.is_horizontal1=is_horizontal1;
            this.is_horizontal2=is_horizontal2;
        }
    }

    [SerializeField] private Pair portails_param = new Pair();
    [SerializeField] private int width=5;
    [SerializeField] private int height=5;
    private int sortie;
    [SerializeField] private GameObject mur;
    [SerializeField] private GameObject mur_sortie;
    [SerializeField] private GameObject portail;
    [SerializeField] private Transform p_h;
    [SerializeField] private Transform p_v;
    //[SerializeField] private Portal portail_foret;
    private List<int> cells=new List<int>();
    private bool[] murs_speciaux_hori;
    private bool[] murs_speciaux_verti;
    private bool[] murs_hori;
    private bool[] murs_verti;
    private Vector3 size_mur;
    //private Portal first_portail=null;
    private Vector3 depart;
    [SerializeField] private bool entree;
    public static generation_laby Instance {get;private set;}
    public Vector2 salle;
    public Vector2 salle_pos_hori;
    public Vector2 salle_pos_ver;
    public int nb_murs_hori;
    public int nb_murs_verti;

    private void Awake()
    {
        if(Instance==null)
        {
            Instance=this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        size_mur=mur.GetComponent<MeshRenderer>().bounds.size;
        //width=game_manager.Instance.laby_taille;
        height=width;
        start_creation();
    }

    // Update is called once per frame
    public void start_creation()
    {
        generate_maze();
        make_sortie();
        make_salles();
        make_specials_walls();
        instantiate_laby();
    }

    void generate_maze()
    {
        cells.Clear();
        for(int i=0;i<width*height;i++)
        {
            cells.Add(i);
        }
        nb_murs_hori=(height+1)*width;
        nb_murs_verti=(width+1)*height;
        murs_hori=new bool[nb_murs_hori];
        murs_verti=new bool[nb_murs_verti];
        murs_speciaux_hori=new bool[nb_murs_hori];
        murs_speciaux_verti=new bool[nb_murs_verti];
        for(int i=0;i<nb_murs_hori;i++)
        {
            murs_hori[i]=true;
            murs_speciaux_hori[i]=false;
        }
        for(int i=0;i<nb_murs_verti;i++)
        {
            murs_verti[i]=true;
            murs_speciaux_verti[i]=false;
        }
        int cpt=0;
        while(!labyrinthe_parfait())
        {
            //Debug.Log("_________"+cpt+"________");
            cpt+=1;
            for(int i=0;i<cells.Count;i++)
            {
                //Debug.Log(cells[i]);
            }
            bool horizontal=(0==Random.Range(0,2));
            int wall=0;
            int first_cell=0; //gauche et haut
            int second_cell=0;
            bool find=false;
            if(horizontal)
            {
                wall=get_random_wall_horizontal();
                if(wall==-1)
                {
                    continue;
                }
                first_cell=cells[wall-width];
                second_cell=cells[wall];
                if(first_cell==second_cell)
                {
                    continue;
                }
                else
                {
                    //Debug.Log("first: "+first_cell+" second: "+second_cell);
                    for(int i=0;i<cells.Count;i++)
                    {
                        if(cells[i]==second_cell)
                        {
                            cells[i]=first_cell;
                        }
                    }
                    murs_hori[wall]=false;
                    //Debug.Log("horizontal: "+wall);
                }
            }
            else
            {
                wall=get_random_wall_vertical();
                if(wall==-1)
                {
                    continue;
                }
                int ligne=wall/(width+1);
                first_cell=cells[wall-(ligne*1+1)];
                second_cell=cells[wall-ligne*1];
                if(first_cell==second_cell)
                {
                    continue;
                }
                else
                {
                    //Debug.Log("first: "+first_cell+" second: "+second_cell);
                    for(int i=0;i<cells.Count;i++)
                    {
                        if(cells[i]==second_cell)
                        {
                            cells[i]=first_cell;
                        }
                    }
                    //Debug.Log("vertical: "+wall);
                    murs_verti[wall]=false;
                }
            }
        }
        Debug.Log("generation end with iterations: "+cpt);
        for(int i=0;i<cells.Count;i++)
        {
            //Debug.Log(cells[i]);
        }
    }

    private void instantiate_laby()
    {
        int ligne=0;
        int colonne=0;
        int cpt_sw=0;
        depart=new Vector3(transform.position.x+(size_mur.z/2),transform.position.y+2,transform.position.z-(size_mur.z/2));
        for(int i=0;i<murs_hori.Length;i++)
        {
            colonne+=1;
            if(i%width==0)
            {
                ligne+=1;
                colonne=0;
            }
            if(murs_hori[i])
            {
                GameObject new_m=Instantiate(mur,new Vector3(transform.position.x+(ligne*size_mur.z),7,transform.position.z+(size_mur.z*colonne)),Quaternion.identity,p_h);
                new_m.GetComponent<mur_nb>().nb_mur=i;
            }
            if(murs_speciaux_hori[i])
            {
                /*
                if(first_portail==null)
                {
                    first_portail=Instantiate(portail,new Vector3(transform.position.x+(ligne*size_mur.z),7,transform.position.z+(size_mur.z*colonne)),Quaternion.identity,p_h).GetComponentInChildren<Portal>();
                }
                else
                {
                    Portal newp=Instantiate(portail,new Vector3(transform.position.x+(ligne*size_mur.z),7,transform.position.z+(size_mur.z*colonne)),Quaternion.identity,p_v).GetComponentInChildren<Portal>();
                    first_portail.Setother(newp);
                    newp.Setother(first_portail);
                    first_portail=null;
                }
                */
                    
            }
            if(sortie==i)
            {
                /*
                GameObject new_m=Instantiate(mur_sortie,new Vector3(transform.position.x+(ligne*size_mur.z),7,transform.position.z+(size_mur.z*colonne)),Quaternion.Euler(0,180,0),p_h);
                Portal portail_sortie=new_m.GetComponentInChildren<Portal>();
                portail_sortie.Setother(portail_foret);
                portail_foret.Setother(portail_sortie);
                */
            }
        }
        ligne=0;
        colonne=0;
        for(int i=0;i<murs_verti.Length;i++)
        {
            colonne+=1;
            if(i%(width+1)==0)
            {
                ligne+=1;
                colonne=0;
            }
            if(murs_verti[i])
            {
                GameObject new_m=Instantiate(mur,new Vector3(transform.position.x+(ligne*size_mur.z)+(size_mur.z/2),7,transform.position.z+(size_mur.z*colonne)-(size_mur.z/2)),Quaternion.Euler(0,90,0),p_v);
                new_m.GetComponent<mur_nb>().nb_mur=i;
            }
            if(murs_speciaux_verti[i])
            {
                /*
                if(first_portail==null)
                {
                    first_portail=Instantiate(portail,new Vector3(transform.position.x+(ligne*size_mur.z)+(size_mur.z/2),7,transform.position.z+(size_mur.z*colonne)-(size_mur.z/2)),Quaternion.Euler(0,90,0),p_h).GetComponentInChildren<Portal>();
                }
                else
                {
                    
                    Portal newp=Instantiate(portail,new Vector3(transform.position.x+(ligne*size_mur.z)+(size_mur.z/2),7,transform.position.z+(size_mur.z*colonne)-(size_mur.z/2)),Quaternion.Euler(0,90,0),p_v).GetComponentInChildren<Portal>();
                    first_portail.Setother(newp);
                    newp.Setother(first_portail);
                    first_portail=null;
                }
                */
            }
        }
    }

    private int get_random_wall_horizontal()
    {   
        int cpt=0;
        for(int i=0;i<murs_hori.Length;i++)
        {
            if(!murs_hori[i])
            {
                cpt+=1;
            }
        }
        if(cpt==murs_hori.Length)
        {
            return -1;
        }
        int wall=-1;
        do
        {
        int val=Random.Range(1*(height+1),murs_hori.Length-(height));
        if(murs_hori[val])
        {
            wall=val;
        }
        }while(wall==-1);
        return wall;
    }

    private int get_random_wall_vertical()
    {
        int cpt=0;
        for(int i=0;i<murs_verti.Length;i++)
        {
            if(!murs_verti[i])
            {
                cpt+=1;
            }
        }
        if(cpt==murs_verti.Length)
        {
            return -1;
        }
        int wall=-1;
        do
        {
        int val=Random.Range(0,murs_verti.Length);
        if((val+1)%(width+1)==0 || val%(width+1)==0)
        {
            continue;
        }
        else
        {
            if(murs_verti[val])
            {
                wall=val;
            }
        }
        }while(wall==-1);
        return wall;
    }

    private bool labyrinthe_parfait()
    {
        int first=cells[0];
        for(int i=1;i<cells.Count;i++)
        {
            if(cells[i]!=first)
            {
                return false;
            }
        }
        return true;
    }

    private void make_sortie()
    {
        sortie=0;
        if(entree)
        {
            murs_hori[sortie]=false;
        }
        sortie=Random.Range(murs_hori.Length-(width),murs_hori.Length);
        murs_hori[sortie]=false;
    }

    private void make_specials_walls()
    {
        if(portails_param.activate)
        {
            for(int i=0;i<portails_param.nombre*2;i++)
            {
                int wall=-1;
                do{
                    bool horizontal=(0==Random.Range(0,2));
                    if(horizontal)
                    {
                        wall=get_random_wall_horizontal();
                        if(wall==-1)
                        {
                            continue;
                        }
                        else
                        {
                            murs_speciaux_hori[wall]=true;
                        }
                    }
                    else
                    {
                        wall=get_random_wall_vertical();
                        if(wall==-1)
                        {
                            continue;
                        }
                        else
                        {
                            murs_speciaux_verti[wall]=true;
                        }
                    }
                }
                while(wall==-1);

            }
        }
    }

    void make_salles()
    {
        //verifier si on sort
        int deb_hor=Random.Range(0,nb_murs_hori);
        int end_hor=deb_hor+((int)salle.x*width);
        int deb_ver=deb_hor+((deb_hor/width));
        int end_ver=deb_ver+((int)salle.y*(width+1));
        salle_pos_hori=new Vector2(deb_hor,end_hor);
        salle_pos_ver=new Vector2(deb_ver,end_ver);
        Debug.Log("murs");
        for(int i=0;i<salle.x;i++)
        {
            Debug.Log(deb_hor+i);
            Debug.Log(end_hor+i);
            murs_hori[deb_hor+i]=true;
            murs_hori[end_hor+i]=true;
        }
        Debug.Log("murs");
        for(int i=0;i<salle.y;i++)
        {
            Debug.Log(deb_ver+(i*salle.y));
            Debug.Log(end_ver+(i*salle.y));
            murs_verti[deb_ver+(i*(int)salle.y)]=true;
            murs_verti[end_ver+(i*(int)salle.y)]=true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        bool[] ancien_murs_hori=murs_hori;
        bool[] ancien_murs_verti=murs_verti;
        generate_maze();
        foreach (Transform child in p_h) 
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in p_v) 
        {
            Destroy(child.gameObject);
        }
        instantiate_laby();
    }

    //code de Tom PITOU 
}
