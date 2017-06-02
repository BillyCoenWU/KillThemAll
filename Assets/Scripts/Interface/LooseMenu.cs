using UnityEngine;
using UnityEngine.UI;

public class LooseMenu : MonoBehaviour
{
	[SerializeField]
	private GameObject m_recordeGo = null;

	[SerializeField]
	public Text m_text = null;

	private void OnEnable ()
	{
		m_text.text =  string.Concat("Pontos: ", ScoreManager.Instance.points.ToString());

		if(ScoreManager.Instance.points > PlayerPrefs.GetInt("Pontos", 0))
		{
			PlayerPrefs.SetInt("Pontos", ScoreManager.Instance.points);
			m_recordeGo.SetActive(true);
		}
	}
}
