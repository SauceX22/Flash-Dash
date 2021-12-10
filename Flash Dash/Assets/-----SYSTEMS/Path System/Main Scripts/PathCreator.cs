using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{

    [HideInInspector]
    public Path path;

    //#FF0054
    public Color anchorCol = new Color32(0xFF, 0x00, 0x54, 0xFF);
    //#FE9BFF
    public Color controlCol = new Color32(0xFE, 0x9B, 0xFF, 0xFF);
    public Color segmentCol = Color.green;
    //#0A04FF
    public Color selectedSegmentCol = new Color32(0x0A, 0x04, 0xFF, 0xFF);

    public float anchorDiameter = 2f;
    public float controlDiameter = 1.2f;
    public float scale = 20f;

    public bool displayControlPoints = true;

    public void CreatePath()
    {
        path = new Path(transform.position, scale);
    }

    void Reset()
    {
        CreatePath();
    }
}
