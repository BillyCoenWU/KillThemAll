using UnityEngine;
using System.Collections;

public class BoxSpawnManager : MonoBehaviour
{
	private float m_randomTime = 0.0f;

	private int m_tempIndex = 1;

	[SerializeField]
	private Box m_box = null;

	[SerializeField]
	private Weapon.Data[] m_weaponsData = null;
	public Weapon.Data basicData
	{
		get
		{
			return m_weaponsData[0];
		}
	}

	[SerializeField]
	public Transform[] m_spawnPoints = null;

	[SerializeField]
	public AudioSource[] m_audioSource = null;

	private static BoxSpawnManager s_instance = null;
	public static BoxSpawnManager Instance
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
	}

	public void StartSpawnBox ()
	{
		StopAllCoroutines();

		m_randomTime = Random.Range(4.0f, 8.0f);

		StartCoroutine(LoadRandomBox());
	}

	private IEnumerator LoadRandomBox ()
	{
		yield return new WaitForSeconds(m_randomTime);

		m_box.weaponData = m_weaponsData[m_tempIndex];
		m_box.sound = m_audioSource[m_tempIndex];
		m_box.transform.position = m_spawnPoints[Random.Range(0, m_spawnPoints.Length)].position;
		m_box.gameObject.SetActive(true);

		m_tempIndex++;
		if(m_tempIndex >= m_weaponsData.Length)
		{
			m_tempIndex = 0;
		}

		StartSpawnBox();
	}
}
