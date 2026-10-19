using UnityEngine;
using System.Collections;
namespace Rekindle {
 public sealed class ActorPresentation:MonoBehaviour {
  [SerializeField]Transform body,sword,leftLeg,rightLeg;[SerializeField]Color color=Color.white;Character actor;Combatant combat;Vector3 previous;float stride;float strike;Renderer[] renderers;Color[] baseColors;MaterialPropertyBlock block;float flash;
  public void Configure(Transform b,Transform s,Transform l,Transform r,Color c){body=b;sword=s;leftLeg=l;rightLeg=r;color=c;}
  void Start(){actor=GetComponent<Character>();combat=GetComponent<Combatant>();renderers=GetComponentsInChildren<Renderer>();baseColors=new Color[renderers.Length];for(int i=0;i<renderers.Length;i++)baseColors[i]=renderers[i].sharedMaterial.color;block=new MaterialPropertyBlock();previous=transform.position;actor.Damaged+=Hit;actor.Died+=Death;if(combat){combat.Committed+=Swing;combat.Resolved+=Impact;}}
  void OnDestroy(){if(actor!=null){actor.Damaged-=Hit;actor.Died-=Death;}if(combat){combat.Committed-=Swing;combat.Resolved-=Impact;}}
  void Hit(float amount){flash=.25f;Feedback.Burst(transform.position+Vector3.up,Color.red,8);Feedback.Tone(130,.1f);}
  void Death(){Feedback.Burst(transform.position+Vector3.up,new Color(1,.65f,.2f),18);}
  void Swing(AbilityDefinition a){strike=a.windup+a.recovery;Feedback.Tone(250,.06f);}
  void Impact(AbilityDefinition a){Feedback.Burst(transform.position+transform.forward*1.2f+Vector3.up,a.tint,8);Feedback.Tone(90,.08f);}
  void Update(){if(!body)return;var speed=(transform.position-previous).magnitude/Mathf.Max(Time.deltaTime,.001f);previous=transform.position;stride+=Time.deltaTime*Mathf.Min(speed,6)*2.6f;float step=Mathf.Sin(stride)*Mathf.Min(speed,4)*7;if(leftLeg)leftLeg.localRotation=Quaternion.Euler(step,0,0);if(rightLeg)rightLeg.localRotation=Quaternion.Euler(-step,0,0);if(strike>0){strike-=Time.deltaTime;if(sword)sword.localRotation=Quaternion.Euler(-80+Mathf.Sin(strike*12)*100,0,-25);}else if(sword)sword.localRotation=Quaternion.Euler(0,0,-15);body.localRotation=Quaternion.Slerp(body.localRotation,actor.Alive?Quaternion.identity:Quaternion.Euler(0,0,80),Time.deltaTime*5);if(flash>0){flash=Mathf.Max(0,flash-Time.deltaTime);for(int i=0;i<renderers.Length;i++){block.SetColor("_BaseColor",Color.Lerp(baseColors[i],Color.white,flash*4));renderers[i].SetPropertyBlock(block);}}}
 }
}
