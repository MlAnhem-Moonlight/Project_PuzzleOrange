using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float tileSize = 1f; // Kích thước 1 tile
    public LayerMask obstacleLayer; // Lớp chướng ngại vật (Không thể di chuyển qua)
    public LayerMask pieceLayer; // Lớp của vật thể có thể di chuyển
    public bool isBlocked = false; // Biến kiểm tra xem có bị chặn hay không
    private Vector2 targetPosition;
    private Vector2 prevPosition;

    void Start()
    {
        // Khởi tạo vị trí mục tiêu ban đầu là vị trí hiện tại
        targetPosition = transform.position;
        prevPosition = targetPosition;

        // Kiểm tra trạng thái ngay từ khi khởi động
        UpdateBlockedStatus();
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, targetPosition) > 0.01f)
        {
            // Trước khi di chuyển, cập nhật vị trí trước đó
            prevPosition = transform.position;

            // Di chuyển đến vị trí mục tiêu một cách mượt mà
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, tileSize * Time.deltaTime * 10);

            // Sau khi di chuyển, cập nhật trạng thái bị chặn
            UpdateBlockedStatus();
            return;
        }

        Vector2 direction = Vector2.zero;

        // Kiểm tra phím nhấn để xác định hướng
        if (Input.GetKeyDown(KeyCode.UpArrow)) direction = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) direction = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) direction = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) direction = Vector2.right;

        if (direction != Vector2.zero)
        {
            Vector2 newPosition = targetPosition + direction * tileSize;
            Collider2D pieceCollider = Physics2D.OverlapPoint(newPosition, pieceLayer);

            // Cập nhật trạng thái bị chặn TRƯỚC khi kiểm tra vị trí mới
            UpdateBlockedStatus();

            if (Physics2D.OverlapPoint(newPosition, obstacleLayer))
            {
                isBlocked = true;
                Debug.Log(name + ": Di chuyển bị chặn bởi vật cản cố định. " + direction);
                return;
            }

            // Kiểm tra vật thể di chuyển được
            if (pieceCollider != null && pieceCollider.TryGetComponent(out MoveObject pieceMovement))
            {
                if (pieceMovement.isBlocked)
                {
                    Debug.Log("Di chuyển bị chặn bởi khối phía trước.");
                    return;
                }

                if (Vector2.Distance(pieceMovement.prevPosition, pieceMovement.targetPosition) > 0)
                {
                    targetPosition = pieceMovement.prevPosition;
                    Debug.Log($"Cập nhật vị trí sang: {targetPosition}");
                    return;
                }
            }

            targetPosition = newPosition;
        }
    }

    void UpdateBlockedStatus()
    {
        // Kiểm tra xem vật thể hiện tại có bị chặn hay không
        isBlocked = Physics2D.OverlapPoint(transform.position, obstacleLayer);
    }
}
