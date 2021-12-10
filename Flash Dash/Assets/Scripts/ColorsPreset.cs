using UnityEngine;

[CreateAssetMenu(menuName = "Color Preset")]
public class ColorsPreset : ScriptableObject
{
    public bool isDark;
    public Material roadMaterial; 
    public Color backgroundColor;
    public Color carColor;
}
