using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour {

	[SerializeField]
	private RectTransform timeRemainingBar;
	[SerializeField]
	private Text timeRemainingText;
	[SerializeField]
	private Text scoreText;

	private float maxTime;
	public void SetMaxTime(float seconds) {
		this.maxTime = seconds;
		SetTimeRemaining (seconds);
	}
	public void SetTimeRemaining(float seconds) {
		timeRemainingBar.localScale = new Vector3 (seconds / maxTime, 1, 1);
		int sec = (int)Mathf.Floor (seconds);
		int microsec = (int)((seconds - sec) * 100);
		int min = sec / 60;
		sec = sec % 60;
		timeRemainingText.text = min.ToString ().PadLeft (2, '0') + ':' +
			sec.ToString ().PadLeft (2, '0') + ':' +
			microsec.ToString ().PadLeft (2, '0');
	}

	public void SetScore(int score) {
		scoreText.text = score.ToString ().PadLeft (6, '0');
	}
}
