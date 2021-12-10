using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PowerUp)), CanEditMultipleObjects]
public class PowerUpTypePropertyHolder : Editor
{

    public SerializedProperty
        powerUpType_prop,
        pickUpVisualEffect_prop,
        duration_prop;

    void OnEnable()
    {
        // Setup the SerializedProperties
        powerUpType_prop = serializedObject.FindProperty("powerUpType");
        pickUpVisualEffect_prop = serializedObject.FindProperty("pickUpVisualEffect");
        duration_prop = serializedObject.FindProperty("duration");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(powerUpType_prop);

        PowerUp.PowerUps st = (PowerUp.PowerUps)powerUpType_prop.enumValueIndex;

        switch (st)
        {
            case PowerUp.PowerUps.Turbo:
                EditorGUILayout.ObjectField(pickUpVisualEffect_prop, typeof(GameObject), new GUIContent("pickUpVisualEffect"));
                EditorGUILayout.IntSlider(duration_prop, 1, 10, new GUIContent("Duration"));
                break;

            case PowerUp.PowerUps.Rewind:
                EditorGUILayout.ObjectField(pickUpVisualEffect_prop, typeof(GameObject), new GUIContent("pickUpVisualEffect"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
