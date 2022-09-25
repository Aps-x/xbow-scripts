using UnityEngine;
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Axis axis;
    [SerializeField] Direction direction;
    [SerializeField] float directionOneLimit;
    [SerializeField] float directionTwoLimit;
    [SerializeField] float platformSpeed;
    public Vector2 platformMovement;
    Vector2 movementDir;
    enum Axis
    {
        Vertical,
        Horizontal
    }
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
                movementDir = Vector2.up;
                break;
            case Direction.Down:
                movementDir = Vector2.down;
                break;
            case Direction.Left:
                movementDir = Vector2.left;
                break;
            case Direction.Right:
                movementDir = Vector2.right;
                break;
        }
    }
    void FixedUpdate()
    {
        if (axis == Axis.Vertical)
		{
            if (transform.position.y > directionOneLimit)
            {
                movementDir = Vector2.down;
            }
            if (transform.position.y < directionTwoLimit)
            {
                movementDir = Vector2.up;
            }
        }
        else if (axis == Axis.Horizontal)
		{
            if (transform.position.x > directionOneLimit)
            {
                movementDir = Vector2.left;
            }
            if (transform.position.x < directionTwoLimit)
            {
                movementDir = Vector2.right;
            }
        }
        platformMovement = movementDir * platformSpeed;
        rb.MovePosition(rb.position + platformMovement * Time.fixedDeltaTime); 
    }
}