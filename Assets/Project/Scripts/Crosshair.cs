using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public float rotationSpeed = 40f;
    public SpriteRenderer dot;
    public Color dotHighlightColor;
    Color dotOriginalColor;

    private void Start()
    {
        Cursor.visible = false;
        dotOriginalColor = dot.color;
    }

    public LayerMask targetMask;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    public void DetectTarget(Ray ray)
    {
        if(Physics.Raycast(ray, 100, targetMask))
        {
            dot.color = dotHighlightColor;
        }

        else
        {
            dot.color = dotOriginalColor;
        }
    }
}
