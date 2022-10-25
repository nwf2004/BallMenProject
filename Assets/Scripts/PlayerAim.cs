using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private FieldOfView fieldOfView;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 mousePosition = Input.mousePosition;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 vec = worldPosition;
        vec.z = 0f;

        Vector3 aimDir = (vec - transform.position).normalized;
        Debug.Log(aimDir);

        fieldOfView.SetAimDirection(aimDir);
        fieldOfView.SetOrigin(transform.position);
    }
}
