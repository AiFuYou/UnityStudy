using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDGeek;
using System;

public class Ctrl : MonoBehaviour {

    private FSM fsm_ = new FSM();
    public View _view = null;
    public void fsmPost(string msg)
    {
        Debug.Log(msg);
        fsm_.post(msg);
    }

    private State beginState()
    {
        StateWithEventMap state = new StateWithEventMap();
        state.onStart += delegate
        {
            _view.begin.gameObject.SetActive(true);
        };
        state.onOver += delegate
        {
            _view.begin.gameObject.SetActive(false);
        };
        state.addEvent("begin", "play");
        return state;
    }

    private State playState()
    {

        //StateWithEventMap state = TaskState.Create(delegate
        //{
        //    TaskWait tw = new TaskWait();
        //    tw.setAllTime(3f);
        //    return tw;
        //}, fsm_, "end");
        StateWithEventMap state = new StateWithEventMap();
        state.onStart += delegate
        {
            _view.play.gameObject.SetActive(true);
            //Square square = _view.play.getSquare(0, 1);
            //square.number = 7;

            //Square square2 = _view.play.getSquare(2, 1);
            //square2.number = 9;

            //square.show();
        };
        state.onOver += delegate
        {
            _view.play.gameObject.SetActive(false);
        };
        return state;
    }

    private State endState()
    {
        StateWithEventMap state = new StateWithEventMap();
        state.onStart += delegate
        {
            _view.end.gameObject.SetActive(true);
        };
        state.onOver += delegate
        {
            _view.end.gameObject.SetActive(false);
        };
        state.addEvent("end", "begin");
        return state;
    }

    // Use this for initialization
    void Start () {
        fsm_.addState("begin", beginState());
        fsm_.addState("play", playState());
        fsm_.addState("end", endState());
        fsm_.init("play");
    }

    

    // Update is called once per frame
    void Update () {
		
	}
}
