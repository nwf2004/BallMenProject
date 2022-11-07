using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thrownFlashlight : MonoBehaviour
{

    [SerializeField]
    private LayerMask whatIsDamageable;
    [SerializeField]
    private LayerMask Enemy;

    private FieldOfView PlayerAim;

    [SerializeField]
    private GameObject PlayerAimObject;

    private Rigidbody2D FlashLightRB;

    [SerializeField]
    private GameObject shatterParticle;

    //Speed of flashLight
    Vector2 shootVector;
    // Start is called before the first frame update
    void Start()
    {
        FlashLightRB = gameObject.GetComponent<Rigidbody2D>();
        PlayerAim = GameObject.Find("PlayerAim").GetComponent<FieldOfView>();
        PlayerAimObject = GameObject.FindGameObjectWithTag("PlayerAim");

        transform.rotation = PlayerAimObject.transform.rotation;
        shootVector = transform.right * PlayerAim.shootSpeed;

        StartCoroutine(travelTime());
        
    }

    // Update is called once per frame
    void Update()
    {
        FlashLightRB.velocity = shootVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If the object that the bullet collides with is 9 (enemy) send the attack details to that enemy
        if (collision.gameObject.layer == 7)
        {
            GameObject currentEnemy = collision.gameObject;
            Vector3 Direction = (currentEnemy.transform.position - transform.position).normalized;
            currentEnemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(Direction.x * 6000, Direction.y * 6000));
            Debug.Log(currentEnemy.GetComponent<enemyHealth>().enemyHP);
        }
        
        
            //Instantiate(bloodParticle, transform.position, bloodParticle.transform.rotation);
            Destroy(gameObject);
        
    }

    IEnumerator travelTime()
    {

        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
