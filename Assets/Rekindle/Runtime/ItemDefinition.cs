using UnityEngine;
namespace Rekindle {
 [CreateAssetMenu(menuName="Rekindle/Item")] public class ItemDefinition:ScriptableObject {public string stableId;public string title;[TextArea]public string description;public Color tint=Color.white;public bool consumable;}
}
