using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableCrystal : Crystal
{
    public GameObject leftovers;

	public virtual void DestroyMe()
    {
        GameObject stones = Instantiate(leftovers, transform.position, transform.rotation) as GameObject;
        stones.transform.localScale = transform.localScale;
        Destroy(this.gameObject);
        Destroy(stones, 10f);
    }
}
