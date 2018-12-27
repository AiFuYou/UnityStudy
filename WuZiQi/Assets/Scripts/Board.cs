using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    public GameObject CrossPrefab;
    const float CrossSize = 40;
    public const int CrossCount = 15;
    public const int Size = 560;
    public const int HalfSize = Size / 2;

    Dictionary<int, Cross> _crossMap = new Dictionary<int, Cross>();
    static int MakeKey(int x , int y)
    {
        return 10000 * x + y;
    }

    public void Reset()
    {
        foreach (Transform child in gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        _crossMap.Clear();
        MainLoop mainLoop = GetComponent<MainLoop>();


        for (int x = 0; x < Board.CrossCount; ++x)
        {
            for (int y = 0; y < Board.CrossCount; ++y)
            {
                var crossObj = GameObject.Instantiate<GameObject>(CrossPrefab);
                crossObj.transform.SetParent(gameObject.transform);
                crossObj.transform.localScale = Vector3.one;

                var pos = crossObj.transform.localPosition;
                pos.x = -Board.HalfSize + x * CrossSize;
                pos.y = -Board.HalfSize + y * CrossSize;
                pos.z = 1;
                crossObj.transform.localPosition = pos;

                var cross = crossObj.GetComponent<Cross>();
                cross.GridX = x;
                cross.GridY = y;
                cross.mainLoop = mainLoop;

                _crossMap.Add(MakeKey(x, y), cross);
            }
        }
    }

    public Cross GetCross (int gridX, int gridY)
    {
        Cross cross;
        if (_crossMap.TryGetValue(MakeKey(gridX, gridY), out cross))
        {
            return cross;
        }
        return null;
    }

	// Use this for initialization
	void Start () {
        Reset();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
