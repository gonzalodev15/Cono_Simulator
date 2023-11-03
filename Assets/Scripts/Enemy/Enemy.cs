using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    //public EnemyType enemyType;
    public float speed = 0.8f;
    private bool isPatrolling = false;
    public bool enemyHit = false;
    public float enemyScore = 0;
    private Vector3 originalPosition;
    public Transform startPatrolPoint;
    public Transform endPatrolPoint;
    private Transform currentPatrolPoint;
    [SerializeField] private Animator enemyAnimator = null;
    public GameObject enemyParticles;
    [SerializeField] float enemyLife = 3;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        //currentPatrolPoint = startPatrolPoint;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            if (transform.position != originalPosition && isPatrolling == false)
            {
                transform.LookAt(originalPosition);
                transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
            }

            if (transform.position == originalPosition)
            {
                isPatrolling = true;
                Idle();
            }

            if (isPatrolling == true)
            {
                /*if (Vector3.Distance(transform.position, currentPatrolPoint.position) < 0.5f)
                {
                    if (currentPatrolPoint == startPatrolPoint)
                    {
                        transform.LookAt(endPatrolPoint);
                        currentPatrolPoint = endPatrolPoint;
                    }
                    else if (currentPatrolPoint == endPatrolPoint)
                    {
                        transform.LookAt(startPatrolPoint);
                        currentPatrolPoint = startPatrolPoint;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        currentPatrolPoint.position,
                        speed * Time.deltaTime);
                }*/
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPatrolling = false;
            player = other.gameObject;
            if (!enemyHit)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                if (player.GetComponent<Player>().state == PlayerState.walking) {
                    Walk();
                } else if (player.GetComponent<Player>().state == PlayerState.running)
                {
                    SprintJump();
                }
            }
            else
            {
                transform.position = transform.position;
                StartCoroutine(EnemyAttacked());
            }
            transform.LookAt(other.gameObject.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = null;
            Walk();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            /*if (enemyType != EnemyType.drone)
            {
                bool hasInvincibility = player.GetComponent<Player>().hasInvincibility;
                bool hasInvincibilityShield = player.GetComponent<Player>().hasInvincibilityShield;
                player = collision.gameObject;
                print("Es invencible: " + hasInvincibility);
                if (!hasInvincibility)
                {
                    substractPlayerLife();
                }
                else if (hasInvincibility && hasInvincibilityShield)
                {
                    player.GetComponent<Player>().shutDownInvincibilityShield();
                }
            }*/
        }
    }

    /*private void substractPlayerLife()
    {
        switch (enemyType)
        {
            case EnemyType.robot:
                player.GetComponent<Player>().substractHealth(1.0f);
                break;
            case EnemyType.turret:
                player.GetComponent<Player>().substractHealth(2.0f);
                break;
            case EnemyType.drone:
                player.GetComponent<Player>().substractHealth(2.0f);
                break;
        }
    }*/


    /*public void gotHit()
    {
        enemyLife -= 1;
        enemyHit = true;
        if (enemyLife <= 0)
        {
            if (enemyType == EnemyType.robot)
            {
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<Rigidbody>().isKinematic = true;
                enemyAnimator.Play("EnemyDefeated");
            }
            else if (enemyType == EnemyType.turret)
            {

            }
            else if (enemyType == EnemyType.drone)
            {
                enemyAnimator.Play("DroneDefeated");
                gameObject.GetComponent<Turret>().fireRate = 0.01f;
                StartCoroutine(playParticlesForDefeatedEnemy());
            }

            ScoreManager.instance.addScore(enemyScore);
            foreach (Collider c in GetComponents<Collider>())
            {
                c.enabled = false;
            }
            StartCoroutine(destroyEnemy());
        }
    }*/

    IEnumerator destroyEnemy()
    {
        yield return new WaitForSeconds(1);
        Destroy(transform.parent.gameObject);
    }

    IEnumerator EnemyAttacked()
    {
        yield return new WaitForSeconds(1);
        enemyHit = false;
    }

    IEnumerator playParticlesForDefeatedEnemy()
    {
        yield return new WaitForSeconds(1f);
        GameObject particles = Instantiate(enemyParticles, transform.position, transform.rotation);
        Destroy(particles, 0.4f);
    }

    public void Idle()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyAnimator.SetBool("Walk", false);
        enemyAnimator.SetBool("SprintJump", false);
        enemyAnimator.SetBool("SprintSlide", false);
    }

    public void Walk()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyAnimator.SetBool("Walk", true);
        enemyAnimator.SetBool("SprintJump", false);
        enemyAnimator.SetBool("SprintSlide", false);
    }

    public void SprintJump()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyAnimator.SetBool("Walk", false);
        enemyAnimator.SetBool("SprintJump", true);
        enemyAnimator.SetBool("SprintSlide", false);
    }

    public void SprintSlide()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyAnimator.SetBool("Walk", false);
        enemyAnimator.SetBool("SprintJump", false);
        enemyAnimator.SetBool("SprintSlide", true);
    }
}