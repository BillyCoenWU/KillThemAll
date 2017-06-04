using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	[SerializeField]
	private Text m_text = null;

	private int m_points = 0;
	public int points
	{
		get
		{
			return m_points;
		}
	}

	private static ScoreManager s_instance = null;
	public static ScoreManager Instance
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

		gameObject.SetActive(false);
	}

	public void AddPoints (int plusPoints)
	{
		m_points += plusPoints;

		m_text.text = string.Concat("Pontos: ", m_points.ToString());
	}
}
