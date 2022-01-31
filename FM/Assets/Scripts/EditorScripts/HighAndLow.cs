using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*FLOWERBOX
 * Programmer: Patrick Naatz
 * Intention: Make a class capable of automatically sorting highest and lowest values in accordance with the grouping tool
 * Reason: Makes reading the code much simpler
 */
public class HighAndLow
{
    #region Properties
    #region High
    public float High
    {
        get
        {
            return high;
        }

        set
        {
            high = Mathf.Max(value, high);
        }
    }
    float high = float.NegativeInfinity;
    #endregion

    #region Low
    public float Low
    {
        get
        {
            return low;
        }

        set
        {
            low = Mathf.Min(value, low);
        }
    }
    float low = float.PositiveInfinity;
    #endregion

    public float Center
    {
        get
        {
            return (high + low) / 2;
        }
    }
    #endregion

    #region Constructors
    public HighAndLow() { }

    public HighAndLow(float f)
    {
        Set(f);
    }
    #endregion

    /// <summary>
    /// Use this method to apply a new value
    /// </summary>
    /// <param name="value"></param>
    public void Set(float value)
    {
        High = value;
        Low = value;
    }

    public static implicit operator float(HighAndLow hal) => hal.Center;
}
