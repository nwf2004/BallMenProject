using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sanity : MonoBehaviour
{
    public bool inHorror;


    public float sanity;
    public float sanityDrain;
    public float horrorDrain;
    public float originalDrain;

    //public float hitDrain;

    // Start is called before the first frame update
    void Start()
    {
        inHorror = false;
    }

    // Update is called once per frame
    void Update()
    {
        sanity -= sanityDrain * Time.deltaTime;
        Debug.Log(sanity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Horror")
        {
            Debug.Log("Entered Area of Horror");
            inHorror = true;
            Debug.Log(inHorror);

            sanityDrain = horrorDrain;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Horror")
        {
            Debug.Log("Exited Area of Horror");
            inHorror = false;
            Debug.Log(inHorror);

            sanityDrain = originalDrain;
        }
    }
}
