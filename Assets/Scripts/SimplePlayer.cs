using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayer : MonoBehaviour
{

    [SerializeField]
    private FieldOfView fieldOfView;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mPos = Input.mousePosition;
        mPos = Camera.main.ScreenToWorldPoint(mPos);
        RotateSelf(mPos);
        fieldOfView.SetAimDirection(transform.position.normalized);
        fieldOfView.SetOrigin(transform.position);
    }

    //Move the reticle to face the user mouse input at all times
    void RotateSelf(Vector3 mousePos)
    {
        Vector2 direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
        transform.right = direction;
    }
}
