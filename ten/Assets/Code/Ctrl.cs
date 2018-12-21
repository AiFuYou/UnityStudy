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
        bool s = false;
        StateWithEventMap state = TaskState.Create(delegate {
            //TaskWait tw = new TaskWait();
            //tw.setAllTime(0.3f);
            //return tw;
            Task task = new Task();
            TaskManager.PushFront(task, delegate
            {
                s = checkAndRemove();
            });
            return task;
        }, fsm_, delegate {
            if (s)
            {
                return "fall";
            } else
            {
                return "input";
            }
        });
        state.onStart += delegate
        {
            Debug.LogWarning("In Remove");
        };
        return state;
    }

    private bool checkAndRemove()
    {
        bool s = false;
        for (int x = 0; x < _model.width; ++x)
        {
            for (int y = 0; y < _model.height; ++y)
            {
                Cube c = _model.getCube(x, y);
                if (c.isEnabled == true)
                {
                    Cube up = _model.getCube(x, y - 1);
                    if (up != null && up.isEnabled == true && up.number + c.number == 10)
                    {
                        c.isEnabled = false;
                        up.isEnabled = false;
                        s = true;
                        break;
                    }

                    Cube down = _model.getCube(x, y + 1);
                    if (down != null && down.isEnabled == true && down.number + c.number == 10)
                    {
                        c.isEnabled = false;
                        down.isEnabled = false;
                        s = true;
                        break;
                    }

                    Cube left = _model.getCube(x - 1, y);
                    if (left != null && left.isEnabled == true && left.number + c.number == 10)
                    {
                        c.isEnabled = false;
                        left.isEnabled = false;
                        s = true;
                        break;
                    }

                    Cube right = _model.getCube(x + 1, y);
                    if (right != null && right.isEnabled == true && right.number + c.number == 10)
                    {
                        c.isEnabled = false;
                        right.isEnabled = false;
                        s = true;
                        break;
                    }
                }
            }
        }
        refreshModel2View();
        return s;
    }

    private State fallState()
    {
        StateWithEventMap state = TaskState.Create(delegate {
            Task fall = doFallTask();

            //TaskWait tw = new TaskWait();
            //tw.setAllTime(0.3f);
            //TaskManager.PushBack(tw, delegate
            //{
            //    doFall();
            //});
            return fall;
        }, fsm_, "remove");
        state.onStart += delegate
        {
            Debug.LogWarning("In Fall");
        };
        return state;
    }

    private Task doFallTask()
    {
        TaskSet ts = new TaskSet();


        for (int x = 0; x < _model.width; ++x)
        {
            for (int y = _model.height - 1; y >= 0; --y)
            {
                Cube c = _model.getCube(x, y);
                Cube end = null;
                int endY = 0;

                if (c.isEnabled)
                {
                    for (int n = y + 1; n < _model.height; ++n)
                    {
                        Cube fall = _model.getCube(x, n);
                        if (fall == null || fall.isEnabled == true)
                        {
                            break;
                        } else
                        {
                            end = fall;
                            endY = n;
                        }
                    }

                    if (end != null)
                    {
                        end.number = c.number;
                        end.isEnabled = true;
                        c.isEnabled = false;
                        ts.push(_view.play.moveTask(c.number, new Vector2(x, y), new Vector2(x, endY)));
                    }
                }
            }
        }

        TaskManager.PushBack(ts, delegate
        {
            refreshModel2View();
        });
        return ts;
    }

    private String input (int x, int number)
    {
        Cube c = _model.getCube(1, 0);
        c.isEnabled = false;

        c = _model.getCube(x, 0);
        c.number = number;
        c.isEnabled = true;

        refreshModel2View();
        return "fall";
    }
    private State inputState()
    {
        StateWithEventMap state = new StateWithEventMap();
        int number = 0;
        state.onStart += delegate
        {
            number = UnityEngine.Random.Range(3, 8);
            Cube c = _model.getCube(1, 0);
            c.isEnabled = true;
            c.number = number;
            refreshModel2View();
        };
        state.addAction("1", delegate (FSMEvent evt)
        {
            Debug.Log("I get 1!");
            return input(0, number);
            //return "fall";
        });
        state.addAction("2", delegate (FSMEvent evt)
        {
            Debug.Log("I get 2!");
            return input(1, number);
            //return "fall";
        });
        state.addAction("3", delegate (FSMEvent evt)
        {
            Debug.Log("I get 3!");
            return input(2, number);
            //return "fall";
        });
        state.addAction("4", delegate (FSMEvent evt)
        {
            Debug.Log("I get 4!");
            return input(3, number);
            //return "fall";
        });
        return state;
    }



    // Update is called once per frame
    void Update () {
		
	}
}
