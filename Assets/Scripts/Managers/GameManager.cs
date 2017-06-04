using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private Object m_bulletObject = null;

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

	[SerializeField]
	private AudioSource m_audioSource = null;

	[SerializeField]
	private AudioSource m_startSound = null;

	[SerializeField]
	private AudioClip[] m_backgroundMusics = null;

	private Bullet[] m_bullets = null;

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
		if(s_instance != null)
		{
			DestroyImmediate(s_instance);
		}

		s_instance = this;

		SetBullets();
	}

	private void LateUpdate ()
	{
		if(!hasStarted)
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				ScoreManager.Instance.gameObject.SetActive(true);
				BoxSpawnManager.Instance.StartSpawnBox();
				WaveManager.Instance.CallWave();
				m_mainMenuGo.SetActive(false);

				m_audioSource.Stop();
				m_audioSource.clip = m_backgroundMusics[0];
				m_audioSource.Play();

				m_startSound.Play();
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

	private void SetBullets ()
	{
		m_bullets = new Bullet[20];

		GameObject go = null;

		for(int i = 0; i < m_bullets.Length; i++)
		{
			go = Instantiate(m_bulletObject, Vector3.zero, Quaternion.identity) as GameObject;
			m_bullets[i] = go.GetComponent<Bullet>();
		}
	}

	public Bullet LoadBullet ()
	{
		for(int i = 0; i < m_bullets.Length; i++)
		{
			if(!m_bullets[i].gameObject.activeSelf)
			{
				return m_bullets[i];
			}
		}

		return null;
	}

	public void EndGame ()
	{
		BoxSpawnManager.Instance.StopAllCoroutines();
		ScoreManager.Instance.gameObject.SetActive(false);
		PauseManager.Instance.Resume();
		m_looseMenu.gameObject.SetActive(true);

		m_audioSource.Stop();
		m_audioSource.clip = m_backgroundMusics[1];
		m_audioSource.Play();
	}
	
	public void Restart ()
	{
		SceneManager.LoadScene(0);
	}
}
