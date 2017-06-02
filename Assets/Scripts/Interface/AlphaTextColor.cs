using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AlphaTextColor : MonoBehaviour
{
	private float m_lerp = 0.0f;

	[SerializeField]
	private Text m_text = null;

	private readonly Color WHITE = Color.white;
	private readonly Color BLACK = Color.black;

	private void Start ()
	{
		m_text.color = WHITE;
		StartCoroutine(ToBlack());
	}
	
	private IEnumerator ToWhite ()
	{
		m_lerp = 0.0f;

		while(m_lerp < 1.0f)
		{
			m_lerp += Time.deltaTime;
			m_text.color = Color.Lerp(BLACK, WHITE, m_lerp);

			yield return null;
		}

		StartCoroutine(ToBlack());
	}

	private IEnumerator ToBlack ()
	{
		m_lerp = 0.0f;

		while(m_lerp < 1.0f)
		{
			m_lerp += Time.deltaTime;
			m_text.color = Color.Lerp(WHITE, BLACK, m_lerp);

			yield return null;
		}

		StartCoroutine(ToWhite());
	}
}
