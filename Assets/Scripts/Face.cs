using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Face : MonoBehaviour
{
    public Renderer Renderer { get; private set; }
    public Material Material { get; set; }
    public Material lockMaterial { get; set; }
    public bool Selected { get; set; }
    public int Side { get; set; }
    public bool Active { get; set; }
    
    private void Awake()
    {
        Active = true;
        Renderer = GetComponent<Renderer>();
    }
}