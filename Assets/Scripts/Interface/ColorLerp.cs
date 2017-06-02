using UnityEngine;
using System.Collections;

public class ColorLerp : MonoBehaviour
{
	private float m_lerp = 0.0f;

	[SerializeField]
	private Color m_mainColor = Color.white;
	[SerializeField]
	private Color m_darkColor = Color.white;

	private SpriteRenderer m_spriteRender = null;

	private void Start ()
	{
		m_spriteRender = GetComponent<SpriteRenderer>();
		StartCoroutine(ToDark());
	}

	private IEnumerator ToDark ()
	{
		m_lerp = 0.0f;

		while(m_lerp < 1.0f)
		{
			m_lerp += Time.deltaTime;
			m_spriteRender.color = Color.Lerp(m_mainColor, m_darkColor, m_lerp);

			yield return null;
		}

		StartCoroutine(ToMain());
	}

	private IEnumerator ToMain ()
	{
		m_lerp = 0.0f;

		while(m_lerp < 1.0f)
		{
			m_lerp += Time.deltaTime;
			m_spriteRender.color = Color.Lerp(m_darkColor, m_mainColor, m_lerp);

			yield return null;
		}

		StartCoroutine(ToDark());
	}
}
