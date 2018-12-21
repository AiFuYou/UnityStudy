using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play : MonoBehaviour {

    private Square[] list_ = null;

    void Awake ()
    {
        list_ = this.GetComponentsInChildren<Square>();
        Debug.Log(list_.Length);
    }

	// Use this for initialization
	void Start () {
        list_[0].gameObject.SetActive(false);
        foreach (Square square in list_)
        {
            //square.hide();
        }
	}

    internal Square getSquare(int v1, int v2)
    {
        int n = v1 + v2 * 4;
        return list_[n];
    }
}
