using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Sanity : MonoBehaviour
{
    public bool inHorror;

    [SerializeField] Slider slider; //So the values show up on the slider

    [SerializeField]
    private FieldOfView FOV;

    public float sanity;
    public float maxSanity;

    public float sanityDrain;
    public float horrorDrain;
    public float originalDrain;


    public float restore;

    public float hitDrain;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip candyClip;
    public AudioClip hit1Clip;
    public AudioClip hit2Clip;
    public AudioClip heartbeatClip;

    public AudioSource sanitySource; //Heartbeat when low sanity
    public bool isLowSanity = false;

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
        //Debug.Log(sanity);
        slider.value = sanity / 100; //So the values show up on the slider

        //Heartbeat for low sanity
        if (sanity <= 40) //If you aren't sprinting but you are moving
        {
            isLowSanity = true;
        }
        else
        {
            isLowSanity = false;
        }
        if (isLowSanity)
        {
            if (!sanitySource.isPlaying)
            {
                sanitySource.Play();
            }
        }
        else
        {
            sanitySource.Stop();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Horror")
        {
            Debug.Log("Entered Area of Horror");
            inHorror = true;
            //Debug.Log(inHorror);

            sanityDrain = horrorDrain;
        }

        if (collision.gameObject.tag == "candy")
        {
            sanity += restore;
            audioSource.PlayOneShot(candyClip, .1f); //Play candy eating sound
            //Debug.Log("Health restored");
            Destroy(collision.gameObject); //Destroy the piece of candy

            if (sanity + restore > maxSanity) //So you can't go over the max sanity when you pick up a piece of candy too early
            {
                sanity = maxSanity;
            }
        }

        if (collision.gameObject.tag == "pileoflights")
        {
            FOV.totalFlashlights += 20.0f;
            Destroy(collision.gameObject); //Destroy the piece of candy
        }

        if (collision.gameObject.tag == "Sister")
        {
            SceneManager.LoadScene("EndScene");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Horror")
        {
            //Debug.Log("Exited Area of Horror");
            inHorror = false;
            //Debug.Log(inHorror);

            sanityDrain = originalDrain;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //Debug.Log("Hit by Enemy");
            sanity -= hitDrain;
            //play hit heartbeat
            audioSource.PlayOneShot(heartbeatClip, .5f);
        }
    }
}
