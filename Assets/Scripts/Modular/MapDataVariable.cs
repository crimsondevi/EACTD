using UnityEngine;

namespace Modular
{
    [CreateAssetMenu(menuName = "Crimson Council/Variables/MapData")]
    public class MapDataVariable : ScriptableObject
    {
        [Multiline] public string DeveloperDescription = "";
        public MapData Value;
        public TextAsset jsonFile;

        [Header("Developer Options")] public bool Resets;
        public MapData DefaultValue;
        
        public void SetValue(MapData value)
        {
            Value = value;
        }
        
        public void SetValue(MapDataVariable value)
        {
            Value = value.Value;
        }

        public MapData GetValue()
        {
            return Value;
        }
        
        private void OnEnable()
        {    
            MapData mapData = JsonUtility.FromJson<MapData>(jsonFile.text);
            Value = mapData;
        }
    }
}