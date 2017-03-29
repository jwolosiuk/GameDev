using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : MonoBehaviour {
    public bool flipX = false;
    public string name2 = "Passerby";

    public void Init()
    {
        foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>()){
            renderer.flipX = flipX;
            renderer.sortingLayerName = name2;
        }
    }

    public void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
