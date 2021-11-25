
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class CustomUtilities
{
    public static Transform TransformFind(Transform transform, string name)
    {
        if(transform.name == name)
        {
            return transform;
        }
        Transform find = null;
        foreach (Transform t in transform)
        {
            Transform f = TransformFind(t, name);
            if(f!= null)
            {
                find = f;
            }
        }
        return find;
    }
    public static void Clear(Transform tr, System.Action<GameObject> clearMethod)
    {
        List<Transform> temp = new List<Transform>();
        foreach (Transform t in tr)
        {
            temp.Add(t);
        }
        foreach (Transform t in temp)
        {
            if (t != tr)
            {
                clearMethod(t.gameObject);
            }

        }

    }
    public static Vector2 RandomVector2(float angle, float angleMin)
    {
        float random = Random.value * angle + angleMin;
        return new Vector2(Mathf.Cos(random), Mathf.Sin(random));
    }

    public static bool RandomBoolean(float threshold)
    {
        float rand = Random.Range(0f,1f);
        return rand > threshold?false:true;
    }

    public static Rectangle Intersection(Rectangle R1, Rectangle R2)
    {
        return new Rectangle(Mathf.Max(R1.minX, R2.minX), Mathf.Max(R1.minY, R2.minY), Mathf.Min(R1.maxX, R2.maxX), Mathf.Min(R1.maxY, R2.maxY));
    }
    public static float IntersectionArea(Rectangle R1 , Rectangle R2)
    {
        Rectangle intersection = Intersection(R1, R2);
        return (intersection.maxX - intersection.minX) * (intersection.maxY - intersection.minY);
    }
    public static Rectangle TranslateRectangle(Rectangle r, Vector2 v)
    {
        return new Rectangle(r.Center + v, r.Size);
    }
    public static void DrawArrowForGizmo(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Gizmos.DrawRay(pos, direction);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
        Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
    }

    public static void DrawArrowForGizmo(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Gizmos.color = color;
        Gizmos.DrawRay(pos, direction);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
        Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
    }

    public static void DrawArrowForDebug(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        UnityEngine.Debug.DrawRay(pos, direction);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        UnityEngine.Debug.DrawRay(pos + direction, right * arrowHeadLength);
        UnityEngine.Debug.DrawRay(pos + direction, left * arrowHeadLength);
    }
    public static void DrawArrowForDebug(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        UnityEngine.Debug.DrawRay(pos, direction, color);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        UnityEngine.Debug.DrawRay(pos + direction, right * arrowHeadLength, color);
        UnityEngine.Debug.DrawRay(pos + direction, left * arrowHeadLength, color);
    }

    public static Vector2 CalculateJumpSpeed(Vector2 start, Vector2 end, float maxVerticalJumpSpeed)
    {
        float a =  Physics.gravity.y * (end.x - start.x); 
        float b = 2f * (-Physics.gravity.y) * (end.y - start.y); 
        float delta = maxVerticalJumpSpeed * maxVerticalJumpSpeed + b; 
        if (delta >= 0) {
            float v0y = 0; 
            if (b <= 0) {
                v0y = Random.Range(Mathf.Sqrt( - b) + 5f, (Mathf.Sqrt( - b) + 5f + maxVerticalJumpSpeed)/2f); 
            }
            else {
                v0y = Random.Range(5f, maxVerticalJumpSpeed); 
            }
            float v0x = a / (v0y + Mathf.Sqrt(v0y * v0y + b)); 
            return new Vector2(v0x, v0y); 
        }
        return new Vector2(a/maxVerticalJumpSpeed, maxVerticalJumpSpeed); 
    }
    
}
[System.Serializable]
public struct Rectangle
{
    public float minX;
    public float minY;

    public float maxX;
    public float maxY;

    public Rectangle(float _minX, float _minY, float _maxX, float _maxY)
    {
        if(_maxX > _minX && _maxY > _minY)
        {
            maxX = _maxX;
            maxY = _maxY;
            minX = _minX;
            minY = _minY;
        }
        else
        {
            maxX = 0;
            maxY = 0;
            minX = 0;
            minY = 0;
        }
        
    }
    public Rectangle(Vector2 center, Vector2 size)
    {
        minX = center.x - size.x/2f;
        minY = center.y - size.y/2f;
        maxX = center.x + size.x/2f;
        maxY = center.y + size.y/2f;
        if(maxX <= minX || maxY <= minY)
        {
            maxX = 0;
            maxY = 0;
            minX = 0;
            minY = 0;
    
        }
    }
    public Vector2 Center
    {
        get
        {
            return new Vector2((maxX + minX)/2f, (minY + maxY)/2f);
        }
    }
    public Vector2 Size
    {
        get
        {
            return new Vector2(maxX - minX, maxY - minY);
        }
    }
    public static Rectangle operator *(Rectangle rectangle, float f) => new Rectangle(rectangle.Center,rectangle.Size*f);
   
}



