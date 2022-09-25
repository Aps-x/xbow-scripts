using UnityEngine;
public class Arrow : MonoBehaviour
{
	#region Header
	[SerializeField] SpriteRenderer sr;
	[SerializeField] Rigidbody2D rb;
	[SerializeField] Collider2D col;
	[SerializeField] Sprite redArrowSprite;
	[SerializeField] Sprite blueArrowSprite;
	[SerializeField] Sprite greenArrowSprite;
	[SerializeField] Sprite yellowArrowSprite;
	[SerializeField] Sprite maroonArrowSprite;
	[SerializeField] float reflectSpeedInterval;
	public float lastTeleportTime;
	public int teleportTicket = 0;
	public int playerIDCopy;
	int reflectCounter = 1;
	float arrowSpeed;
	bool portalReflect;
	#endregion
	public void Initialize(int playerID, float arrowspeed)
	{
		playerIDCopy = playerID;
		arrowSpeed = arrowspeed;
		switch(playerIDCopy)
		{
			case 0:
				sr.sprite = redArrowSprite;
				gameObject.layer = 6;
				break;
			case 1:
				sr.sprite = blueArrowSprite;
				gameObject.layer = 7;
				break;
			case 2:
				sr.sprite = greenArrowSprite;
				gameObject.layer = 8;
				break;
			case 3:
				sr.sprite = yellowArrowSprite;
				gameObject.layer = 9;
				break;
			case 4:
				sr.sprite = maroonArrowSprite;
				gameObject.layer = 17;
				break;
		}
	}
	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.CompareTag("Wall"))
		{
			rb.velocity = Vector2.zero;
			sr.sortingOrder = Random.Range(-10,0);
			Destroy(col);
			Destroy(this);
		}
		if (other.gameObject.CompareTag("MovingPlatform"))
		{
			Destroy(this.gameObject);
		}
		if (other.gameObject.CompareTag("Player"))
		{
			Player player = other.gameObject.GetComponent<Player>();
			if(player.playerID != playerIDCopy)
			{
				player.OnPlayerDeath(playerIDCopy);
				Destroy(this.gameObject);
			}
		}
	}
	public void ArrowReflect(Vector3 targetPos, int reflectorID)
	{
		switch (reflectorID)
		{
			case 0:
				sr.sprite = redArrowSprite;
				gameObject.layer = 6;
				break;
			case 1:
				sr.sprite = blueArrowSprite;
				gameObject.layer = 7;
				break;
			case 2:
				sr.sprite = greenArrowSprite;
				gameObject.layer = 8;
				break;
			case 3:
				sr.sprite = yellowArrowSprite;
				gameObject.layer = 9;
				break;
			case 4:
				portalReflect = true;
				reflectorID = playerIDCopy;
				break;
		}
		playerIDCopy = reflectorID;
		// Direction
		var displacement = targetPos - transform.position;
		var enterSpeed = rb.velocity.magnitude;
		rb.velocity = Vector2.zero;
		if (portalReflect)
		{
			rb.AddForce(displacement.normalized * enterSpeed, ForceMode2D.Impulse);
		}
		else
		{
			rb.AddForce(displacement.normalized * arrowSpeed, ForceMode2D.Impulse);
		}
		// Rotation
		Vector2 dir = rb.velocity;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		if (!portalReflect)
		{
			IncreaseSpeed();
		}
		portalReflect = false;
	}
	public void IncreaseSpeed()
	{
		var enterVelocity = rb.velocity;
		rb.velocity = Vector2.zero;
		rb.AddForce(enterVelocity.normalized * (arrowSpeed + (reflectCounter * reflectSpeedInterval)), ForceMode2D.Impulse);
		reflectCounter++;
	}
}