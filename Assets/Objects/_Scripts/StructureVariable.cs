#nullable enable
using Framework.Variables;
using UnityEngine;

namespace DarkFrontier.Objects
{
    [CreateAssetMenu(menuName = "Variable/Structure", fileName = "NewStructure")]
    public class StructureVariable : ScriptableVariable<Structure?>
    {
    }
}