using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    public float enemyHP = 5;
    public LayerMask visionCone;
    public GameObject EnemySprite;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
            if (enemyHP <= 0.0f)
            {
            Destroy(EnemySprite);
                Destroy(gameObject);
            }
    }

    public float getEnemyHP()
    {
        return enemyHP;
    }

    public void setEnemyHP(float newValue)
    {
        enemyHP = newValue;
    }
}
