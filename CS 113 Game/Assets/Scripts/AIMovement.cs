using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    public Transform target;
    public float movespeed;
    public SpriteRenderer spriteRenderer;
    [SerializeField] private LayerMask layerMask;

    public float waitTime = 1f;
    public bool inChase = false;
    public bool inPhase = false;
    public bool permaChase = false;
    public bool pushed = false;
    public Vector3 pushedDestination;

    // Wander implementation
    float maxDistance;
    Vector3 wayPoint;
    public bool isWander = false;
    public float wanderSpeed;
    
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
        movespeed = GetComponent<Enemy>().movespeed;
        wanderSpeed = GetComponent<Enemy>().wanderSpeed;
        maxDistance = 4f;
    }

    void Update()
    {
        if (GameManager.instance.difficulty == "hard" && Time.timeScale == .1f) 
        {
            inChase = false;
            return;
        }

        if (movespeed == 0 && gameObject.name.Contains("dragon")) return;
        inChase = (!target.gameObject.GetComponent<Player>().isDead && (Vector3.Distance(transform.position, target.position) <= 4f || permaChase || inPhase)) ? true : false;

        if (waitTime > 0)
        {
            if (permaChase || pushed) waitTime = 0;
            else {
                inChase = false;
                waitTime -= Time.deltaTime;
            }
        }

        if (inChase && !pushed) GetComponent<Animator>().SetBool("walking", true); else GetComponent<Animator>().SetBool("walking", false);

        if (!inChase)
        {
            if (!isWander) Wander();
            else if (isWander && Vector2.Distance(transform.position, wayPoint) < 0.1f)
            {
                isWander = false;
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, wayPoint, wanderSpeed * Time.deltaTime);
                spriteRenderer.flipX = (wayPoint.x < transform.position.x) ? true : false;
            }
        }
        else if (!pushed && inChase) 
        {
            spriteRenderer.flipX = (target.transform.position.x < transform.position.x) ? true : false;

            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, target.position - transform.position, Vector3.Distance(transform.position, target.position), layerMask);
            RaycastHit2D feetRaycastHit2D = Physics2D.Raycast((transform.position-new Vector3(0,.4f,0)), target.position - (transform.position-new Vector3(0,.4f,0)), Vector3.Distance((transform.position-new Vector3(0,.4f,0)), target.position), layerMask);
            RaycastHit2D headRaycastHit2D = Physics2D.Raycast((transform.position+new Vector3(0,.4f,0)), target.position - (transform.position+new Vector3(0,.4f,0)), Vector3.Distance((transform.position+new Vector3(0,.4f,0)), target.position), layerMask);
            if (raycastHit2D.collider != null || feetRaycastHit2D.collider != null || headRaycastHit2D.collider != null)
            {
                List<GameObject> corners = new List<GameObject>();
                if (raycastHit2D.collider != null)
                    foreach(Transform child in raycastHit2D.collider.gameObject.transform) corners.Add(child.gameObject);
                else if (feetRaycastHit2D.collider != null)
                    foreach(Transform child in feetRaycastHit2D.collider.gameObject.transform) corners.Add(child.gameObject);
                else if (headRaycastHit2D.collider != null)
                    foreach(Transform child in headRaycastHit2D.collider.gameObject.transform) corners.Add(child.gameObject);
                float closestDistance = 12f;
                GameObject closestViableCorner = null;
                foreach(GameObject corner in corners)
                {
                    if (Physics2D.Raycast(transform.position, corner.transform.position - transform.position, Vector3.Distance(transform.position, corner.transform.position), layerMask).collider != null ||
                        Physics2D.Raycast((transform.position-new Vector3(0,.4f,0)), corner.transform.position - (transform.position-new Vector3(0,.4f,0)), Vector3.Distance((transform.position-new Vector3(0,.4f,0)), corner.transform.position), layerMask).collider != null || 
                        Physics2D.Raycast((transform.position+new Vector3(0,.4f,0)), corner.transform.position - (transform.position+new Vector3(0,.4f,0)), Vector3.Distance((transform.position+new Vector3(0,.4f,0)), corner.transform.position), layerMask).collider != null) continue;
                    if (Vector3.Distance(target.position, corner.transform.position) <= closestDistance)
                    {
                        closestDistance = Vector3.Distance(target.position, corner.transform.position);
                        closestViableCorner = corner;
                    }
                }
                if (closestViableCorner != null)
                {
                    transform.position = Vector2.MoveTowards(transform.position, closestViableCorner.transform.position, movespeed * Time.deltaTime);
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, target.position, movespeed * Time.deltaTime);
                }
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, movespeed * Time.deltaTime);
            }
        }

        // pushed logic
        else if (pushed && Vector3.Distance(transform.position, pushedDestination) >= 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, pushedDestination, (movespeed+1) * Time.deltaTime);
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
        else if (pushed && Vector3.Distance(transform.position, pushedDestination) < 0.1f)
        {
            pushed = false;
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    void Wander()
    {
        isWander = true;
        wayPoint = new Vector2(Random.Range(transform.position.x-maxDistance, transform.position.x+maxDistance), Random.Range(transform.position.y-maxDistance, transform.position.y+maxDistance));
    }

    void OnCollisionStay2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag != "Obstacle" && collision2D.gameObject.tag != "brick") return;
        GetComponent<BoxCollider2D>().isTrigger = false;
        Wander();
    }

    public void PushForce(Damage dmg)
    {
        if (!target.GetComponent<Player>().hasKnockBack) return;
        pushed = true;
        pushedDestination = transform.position + (transform.position - dmg.origin).normalized * dmg.pushForce;
    }
}
