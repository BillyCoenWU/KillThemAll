using UnityEngine;

[RequireComponent (typeof (SpriteRenderer))]
public class SpriteRendererAnimation : AnimationController
{
	private SpriteRenderer m_spriteRenderer = null;
	public SpriteRenderer spriteRenderer
	{
		get
		{
			SetSpriteRendererComponent();

			return m_spriteRenderer;
		}
	}

	private void Awake ()
	{
		SetSpriteRendererComponent();
	}

	protected override void Update ()
	{
		base.Update ();

		if (!m_playing)
		{
			return;
		}

		m_spriteRenderer.sprite = m_currentAnimation.frames[m_currentFrame];
	}

	private void SetSpriteRendererComponent ()
	{
		if(m_spriteRenderer != null)
		{
			return;
		}

		m_spriteRenderer = GetComponent<SpriteRenderer>();
	}
}
