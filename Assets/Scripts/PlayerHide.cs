using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHide : MonoBehaviour
{
    public bool isHiding;

    //Temporary Hiding Color Change variables
    public SpriteRenderer sr;


    //Hiding Within Interactable Objects
    //public HideInteractable objHideScr;
    public Transform pos;
    void Start()
    {
        isHiding = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    //COLLISIONS



    //TRIGGERS
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CanHideIn")
        {
            isHiding = true;
            sr.color = Color.gray;
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CanHideIn")
        {
            isHiding = false;
            sr.color = Color.white;
        }

        if (collision.gameObject.tag == "CanHideInteractable")
        {
            isHiding = false;
            sr.color = Color.white;
        }
    }
}

