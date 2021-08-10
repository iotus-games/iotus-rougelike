using System;
using System.Collections.Generic;
using UnityEngine;

public class StepState : MonoBehaviour
{
    public InfoMessage logger;
    
    public void AddStepObject(GameObject obj)
    {
        stepsObjects.Add(obj);
    }

    public void RemoveStepObject(GameObject obj)
    {
        stepsObjects.Remove(obj);
    }

    public GameObject CurrentStepObject()
    {
        if (stepsObjects.Count == 0)
        {
            throw new Exception("Level doesn't contain any step objects");
        }

        return stepsObjects[currentStepObjectIndex];
    }

    public void NextStep()
    {
        currentStepObjectIndex += 1;
        if (currentStepObjectIndex == stepsObjects.Count)
        {
            currentStepObjectIndex = 0;
        }
        
        logger.Message("Step: " + CurrentStepObject().name);
    }

    private int currentStepObjectIndex = 0;
    private List<GameObject> stepsObjects = new List<GameObject>();
}