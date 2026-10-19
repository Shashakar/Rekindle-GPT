using UnityEngine;
using UnityEngine.InputSystem;
namespace Rekindle {
 public sealed class PlayerInputSource:MonoBehaviour {
  public InputAction Move{get;private set;}public InputAction Strafe{get;private set;}public InputAction Turn{get;private set;}public InputAction Dodge{get;private set;}public InputAction Interact{get;private set;}public InputAction Target{get;private set;}public InputAction Bag{get;private set;}public InputAction Pause{get;private set;}public InputAction[] Actions{get;private set;}
  InputAction Axis(string name,string negative,string positive){var a=new InputAction(name,InputActionType.Value);a.AddCompositeBinding("1DAxis").With("Negative","<Keyboard>/"+negative).With("Positive","<Keyboard>/"+positive);return a;}
  InputAction Button(string name,string binding)=>new InputAction(name,InputActionType.Button,binding);
  void Awake(){Move=Axis("Move","s","w");Turn=Axis("Turn","a","d");Strafe=Axis("Strafe","q","e");Dodge=Button("Evade","<Keyboard>/space");Interact=Button("Interact","<Keyboard>/f");Target=Button("Target","<Keyboard>/tab");Bag=Button("Inventory","<Keyboard>/i");Pause=Button("Pause","<Keyboard>/escape");Actions=new InputAction[6];for(int i=0;i<6;i++)Actions[i]=Button("Action"+i,"<Keyboard>/digit"+(i+1));Set(true);}
  void Set(bool on){if(Move==null)return;foreach(var a in new[]{Move,Turn,Strafe,Dodge,Interact,Target,Bag,Pause}){if(on)a.Enable();else a.Disable();}foreach(var a in Actions){if(on)a.Enable();else a.Disable();}}
  void OnEnable(){Set(true);}void OnDisable(){Set(false);}void OnDestroy(){if(Move==null)return;foreach(var a in new[]{Move,Turn,Strafe,Dodge,Interact,Target,Bag,Pause})a.Dispose();foreach(var a in Actions)a.Dispose();}
 }
}
