using UnityEngine;
using System.Collections;

public class SpriteVisibility : MonoBehaviour {
    PlayerMovement father;
    void Start()
    {
        father = GetComponentInParent<PlayerMovement>();
    }

    void OnBecameInvisible()
    {
            father.gameManager.GetComponent<GameManager>().KonczGre(father.tag);
            father.enabled = false;
            Destroy(father);

    }

}
