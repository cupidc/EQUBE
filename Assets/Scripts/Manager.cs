using System;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Dictionary<int, Side> Sides;
    public Dictionary<string, Face> Faces;

    public Cube cube;
    
    private Side[] _sides;
    private Face[] _faces;

    private AudioSource AudioSource { get; set; }

    private void Awake()
    {
        Gameplay.Instance = this;
        AudioSource = GetComponent<AudioSource>();
        
        _sides = FindObjectsOfType<Side>();
        _faces = FindObjectsOfType<Face>();
    }

    private void Start()
    {
        AudioSource.Play();
        
        cube.ColourStats = new Dictionary<Material, int>();
        Sides = new Dictionary<int, Side>();
        Faces = new Dictionary<string, Face>();

        foreach (Material mat in cube.facePalette)
            cube.ColourStats.Add(mat, 0);
        
        foreach (Side side in _sides)
        {
            Sides.Add(side.id, side);
            
            if (side.id > cube.activeSides)
                continue;

            side.IsActive = true;
        }
        
        // assign a random colour to each face
        foreach (Face face in _faces)
        {
            // disable faces of disabled sides
            if (face.Side > cube.activeSides)
            {
                face.gameObject.SetActive(false);
                continue;
            }

            face.Renderer.material = cube.blankFaceMaterial;
            (face.Material, face.lockMaterial) = ColourRandomizer.RandomFromList(cube.facePalette, cube.lockedFacePalette);
            cube.ColourStats[face.Material] += 1;
            Faces.Add(face.name, face);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public static class Gameplay
    {
        public static bool IsPaused { get; set; }
        public static Manager Instance { get; set; }
    }
}