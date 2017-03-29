using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgorEventSystemS : MonoBehaviour {

    public string[] eventsList;

    public delegate void eventDelegate(EventInfoS e);
    eventDelegate[] events;

    public bool Subscribe(string eventName, eventDelegate client)
    {

    }

    public bool Unsubscribe(string eventName, eventDelegate client)
    {
    }

    public bool Call(string eventName, IgorEventSystemS caller)
    {

    }

}
