using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    private Rigidbody Rbd;
    [SerializeField] private float speed = 5;
    [SerializeField] private float distanceSeuil = 3;
    private Animator Animator;
    public bool canAttack;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Rbd = gameObject.GetComponent<Rigidbody>();
        Animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (!player) return;

        if (gameObject.tag == "Liche")
        {
            if (!gameObject.GetComponent<LicheBehaviour>().isSpelling)
            {
                Move();
            }
        }else { Move(); }
        
       
        

        transform.LookAt(player.transform);
    }

    void Move()
    {
        Vector3 direction = player.transform.position - transform.position;
        Vector3 direction2D = new Vector3(direction.x, 0, direction.z);
        if (direction2D.magnitude > distanceSeuil)
        {
            Vector3 Ndirection2D = direction2D.normalized;
            Rbd.MovePosition(transform.position + Ndirection2D * Time.deltaTime * speed);

            if (direction2D.magnitude > distanceSeuil + 1)
            {
                Animator.SetBool("ForwardSpeed", true);
                Animator.SetBool("onPlayerContact", false);
                canAttack = false;
            }
        } 
        else 
        {
            Animator.SetBool("ForwardSpeed", false);
            Animator.SetBool("onPlayerContact", true);
            canAttack = true;
        }
        
    }
}
