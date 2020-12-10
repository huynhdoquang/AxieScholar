using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class MinMaxSliderAttribute : PropertyAttribute
{
    public readonly float min;
    public readonly float max;
    public readonly bool isInt;

    public MinMaxSliderAttribute(float min, float max)
    {
        this.isInt = false;
        this.min = min;
        this.max = max;
    }

    public MinMaxSliderAttribute(int min, int max)
    {
        this.isInt = true;
        this.min = min;
        this.max = max;
    }
}
