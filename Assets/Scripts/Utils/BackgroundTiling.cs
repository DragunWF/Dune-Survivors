using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Creates a seamless, infinite tiling world using a 3x3 grid of tilemap clones.
/// This script should be placed on the primary GameObject that contains all tilemap layers
/// meant to be repeated. It will automatically create 8 clones of itself and arrange them
/// in a grid. As the player moves, the grid shifts to keep the player in the center tile,
/// creating a smooth, infinite effect.
/// </summary>
public class BackgroundTiling : MonoBehaviour
{
    public Transform playerTransform; // Assign in inspector for reliability

    private Vector2 worldSize;
    private Transform[,] grid = new Transform[3, 3];
    private int currentX = 1;
    private int currentY = 1;

    void Awake()
    {
        if (playerTransform == null)
        {
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogError("Player object not found! BackgroundTiling will be disabled. Please assign the player's Transform in the inspector.");
                this.enabled = false;
                return;
            }
        }
    }

    void Start()
    {
        // --- 1. Calculate World Size ---
        Tilemap tilemap = GetComponentInChildren<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("No Tilemap component found in children. Cannot determine world size.");
            this.enabled = false;
            return;
        }
        worldSize = Vector3.Scale(tilemap.cellBounds.size, tilemap.layoutGrid.cellSize);
        if (worldSize.x <= 0 || worldSize.y <= 0)
        {
            Debug.LogError("World size calculated as zero or negative. Disabling script.");
            this.enabled = false;
            return;
        }

        // --- 2. Create and Position the 3x3 Grid ---
        // The original object is the center of our grid at the start.
        grid[1, 1] = transform;

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (x == 1 && y == 1) continue; // Skip the center (it's the original)

                Vector3 positionOffset = new Vector3((x - 1) * worldSize.x, (y - 1) * worldSize.y, 0);

                // We instantiate a clone of the original object
                Transform newTile = Instantiate(transform, transform.position + positionOffset, Quaternion.identity);
                newTile.name = $"{transform.name}_Clone({x},{y})";

                // CRITICAL: Disable the script on the clone to prevent an infinite loop of instantiation.
                newTile.GetComponent<BackgroundTiling>().enabled = false;

                // Parent to the same object as the original for a clean hierarchy
                newTile.parent = transform.parent;
                grid[x, y] = newTile;
            }
        }
    }

    void LateUpdate()
    {
        Transform centerTile = grid[currentX, currentY];
        Vector3 centerPos = centerTile.position;

        float deltaX = playerTransform.position.x - centerPos.x;
        float deltaY = playerTransform.position.y - centerPos.y;

        // --- 3. Check for Grid Shift ---
        if (Mathf.Abs(deltaX) > worldSize.x / 2f)
        {
            int moveDir = (int)Mathf.Sign(deltaX);
            ShiftGridHorizontal(moveDir);
        }

        if (Mathf.Abs(deltaY) > worldSize.y / 2f)
        {
            int moveDir = (int)Mathf.Sign(deltaY);
            ShiftGridVertical(moveDir);
        }
    }

    private void ShiftGridHorizontal(int moveDir)
    {
        // Identify the column to move and its new position
        int oldColumnIndex = (currentX - moveDir + 3) % 3;

        // Move the entire column
        for (int y = 0; y < 3; y++)
        {
            Transform tileToMove = grid[oldColumnIndex, y];
            tileToMove.position += new Vector3(3 * worldSize.x * moveDir, 0, 0);
        }

        // The new center is one step in the move direction
        currentX = (currentX + moveDir + 3) % 3;
    }

    private void ShiftGridVertical(int moveDir)
    {
        // Identify the row to move and its new position
        int oldRowIndex = (currentY - moveDir + 3) % 3;

        // Move the entire row
        for (int x = 0; x < 3; x++)
        {
            Transform tileToMove = grid[x, oldRowIndex];
            tileToMove.position += new Vector3(0, 3 * worldSize.y * moveDir, 0);
        }

        // The new center is one step in the move direction
        currentY = (currentY + moveDir + 3) % 3;
    }
}
