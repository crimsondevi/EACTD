using UnityEngine;

namespace Modular
{
    [CreateAssetMenu(menuName = "Crimson Council/Variables/MapNode")]
    public class MapNodeVariable : ScriptableObject
    {
        [Multiline] public string DeveloperDescription = "";
        public MapNode Value;

        [Header("Developer Options")] public bool Resets;
        public MapNode DefaultValue;

        public void SetValue(MapNode value)
        {
            Value = value;
        }

        public void SetValue(MapNodeVariable value)
        {
            Value = value.Value;
        }
        
        private void OnEnable()
        {
            if (Resets && Value != DefaultValue)
            {
                Value = DefaultValue;
            }
        }
    }
}

