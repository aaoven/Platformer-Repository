using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    Vector2 startPos;
    SpriteRenderer spriteRenderer;
    Rigidbody2D playerRb;

    AudioManager audioManager;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerRb = GetComponent<Rigidbody2D>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    

    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hazard"))
        {
            Die();
            audioManager.PlaySFX(audioManager.death);
        }
    }

    IEnumerator RespawnTimer(float duration)
    {
        playerRb.simulated = false;
        playerRb.velocity = new Vector2(0, 0);
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(duration);
        transform.position = startPos;
        spriteRenderer.enabled = true;
        playerRb.simulated = true;
    }
    

    void Die()
    {
        StartCoroutine(RespawnTimer(0.5f));
    }

    
}
