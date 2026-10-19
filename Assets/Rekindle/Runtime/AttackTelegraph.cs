using UnityEngine;
using UnityEngine.AI;
namespace Rekindle {
 public sealed class AttackTelegraph:MonoBehaviour {
  Combatant combat;LineRenderer line;void Start(){combat=GetComponent<Combatant>();var g=new GameObject("Danger ring");g.transform.SetParent(transform,false);line=g.AddComponent<LineRenderer>();line.useWorldSpace=false;line.loop=true;line.positionCount=64;line.widthMultiplier=.14f;line.sharedMaterial=Resources.Load<Material>("RekindleThreat");}
  void Update(){float r=combat.Telegraph/Mathf.Max(.001f,transform.lossyScale.x);line.enabled=r>0;if(r>0)for(int i=0;i<64;i++){float a=i*Mathf.PI/32;line.SetPosition(i,new Vector3(Mathf.Cos(a)*r,.12f,Mathf.Sin(a)*r));}}
 }
}
