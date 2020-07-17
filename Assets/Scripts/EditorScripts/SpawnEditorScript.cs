using UnityEditor;
using UnityEngine;

namespace EditorScripts
{
    [CustomEditor(typeof(SpawnController))]
    public class SpawnEditorScript : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SpawnController spawnController = (SpawnController)target;
            if (GUILayout.Button("Spawn enemy"))
            {
                spawnController.SpawnEnemy(spawnController.HealthToSpawn, spawnController.SpeedToSpawn);
            }
        }
    }
}