using UnityEngine;

public class PlayerController : MonoBehaviour, IStepSystem
{
    public Location location;
    public GameObject player;
    public GameObject playerCamera;
    public float cameraHeight;
    private Cell playerCell;

    void Start()
    {
        playerCell = player.GetComponent<Cell>();
        var pos = transform.position;
        playerCamera.transform.position = new Vector3(pos.x, cameraHeight, pos.z);
        playerCamera.transform.Rotate(90, 0, 0);
    }

    public StepResult Step()
    {
        var result = new StepResult(StepAction.Wait);

        Vector2Int? newPos = null;

        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            newPos = playerCell.ToVec();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            newPos = playerCell.ToVec() + Vector2Int.up;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            newPos = playerCell.ToVec() + Vector2Int.down;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            newPos = playerCell.ToVec() + Vector2Int.left;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            newPos = playerCell.ToVec() + Vector2Int.right;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            newPos = playerCell.ToVec() + Vector2Int.up + Vector2Int.left;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            newPos = playerCell.ToVec() + Vector2Int.up + Vector2Int.right;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            newPos = playerCell.ToVec() + Vector2Int.down + Vector2Int.left;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            newPos = playerCell.ToVec() + Vector2Int.down + Vector2Int.right;
        }

        if (!newPos.HasValue)
        {
            return result;
        }

        if (location.Has(newPos.Value, typeof(ObstacleTag)))
        {
            return result.Message("Can't move, obstacle at " + newPos.Value, MessageType.Error);
        }

        if (!location.Has(newPos.Value, typeof(GroundTag)))
        {
            return result.Message("Can't move, no ground at " + newPos.Value, MessageType.Error);
        }

        location.MoveObject(player, playerCell, newPos.Value);

        var pos = transform.position;
        playerCamera.transform.position = new Vector3(pos.x, cameraHeight, pos.z);

        result.Action = StepAction.Continue;
        return result;
    }
}