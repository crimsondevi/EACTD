using UnityEditor;
using UnityEngine;

namespace EditorScripts
{
    [CustomEditor(typeof(GameGrid))]
    public class GameGridEditorScript : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GameGrid gamegrid = (GameGrid) target;
            if (GUILayout.Button("Generate Map"))
            {
                gamegrid.GenerateMap();
            }

            if (GUILayout.Button("Generate From Json"))
            {
                gamegrid.GenerateFromJson();
            }
            
            
            if (GUILayout.Button("Update Map"))
            {

                gamegrid.UpdateMap();
            }
            
        }

    }
}