using UnityEngine;
using UnityEngine.AI;

public class AnimNavmesh : MonoBehaviour
{
	private Animator anim;
	private NavMeshAgent agent;
	private MeleeAttacker attacker;
	private bool isMoving = true;

	void Start( )
	{
		anim = GetComponent<Animator>( );
		agent = GetComponent<NavMeshAgent>( );
		attacker = GetComponent<MeleeAttacker>( );
	}

	void Update( )
	{
		if ( isMoving )
			anim.SetFloat( "Speed", agent.velocity.magnitude );
	}

	public void IsMoving( bool isMoving )
	{
		this.isMoving = isMoving;

		if ( isMoving )
		{
			anim.SetBool( "isMoving", true );
		}
		else
		{
			anim.SetBool( "isMoving", false );
		}
	}
}
