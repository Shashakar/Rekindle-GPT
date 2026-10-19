using System;
using System.IO;
using UnityEngine;
namespace Rekindle {
 public sealed class Equipment {
  public string Weapon{get;private set;}="traveller-blade";public bool Improved=>Weapon=="dawn-blade";public event Action Changed;
  public void Derive(WorldState world){string next=world.Stage("forge")==2?"dawn-blade":"traveller-blade";if(next!=Weapon){Weapon=next;Changed?.Invoke();}}
 }
}
