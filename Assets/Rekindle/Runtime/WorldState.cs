using System;
using System.Collections.Generic;
using System.Linq;
namespace Rekindle {
 public class WorldState {
  readonly Dictionary<string,int> buildings=new Dictionary<string,int>{{"hearth",0},{"forge",0},{"garden",0},{"beacon",0}};
  readonly HashSet<string> defeated=new HashSet<string>();readonly HashSet<string> depleted=new HashSet<string>();
  public bool BossDefeated=>defeated.Contains("warden");public bool Complete=>Stage("beacon")==2;public event Action Changed;
  public int Stage(string id)=>buildings.TryGetValue(id,out var s)?s:0;
  public bool IsDefeated(string id)=>defeated.Contains(id);public bool IsDepleted(string id)=>depleted.Contains(id);
  public void Defeat(string id){if(defeated.Add(id))Changed?.Invoke();}public void Deplete(string id){if(depleted.Add(id))Changed?.Invoke();}
  public Stack[] Cost(string id){int s=Stage(id);if(s>=2)return Array.Empty<Stack>();switch(id){case "hearth":return s==0?new[]{new Stack("wood",4),new Stack("stone",2)}:new[]{new Stack("wood",3)};case "forge":return s==0?new[]{new Stack("stone",4),new Stack("iron",2)}:new[]{new Stack("hammer",1)};case "garden":return s==0?new[]{new Stack("wood",2),new Stack("food",2)}:new[]{new Stack("wood",2)};default:return s==0?new[]{new Stack("stone",4),new Stack("iron",2)}:new[]{new Stack("ember",1)};}}
  public string Block(string id){if(Stage(id)>=2)return "Already restored";if(id!="hearth"&&Stage("hearth")<2)return "Restore the Hearth House first";if(id=="forge"&&Stage(id)==1&&!BossDefeated)return "Recover Mira's hammer from the Ash Warden";if(id=="beacon"&&Stage(id)==1&&(!BossDefeated||Stage("forge")<2||Stage("garden")<2))return "Restore the smith and garden, then cleanse the sanctuary";return "";}
  public bool Restore(string id,Inventory bag){if(!buildings.ContainsKey(id)||Block(id)!=""||!bag.Consume(Cost(id)))return false;buildings[id]++;Changed?.Invoke();return true;}
  public string Objective=>Stage("hearth")<2?"A place to remember : Restore the Hearth House":Stage("forge")==0?"A duty unfinished : Stabilize Mira's forge":!BossDefeated?"Into the ash : Follow the east road to the sanctuary; recover Mira's hammer":Stage("forge")<2?"Bring her home : Return Mira's hammer to the forge":!Complete?"Light the valley : Restore the garden and the Dawn Beacon":"Dawn returns : Cinderhollow remembers. Your work endures.";
  public SaveData Snapshot(Inventory bag)=>new SaveData{buildings=buildings.Select(x=>new BuildingState(x.Key,x.Value)).ToList(),defeated=defeated.ToList(),depleted=depleted.ToList(),items=bag.Slots.Select(x=>new Stack(x.id,x.quantity)).ToList()};
  public bool Load(SaveData s,Inventory bag){if(s==null||s.version!=1||s.buildings==null||s.defeated==null||s.depleted==null||s.items==null||s.buildings.Count!=4||s.buildings.Select(x=>x.id).Distinct().Count()!=4||s.buildings.Any(x=>!buildings.ContainsKey(x.id)||x.stage<0||x.stage>2))return false;if(!bag.Load(s.items))return false;foreach(var b in s.buildings)buildings[b.id]=b.stage;defeated.Clear();defeated.UnionWith(s.defeated);depleted.Clear();depleted.UnionWith(s.depleted);Changed?.Invoke();return true;}
 }
}
