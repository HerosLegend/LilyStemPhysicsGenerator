using UnityEngine;
using UnityEditor;
using BlindkatStudios.Generators;

[CustomEditor(typeof(LilyStemPhysicsGenerator))]
public class LilyStemPhysicsEditor : Editor {
    /// <summary>
    /// Draws the custom inspector GUI for the <see cref="LilyStemPhysicsGenerator"/> component.
    /// </summary>
    /// <remarks>This method provides additional functionality in the Unity Inspector for the  <see
    /// cref="LilyStemPhysicsGenerator"/> component. It includes buttons to generate or remove components on child
    /// objects of the target component.</remarks>
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        LilyStemPhysicsGenerator generator = (LilyStemPhysicsGenerator)target;
        if (GUILayout.Button("Generate Components On Children")) {
            generator.GenerateComponentsOnChildren();
        }

        if (GUILayout.Button("Remove All Child Components")) {
            generator.RemoveAllComponents();
        }
    }
}

