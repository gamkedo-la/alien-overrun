using UnityEngine;
using UnityEngine.AI;

public class AnimNavmesh : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;
    private MeleeAttacker attacker;
	private bool isMoving = true;

	void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
		attacker = GetComponent<MeleeAttacker>();
    }

    void Update()
    {
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
    }

	public void IsMoving( bool isMoving )
	{
		this.isMoving = isMoving;
	}
}
