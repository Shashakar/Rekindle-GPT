using UnityEngine;
using System.Collections;
namespace Rekindle {
 [RequireComponent(typeof(Character))]public sealed class Combatant:MonoBehaviour {
  [SerializeField] AbilityDefinition basic,power;Character actor;float nextAttack;public bool Busy{get;private set;}public Character Target{get;private set;}public bool AutoAttack{get;private set;}public float Telegraph{get;private set;}public AbilityDefinition Basic=>basic;
  public event System.Action<AbilityDefinition> Committed;public event System.Action<AbilityDefinition> Resolved;
  public void Configure(AbilityDefinition a,AbilityDefinition b){basic=a;power=b;}void Awake(){actor=GetComponent<Character>();}
  public void Select(Character c,bool auto=false){Target=c;AutoAttack=auto;}public void Stop(){AutoAttack=false;}
  void Update(){if(AutoAttack&&Target&&Target.Alive)TryAttack(false);if(Target&&!Target.Alive)AutoAttack=false;}
  public bool TryAttack(bool strong){var a=strong?power:basic;if(!a||Busy||!actor.Alive||Time.time<nextAttack)return false;if(a.radius<=0&&(!Target||!CombatRules.CanHit(Target.Alive,Target.Allegiance!=actor.Allegiance,Vector3.Distance(transform.position,Target.transform.position),a.range)))return false;if(!actor.Energy.Spend(a.cost))return false;StartCoroutine(Execute(a));return true;}
  IEnumerator Execute(AbilityDefinition a){Busy=true;Telegraph=a.radius;nextAttack=Time.time+a.windup+a.recovery;Committed?.Invoke(a);yield return new WaitForSeconds(a.windup);if(actor.Alive){if(a.radius>0){var hits=Physics.OverlapSphere(transform.position,a.radius);var seen=new System.Collections.Generic.HashSet<Character>();foreach(var h in hits){var c=h.GetComponentInParent<Character>();if(c&&seen.Add(c)&&CombatRules.CanHit(c.Alive,c.Allegiance!=actor.Allegiance,Vector3.Distance(c.transform.position,transform.position),a.radius))c.ReceiveDamage(a.damage);}}else if(Target&&CombatRules.CanHit(Target.Alive,Target.Allegiance!=actor.Allegiance,Vector3.Distance(transform.position,Target.transform.position),a.range))Target.ReceiveDamage(a.damage);Resolved?.Invoke(a);}Telegraph=0;yield return new WaitForSeconds(a.recovery);Busy=false;}
 }
}
