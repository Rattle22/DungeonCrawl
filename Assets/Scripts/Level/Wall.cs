using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().size = GetComponent<BoxCollider2D>().size;
	}

    //TODO: Implement variable tileset to add visual variety
    public void setTileset(string tilesetName) {

    }
}
