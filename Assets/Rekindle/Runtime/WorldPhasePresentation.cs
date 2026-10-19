using UnityEngine;
namespace Rekindle {
 public sealed class WorldPhasePresentation:MonoBehaviour {
  [SerializeField]GameObject corruption,life,celebration;[SerializeField]Light sun;Session session;
  public void Configure(GameObject c,GameObject l,GameObject f,Light s){corruption=c;life=l;celebration=f;sun=s;}
  public void Bind(Session s){session=s;s.World.Changed+=Refresh;Refresh();}void OnDestroy(){if(session)session.World.Changed-=Refresh;}
  void Refresh(){corruption.SetActive(!session.World.BossDefeated);life.SetActive(session.World.BossDefeated);celebration.SetActive(session.World.Complete);sun.color=session.World.Complete?new Color(1,.86f,.65f):new Color(.75f,.82f,1);RenderSettings.fogColor=session.World.Complete?new Color(.39f,.48f,.46f):new Color(.2f,.28f,.34f);}
 }
}
