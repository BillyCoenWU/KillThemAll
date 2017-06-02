using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private GameObject m_mainMenuGo = null;
	public bool hasStarted
	{
		get
		{
			return !m_mainMenuGo.activeSelf;
		}
	}

	[SerializeField]
	private LooseMenu m_looseMenu = null;
	public bool gameOver
	{
		get
		{
			return m_looseMenu.gameObject.activeSelf;
		}
	}

	private static GameManager s_instance = null;
	public static GameManager Instance
	{
		get
		{
			return s_instance;
		}
	}

	private void Awake ()
	{
		s_instance = this;
	}

	private void LateUpdate ()
	{
		if(!hasStarted)
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				ScoreManager.Instance.gameObject.SetActive(true);
				BoxSpawnManager.Instance.StartSpawnBox();
				m_mainMenuGo.SetActive(false);
			}
		}
		else
		{
			if(gameOver)
			{
				if(Input.GetKeyDown(KeyCode.Space))
				{
					Restart();
				}
			}
		}
	}

	public void EndGame ()
	{
		BoxSpawnManager.Instance.StopAllCoroutines();
		ScoreManager.Instance.gameObject.SetActive(false);
		PauseManager.Instance.Resume();
		m_looseMenu.gameObject.SetActive(true);
	}
	
	public void Restart ()
	{
		SceneManager.LoadScene(0);
	}
}
