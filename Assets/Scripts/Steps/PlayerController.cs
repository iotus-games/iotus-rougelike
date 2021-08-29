using Actions;
using UnityEngine;

namespace Steps
{
// Считывает команды игрока для осуществления хода
    public class PlayerController : MonoBehaviour, IStepSystem
    {
        private Cell playerCell;
        private UnitActions unitActions;

        void Start()
        {
            playerCell = GetComponent<Cell>();
            unitActions = GetComponent<UnitActions>();
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

            return StepAction.Continue;
        }
    }
}