using System.Collections.Generic;
using UnityEngine;

public class MoveObjectMobile : MonoBehaviour
{
    public float tileSize = 1f;
    public LayerMask obstacleLayer;
    public LayerMask pieceLayer;
    private Vector2 targetPosition;
    private Vector2 prevPosition;

    public bool complete = false;
    public int blockID;

    private Vector2 swipeStart; // Vị trí bắt đầu vuốt
    private Vector2 swipeEnd;   // Vị trí kết thúc vuốt

    public Dictionary<Vector2, bool> blockedDirections = new Dictionary<Vector2, bool>
    {
        { Vector2.up, false },
        { Vector2.down, false },
        { Vector2.left, false },
        { Vector2.right, false }
    };

    private static readonly Dictionary<int, Dictionary<Vector2, int>> expectedNeighbors = new Dictionary<int, Dictionary<Vector2, int>>
    {
        { 1, new Dictionary<Vector2, int> { { Vector2.right, 2 }, { Vector2.down, 3 } } },
        { 2, new Dictionary<Vector2, int> { { Vector2.left, 1 }, { Vector2.down, 4 } } },
        { 3, new Dictionary<Vector2, int> { { Vector2.up, 1 }, { Vector2.right, 4 } } },
        { 4, new Dictionary<Vector2, int> { { Vector2.up, 2 }, { Vector2.left, 3 } } }
    };

    void Start()
    {
        targetPosition = transform.position;
        prevPosition = targetPosition;

        UpdateBlockedStatus();
    }

    void Update()
    {
        UpdateBlockedStatus();

        if (Vector2.Distance(transform.position, targetPosition) > 0.01f)
        {
            prevPosition = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, tileSize * Time.deltaTime * 10);
            return;
        }

        Vector2 direction = Vector2.zero;

        // Handle swipe input for movement
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    swipeStart = touch.position;
                    break;

                case TouchPhase.Ended:
                    swipeEnd = touch.position;
                    direction = DetectSwipeDirection(swipeStart, swipeEnd);
                    break;
            }
        }

        if (direction != Vector2.zero)
        {
            if (blockedDirections[direction])
            {
                Debug.Log("Di chuyển bị chặn bởi vật cản ở hướng: " + direction);
                return;
            }

            targetPosition = (Vector2)transform.position + direction * tileSize;
        }
    }

    void UpdateBlockedStatus()
    {
        var directions = new List<Vector2>(blockedDirections.Keys);

        foreach (var direction in directions)
        {
            Vector2 checkPosition = (Vector2)transform.position + direction * tileSize;

            if (Physics2D.OverlapPoint(checkPosition, obstacleLayer))
            {
                blockedDirections[direction] = true;
            }
            else
            {
                Collider2D pieceCollider = Physics2D.OverlapPoint(checkPosition, pieceLayer);
                if (pieceCollider != null && pieceCollider.TryGetComponent(out MoveObjectMobile pieceMovement))
                {
                    blockedDirections[direction] = pieceMovement.blockedDirections[direction];
                }
                else
                {
                    blockedDirections[direction] = false;
                }
            }
        }
    }

    private Vector2 DetectSwipeDirection(Vector2 start, Vector2 end)
    {
        Vector2 swipeDelta = end - start;

        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            return swipeDelta.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            return swipeDelta.y > 0 ? Vector2.up : Vector2.down;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("piece"))
        {
            if (CheckStructureComplete())
            {
                complete = true;
                Debug.Log("Cấu trúc đã hoàn thành!");
            }
            else
            {
                complete = false;
            }
        }
    }

    private bool CheckStructureComplete()
    {
        if (!expectedNeighbors.ContainsKey(blockID)) return false;

        var neighbors = expectedNeighbors[blockID];

        foreach (var neighbor in neighbors)
        {
            Vector2 direction = neighbor.Key;
            int expectedNeighborID = neighbor.Value;

            Vector2 checkPosition = (Vector2)transform.position + direction * tileSize;
            Collider2D pieceCollider = Physics2D.OverlapPoint(checkPosition, pieceLayer);

            if (pieceCollider == null || !pieceCollider.TryGetComponent(out MoveObjectMobile pieceMovement) || pieceMovement.blockID != expectedNeighborID)
            {
                return false;
            }
        }

        Debug.Log("Cấu trúc hoàn thành với ID: " + blockID);
        return true;
    }
}
