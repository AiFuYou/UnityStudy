using System;
using System.Collections;
using System.Collections.Generic;
using GDGeek;
using UnityEngine;

public class Play : MonoBehaviour {

    private Square[] list_ = null;
    public Square _phototype = null;

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

    internal Task moveTask(int number, Vector2 vector21, Vector2 vector22)
    {
        Debug.Log("sssssssssssssssssssssssssssss");

        Square s = (Square)GameObject.Instantiate(_phototype);
        Square b = this.getSquare((int)vector21.x, (int)vector21.y);
        Square e = this.getSquare((int)vector22.x, (int)vector22.y);

        b.hide();

        s.transform.parent = b.transform.parent;
        s.transform.localScale = b.transform.localScale;
        s.transform.localPosition = b.transform.localPosition;
        s.show();
        s.number = number;
        TweenTask tt = new TweenTask(delegate
        {
            return TweenLocalPosition.Begin(s.gameObject, 0.5f, e.transform.localPosition);
        });

        TaskManager.PushBack(tt, delegate
        {
            GameObject.DestroyObject(s.gameObject);
            //s.gameObject.Destroy();
        });

        return tt;
    }
}
