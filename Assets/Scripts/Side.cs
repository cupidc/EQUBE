using System;
using UnityEngine;
using UnityEngine.Assertions;

public class Side : MonoBehaviour
{
    public int id;
    public Color frameColour;
    
    private Face[] _faces;
    
    public bool IsActive { get; set; }
    
    private Renderer Renderer { get; set; }

    private void Awake()
    {
        Assert.IsTrue(id > 0 && id <= 6);
        
        _faces = GetComponentsInChildren<Face>();
        
        foreach (Face face in _faces)
            face.Side = id;
        
        Renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (IsActive)
        {
            Renderer.materials[1].color = frameColour;
        }
        else
        {
            Renderer.materials[1] = Manager.Gameplay.Instance.cube.blankSideMaterial;
        }
    }

    public int GetColourCount(Material selectedColour)
    {
        int count = 0;
        foreach (Face face in _faces)
        {
            if (!face.Selected && face.Material == selectedColour)
            {
                count++;
            }
        }
        return count;
    }
}