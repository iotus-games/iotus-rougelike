using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Actions
{
    Move = 0,
    MeleeAttack = 1,
    Heal = 2,
}

struct ActionState
{
    public ActionState(bool active = true, uint stepsRemain = 0)
    {
        Active = active;
        StepsRemain = stepsRemain;
    }

    public bool Active;
    public uint StepsRemain;
}

public class UnitActions : MonoBehaviour
{
    void Start()
    {
        cell = GetComponent<Cell>();
    }

    void Update()
    {
    }
    
    

    public void Move(Vector2Int newPosition)
    {
        location.MoveObject(gameObject, cell, newPosition);
        //state[(int) Actions.Move].StepsRemain;
    }

    

    public Location location;
    private Cell cell;
    private ActionState[] state = new ActionState[3];
}