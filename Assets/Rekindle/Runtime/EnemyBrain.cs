using UnityEngine;
using UnityEngine.AI;
namespace Rekindle {
 [RequireComponent(typeof(Character),typeof(Combatant),typeof(NavMeshAgent))]public sealed class EnemyBrain:MonoBehaviour {
  [SerializeField]bool boss,ranged;[SerializeField]float perception=13,leash=24;Character actor,player;Combatant combat;NavMeshAgent agent;Vector3 home;Session session;int attacks;public EnemyState State{get;private set;}
  public void Configure(bool isBoss,bool isRanged){boss=isBoss;ranged=isRanged;}
  public void Bind(Session s,Character p){session=s;player=p;}
  void Start(){actor=GetComponent<Character>();combat=GetComponent<Combatant>();agent=GetComponent<NavMeshAgent>();home=transform.position;actor.Died+=OnDeath;combat.Resolved+=OnResolved;Debug.Log("REKINDLE_NAV "+actor.Id+" bound="+agent.isOnNavMesh+" vertices="+NavMesh.CalculateTriangulation().vertices.Length);agent.speed=boss?2.5f:ranged?3:3.8f;if(session.World.IsDefeated(actor.Id)){gameObject.SetActive(false);}}
  void OnDestroy(){if(actor!=null)actor.Died-=OnDeath;if(combat)combat.Resolved-=OnResolved;}
  void OnResolved(AbilityDefinition a){attacks++;}
  void OnDeath(){State=EnemyState.Dead;if(agent.isOnNavMesh)agent.isStopped=true;GetComponent<Collider>().enabled=false;session.EnemyDefeated(actor);}
  void Update(){if(!actor.Alive||!player||!agent.isOnNavMesh)return;float distance=Vector3.Distance(transform.position,player.transform.position);if(!player.Alive||Vector3.Distance(transform.position,home)>leash){State=EnemyState.Return;combat.Stop();agent.isStopped=false;agent.SetDestination(home);if(Vector3.Distance(transform.position,home)<1){actor.Revive();State=EnemyState.Idle;}return;}if(State==EnemyState.Return){agent.isStopped=false;agent.SetDestination(home);if(Vector3.Distance(transform.position,home)<=1)ResetEncounter();return;}if(State==EnemyState.Idle&&distance>perception)return;if(distance>perception+8){State=EnemyState.Return;combat.Stop();agent.isStopped=false;agent.SetDestination(home);return;}combat.Select(player);if(combat.Busy){State=EnemyState.Recover;agent.isStopped=true;return;}float range=combat.Basic.range;if(distance>range-.2f){State=EnemyState.Approach;agent.isStopped=false;agent.SetDestination(player.transform.position);}else{State=EnemyState.Attack;agent.isStopped=true;var facing=player.transform.position-transform.position;facing.y=0;if(facing.sqrMagnitude>.01f)transform.rotation=Quaternion.LookRotation(facing);combat.TryAttack(boss&&attacks%3==2);}}
  public void ResetEncounter(){if(!actor||!actor.Alive)return;combat.Stop();combat.Select(null);actor.Revive();State=EnemyState.Idle;attacks=0;if(agent.isOnNavMesh){agent.isStopped=true;agent.ResetPath();}}
  void OnRenderObject(){if(!combat||combat.Telegraph<=0||!actor.Alive)return;}
 }
}

