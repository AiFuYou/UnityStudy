using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDGeek;
using System;

public class Ctrl : MonoBehaviour {

    private FSM fsm_ = new FSM();
    public View _view = null;
    public Model _model = null;

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
        state.addEvent("begin", "input");
        return state;
    }

    private void refreshModel2View()
    {
        for (int x = 0; x < _model.width; ++x)
        {
            for (int y = 0; y < _model.height; ++y)
            {
                Square s = _view.play.getSquare(x, y);
                Cube c = _model.getCube(x, y);
                if (c.isEnabled)
                {
                    s.number = c.number;
                    s.show();
                } else
                {
                    s.hide();
                }
            }
        }
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
            refreshModel2View();
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
        fsm_.addState("input", inputState(), "play");
        fsm_.addState("fall", fallState(), "play");
        fsm_.addState("remove", removeState(), "play");
        fsm_.addState("end", endState());
        fsm_.init("input");
    }

    private State removeState()
    {
        StateWithEventMap state = TaskState.Create(delegate {
            TaskWait tw = new TaskWait();
            tw.setAllTime(1f);
            return tw;
        }, fsm_, "input");
        state.onStart += delegate
        {
            Debug.LogWarning("In Remove");
        };
        return state;
    }

    private State fallState()
    {
        StateWithEventMap state = TaskState.Create(delegate {
            TaskWait tw = new TaskWait();
            tw.setAllTime(1f);
            return tw;
        }, fsm_, "remove");
        state.onStart += delegate
        {
            Debug.LogWarning("In Fall");
        };
        return state;
    }

    private void input (int x)
    {

    }
    private State inputState()
    {
        StateWithEventMap state = new StateWithEventMap();
        state.onStart += delegate
        {
            Debug.LogWarning("In Input");
        };
        state.addAction("1", delegate (FSMEvent evt)
        {
            Debug.Log("I get 1!");
            input(0);
            return "fall";
        });
        state.addAction("2", delegate (FSMEvent evt)
        {
            Debug.Log("I get 2!");
            input(1);
            return "fall";
        });
        state.addAction("3", delegate (FSMEvent evt)
        {
            Debug.Log("I get 3!");
            input(2);
            return "fall";
        });
        state.addAction("4", delegate (FSMEvent evt)
        {
            Debug.Log("I get 4!");
            input(3);
            return "fall";
        });
        return state;
    }



    // Update is called once per frame
    void Update () {
		
	}
}
