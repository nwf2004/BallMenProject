using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyGoTo : MonoBehaviour
{
    public GameObject mainEnemy;

    public float OldXPos = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = mainEnemy.transform.position;

        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z - mainEnemy.transform.rotation.z);

        OldXPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = mainEnemy.transform.position;

        if (OldXPos > transform.position.x)
        {
            transform.localScale = new Vector3(-5f, 5f, 5f);
        }
        else if (OldXPos < transform.position.x)
        {
            //rotate right
            transform.localScale = new Vector3(5f, 5f, 5f);
        }
        OldXPos = transform.position.x;

        transform.rotation = Quaternion.Euler(0, 0, 0 - mainEnemy.transform.rotation.z);
    }

}
    


