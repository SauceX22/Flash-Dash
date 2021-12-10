using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrapTrigger)), CanEditMultipleObjects]
public class TrapTypePropertiesHolder : Editor
{

    public SerializedProperty
        trapType_prop,
        crossing_prop,
        spaceships_prop,
        speed_prop;

    void OnEnable()
    {
        // Setup the SerializedProperties
        trapType_prop = serializedObject.FindProperty("trapType");
        crossing_prop = serializedObject.FindProperty("crossingTrap");
        spaceships_prop = serializedObject.FindProperty("spaceShipTrap");
        speed_prop = serializedObject.FindProperty("speed");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(trapType_prop);

        TrapTrigger.TrapType st = (TrapTrigger.TrapType)trapType_prop.enumValueIndex;

        switch (st)
        {
            case TrapTrigger.TrapType.crossingTrap:
                EditorGUILayout.ObjectField(crossing_prop, typeof(CrossingTrap), new GUIContent("Crossing Trap"));
                break;

            case TrapTrigger.TrapType.spaceshipsTrap:
                EditorGUILayout.ObjectField(spaceships_prop, typeof(SpaceShipTrap), new GUIContent("SpaceShip Trap"));
                break;
        }

        EditorGUILayout.Slider(speed_prop, 30, 40f, new GUIContent("Speed"));

        serializedObject.ApplyModifiedProperties();
    }
}
