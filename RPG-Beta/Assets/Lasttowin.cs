using UnityEngine;
using System.Collections;

public class Lasttowin : MonoBehaviour {
    public string Scenename;
    public GameObject SmallEmery;
    public GameObject Splider;


    public GameObject player;
    
    void Start()
    {
        // OverBag.gameObject.SetActive(false);
    }

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        SmallEmery = GameObject.Find("FieldGoblinC");
        Splider = GameObject.Find("SpiderC");
        if (SmallEmery == null && Splider == null)
        {
            Destroy(player);
          player. GetComponent<UiMasterC>().DestroyAllUi();
            Application.LoadLevel(Scenename);
        }

    }
}
