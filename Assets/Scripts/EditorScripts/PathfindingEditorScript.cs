using UnityEditor;
using UnityEngine;

namespace EditorScripts
{
    [CustomEditor(typeof(Pathfinding))]
    public class PathfindingEditorScript : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Pathfinding pathfinding = (Pathfinding)target;
            if (GUILayout.Button("BFS"))
            {
                pathfinding.BFS(pathfinding.StartNode.Value);
            }
        }
    }
}