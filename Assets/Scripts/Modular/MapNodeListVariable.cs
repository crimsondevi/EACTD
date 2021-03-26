using System.Collections.Generic;
using UnityEngine;

namespace Modular
{
    [CreateAssetMenu(menuName = "Crimson Council/Variables/MapNodeList")]
    public class MapNodeListVariable : ScriptableObject
    {
        [Multiline] public string DeveloperDescription = "";
        public List<MapNode> Value;

        public void SetValue(List<MapNode> value)
        {
            Value = value;
        }

        public void SetValue(MapNodeListVariable value)
        {
            Value = value.Value;
        }
    }
}