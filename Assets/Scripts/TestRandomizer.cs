using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TestRandomizer : MonoBehaviour
{
    public List<Material> materials;
    public List<Material> faces;

    private void Start()
    {
        foreach (Material mat in materials)
            Assert.IsNotNull(mat);
        
        //if (materials.Count > 0 && faces.Count > 0)
            //ColourRandomizer.RandomizeList(materials, faces);
        
        foreach (Material mat in faces)
            Assert.IsNotNull(mat);
    }
}