using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollow : MonoBehaviour
{
    public float followSpeed;
    public Vector3 offset;
    public float maxDist;
    private Transform cammyCam;
    public bool useMouse;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cammyCam == null)
        {
            cammyCam = Camera.main.transform;
            return;
        }

        Vector3 myPos = (transform.position + offset);
        Vector3 targetPos = myPos;

        if (useMouse)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            targetPos = Vector3.Lerp(myPos, mousePos, 0.5f);
            if(Vector3.Distance(myPos, targetPos) > maxDist)
            {
                Vector3 fromOriginToObject = targetPos - myPos;
                fromOriginToObject *= maxDist / Vector3.Distance(myPos, targetPos);
                targetPos = myPos + fromOriginToObject;
            }
        }
        cammyCam.position = Vector3.Lerp(cammyCam.position, targetPos, followSpeed * Time.deltaTime);
    }

    void getCam()
    {

    }
}
