using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
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
