using UnityEngine;


public class PlayerInputHandler : MonoBehaviour
{
    private PlayerControls _controls;
    
    public Vector2 MoveInput { get; private set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _controls = new PlayerControls();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
