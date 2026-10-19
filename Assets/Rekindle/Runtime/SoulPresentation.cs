using UnityEngine;
namespace Rekindle {
 public sealed class SoulPresentation:MonoBehaviour {
  [SerializeField]string building="hearth";[SerializeField]GameObject wisp,spectre,person;Session session;Vector3 origin;
  public void Configure(string b,GameObject a,GameObject s,GameObject p){building=b;wisp=a;spectre=s;person=p;}
  public void Bind(Session s){session=s;origin=transform.position;session.World.Changed+=Refresh;Refresh();}void OnDestroy(){if(session)session.World.Changed-=Refresh;}
  void Refresh(){int stage=session.World.Stage(building);wisp.SetActive(stage==0);spectre.SetActive(stage==1);person.SetActive(stage==2);}
  void Update(){if(session&&session.World.Stage(building)<2)transform.position=origin+Vector3.up*Mathf.Sin(Time.time*1.5f)*.18f;}
 }
}
