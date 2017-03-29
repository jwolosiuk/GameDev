using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pad : IControl
{
    public bool CanDouble()
    {
        return (Input.GetAxis("Vertical") > 0.1);
    }

    public bool CanSlide()
    {
        return (Input.GetAxis("Vertical") < -0.1);
    }

    public bool Default()
    {
        return !CanDouble() && !CanSlide();
    }

 
}
