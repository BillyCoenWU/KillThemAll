using UnityEngine;
using RGS.Animation;
using System.Collections;

public enum ENEMY_STATE
{
	IDLE,
	MOVE,
	DEATH
}

public class Enemy : MonoBehaviour
{
	[System.Serializable]
	public struct Data
	{
		public int life;

		public float scale;
		public float speed;

		public AnimationData animationData;

		public Vector2 facePoint;

		public Vector2 colliderSize;
		public Vector2 colliderOffset;
	}

	private bool m_facingRight = true;

	private ENEMY_STATE m_state = ENEMY_STATE.IDLE;

	[SerializeField]
	private BoxCollider2D m_collider = null;

	private Vector3 direction
	{
		get
		{
			return m_facingRight ? Vector3.right : Vector3.left;
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
	private Enemy.Data m_data;
	public Enemy.Data data
	{
		get
		{
			return m_data;
		}

		set
		{
			m_data = value;
		}
	}

	[SerializeField]
	private LifeManager m_lifeManager = null;
	public LifeManager lifeManager
	{
		get
		{
			return m_lifeManager;
		}
	}

	[SerializeField]
	private SpriteRendererAnimation m_animationController = null;
	public SpriteRendererAnimation animationController
	{
		get
		{
			return m_animationController;
		}
	}
	
	private void Update ()
    {
		if(PauseManager.Instance.isPaused || !GameManager.Instance.hasStarted || GameManager.Instance.gameOver)
		{
			return;
		}
	}

	private void FixedUpdate ()
	{
		if(PauseManager.Instance.isPaused || !GameManager.Instance.hasStarted || GameManager.Instance.gameOver)
		{
			return;
		}

		if(m_state == ENEMY_STATE.MOVE)
		{
			transform.localPosition += direction * (m_data.speed * Time.fixedDeltaTime);
		}
	}

	public void SetInformations (Enemy.Data newData, bool facingRight)
	{
		data = newData;

		transform.localScale = new Vector3(m_data.scale, m_data.scale, 1.0f);

		m_animationController.animations = m_data.animationData.animationsList;

		m_animationController.animations[0].genericType = ENEMY_STATE.IDLE;
		m_animationController.animations[1].genericType = ENEMY_STATE.MOVE;
		m_animationController.animations[2].genericType = ENEMY_STATE.DEATH;

		m_collider.size = m_data.colliderSize;
		m_collider.offset = m_data.colliderOffset;

		m_lifeManager = new LifeManager(m_data.life);
		m_lifeManager.Death += Death;

		if(m_facingRight != facingRight)
		{
			Flip();
		}

		SetState(ENEMY_STATE.MOVE);
	}

	private void SetState(ENEMY_STATE newState)
	{
		m_state = newState;

		m_animationController.PlayByGenericType(m_state);
	}

	private void Flip ()
	{
		m_facingRight = !m_facingRight;

		m_animationController.spriteRenderer.flipX = !m_facingRight;
	}

	private void Death ()
	{

	}

	private void OnCollisionEnter2D (Collision2D other)
	{
		if(other.gameObject == Character.Instance.gameObject)
		{
			Character.Instance.lifeManager.TakeDamage(1);
		}
		else
		{
			Vector2 facePoint = transform.localPosition;
			facePoint.y += m_data.facePoint.y;

			if(!m_facingRight)
			{
				facePoint.x -= m_data.facePoint.x;
			}
			else facePoint.x += m_data.facePoint.x;

			if(Physics2D.OverlapCircle(facePoint, 0.1f) != null)
			{
				Flip();
			}
		}
	}
}
