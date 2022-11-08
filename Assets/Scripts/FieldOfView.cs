using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Material lightMaterial;
    [SerializeField] private Material brightLightMaterial;

    [Header("Cone Properties")]
    public float viewDistance = 50.0f;
    public float fov;
    public float targetFov;
    public int rayCount;

    [Header("Throwing Flashlights")]
    public GameObject flashLight;
    public float shootSpeed;
    public float firingSpeed;
    private GameObject newflashLight;
    public bool flashlightReady = true;
    public float totalFlashlights = 20.0f;

    private Mesh mesh;
    private MeshRenderer myRend;
    private Vector3 origin = Vector3.zero;
    public float startingAngle;

    public Transform personalView;

    public GameObject coneObj;
    public LayerMask walls;

    [Header("Audio")]
    public AudioSource throwClip;

    public string EnemyTag = "Enemy";
    private bool isAttacking = false;
    [SerializeField]
    private flashlightScript FlashLightScript;
    public Text UIdisplay;

    // Start is called before the first frame update
    private void Start()
    {
        mesh = new Mesh();
        coneObj.GetComponent<MeshFilter>().mesh = mesh;
        myRend = coneObj.GetComponent<MeshRenderer>();
        origin = Vector3.zero;
    }

    private void Update()
    {
        CheckInputForAttack();
        UIdisplay.text = "x " + totalFlashlights.ToString();
    }

    private void LateUpdate()
    {
        fov = targetFov;

        coneObj.transform.rotation = Quaternion.Euler(0, 0, 0);

        origin = transform.position;
        Vector2 origin2 = new Vector2(transform.position.x, transform.position.y);

        float angle = (transform.rotation.eulerAngles.z) + (fov / 2);
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero;
        //
        uv[0] = Vector2.zero;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            Vector3 rayAngle = GetVectorFromAngle(angle);

            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, rayAngle, viewDistance, layerMask);
            if (raycastHit2D.collider == null)
            {//No Hit
                vertex = GetVectorFromAngle(angle) * viewDistance;
                //Debug.DrawRay(origin, rayAngle.normalized * viewDistance, Color.red, 0.5f);
            }
            else
            {// Hit
                vertex = (raycastHit2D.point - origin2) + new Vector2(rayAngle.normalized.x, rayAngle.normalized.y);
                //Debug.DrawLine(origin, raycastHit2D.point, Color.red, 0.5f);
                //vertex = raycastHit2D.point;
            }

            if (FlashLightScript.flashlightIsOn)
            {
                RaycastHit2D scanRay = Physics2D.Raycast(origin, rayAngle, viewDistance, enemyMask);

                if (scanRay.collider != null)
                {

                    if (scanRay.collider.tag == EnemyTag)
                    {
                        scanRay.collider.GetComponent<enemyLogic>().gotTarget(gameObject.transform); ;
                    }

                    Debug.DrawLine(origin, scanRay.point, Color.red, 0.1f);
                }
                else
                {
                    Debug.DrawRay(origin, rayAngle.normalized * viewDistance, Color.red, 0.1f);
                }

            }

            vertices[vertexIndex] = vertex;
            //
            uv[vertexIndex] = rayAngle;


            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            vertexIndex++;

            angle -= angleIncrease;
        }






        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    void CheckInputForAttack()
    {
        if (Input.GetMouseButton(0) && flashlightReady == true && !(totalFlashlights <= 0) )
        {
            throwClip.Play();
            totalFlashlights -= 1;
            newflashLight = Instantiate(flashLight);
            newflashLight.transform.position = transform.position;
            StartCoroutine(fireRate());
           
            /*Material[] materials = myRend.materials;
            materials[1] = brightLightMaterial;
            myRend.materials = materials;
            isAttacking = true;*/
        }
        else
        {
            /*Material[] materials = myRend.materials;
            materials[1] = lightMaterial;
            myRend.materials = materials;
            isAttacking = false;
            coneObj.GetComponent<MeshRenderer>().materials[1] = lightMaterial;*/
        }
    }

    public void SetOrigin(Vector3 Origin)
    {
        this.origin = Origin;
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = GetAngleFromVectorFloat(aimDirection) - fov / 2f;
    }

    public static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) { n += 360; }

        return n;
    }

    IEnumerator fireRate()
    {
        flashlightReady = false;
        yield return new WaitForSeconds(firingSpeed);
        flashlightReady = true;
    }
}


