using UnityEngine;
using UnityEngine.InputSystem;
namespace Rekindle {
 public sealed class OrbitCamera:MonoBehaviour {
  [SerializeField]Transform follow;[SerializeField]float distance=9,yaw,pitch=35,sensitivity=.16f;PlayerMotor motor;Vector2 down;bool moved;public bool Blocked{get;set;}public bool PointerOverUI{get;set;}public event System.Action<Vector2> RightClicked;
  public void Configure(Transform t){follow=t;}void Start(){motor=follow.GetComponent<PlayerMotor>();}
  void LateUpdate(){if(!follow||Mouse.current==null)return;var mouse=Mouse.current;var pos=mouse.position.ReadValue();if(mouse.rightButton.wasPressedThisFrame||mouse.leftButton.wasPressedThisFrame){down=pos;moved=false;}if(Vector2.Distance(pos,down)>5)moved=true;bool held=mouse.rightButton.isPressed||mouse.leftButton.isPressed;motor.RightDragging=mouse.rightButton.isPressed&&!Blocked&&!PointerOverUI;if(held&&moved&&!Blocked&&!PointerOverUI){var d=mouse.delta.ReadValue();yaw+=d.x*sensitivity;pitch=Mathf.Clamp(pitch-d.y*sensitivity,15,70);if(mouse.rightButton.isPressed)follow.rotation=Quaternion.Euler(0,yaw,0);}if(mouse.rightButton.wasReleasedThisFrame&&!moved&&!Blocked&&!PointerOverUI)RightClicked?.Invoke(pos);if(!Blocked&&!PointerOverUI)distance=Mathf.Clamp(distance-mouse.scroll.ReadValue().y*.006f,4,15);var pivot=follow.position+Vector3.up*1.6f;var offset=Quaternion.Euler(pitch,yaw,0)*Vector3.back;float length=distance;if(Physics.SphereCast(pivot,.25f,offset,out var hit,distance,~(1<<8),QueryTriggerInteraction.Ignore))length=Mathf.Max(1,hit.distance-.2f);transform.position=Vector3.Lerp(transform.position,pivot+offset*length,1-Mathf.Exp(-14*Time.unscaledDeltaTime));transform.LookAt(pivot);}
 }
}
