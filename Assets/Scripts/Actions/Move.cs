using Steps;
using UnityEngine;

namespace Actions
{
    public class Move : MonoBehaviour, IActionSystem
    {
        public Location location;
        public Vector2Int position;
        
        private Cell playerCell;

        public void Prepare(GameObject caller)
        {
            playerCell = caller.GetComponent<Cell>();
        }

        public bool CanCast(UI.Logger logger)
        {
            if (location.Has(position, typeof(ObstacleTag)))
            {
                logger.Message("Can't move, obstacle at " + position, UI.MessageType.Error);
                return false;
            }

            if (!location.Has(position, typeof(GroundTag)))
            {
                logger.Message("Can't move, no ground at " + position, UI.MessageType.Error);
                return false;
            }

            return true;
        }

        public StepAction Step(UI.Logger logger)
        {
            location.MoveObject(playerCell.gameObject, playerCell, position);
            return StepAction.Continue;
        }
    }
}