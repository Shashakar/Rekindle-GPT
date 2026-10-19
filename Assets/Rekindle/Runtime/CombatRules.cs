using System;
using System.Collections.Generic;
using System.Linq;
namespace Rekindle {
 public static class CombatRules {public static bool CanHit(bool alive,bool hostile,float distance,float range)=>alive&&hostile&&distance<=range&&distance>=0;}
}
