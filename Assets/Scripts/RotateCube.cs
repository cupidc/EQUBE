using UnityEngine;

public class RotateCube : MonoBehaviour
{
    public GameObject target;
    public bool swipeOnly;
    
    private Vector2 _firstPressPos;
    private Vector2 _secondPressPos;
    private Vector2 _currentSwipe;
    private Vector3 _previousMousePosition;
    private Vector3 _mouseDelta;
    public AudioClip swipeLeftSFX;
    public AudioClip swipeRightSFX;
    public AudioClip swipeUpSFX;
    public AudioClip swipeDownSFX;
    public AudioClip swipeUpLeftSFX;
    public AudioClip swipeUpRightSFX;
    public AudioClip swipeDownLeftSFX;
    public AudioClip swipeDownRightSFX;

    AudioSource audioSource;
    
    // int cubeStatus = 0; // when cube is idle.
    private const float Speed = 200f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        Swipe();
        Drag();
    }

    private void Drag()
    {
        if (Input.GetMouseButton(1) && swipeOnly == false)
        {
            // while the mouse button is held down the cube can be moved around its central axis to provide visual feedback
            _mouseDelta = Input.mousePosition - _previousMousePosition;
            _mouseDelta *= 0.1f; // reduction of rotation speed
            transform.rotation = Quaternion.Euler(_mouseDelta.y, -_mouseDelta.x, 0) * transform.rotation;
        }
        else
        {
            // automatically move to the target position
            if (transform.rotation != target.transform.rotation)
            {
                var step = Speed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, step);
            }
        }

        _previousMousePosition = Input.mousePosition;
    }

    private void Swipe()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // get the 2D position of the first mouse Click
            _firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        if (!Input.GetMouseButtonUp(1))
            return;
        
        // get the 2nd position of the second mouse click
        _secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        // create a vector from the first and second click positions
        _currentSwipe = new Vector2(_secondPressPos.x - _firstPressPos.x, _secondPressPos.y - _firstPressPos.y);
        // normalize the 2D vector
        _currentSwipe.Normalize();

        if (LeftSwipe(_currentSwipe))
        {
            target.transform.Rotate(0, 90, 0, Space.World);
            audioSource.PlayOneShot(swipeLeftSFX, 0.7F);
        }
        else if (RightSwipe(_currentSwipe))
        {
            target.transform.Rotate(0, -90, 0, Space.World);
            audioSource.PlayOneShot(swipeRightSFX, 0.7F);
        }
        else if (UpLeftSwipe(_currentSwipe))
        {
            target.transform.Rotate(90, 0, 0, Space.World);
            audioSource.PlayOneShot(swipeUpLeftSFX, 0.7F);
        }
        else if (UpRightSwipe(_currentSwipe))
        {
            target.transform.Rotate(0, 0, -90, Space.World);
            audioSource.PlayOneShot(swipeUpRightSFX, 0.7F);
        }
        else if (DownLeftSwipe(_currentSwipe))
        {
            target.transform.Rotate(0, 0, 90, Space.World);
            audioSource.PlayOneShot(swipeDownLeftSFX, 0.7F);
        }
        else if (DownRightSwipe(_currentSwipe))
        {
            target.transform.Rotate(-90, 0, 0, Space.World);
            audioSource.PlayOneShot(swipeDownRightSFX, 0.7F);
        }
    }

    private bool LeftSwipe(Vector2 swipe)
    {
        return _currentSwipe.x < 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f;
    }

    private bool RightSwipe(Vector2 swipe)
    {
        return _currentSwipe.x > 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f;
    }

    private bool UpLeftSwipe(Vector2 swipe)
    {
        return _currentSwipe.y > 0 && _currentSwipe.x < 0f;
    }

    private bool UpRightSwipe(Vector2 swipe)
    {
        return _currentSwipe.y > 0 && _currentSwipe.x > 0f;
    }

    private bool DownLeftSwipe(Vector2 swipe)
    {
        return _currentSwipe.y < 0 && _currentSwipe.x < 0f;
    }

    private bool DownRightSwipe(Vector2 swipe)
    {
        return _currentSwipe.y < 0 && _currentSwipe.x > 0f;
    }
}