using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameMaster gm;
    private SpriteRenderer spriteRenderer;
    public Sprite off;
    public Sprite on;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == null)
        {
            spriteRenderer.sprite = off;
        }
    }

    private void Update()
    {

    }
    
    //Sets the new Checkpoint Position
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Avatar"))
        {
            if (spriteRenderer.sprite == off)
            {
                spriteRenderer.sprite = on;
            }
            gm.lastCheckPointPos = transform.position;            
        }
    }
}
