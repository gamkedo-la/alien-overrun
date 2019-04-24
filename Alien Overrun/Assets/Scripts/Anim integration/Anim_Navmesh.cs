using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Anim_Navmesh : MonoBehaviour
{
    static Animator anim;
     NavMeshAgent agent;
    
    void Start()
    {
        anim = GetComponent<Animator>();  
        agent = this.GetComponent<NavMeshAgent>();  
    }

    // Update is called once per frame
    void Update()
    {
        var translation = 1;
        var speed = agent.velocity.magnitude;
        anim.SetFloat("Speed", speed);

        if(speed >=0.001 )
        {
            anim.SetBool("isMoving", true);
         }
        else
        {
            anim.SetBool("isMoving", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("Attack");
        }

    }
}
