using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    //public GameObject levelObject;
    public GameObject testObject = null;
    public GameObject testCell = null;
    public Level level = null;
    
    void Start()
    {
        //level = levelObject.GetComponent<Level>();
        
        // сгенерировать пробный уровень
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                level.Add(new Vector2(i, j), Instantiate(testCell), Layer.Ground);
                if (i % 3 == 0 && j % 3 == 0)
                {
                    level.Add(new Vector2(i, j), Instantiate(testObject), Layer.Object);
                }
            }
        }    
    }

    void Update()
    {
        
    }
}
