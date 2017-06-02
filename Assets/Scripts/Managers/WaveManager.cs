using UnityEngine;

public class WaveManager : MonoBehaviour
{
	[SerializeField]
	private Enemy.Data[] m_enemyDatas = null;

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
		s_instance = this;
	}
}
