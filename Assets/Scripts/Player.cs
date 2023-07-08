using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    Vector2 rawInput;

    [Header("Player Boundary Padding")]
    //[SerializeField] float paddingLeft;
    //[SerializeField] float paddingRight;
    [SerializeField] float paddingTop;
    [SerializeField] float paddingBottom;

    Vector2 minGameFieldBounds;
    Vector2 maxGameFieldBounds;
    Rigidbody2D rb;

    void Start()
    {
        initBounds();
        rb = GetComponent<Rigidbody2D>();
    }

    private void initBounds()
    {
        Camera mainCamera = Camera.main;
        minGameFieldBounds = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        maxGameFieldBounds = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));

        //Adjust screenbounds based on player sprite
        SpriteRenderer playerSprite = GetComponentInChildren<SpriteRenderer>();
        Vector2 spriteBounds = new Vector2(playerSprite.bounds.extents.x, playerSprite.bounds.extents.y);

        //Traps the sprite of the player inside the playable area
        minGameFieldBounds.x += spriteBounds.x;
        minGameFieldBounds.y += spriteBounds.y;
        maxGameFieldBounds.x -= spriteBounds.x;
        maxGameFieldBounds.y -= spriteBounds.y;

        //Player adjusted padding
        if (paddingTop > 0 || paddingBottom > 0)
        {
            // Have bottom and top padding be multiples of the size of the ship
            minGameFieldBounds.y += paddingBottom * (playerSprite.bounds.extents.y * 2);
            maxGameFieldBounds.y -= paddingTop * (playerSprite.bounds.extents.y * 2);
        }
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector2 delta = rawInput * moveSpeed * Time.deltaTime;
        Vector2 newPos = new Vector2();
        newPos.x = Mathf.Clamp(transform.position.x + delta.x, minGameFieldBounds.x, maxGameFieldBounds.x);
        newPos.y = Mathf.Clamp(transform.position.y + delta.y, minGameFieldBounds.y, maxGameFieldBounds.y);
        transform.position = newPos;
    }

    void OnMove(InputValue value)
    {
        rawInput = value.Get<Vector2>();
        Debug.Log(rawInput);
    }
}

