using UnityEngine;
namespace Rekindle {
 public sealed class Character:MonoBehaviour {
  [SerializeField] string stableId;[SerializeField] string displayName="Wanderer";[SerializeField] Allegiance allegiance;[SerializeField] float maximumHealth=120;
  public string Id=>stableId;public string Title=>displayName;public Allegiance Allegiance=>allegiance;public Pool Health{get;private set;}public Pool Energy{get;private set;}public bool Alive=>Health!=null&&Health.Value>0;public float InvulnerableUntil{get;private set;}
  public event System.Action<float> Damaged;public event System.Action Died;
  public void Configure(string id,string title,Allegiance side,float health){stableId=id;displayName=title;allegiance=side;maximumHealth=health;Initialize();}
  void Awake(){Initialize();}void Initialize(){Health=new Pool(maximumHealth);Energy=new Pool(100);}
  void Update(){if(Alive)Energy.Add(Time.deltaTime*11);}
  public bool ReceiveDamage(float amount){if(!Alive||Time.time<InvulnerableUntil||amount<=0)return false;Health.Spend(Mathf.Min(amount,Health.Value));Damaged?.Invoke(amount);if(!Alive)Died?.Invoke();return true;}
  public void Evade(float seconds){InvulnerableUntil=Time.time+seconds;}
  public void Revive(){Health.Restore(Health.Max);Energy.Restore(Energy.Max);InvulnerableUntil=Time.time+2;}
 }
}
