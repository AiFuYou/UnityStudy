using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultWindow : MonoBehaviour {

    public Button ReplayButton;
    public Text Message;
    public MainLoop mainLoop;

	// Use this for initialization
	void Start () {
        ReplayButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            mainLoop.Restart();
        });
    }

    public void Show (ChessType winType)
    {
        switch (winType)
        {
            case ChessType.Black:
                Message.text = string.Format("恭喜战胜电脑！");
                break;
            case ChessType.White:
                Message.text = string.Format("你被电脑击败了！");
                break;
        }
    }
}
