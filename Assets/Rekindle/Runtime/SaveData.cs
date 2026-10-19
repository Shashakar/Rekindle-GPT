using System;
using System.Collections.Generic;
using System.Linq;
namespace Rekindle {
 [Serializable] public class SaveData {public int version=1;public List<BuildingState> buildings;public List<string> defeated,depleted;public List<Stack> items;public float x=0,y=2,z=-8;public float health=120;}
}
