using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
	[SerializeField]
	private Enemy m_enemy = null;

	private void OnTriggerEnter2D (Collider2D other)
    {
		m_enemy.Flip();
	}
}
