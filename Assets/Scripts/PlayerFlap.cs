using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlap : MonoBehaviour
{
    public float flyForce = 10f;
    public bool isDead = false;
    private Rigidbody2D rb;
    float time = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    void Update()
    {
        if (isDead) return;
        Score();

        if (Input.GetMouseButton(0))
        {
            rb.gravityScale = 0; // Tắt trọng lực
            rb.velocity = new Vector2(rb.velocity.x, flyForce + GameManager.Instance.hard);
        }
        else
        {
            rb.gravityScale = 1; // Bật trọng lực
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -5f, flyForce));
        }
    }

    void Score()
    {
        if (isDead || GameManager.Instance.State != GameManager.GameState.Game) return;
        time -= Time.deltaTime;
        if (time > 0) return;
        GameManager.Instance.Score(1);
        GameManager.Instance.HighScore();
        time = 0.5f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<ObstacleMove>())
        {
            if (isDead) return;
            AudioManager.Instance.sfxSource.PlayOneShot(AudioManager.Instance.die);
            isDead = true;
            GameManager.Instance.EndGame(false);
        }
    }
}