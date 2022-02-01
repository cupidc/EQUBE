using System.Collections.Generic;
using UnityEngine;

public static class ColourRandomizer
{
    public static (Material, Material) RandomFromList(List<Material> select, List<Material> select2)
    {
        var n = Random.Range(0, select.Count);
        return (select[n], select2[n]);
        //return select[Random.Range(0, select.Count)];
    }
/*
    public static void RandomizeList(List<Material> materials, List<Material> faces)
    {
        List<Material> select = new List<Material>();
        
        while (select.Count < faces.Count)
        {
            foreach (Material mat in materials)
            {
                select.Add(mat);
                
                if (select.Count == faces.Count)
                    break;
            }
        }

        for (int i = 0; i < faces.Count; i++)
            faces[i] = RandomFromList(select);
    }
    */
}