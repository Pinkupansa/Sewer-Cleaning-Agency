using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    
    List<GameObject> points = new List<GameObject>();
    [SerializeField] float rigidity;
    
    
    [SerializeField] float addPointDistance;
    
    LineRenderer lR;
    [SerializeField] GameObject pointPrefab;
    [SerializeField] float linearMass;
    Transform caster;
    [SerializeField] float linearDrag;
    bool parametersSet = false;
    public void SetParameters(Material _material, Transform _caster, float _width)
    {
        lR = gameObject.AddComponent<LineRenderer>();
        lR.startWidth = _width;
        lR.endWidth = _width;
        caster = _caster;
        lR.sharedMaterial = _material;
        lR.positionCount = 1;
        AddPoint(0, caster.position);
        parametersSet = true;
    }

    public void FixedUpdate()
    {
        if (parametersSet)
        {
            CheckExtremePoints();
            UpdateLR();
            ApplyForces();
            
           
        }
        
    }
    void CheckExtremePoints()
    {
        float beginDist = Vector2.Distance(caster.position, points[0].transform.position);
        float endDist = Vector2.Distance(transform.position, points[points.Count - 1].transform.position);
        if(beginDist > addPointDistance)
        {
            AddPoint(0, caster.position);
        }
        if(endDist > addPointDistance)
        {
            AddPoint(points.Count, transform.position);
        }
    }
    void AddPoint(int index, Vector2 position)
    {
        GameObject point = Instantiate(pointPrefab, position, Quaternion.identity);
        if(index < points.Count)
        {
            points.Insert(index, point);
        }
        else
        {
            points.Add(point);
        }
        point.GetComponent<Rigidbody2D>().drag = linearDrag;
        point.GetComponent<Rigidbody2D>().mass = linearMass;
        point.transform.SetParent(transform);
        
        lR.positionCount = lR.positionCount + 1;
        
    }
    void UpdateLR()
    {
        lR.SetPosition(0, caster.position);
        for (int i = 0; i < points.Count; i++)
        {
            lR.SetPosition(i, points[i].transform.position);
        }
        lR.SetPosition(lR.positionCount - 1, transform.position);
    }
    void ApplyForces()
    {
       
        for (int i = 1; i < points.Count - 1; i++)
        {
            Vector2 left = points[i - 1].transform.position - points[i].transform.position;
            points[i].GetComponent<Rigidbody2D>().AddForce(left * rigidity);
            Vector2 right = points[i + 1].transform.position - points[i].transform.position;
            points[i].GetComponent<Rigidbody2D>().AddForce(right * rigidity);
        }
        
        //points[points.Count - 1].transform.position = transform.position;
    }
    
}
