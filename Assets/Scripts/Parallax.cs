using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float scrollSpeed = 3f;
    private float width;

    private void Start()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        width = (texture.width / sprite.pixelsPerUnit * transform.localScale.x) - 2f;
    }

    private void Update()
    {
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        if (transform.position.x <= -width)
        {
            transform.position += new Vector3(width * 2f, 0f, 0f);
        }
    }
}
