using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sanity : MonoBehaviour
{
    public bool inHorror;


    public float sanity;
    public float maxSanity;

    public float sanityDrain;
    public float horrorDrain;
    public float originalDrain;

    public float restore;

    public float hitDrain;

    // Start is called before the first frame update
    void Start()
    {
        inHorror = false;
        sanity = maxSanity;
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

        if (collision.gameObject.tag == "candy")
        {
            sanity += restore;
            Debug.Log("Health restored");
            Destroy(collision.gameObject); //Destroy the piece of candy

            if (sanity + restore > maxSanity) //So you can't go over the max sanity when you pick up a piece of candy too early
            {
                sanity = maxSanity;
            }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit by Enemy");
            sanity -= hitDrain;
        }
    }
}
