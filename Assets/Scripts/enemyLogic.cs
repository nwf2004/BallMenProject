using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Events;

public class enemyLogic : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public Transform[] pathPositions;
    public Transform nextPos;
    public Transform firstStartPos;
    public int currentPos = 0;
    public float pathUpdateRate;
    public float moveSpeed;
    public float turnSpeed;
    public float nextWaypointDistance;
    public float stopDistance;
    public PlayerHide plyHide;
    Path myPath;
    int currentWaypoint = 0;
    public bool reachedEnd;
    Seeker mySeeker;
    Rigidbody2D myBody;
    public GameObject playerAimObject;

    [Header("Enemy Vision")]
    public float viewDist;
    public float fov;
    public int rayCount;
    public float visionUpdateRate;
    public bool hasLOSonTarget;
    public string targetTag = "Player";
    public LayerMask visibleLayers;
    private Vector3 origin;
    public float cullDist;
    public bool culled;

    /*[Header("Enemy Hearing")]
    public float hearDist;
    public bool requiresSprinting;
    [Header("Attacking")]
    public Vector2 attackRange;
    public bool needsLOS;
    public List<string> attackTags;
    public Transform attackRangeVis;
    public Transform hearingRangeVis;
    public bool recharging;
    public List<enemyAttack> myAttacks;*/


    [Header("Events")]
    public UnityEvent newTarget;
    public UnityEvent targetInAttackRange;
    private Transform player;
    [HideInInspector]
    public bool alerted;


    // Start is called before the first frame update
    void Start()
    {
        nextPos = pathPositions[currentPos];
        mySeeker = GetComponent<Seeker>();
        myBody = GetComponent<Rigidbody2D>();
        InvokeRepeating("pathUpdate", pathUpdateRate, pathUpdateRate);
        InvokeRepeating("visionUpdate", visionUpdateRate, visionUpdateRate);
        //visionUpdate();
        /*hearingRangeVis.transform.position = transform.position;
        hearingRangeVis.localScale = Vector2.one * (hearDist * 2);
        recharging = false;*/

        Quaternion randRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Random.Range(0, 360));
        transform.rotation = randRotation;

        playerAimObject = GameObject.FindGameObjectWithTag("PlayerAim");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        plyHide = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHide>();
        InvokeRepeating("distCheck", Random.Range(0, 5), 5);
        firstStartPos = target;
    }

    void pathUpdate()
    {
        if (culled)
            return;

        if (target != null && mySeeker.IsDone())
        {
            mySeeker.StartPath(transform.position, target.position, onReachTarget);
        }
    }

    void FixedUpdate()
    {
        if ((target.transform == playerAimObject.transform) && (plyHide.isHiding == true))
        {
            target = firstStartPos;
        }
        if (myPath == null || culled)
            return;

        float distToTarget = Vector2.Distance(myBody.position, myPath.vectorPath[myPath.vectorPath.Count - 1]);
        if (target != null)
        {
            distToTarget = Vector2.Distance(myBody.position, target.position);
        }


        if (currentWaypoint > myPath.vectorPath.Count - 1 || distToTarget < stopDistance)
        {
            reachedEnd = true;
            //Debug.Log(target.transform);
            if (target.transform != playerAimObject.transform)
            {
                NextPos();
                target = nextPos;
            }
            if ((target.transform == playerAimObject.transform) && (plyHide.isHiding == true))
            {
                target = firstStartPos;
            }
        }
        else
        {
            reachedEnd = false;

            Vector2 direction = ((Vector2)myPath.vectorPath[currentWaypoint] - myBody.position).normalized;
            transform.right = Vector3.MoveTowards(transform.right, direction, turnSpeed * Time.deltaTime);
            if (transform.rotation.eulerAngles.y == 180)
            {
                Quaternion targetRot = Quaternion.Euler(0, 0, 180);
                transform.rotation = targetRot;
            }
            Vector2 moveForce = (direction * moveSpeed);

            myBody.AddForce(moveForce);

            float distance = Vector2.Distance(myBody.position, myPath.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }
        if(target != null)
        {
            Vector2 direction = ((Vector2)target.position - myBody.position).normalized;
            transform.right = Vector3.MoveTowards(transform.right, direction, turnSpeed * Time.deltaTime);
            if (transform.rotation.eulerAngles.y == 180)
            {
                Quaternion targetRot = Quaternion.Euler(0, 0, 180);
                transform.rotation = targetRot;
            }
        }
    }

    void visionUpdate()
    {
        if (culled)
            return;

        origin = transform.position;
        Vector2 origin2 = new Vector2(transform.position.x, transform.position.y);

        //Vision Checking
        if(viewDist > 0 && target == null)
        {
            hasLOSonTarget = false;

            float angle = (transform.rotation.eulerAngles.z) + (fov / 2);
            float angleIncrease = fov / rayCount;
            for (int i = 0; i <= rayCount; i++)
            {
                Vector3 rayAngle = vectorFromAngle(angle);
                RaycastHit2D scanRay = Physics2D.Raycast(origin, rayAngle, viewDist, visibleLayers);

                if (scanRay.collider != null)
                {
                    if (scanRay.collider.tag == targetTag && target == null)
                    {
                        gotTarget(scanRay.collider.transform);
                    }
                    else if(scanRay.collider.tag == targetTag)
                    {
                        hasLOSonTarget = true;
                    }

                    Debug.DrawLine(origin, scanRay.point, Color.red, visionUpdateRate);
                }
                else
                {
                    Debug.DrawRay(origin, rayAngle.normalized * viewDist, Color.red, visionUpdateRate);
                }
                angle -= angleIncrease;
            }
        }

        /*Hearing Checking
        if(hearDist > 0 && target == null)
        {
            Collider2D[] hearCheck = Physics2D.OverlapCircleAll(transform.position, hearDist, visibleLayers);
            
            if (hearCheck.Length > 0)
            {
                foreach (Collider2D colCheck in hearCheck)
                {
                    if (colCheck.tag == targetTag)
                    {
                        if(requiresSprinting)
                        {
                            if(colCheck.GetComponent<PlayerControl>().sprinting)
                            {
                                gotTarget(colCheck.transform);
                            }
                        }
                        else
                        {
                            gotTarget(colCheck.transform);
                        }
                    }
                }
            }
        }*/

        /*Attack checking
        if(target != null)
        {
            Vector2 checkBoxPoint = midPoint((myBody.position + (Vector2)(transform.right * attackRange.x)), myBody.position);
            attackRangeVis.position = checkBoxPoint;
            attackRangeVis.rotation = transform.rotation;
            attackRangeVis.localScale = attackRange;
            Collider2D[] attackCheck = Physics2D.OverlapBoxAll(checkBoxPoint, attackRange, transform.rotation.eulerAngles.z, visibleLayers);
            if(attackCheck.Length > 0)
            {
                foreach(Collider2D colCheck in attackCheck)
                {
                    if(attackTags.Contains(colCheck.tag))
                    {
                        if(needsLOS && hasLOSonTarget)
                        {
                            attack();
                            targetInAttackRange.Invoke();
                        }
                        else
                        {
                            attack();
                            targetInAttackRange.Invoke();
                        }
                    }
                }
            }
        }*/
    }

    /*void attack()
    {
        if (recharging)
            return;

        if (myAttacks.Count == 0)
            return;

        int attackNum = Random.Range(0, myAttacks.Count);
        enemyAttack currentAttack = myAttacks[attackNum];
        currentAttack.activate(transform);
        StartCoroutine(attackReset(currentAttack.initialDelay + currentAttack.attackCooldown, currentAttack.moveSpeedMod));
    }*/

    /*IEnumerator attackReset(float rechargeTime, float moveMod)
    {
        recharging = true;
        float initSpeed = moveSpeed;
        moveSpeed *= moveMod;
        yield return new WaitForSeconds(rechargeTime);
        moveSpeed = initSpeed;
        recharging = false;
    }*/

    void onReachTarget(Path thePath)
    {
        if(!thePath.error)
        {
            myPath = thePath;
            currentWaypoint = 0;
        }
    }

    public void gotTarget(Transform foundTarget)
    {
        newTarget.Invoke();
        target = foundTarget;
        if (!alerted)
        {
            //alertOthers();
        }
        culled = false;
    }

    /*public void alertOthers()
    {
        if (target == null)
            return;

        Collider2D[] alertCheck = Physics2D.OverlapCircleAll(transform.position, hearDist * 2);

        if (alertCheck.Length > 0)
        {
            foreach (Collider2D colCheck in alertCheck)
            {
                if (colCheck.tag == gameObject.tag)
                {
                    enemyLogic fellowEnemy = colCheck.GetComponent<enemyLogic>();
                    fellowEnemy.alerted = true;
                    fellowEnemy.gotTarget(target);
                }
            }
        }
    }*/

    public void getPlayer()
    {
        if (target != null)
            return;
        gotTarget(player);
    }

    public void moveToPos(Vector3 pos)
    {
        culled = false;
        mySeeker.StartPath(transform.position, pos, onReachTarget);
    }

    Vector3 vectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
    Vector2 midPoint(Vector2 v1, Vector2 v2)
    {
        Vector2 mid;
        mid = new Vector2((v1.x + v2.x) / 2, (v1.y + v2.y) / 2);
        return mid;
    }

    void distCheck()
    {
        if (target != null)
            return;

        if(Vector3.Distance(transform.position, player.position) > cullDist)
        {
            culled = true;
        }
        else
        {
            culled = false;
        }
    }

    void NextPos()
    {
        currentPos++;
        if (currentPos > pathPositions.Length - 1)
        {
            currentPos = 0;
            nextPos = pathPositions[currentPos];
        }
        else
        {
            nextPos = pathPositions[currentPos];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (target != null)
            return;

        if(collision.gameObject.tag == targetTag)
        {
            gotTarget(collision.transform);
        }
    }
}
