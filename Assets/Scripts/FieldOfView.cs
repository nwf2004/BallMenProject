using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    [Header("Cone Properties")]
    public float viewDistance = 50.0f;
    public float fov;
    public float targetFov;
    public int rayCount;

    private Mesh mesh;
    private Vector3 origin = Vector3.zero;
    public float startingAngle;


    public GameObject coneObj;
    public LayerMask walls;

    // Start is called before the first frame update
    private void Start()
    {
        mesh = new Mesh();
        coneObj.GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero;
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
            }
            else
            {// Hit
                vertex = (raycastHit2D.point - origin2) + new Vector2(rayAngle.normalized.x, rayAngle.normalized.y);
                //vertex = raycastHit2D.point;
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
}


