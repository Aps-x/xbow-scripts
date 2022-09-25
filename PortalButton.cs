using UnityEngine;
public class PortalButton : MonoBehaviour
{
    [SerializeField] Sprite targetActivated;
    [SerializeField] Sprite targetDeactivated;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject[] portals;
	[SerializeField] bool alternateActivation;
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Arrow"))
		{
			if (!alternateActivation)
			{
				for (int i = 0; i < portals.Length; i++)
				{
					portals[i].SetActive(true);
				}
				Invoke("HidePortals", 5f);
			}
			else
			{
				for (int i = 0; i < portals.Length; i++)
				{
					if (portals[i].gameObject.activeInHierarchy == true)
					{
						portals[i].SetActive(false);
					}
					else
					{
						portals[i].SetActive(true);
					}
				}
			}
		}
	}
	void HidePortals()
	{
		for (int i = 0; i < portals.Length; i++)
		{
			portals[i].SetActive(false);
		}
	}
}