using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public Dictionary<Material, int> ColourStats { get; set; }

    public List<Material> facePalette;
    public List<Material> lockedFacePalette;
    public Material blankFaceMaterial;
    public Material blankSideMaterial;
    public int activeSides;
    public Animator faceAnimator;

    public Side[] sides;
    public Animator[] sideEffects;

    int bestSide = 0;

    public void playHint(bool enable, Material selectedColour)
    {
        if (enable)
        {
            // find the side with the most selected Colour
            int maxFaces = 0;
            for (int i = 0; i < sides.Length; ++i)
            {
                int faces = sides[i].GetColourCount(selectedColour);
                Debug.Log($"face {i} has {faces} faces");
                if (faces > maxFaces)
                {
                    bestSide = i;
                    maxFaces = faces;
                }
            }
        }

        sideEffects[bestSide].gameObject.SetActive(enable);
        sideEffects[bestSide].SetTrigger("Hint");
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 200, facePalette.Count * 30 + 10), "COLOUR STATS");

        int currentLine = 1;
        
        foreach (Material mat in facePalette)
        {
            currentLine++;
            GUI.Label(new Rect(25, currentLine * 20, 100, 30), $"{mat.name}: {ColourStats[mat]}");
        }
    }
}