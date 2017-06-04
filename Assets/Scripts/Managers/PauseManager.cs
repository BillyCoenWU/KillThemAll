using UnityEngine;

public class PauseManager : MonoBehaviour
{
	private AudioSource m_sound = null;

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
		if(s_instance != null)
		{
			DestroyImmediate(s_instance);
		}

		s_instance = this;

		m_sound = GetComponent<AudioSource>();

		gameObject.SetActive(false);
	}

	public void Pause ()
	{
		Time.timeScale = 0.0f;
		gameObject.SetActive(true);
		m_sound.Play();
	}

	public void Resume ()
	{
		Time.timeScale = 1.0f;
		gameObject.SetActive(false);
	}
}
