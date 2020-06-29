using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector2 initialPosition;
    private Vector2 mousePosition;
    private float deltaX, deltaY;
    private float xPos,yPos;
    public bool Clingable;
    private AudioSource audioSource;
    private bool PlatformSoundPlaying;

    [Header("Boundary Settings")]
    public float PositiveXPosLimit, NegativeXPosLimit;
    public float PositiveYPosLimit, NegativeYPosLimit;

    void Start()
    {
        initialPosition = transform.position;
        switch (transform.eulerAngles.z)
        {
            case 0:
            case 180:
                Clingable = false;
                break;
            case 90:
            case 270:
                Clingable = true;
                break;
        }
        audioSource = GetComponent<AudioSource>();
        PlatformSoundPlaying = false;
    }

    private void Update()
    {
       
    }
    
    //When left click is pressed, this sets the Initial Position the mouse was on
    private void OnMouseDown()
    {
        deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
    }

    //Handles the movement of Platforms based on their Tags while dragging the mouse
    private void OnMouseDrag()
    {
        if (CompareTag("MovementX")) 
        { 
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            xPos = mousePosition.x - deltaX;
            transform.position = new Vector2(Mathf.Clamp(xPos, initialPosition.x -NegativeXPosLimit, initialPosition.x + PositiveXPosLimit), transform.position.y);
            PlaySound();
        }
        if (CompareTag("MovementY"))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            yPos = mousePosition.y - deltaY;
            transform.position = new Vector2(transform.position.x, Mathf.Clamp(yPos, initialPosition.y - NegativeYPosLimit, initialPosition.y + PositiveYPosLimit));
            PlaySound();
        }
        if (CompareTag("MovementXY"))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            xPos = mousePosition.x - deltaX;
            yPos = mousePosition.y - deltaY;
            transform.position = new Vector2(Mathf.Clamp(xPos, initialPosition.x - NegativeXPosLimit, initialPosition.x + PositiveXPosLimit), Mathf.Clamp(yPos, initialPosition.y - NegativeYPosLimit, initialPosition.y + PositiveYPosLimit));
            PlaySound();
        }
    }
    
    //Currently empty
    private void OnMouseUp()
    {
        audioSource.Pause();
        PlatformSoundPlaying = false;
    }

    private void PlaySound()
    {
        if (!PlatformSoundPlaying)
        {
            audioSource.Play();
            PlatformSoundPlaying = true;
            audioSource.loop = true;
        }
    }
}
