using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
	[System.Serializable]
	public struct Data
	{
		public float spawnDelay;

		public bool[] willFaceRight;

		public int[] enemysDataIndex;
	}

	[SerializeField]
	private Object m_enemyObject = null;

	[SerializeField]
	private Enemy.Data[] m_enemysData = null;

	[SerializeField]
	private Data[] m_wavesData = null;

	private int m_killedEnemys = 0;
	private int m_currentIndex = 0;
	private int m_limitedIndex = -1;
	private int m_currentEnemys = 0;

	private Enemy[] m_enemys = null;

	private Transform m_transform = null;
	public new Transform transform
	{
		get
		{
			if(m_transform == null)
			{
				m_transform = base.transform;
			}

			return m_transform;
		}
	}

	private static WaveManager s_instance = null;
	public static WaveManager Instance
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

		SetInformations();
	}

	private void SetInformations ()
	{
		m_enemys = new Enemy[m_wavesData[m_wavesData.Length-1].enemysDataIndex.Length];

		GameObject go = null;

		for(int i = 0; i < m_enemys.Length; i++)
		{
			go = Instantiate(m_enemyObject, Vector3.zero, Quaternion.identity) as GameObject;
			m_enemys[i] = go.GetComponent<Enemy>();
		}
	}

	private void LoadEnemy ()
	{
		if(GameManager.Instance.gameOver)
		{
			return;
		}

		for(int i = 0; i < m_enemys.Length; i++)
		{
			if(!m_enemys[i].gameObject.activeSelf)
			{
				m_enemys[i].transform.position = transform.position;
				m_enemys[i].SetInformations(m_enemysData[m_wavesData[m_limitedIndex].enemysDataIndex[m_currentEnemys]],
											m_wavesData[m_limitedIndex].willFaceRight[m_currentEnemys]);
				break;
			}
		}
	}

	public void CallWave ()
	{
		m_currentIndex++;
		m_limitedIndex++;

		m_killedEnemys = 0;
		m_currentEnemys = 0;

		if(m_limitedIndex >= m_wavesData.Length)
		{
			m_limitedIndex = m_wavesData.Length-1;
		}

		StartCoroutine(Spawn());
	}

	public void UpdateKilledEnemys ()
	{
		m_killedEnemys++;

		if(m_killedEnemys >= m_wavesData[m_limitedIndex].enemysDataIndex.Length)
		{
			CallWave();
		}
	}

	public void StopSpawn ()
	{
		StopCoroutine(Spawn());

		for(int i = 0; i < m_enemys.Length; i++)
		{
			if(m_enemys[i].gameObject.activeSelf)
			{
				m_enemys[i].SetState(ENEMY_STATE.IDLE);
			}
		}
	}

	private IEnumerator Spawn ()
	{
		while (m_currentEnemys < m_wavesData[m_limitedIndex].enemysDataIndex.Length)
		{
			yield return new WaitForSeconds(m_wavesData[m_limitedIndex].spawnDelay);

			LoadEnemy();

			m_currentEnemys++;
		}
	}
}
