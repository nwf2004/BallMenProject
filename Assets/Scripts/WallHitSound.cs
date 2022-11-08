using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHitSound : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource hitWallClip;

    private bool destroyMe = false;
    // Start is called before the first frame update
    void Start()
    {
        hitWallClip.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyMe)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator waitForSeconds()
    {
        yield return new WaitForSeconds(2);
        destroyMe = true;
    }
}
