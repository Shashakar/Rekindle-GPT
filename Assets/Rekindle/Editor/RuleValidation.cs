#if UNITY_EDITOR
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Rekindle;
namespace Rekindle.Editor {
 public static class RuleValidation {
  public static List<string> Execute(){var results=new List<string>();Action<string,Action> test=(name,action)=>{try{action();results.Add("PASS "+name);}catch(Exception e){results.Add("FAIL "+name+": "+e.Message);}};
   test("Damage clamps at zero; death once",()=>{var g=new GameObject("test");try{var c=g.AddComponent<Character>();c.Configure("test","Test",Allegiance.Friendly,40);int deaths=0;c.Died+=()=>deaths++;Require(c.ReceiveDamage(60),"damage");Require(!c.Alive&&c.Health.Value==0,"clamp");Require(!c.ReceiveDamage(5)&&deaths==1,"death idempotent");c.Revive();Require(c.Alive,"revive");}finally{UnityEngine.Object.DestroyImmediate(g);}});
   test("Resource spending and regeneration",()=>{var p=new Pool(100);Require(!p.Spend(101)&&p.Value==100,"overspend");Require(!p.Spend(-2)&&!p.Spend(float.NaN),"invalid");Require(p.Spend(30)&&p.Value==70,"spend");p.Add(90);Require(p.Value==100,"regen cap");});
   test("Inventory capacity and stacking",()=>{var b=new Inventory(1);Require(b.Add("wood",2)&&b.Add("wood",3),"stack");Require(!b.Add("stone",1)&&b.Count("wood")==5,"capacity");});
   test("Inventory transaction cannot partially consume",()=>{var b=new Inventory();b.Add("wood",4);Require(!b.Consume(new Stack("wood",3),new Stack("stone",1)),"reject");Require(b.Count("wood")==4,"atomic");Require(!b.Consume(new Stack("wood",3),new Stack("wood",3)),"aggregated cost");});
   test("Inventory slot swap and invalid snapshot",()=>{var b=new Inventory();b.Add("wood",2);b.Add("stone",4);Require(b.Swap(0,1)&&b.Slots[0].id=="stone","swap");Require(!b.Load(new[]{new Stack("wood",-1)})&&b.Count("stone")==4,"validated restore");});
   test("Restoration requirements commit before notification",()=>{var w=new WorldState();var b=new Inventory();Require(!w.Restore("hearth",b),"requirements");b.Add("wood",7);b.Add("stone",2);int observed=-1;w.Changed+=()=>observed=w.Stage("hearth");Require(w.Restore("hearth",b)&&observed==1,"commit event");Require(w.Restore("hearth",b)&&w.Stage("hearth")==2,"final stage");Require(!w.Restore("hearth",b),"no duplicate");});
   test("Hostility range and dead targets",()=>{Require(CombatRules.CanHit(true,true,2,3),"valid");Require(!CombatRules.CanHit(true,false,1,3),"friendly");Require(!CombatRules.CanHit(false,true,1,3),"dead");Require(!CombatRules.CanHit(true,true,4,3),"range");});
   test("Encounter and dungeon completion idempotence",()=>{var w=new WorldState();int count=0;w.Changed+=()=>count++;w.Defeat("road-1");Require(!w.BossDefeated,"ordinary enemy");w.Defeat("warden");w.Defeat("warden");Require(w.BossDefeated&&count==2,"boss commit once");});
   test("Complete restoration storyline and capability consequence",()=>{var w=new WorldState();var b=new Inventory();b.Add("wood",30);b.Add("stone",30);b.Add("iron",10);b.Add("food",5);Require(!w.Restore("forge",b),"house gate");Require(w.Restore("hearth",b)&&w.Restore("hearth",b),"house");Require(w.Restore("forge",b),"forge stabilized");b.Add("hammer",1);Require(!w.Restore("forge",b),"dungeon gate");w.Defeat("warden");b.Add("ember",1);Require(w.Restore("forge",b),"smith");var e=new Equipment();e.Derive(w);Require(e.Improved,"capability");Require(w.Restore("garden",b)&&w.Restore("garden",b),"garden");Require(w.Restore("beacon",b)&&w.Restore("beacon",b)&&w.Complete,"ending");Require(w.Objective.Contains("Dawn returns"),"objective ending");});
   test("Snapshot roundtrip preserves actual world and inventory",()=>{var a=new WorldState();var b=new Inventory();b.Add("wood",10);b.Add("stone",5);a.Restore("hearth",b);a.Defeat("warden");a.Deplete("wood-1");var encoded=JsonUtility.ToJson(a.Snapshot(b));var data=JsonUtility.FromJson<SaveData>(encoded);var c=new WorldState();var d=new Inventory();Require(c.Load(data,d),"load");Require(c.Stage("hearth")==1&&c.BossDefeated&&c.IsDepleted("wood-1")&&d.Count("wood")==6,"state");data.version=99;Require(!c.Load(data,d)&&c.Stage("hearth")==1,"version rejects without mutation");});
   test("Safe writes and corrupt-primary recovery",()=>{string path=System.IO.Path.GetFullPath("Artifacts/save-test.json");var store=new SaveStore(path);var w=new WorldState();var b=new Inventory();b.Add("wood",4);store.Write(w.Snapshot(b));b.Add("wood",2);store.Write(w.Snapshot(b));File.WriteAllText(path,"{BROKEN");var recovered=store.Read(s=>new WorldState().Load(s,new Inventory()));Require(recovered!=null&&recovered.items[0].quantity==4,"backup recovery");});
   test("Authored scene integration",()=>{Require(UnityEngine.Object.FindFirstObjectByType<Session>()!=null,"session");Require(UnityEngine.Object.FindObjectsByType<WorldInteractable>(FindObjectsInactive.Include,FindObjectsSortMode.None).Length>=40,"interactions");Require(UnityEngine.Object.FindObjectsByType<EnemyBrain>(FindObjectsInactive.Include,FindObjectsSortMode.None).Length==7,"encounters");var all=UnityEngine.Object.FindObjectsByType<Transform>(FindObjectsInactive.Include,FindObjectsSortMode.None);foreach(var t in all)Require(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject)==0,"missing script "+t.name);Require(UnityEngine.AI.NavMesh.CalculateTriangulation().vertices.Length>0,"baked navigation");});return results;
  }
  static void Require(bool value,string reason){if(!value)throw new Exception(reason);}
  [MenuItem("Rekindle/Validate rules and scene")]public static void Run(){var lines=Execute();Directory.CreateDirectory("Artifacts");File.WriteAllLines("Artifacts/rule-validation.txt",lines);foreach(var line in lines)if(line.StartsWith("FAIL"))Debug.LogError(line);else Debug.Log(line);}
 }
}
#endif
