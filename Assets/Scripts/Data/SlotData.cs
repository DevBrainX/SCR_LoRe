using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class SlotData
{
    public BoxBehaviour placedBox;
    public int index;
    public Vector3 pos;
    //public bool hasBox;

    public SlotData(int _index, Vector3 _pos)
    //public SlotData(int _index, Vector3 _pos, bool _hasBox = false)
    {
        placedBox = null;
        index = _index;
        pos = _pos;
        //hasBox = _hasBox;
    }
}
