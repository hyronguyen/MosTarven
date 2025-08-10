using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; // Thêm dòng này

public class PlayerScripts : MonoBehaviour
{
    [Header("Player Setting")]
    public float movespeed;
    private Vector3 playerInput;
    private SpriteRenderer playerSpriteRenderer;
    private Animator playerAnimator;

    [Header("Tilemap Reference")]
    public Tilemap tilemap; // Thêm dòng này

    void Start()
    {
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;

        if (playerInput != Vector3.zero)
        {
            transform.position += playerInput * movespeed * Time.deltaTime;
            playerAnimator.SetBool("playerIsRunning", true);

            if (playerInput.x != 0)
            {
                playerSpriteRenderer.flipX = playerInput.x < 0;
            }
        }
        else
        {
            playerAnimator.SetBool("playerIsRunning", false);
            Debug.Log("Player đứng yên");
        }

        // Sorting order theo tilemap giống BuildSystem
        if (tilemap != null)
        {
            Vector3Int cellPos = tilemap.WorldToCell(transform.position);
            int sortingOrder = -(cellPos.x * 10000) - cellPos.y;
            playerSpriteRenderer.sortingOrder = sortingOrder;
        }
    }
}
