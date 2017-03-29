using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRandomizer : MonoBehaviour
{
    public Color[] palette;
    public SpriteRenderer[] sprites;

	void Start ()
    {
        if(palette.Length>0)
        {
            int colorIndex = Random.Range(0, palette.Length);
            foreach(SpriteRenderer sprite in sprites)
                sprite.color = palette[colorIndex];
        }
	}

}
