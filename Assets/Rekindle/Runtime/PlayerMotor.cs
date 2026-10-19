using UnityEngine;
using UnityEngine.InputSystem;
namespace Rekindle {
 [RequireComponent(typeof(CharacterController))]public sealed class PlayerMotor:MonoBehaviour {
  [SerializeField]float speed=5.3f,acceleration=12,turnSpeed=130;PlayerInputSource input;Character actor;CharacterController controller;Vector3 velocity;float vertical,dodgeUntil;Vector3 dodgeDirection;public bool Blocked{get;set;}public bool RightDragging{get;set;}
  void Awake(){input=GetComponent<PlayerInputSource>();actor=GetComponent<Character>();controller=GetComponent<CharacterController>();}
  void Update(){if(Blocked||!actor.Alive)return;float dt=Time.deltaTime;float turn=input.Turn.ReadValue<float>();if(!RightDragging)transform.Rotate(0,turn*turnSpeed*dt,0);var direction=transform.forward*input.Move.ReadValue<float>()+transform.right*(input.Strafe.ReadValue<float>()+(RightDragging?turn:0));direction=Vector3.ClampMagnitude(direction,1);if(input.Dodge.WasPressedThisFrame())Evade(direction);velocity=Vector3.MoveTowards(velocity,direction*speed,acceleration*dt);vertical=controller.isGrounded?-2:vertical-22*dt;controller.Move(((Time.time<dodgeUntil?dodgeDirection*14:velocity)+Vector3.up*vertical)*dt);if(transform.position.y<-12)Teleport(new Vector3(0,2,-8));}
  public bool Evade(Vector3 dir){if(Blocked||!actor.Alive||Time.time<dodgeUntil+.35f||!actor.Energy.Spend(22))return false;dodgeDirection=dir.sqrMagnitude>.01f?dir.normalized:transform.forward;dodgeUntil=Time.time+.28f;actor.Evade(.35f);Feedback.Burst(transform.position+Vector3.up,new Color(.3f,.8f,1),7);return true;}
  public void Teleport(Vector3 p){controller.enabled=false;transform.position=p;controller.enabled=true;velocity=Vector3.zero;vertical=0;}
 }
}
