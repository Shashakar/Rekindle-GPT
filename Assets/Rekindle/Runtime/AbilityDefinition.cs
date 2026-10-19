using UnityEngine;
namespace Rekindle {
 [CreateAssetMenu(menuName="Rekindle/Ability")] public class AbilityDefinition:ScriptableObject {public string title;public float damage=15,cost,range=3,windup=.3f,recovery=.8f,radius;public Color tint=Color.white;}
}
