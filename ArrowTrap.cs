using UnityEngine;
public class ArrowTrap : MonoBehaviour
{
    [SerializeField] GameObject arrow;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip fire;
    [SerializeField] Direction direction;
    [SerializeField] bool canPlaySound;
    [SerializeField] bool usedByButtonOverride;
    float nextFire = 2.5f;
    float fireRate = 2.5f;
    float arrowSpeed = 20;
    Quaternion arrowRotation;
    Vector2 arrowDirection;
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    void Start()
    {
        switch (direction)
		{
            case Direction.Up:
                arrowDirection = Vector2.up;
                arrowRotation = Quaternion.Euler(0, 0, 90);
                break;
            case Direction.Down:
                arrowDirection = Vector2.down;
                arrowRotation = Quaternion.Euler(0, 0, -90);
                break;
            case Direction.Left:
                arrowDirection = Vector2.left;
                arrowRotation = Quaternion.Euler(0, 0, -180);
                break;
            case Direction.Right:
                arrowDirection = Vector2.right;
                arrowRotation = Quaternion.identity;
                break;
		}
    }
    void Update()
    {
        if (!usedByButtonOverride)
		{
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                FireArrow(4);       
            }
        }
    }
    public void FireArrow(int playerID)
	{
        GameObject newArrow = Instantiate(arrow, transform.position, arrowRotation);
        Rigidbody2D arrowRB = newArrow.GetComponent<Rigidbody2D>();

        arrowRB.AddForce(arrowDirection * arrowSpeed, ForceMode2D.Impulse);
        newArrow.GetComponent<Arrow>().Initialize(playerID, arrowSpeed);

        if (canPlaySound) { audioSource.PlayOneShot(fire); }
    }
}