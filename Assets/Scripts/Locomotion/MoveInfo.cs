using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MoveInfo
{
    public int X { get; }
    public MoveType Mode { get; }

    public MoveInfo(int x, MoveType moveType) => (X, Mode) = (x, moveType);
}
