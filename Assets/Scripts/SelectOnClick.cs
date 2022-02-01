using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SelectOnClick : MonoBehaviour
{
    public LayerMask layerMask;
    public Material hitMaterial;
    public Score score;
    public float hintTime;
    public UnityEvent<bool, Material> onHint;
    public GameObject levelComplete;

    private GameObject _selectedObject;
    private GameObject _hitObject;
    private RaycastHit _hitData;
    private Ray _ray;
    private Camera _camera;
    private Manager _manager;
    private Material _blankMaterial;
    private Material _selectedMaterial;
    private Material _lockedMaterial;
    private int _count;
    private float _idleTime = 0;
    

    private void Start()
    {
        _camera = Camera.main;
        _manager = Manager.Gameplay.Instance;
        _blankMaterial = _manager.cube.blankFaceMaterial;
        _selectedMaterial = _blankMaterial;
    }

    private void HintTimeProcessor()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _idleTime = 0;
            onHint.Invoke(false, null);
        }
        else if (_selectedMaterial != _blankMaterial)
        {
            _idleTime += Time.deltaTime;
            if (_idleTime > hintTime)
            {
                onHint.Invoke(true, _selectedMaterial);
                _idleTime = 0;
            }
        }
    }

    private void HighLightHoveredFace()
    {
        // if current hit object is different from stored hit object...
        if (_hitData.transform.gameObject != _hitObject)
        {
            // 1 ...if any stored hit object reset if not selected TO blank
            if (_hitObject)
                if (_manager.Faces[_hitObject.name].Selected == false && _manager.Faces[_hitObject.name].Active == true)
                {
                    _manager.Faces[_hitObject.name].Renderer.material = _blankMaterial;
                }
                
            // 2 ...store current hit object...
            _hitObject = _hitData.transform.gameObject;
                
            // ...and if NOT selected yet change colour to hit colour
            if (_manager.Faces[_hitObject.name].Selected == false  && _manager.Faces[_hitObject.name].Active == true)
                _manager.Faces[_hitObject.name].Renderer.material = hitMaterial;
        }
    }   

    private void ClickedFace()
    {
        // select and store current hit object
        _selectedObject = _hitData.transform.gameObject;
        Face face = _manager.Faces[_selectedObject.name];
        if (face.Active == false)
        {
            return;
        }
        
        _count++;
        
        // apply colour
        face.Renderer.material = face.Material;
        face.Selected = true;
        face.Active = false;//
                
            // decrease matching colour faces left
        if (_manager.cube.ColourStats[face.Material] > 0)
            _manager.cube.ColourStats[face.Material] -= 1;
        if (LevelComplete())
        {
            //print("Level complete");
            levelComplete.SetActive(true);
            //SceneManager.LoadScene("LevelComplete");
        }
                
        // if selected colour is blank store current selected colour
        if (_selectedMaterial == _blankMaterial)
            _selectedMaterial = face.Material;
                
        if (_manager.cube.ColourStats[_selectedMaterial] > 0)
        {
            // if selected colour is different from current hit colour reset everything
            if (_selectedMaterial == face.Material)
            {
                if (_count > 1)
                {
                    score.AddPoints((_count - 1) * 5);

                    Debug.Log("Multiplier");
                }
            }
            else
            {
                if (_count > 2)
                {
                    face.Active = true;
                    DeActivateMatchedFaces();
                }
                score.SetMultiplier(Mathf.Clamp(_count - 1, 0, 3));
                StartCoroutine(ResetFaces(1));
            }
        }
        else
        {
            _selectedMaterial = _blankMaterial;
        }
    }

    bool LevelComplete()
    {
        // cheat
        if (Input.GetKey(KeyCode.G)) return true;

        bool complete = true;
        foreach (Material key in _manager.cube.ColourStats.Keys) {
            print(key + " : " + _manager.cube.ColourStats[key]);
            if (_manager.cube.ColourStats[key] != 0)
            {
                complete = false;
            }
        }
        return complete;    
    }
   
    private void Update()
    {
        if (Manager.Gameplay.IsPaused)
            return;
        
        //HintTimeProcessor();
        
        _ray = _camera.ScreenPointToRay(Input.mousePosition);
        
        // if mouse hits an object
        if (Physics.Raycast(_ray, out _hitData, Mathf.Infinity, layerMask))
        {
            HighLightHoveredFace();
            
            if (Input.GetMouseButtonDown(0) && _manager.Faces[_hitObject.name].Selected == false)
            {
                ClickedFace();

            }
        }
        else if (_hitObject != null)
        {
            Face face = _manager.Faces[_hitObject.name];
            if (!face.Selected && face.Active)
            {
              //  Debug.Log("Blank material", face.Renderer);
                face.Renderer.material = _blankMaterial;
                _hitObject = null;
            }
        }
    }

    // reset all faces to blank after delay and pause gameplay during the process
    private IEnumerator ResetFaces(float delay)
    {
        Manager.Gameplay.IsPaused = true;
        
        yield return new WaitForSeconds(delay);
        
        _selectedMaterial = _blankMaterial;

        foreach (KeyValuePair<string,Face> face in _manager.Faces)
        {
            // reset colour stats
            if (face.Value.Selected)
            {
                _manager.cube.ColourStats[face.Value.Material] += 1;
                face.Value.Renderer.material = _blankMaterial;
                face.Value.Selected = false;
                face.Value.Active = true;
            }
                
        }
        
        Manager.Gameplay.IsPaused = false;
        _count = 0;
    }

    private void DeActivateMatchedFaces()
    {
        foreach (KeyValuePair<string,Face> face in _manager.Faces)
        {
            if (face.Value.Active == false)
            {
                face.Value.Selected = false;
                face.Value.Renderer.material = face.Value.lockMaterial; 
            }
        }
    }
}