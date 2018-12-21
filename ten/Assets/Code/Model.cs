using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour {

    public int width = 4;
    public int height = 7;
	public Cube[] list = null;

    internal Cube getCube(int x, int y)
    {
        int n = x + y * 4;
        return list[n];
    }
}
