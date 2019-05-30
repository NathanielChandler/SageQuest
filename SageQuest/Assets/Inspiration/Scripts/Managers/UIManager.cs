using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	[SerializeField]
	private Text scoreText;
	private GameManager gameManager;

    [SerializeField]
    private Text countdownText;
    [SerializeField]
    private float countDownTime;

    float countDownTimer;

	void Start () {
		gameManager = GameManager.Instance;

        countDownTimer = countDownTime;
	}

	void Update () {
		scoreText.text = gameManager.Score.ToString ();

        if (countDownTimer > 0 && countdownText != null)
        {
            countDownTimer -= Time.deltaTime;
            int countdown = (int)countDownTimer;
            countdownText.text = countdown.ToString();
        }

	}

}
