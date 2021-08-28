using Actions;
using UnityEngine;

// Считывает команды игрока для осуществления хода
public class PlayerController : MonoBehaviour, IStepSystem
{
    public GameObject playerCamera;
    public float cameraHeight;
    public Location location;

    private Cell playerCell;
    private UnitActions unitActions;

    void Start()
    {
        playerCell = GetComponent<Cell>();
        unitActions = GetComponent<UnitActions>();

        var pos = transform.position;
        playerCamera.transform.position = new Vector3(pos.x, cameraHeight, pos.z);
        playerCamera.transform.Rotate(90, 0, 0);
    }

    public StepAction Step(UI.Logger logger)
    {
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
            return StepAction.Wait;
        }

        var action = unitActions.GetActionComponent<Move>("Move");
        action.position = newPos.Value;

        if (!unitActions.TryCast(logger, "Move"))
        {
            return StepAction.Wait;
        }
       
        var pos = transform.position;
        playerCamera.transform.position = new Vector3(pos.x, cameraHeight, pos.z);

        return StepAction.Continue;
    }
}