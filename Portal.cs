using UnityEngine;
public class Portal : MonoBehaviour
{
	[SerializeField] Transform destinationPortal;
	[SerializeField] AudioSource audiosource;
	[SerializeField] AudioClip portalSound1;
	[SerializeField] AudioClip portalSound2;
	[SerializeField] bool isRedirectArrowPortal;
	float portalDelay = 0.1f;
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			Player player = other.GetComponent<Player>();
			if (player.lastTeleportTime < Time.time - portalDelay)
			{
				player.lastTeleportTime = Time.time;
				player.transform.position = destinationPortal.transform.position;
				audiosource.PlayOneShot(portalSound1);
			}
		}
		if (other.CompareTag("Arrow"))
		{
			Arrow arrow = other.GetComponent<Arrow>();
			if (arrow.teleportTicket == 3)
			{
				Destroy(arrow.gameObject);
			}
			else
			{
				if (arrow.lastTeleportTime < Time.time - portalDelay)
				{
					arrow.lastTeleportTime = Time.time;
					arrow.transform.position = destinationPortal.transform.position;
					if (isRedirectArrowPortal)
					{
						Vector3 destinationpos = new Vector3(destinationPortal.transform.position.x, destinationPortal.transform.position.y);
						arrow.ArrowReflect(destinationpos + destinationPortal.transform.right * 10f, 4);
					}
					audiosource.PlayOneShot(portalSound2);
					arrow.teleportTicket++;
				}
			}
		}
	}
}