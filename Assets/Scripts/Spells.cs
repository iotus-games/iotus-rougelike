using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Что делают навыки/способности?
// -- Применяются в определенном радиусе
// -- Действуют временно или мгновенно
// -- Накладывают определенные эффекты

class SpellInfo
{
    public SpellInfo(Vector2 position, Vector2 area, uint stepsRemain)
    {
        this.Position = position;
        this.Area = area;
        this.StepsRemain = stepsRemain;
    }

    public Vector2 Position; // Левый верхний угол
    public Vector2 Area;
    public uint StepsRemain;
}

public class Spells : MonoBehaviour
{
    public Level level = null;

    public void CastSpell(Vector2 position, Func<> Vector2 area, uint stepsDuration = 1)
    {
        if (stepsDuration == 0)
        {
            return;
        }

        spells.Add(new SpellInfo(position, area, stepsDuration));
        for (var i = 0; i < area.x; i++)
        {
            for (var j = 0; j < area.y; j++)
            {
            }
        }
    }

    public void CastSpell(Vector2 position, uint stepsDuration = 1)
    {
        spells.Add(new SpellInfo(position, new Vector2(1, 1), stepsDuration));
    }

    public void NextStep()
    {
        foreach (var spell in spells)
        {
            CastSpell(spell.Position, spell.Area, spell.StepsRemain - 1);
        }

        spells.Clear();
    }

    void Start()
    {
    }

    void Update()
    {
    }

    private List<SpellInfo> spells;
}