using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : IControl
{
    public bool CanDouble()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
    }

    public bool CanSlide()
    {
        return Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
    }

    public bool Default()
    {
        return !CanDouble() && !CanSlide();
    }
}
