using UnityEngine;

public class PauseManager : MonoBehaviour
{
	private static PauseManager s_instance = null;
	public static PauseManager Instance
	{
		get
		{
			return s_instance;
		}
	}

	public bool isPaused
	{
		get
		{
			return gameObject.activeSelf;
		}
	}

	private void Awake ()
	{
		s_instance = this;
		gameObject.SetActive(false);
	}

	public void Pause ()
	{
		Time.timeScale = 0.0f;
		gameObject.SetActive(true);
	}

	public void Resume ()
	{
		Time.timeScale = 1.0f;
		gameObject.SetActive(false);
	}
}
