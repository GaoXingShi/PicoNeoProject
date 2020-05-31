/// <summary>
/// Author: Lele Feng
/// </summary>
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ColorPicker : MonoBehaviour
{
    private Vector3 screenPosition;
    private Material setMaterial;
    private Color setColor;
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (Input.GetMouseButtonDown(2))
            {
                setMaterial = hit.transform.GetComponent<MeshRenderer>().material;
                setColor = setMaterial.color;
            }
        }

    }
    void OnGUI()
    {
        if (setMaterial == null)
        {
            return;
        }
        GUI.Box(new Rect(0, 0, 220, 200), "Color Picker");
        GUIDrawRect(new Rect(20, 30, 80, 80), setMaterial.color);
        GUI.Label(new Rect(10, 120, 100, 20), "R: " + System.Math.Round((double)setColor.r, 4) + "\t(" + Mathf.FloorToInt(setColor.r * 255) + ")");
        GUI.Label(new Rect(10, 140, 100, 20), "G: " + System.Math.Round((double)setColor.g, 4) + "\t(" + Mathf.FloorToInt(setColor.g * 255) + ")");
        GUI.Label(new Rect(10, 160, 100, 20), "B: " + System.Math.Round((double)setColor.b, 4) + "\t(" + Mathf.FloorToInt(setColor.b * 255) + ")");
        GUI.Label(new Rect(10, 180, 100, 20), "A: " + System.Math.Round((double)setColor.a, 4) + "\t(" + Mathf.FloorToInt(setColor.a * 255) + ")");

        setColor.r = GUI.HorizontalSlider(new Rect(110, 125, 70, 20), setColor.r, 0, 1);
        setColor.g = GUI.HorizontalSlider(new Rect(110, 145, 70, 20), setColor.g, 0, 1);
        setColor.b = GUI.HorizontalSlider(new Rect(110, 165, 70, 20), setColor.b, 0, 1);
        setColor.a = GUI.HorizontalSlider(new Rect(110, 185, 70, 20), setColor.a, 0, 1);

        setMaterial.color = setColor;
    }

    private static Texture2D m_staticRectTexture;
    private static GUIStyle m_staticRectStyle;
    private static Vector3 m_pixelPosition = Vector3.zero;
    public static void GUIDrawRect(Rect position, Color color)
    {
        if (m_staticRectTexture == null)
        {
            m_staticRectTexture = new Texture2D(1, 1);
        }

        if (m_staticRectStyle == null)
        {
            m_staticRectStyle = new GUIStyle();
        }

        m_staticRectTexture.SetPixel(0, 0, color);
        m_staticRectTexture.Apply();

        m_staticRectStyle.normal.background = m_staticRectTexture;

        GUI.Box(position, GUIContent.none, m_staticRectStyle);
    }

}