using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearButton : MonoBehaviour
{
    public CycleObjects cycler;

    public void ClearCurrent()
    {
        TouchDraw td = cycler.GetCurrentTouchDraw();

        if (td != null)
        {
            td.ClearLines();
        }
    }
}
