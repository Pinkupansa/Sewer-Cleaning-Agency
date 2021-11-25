using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Manages all calculations made by an enemy
public class EnemyAI : MonoBehaviour
{
    //Target chased
    private Transform target;
    private Enemy enemy;    
    //Points to follow when not chasing
    private Vector2[] patrolPoints = new Vector2[0];
    public Path path;
    //Time between two path calculations
    private float pathCalculationPeriod = 0.5f;
    private float pathCalculationTimer;
    [SerializeField] private Transform groundCheck;
    private Rigidbody2D rb;
    //Used to time the attacks
    private Coroutine attackCoroutine = null;
    private Vector2 input;
    
    public Navmesh roomNavmesh;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        target = PlayerIdentity.instance.player.transform;
        RecalculatePath();
        pathCalculationTimer = pathCalculationPeriod;
        
    }
    private void Update()
    {
        input = Vector2.zero;
        if(target != null)
        {
            //If we have a target, we calculate a path to the target every pathCalculationTimer seconds
            if (pathCalculationTimer <= 0f)
            {
                RecalculatePath();
                
                pathCalculationTimer = pathCalculationPeriod;
            }
            else
            {
                pathCalculationTimer -= Time.deltaTime;
            }
        }
        else
        {
            target = PlayerIdentity.instance.player.transform;
        }
        
        ShowPath();
    }
    private void FixedUpdate()
    {
        if(enemy.CanMove)
        {
            rb.AddForce(input);
        }
        
    }
    //Movement method on ladders
    public void ClimbTo(Vector2 point, float speed)
    {
        input += (point - (Vector2)transform.position).normalized * speed;
    }
    //Movement method on ground
    public void MoveTowards(Vector2 point, float speed)
    {
        input += (point - (Vector2)transform.position).normalized * speed;
        
    }
    
    public void Jump(Vector2 jumpSpeed)
    {
        StartCoroutine(JumpCoroutine(jumpSpeed));
    }
    public void Attack()
    {
        rb.velocity = Vector2.zero;
        if(attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(AttackCoroutine());
        }
        
    }
    //Used to avoid vertical jumping stuck on a wall
    IEnumerator JumpCoroutine(Vector2 initialVelocity)
    {
        GetComponent<Rigidbody2D>().velocity = initialVelocity;
        yield return new WaitForSeconds(0.3f);
        if (initialVelocity.x != 0 && GetComponent<Rigidbody2D>().velocity.x == 0)
        {
            GetComponent<Rigidbody2D>().velocity += Vector2.right * Mathf.Sign(initialVelocity.x) * 2f;
        }
    }
    //Delay before attack
    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(GetEnemy().AttackPeriod);
        Collider2D attackHit = Physics2D.OverlapBoxAll((Vector2)transform.position, Vector2.one * enemy.AttackRange, 0).Where(x => x.transform == target).FirstOrDefault();
      
        if (attackHit != null)
        {
            IDamageable damageable = attackHit.GetComponentInParent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(enemy.Damages, (attackHit.transform.position-transform.position).normalized);
            }
        }
        attackCoroutine = null;
    }
    
    
    public Vector2[] CalculatePatrolPoints()
    {
        //Casts down rays o multiple points of fixed step to find the borders of the platform

        //Step for the directions
        float xStep = 1f;

        //Current x Coordinate of the point
        float x = 0f;

        bool foundRightPoint = false;
        bool foundLeftPoint = false;

        //Used to memorize last point if end of the platform is found
        Vector2 lastRightPoint = Vector2.zero;
        Vector2 lastLeftPoint = Vector2.zero;

        Vector2[] patrolPoints = new Vector2[2];

        //Point where the enemy is projected on the ground
        Vector2 groundPoint = Physics2D.RaycastAll((Vector2)transform.position, Vector2.down,Mathf.Infinity).Where(t => t.collider.tag == "Ground").FirstOrDefault().point;

        //yDistance to the ground
        float groundDist = Vector2.Distance((Vector2)transform.position, groundPoint);
        while ((!foundLeftPoint || !foundRightPoint) && x < 1000f)
        {

            //Casting diagonal rays left and right towards the points on the ground at the current x coordinate : the considered points will be the first ground hit points encountered
            RaycastHit2D[] _rightHit = Physics2D.RaycastAll((Vector2)transform.position, Vector2.right * x - Vector2.up * groundDist);
            RaycastHit2D[] _leftHit = Physics2D.RaycastAll((Vector2)transform.position, Vector2.left * x - Vector2.up * groundDist);
            Vector2 leftPoint = _leftHit.Where(t => t.collider.tag == "Ground").FirstOrDefault().point;
            Vector2 rightPoint = _rightHit.Where(t => t.collider.tag == "Ground").FirstOrDefault().point;
            
            if(x!=0f)
            {
                //Distance to the ground at this x coordinate
                float predictedDistance = Mathf.Sqrt(x * x + groundDist * groundDist);
                
                //for left and right, if the actual distance is not the predicted one, we've reached the end of the platform
                if(leftPoint != null && leftPoint != Vector2.zero)
                {
                    
                    if (!foundLeftPoint && Mathf.Abs(predictedDistance - Vector2.Distance(leftPoint, (Vector2)transform.position)) > 0.1f)
                    {

                        foundLeftPoint = true;
                        patrolPoints[0] = lastLeftPoint;
                    }
                }
                else
                {
                    foundLeftPoint = true;
                    patrolPoints[0] = lastLeftPoint;
                }
                if(rightPoint != null && rightPoint != Vector2.zero)
                {
                    if (!foundRightPoint && Mathf.Abs(predictedDistance - Vector2.Distance(rightPoint, (Vector2)transform.position)) > 0.1f)
                    {
                        foundRightPoint = true;
                        patrolPoints[1] = lastRightPoint;
                    }
                }
                else
                {
                    foundRightPoint = true;
                    patrolPoints[1] = lastRightPoint;

                }
                
            }
            lastLeftPoint = leftPoint;
            lastRightPoint = rightPoint;

            //increasing x to go to next points
            x += xStep;
        }


        
        return patrolPoints;
    }

    //Flips the sprite according to the rb velocity
    public void LookAtTarget()
    {
        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.y, transform.localScale.y, transform.localScale.z);
        }
        else if(rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(transform.localScale.y, transform.localScale.y, transform.localScale.z);
        }
    }
    
    //Asking the navmesh to find a path
    private void RecalculatePath()
    {
       
        path = PathFinder.FindPath(roomNavmesh.GetClosestNavpoint(transform.position),roomNavmesh.GetClosestNavpoint(target.position), roomNavmesh);
        
    }
    private void ShowPath()
    {
        for(int i = 0; i < path.Length; i++)
        {
            Debug.DrawLine(path[i].Start.Transform.position, path[i].End.Transform.position,Color.green);
        }
    }
    //Ground check
    public bool IsOnGround()
    {
        RaycastHit2D[] _hit = Physics2D.RaycastAll(groundCheck.position, Vector2.down, 0.01f);
        return _hit.Where(x => x.collider.tag == "Ground").Count() > 0;
    }
    public Transform GetTarget()
    {
        return target;
    }
    public Enemy GetEnemy()
    {
        return enemy;
    }
    
    
}
