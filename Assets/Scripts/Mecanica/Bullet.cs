using UnityEngine;

public class Bullet : MonoBehaviour
{
	private int m_damage = 0;
	public int damage
	{
		set
		{
			m_damage = value;

			transform.localEulerAngles = new Vector3(0.0f, 0.0f, Character.Instance.spriteRenderer.flipX ? 180.0f : 0.0f);

			gameObject.SetActive(true);

			rigidbody2D.AddForce(new Vector2(Character.Instance.spriteRenderer.flipX ? -10.0f : 10.0f, 0.0f), ForceMode2D.Impulse);

			m_dropSound.Play();
		}
	}

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

	[SerializeField]
	private AudioSource m_dropSound = null;
	[SerializeField]
	private AudioSource m_impactSound = null;

	private Rigidbody2D m_rigidbody2D = null;
	public new Rigidbody2D rigidbody2D
	{
		get
		{
			if(m_rigidbody2D == null)
			{
				m_rigidbody2D = GetComponent<Rigidbody2D>();
			}

			return m_rigidbody2D;
		}
	}

	private void OnTriggerStay2D (Collider2D other)
	{
		Enemy e = other.GetComponent<Enemy>();

		if(e != null)
		{
			e.lifeManager.TakeDamage(m_damage);
		}

		m_impactSound.Play();

		rigidbody2D.velocity = Vector2.zero;
		gameObject.SetActive(false);
	}
}
