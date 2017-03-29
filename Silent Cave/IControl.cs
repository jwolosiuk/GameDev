using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControl
{
    bool CanDouble();
    bool CanSlide();
    bool Default();
}
