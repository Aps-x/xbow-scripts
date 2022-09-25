using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
public class Player : MonoBehaviour
{
	#region Header
	// References
	[Header("References")]
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject sword1;
    [SerializeField] GameObject sword2;
    [SerializeField] GameObject crossbow;
    [SerializeField] GameObject firePoint;
    [SerializeField] GameObject pivotPoint;
    [SerializeField] GameObject laserOrigin;
    // Reference Components
    [Header("Reference Components")]
    [SerializeField] Animator meleeAnimator;
    [SerializeField] Collider2D meleeCollider;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] ParticleSystem dustParticles;
    [SerializeField] SpriteRenderer meleeSpriteRenderer;
    [SerializeField] SpriteRenderer crossbowSpriteRenderer;
    // Components
    [Header("Components")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] Collider2D hitbox;
    [SerializeField] AudioSource audioSource;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] SpriteRenderer spriteRenderer;
    // Audio
    [Header("Audio")]
    [SerializeField] AudioClip fire;
    [SerializeField] AudioClip dodgeRoll;
    [SerializeField] AudioClip meleeSlash;
    [SerializeField] AudioClip deathSound;
    // Serialized Variables
    [Header("Serialized Variables")]
    [SerializeField] float playerSpeed;
    [SerializeField] float nextFire;
    [SerializeField] float fireRate;
    [SerializeField] float arrowSpeed;
    [Space(10)]
    [SerializeField] float nextRoll;
    [SerializeField] float rollRate;
    [SerializeField] float rollDistance;
    [SerializeField] float rollDuration;
    [SerializeField] float iFrameMultiplier;
    [Space(10)]
    [SerializeField] float nextMelee;
    [SerializeField] float meleeRate;
    [SerializeField] float meleeDistance;
    [SerializeField] float meleeDuration;
    public float meleePauseDuration;
    // Public Variables
    public int playerID { get; private set; }
    public bool invulnerable = true;
    public float lastTeleportTime;
    public int playerScore = 0;
    public bool inputDisabled;
    public bool outOfRotation;
    public bool meleeImmune;
    public bool usingMouse;
    public int spawnTicket;
    bool actionInProgress;
    float distancePercent;
    bool swipeDirection;
    public bool isDead;
    Vector2 playerMovement;
    Vector2 movement;
    int layerMask;
    float angle;
    bool onIce;
    float timeToHold = 2.5f;
    float countdownTimer = 2.5f;
    int uiLayerMask = 1 << 5;
    int arrowRedLayerMask = 1 << 6;
    int arrowBlueLayerMask = 1 << 7;
    int arrowGreenLayerMask = 1 << 8;
    int arrowYellowLayerMask = 1 << 9;
    int playerBlockLayerMask = 1 << 16;
    int movingPlatformLayerMask = 1 << 18;
    // Declarations
    Vector2 cursorOffset = new Vector2(36, 36);
    Gradient gradient = new Gradient();
    MovingPlatform currentPlatform;
    GradientColorKey[] laserColor;
    GradientAlphaKey[] laserAlpha;
    GameManager gameManager;
    PlayerControls controls;
    Camera cam;
	#endregion
	void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        controls = new PlayerControls();
        playerID = playerInput.playerIndex;
        switch (playerInput.playerIndex)
        {
            case 0:
                spawnTicket = 0;
                transform.position = new Vector2(-17, 7.35f);// Spawn position in lobby
                animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animations/RedDino");
                gameObject.layer = 10;
                if (playerInput.currentControlScheme == "Keyboard/Mouse")
                {
                    usingMouse = true;
                    Cursor.SetCursor((Texture2D)Resources.Load("Cursors/RedCursor"), cursorOffset, CursorMode.Auto);
                }
				else
				{
                    Physics2D.queriesStartInColliders = false;
                    gradient.SetKeys
                    (
                        laserColor = new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 1.0f) },
                        laserAlpha = new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
                    );
                    lineRenderer.colorGradient = gradient;
                }
                break;
            case 1:
                spawnTicket = 2;
                transform.position = new Vector2(-15, 7.35f);// Spawn position in lobby
                animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animations/BlueDino");
                gameObject.layer = 11;
                if (playerInput.currentControlScheme == "Keyboard/Mouse")
                {
                    usingMouse = true;
                    Cursor.SetCursor((Texture2D)Resources.Load("Cursors/BlueCursor"), cursorOffset, CursorMode.Auto);
                }
				else
				{
                    Physics2D.queriesStartInColliders = false;
                    gradient.SetKeys
                    (
                        laserColor = new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.blue, 1.0f) },
                        laserAlpha = new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
                    );
                    lineRenderer.colorGradient = gradient;
                }
                break;
            case 2:
                spawnTicket = 1;
                transform.position = new Vector2(-11, 7.35f);// Spawn position in lobby
                if (GameSettings.GameMode == 1)
				{
                    animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animations/RedDinoAlt");
                    gameObject.layer = 10;
                    if (playerInput.currentControlScheme == "Keyboard/Mouse")
                    {
                        usingMouse = true;
                        Cursor.SetCursor((Texture2D)Resources.Load("Cursors/RedCursor"), cursorOffset, CursorMode.Auto);
                    }
                    else
                    {
                        Physics2D.queriesStartInColliders = false;
                        gradient.SetKeys
                        (
                            laserColor = new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 1.0f) },
                            laserAlpha = new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
                        );
                        lineRenderer.colorGradient = gradient;
                    }
                }
				else
				{
                    animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animations/GreenDino");
                    gameObject.layer = 12;
                    if (playerInput.currentControlScheme == "Keyboard/Mouse")
                    {
                        usingMouse = true;
                        Cursor.SetCursor((Texture2D)Resources.Load("Cursors/GreenCursor"), cursorOffset, CursorMode.Auto);
                    }
                    else
                    {
                        Physics2D.queriesStartInColliders = false;
                        gradient.SetKeys
                        (
                            laserColor = new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.green, 1.0f) },
                            laserAlpha = new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
                        );
                        lineRenderer.colorGradient = gradient;
                    }
                }
                break;
            case 3:
                spawnTicket = 3;
                transform.position = new Vector2(-9, 7.35f);// Spawn position in lobby
                if (GameSettings.GameMode == 1)
				{
                    animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animations/BlueDinoAlt");
                    gameObject.layer = 11;
                    if (playerInput.currentControlScheme == "Keyboard/Mouse")
                    {
                        usingMouse = true;
                        Cursor.SetCursor((Texture2D)Resources.Load("Cursors/BlueCursor"), cursorOffset, CursorMode.Auto);
                    }
                    else
                    {
                        Physics2D.queriesStartInColliders = false;
                        gradient.SetKeys
                        (
                            laserColor = new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.blue, 1.0f) },
                            laserAlpha = new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
                        );
                        lineRenderer.colorGradient = gradient;
                    }
                }
				else
				{
                    animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animations/YellowDino");
                    gameObject.layer = 13;
                    if (playerInput.currentControlScheme == "Keyboard/Mouse")
                    {
                        usingMouse = true;
                        Cursor.SetCursor((Texture2D)Resources.Load("Cursors/YellowCursor"), cursorOffset, CursorMode.Auto);
                    }
                    else
                    {
                        Physics2D.queriesStartInColliders = false;
                        gradient.SetKeys
                        (
                            laserColor = new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.yellow, 1.0f) },
                            laserAlpha = new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
                        );
                        lineRenderer.colorGradient = gradient;
                    }
                }
                break;
        }
        int combinedMask = arrowRedLayerMask | arrowBlueLayerMask | arrowGreenLayerMask | arrowYellowLayerMask | uiLayerMask | playerBlockLayerMask | movingPlatformLayerMask;
        layerMask = ~combinedMask;
    }
    void OnEnable()
    {
        controls.Enable();
        if (usingMouse) { Cursor.visible = true; }
        crossbow.SetActive(true);
    }
    public void Initialize(GameManager GM)
	{
        gameManager = GM;
        outOfRotation = false;
        playerScore = 0;
        onIce = false;
        currentPlatform = null;
        if (GameSettings.GameMode == 1)
        {
            switch (playerID)
            {
                case 2:
                    playerID = 0;
                    break;
                case 3:
                    playerID = 1;
                    break;
            }
        }
    }
    public void MousePosition(InputAction.CallbackContext context)
    {
        
    }
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1 && Time.time > nextFire && !inputDisabled)
        {
            nextFire = Time.time + fireRate;
            GameObject newArrow = Instantiate(arrow, firePoint.transform.position, crossbow.transform.rotation);
            Rigidbody2D arrowRB = newArrow.GetComponent<Rigidbody2D>();
            arrowRB.AddForce(firePoint.transform.right * arrowSpeed, ForceMode2D.Impulse);
            newArrow.GetComponent<Arrow>().Initialize(playerID, arrowSpeed);
            audioSource.PlayOneShot(fire);
        }
    }
    public void OnDodgeRoll(InputAction.CallbackContext context)
	{
        if (context.ReadValue<float>() == 1 && Time.time > nextRoll && !inputDisabled)
		{
            nextRoll = Time.time + rollRate;
            StartCoroutine(DodgeRoll(rollDuration));
        }
    }
    IEnumerator DodgeRoll(float duration)
    {
        // Pre-roll
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
        actionInProgress = true;
        int originalLayer = gameObject.layer;
        gameObject.layer = 14;
        inputDisabled = true;
        audioSource.PlayOneShot(dodgeRoll);
        dustParticles.Play();
        // Initialization of variables
        Vector2 rollDir = (Vector2)pivotPoint.transform.right;
        Quaternion startRotation = transform.rotation;
        Vector3 rotationAxis;
        float t = 0f;
        // Rotation axis depends on direction player is facing
        if (spriteRenderer.flipX == true) { rotationAxis = transform.forward; }
        else { rotationAxis = -transform.forward; }
        // Dodge Roll
        while (t < duration)
        {
            t += Time.deltaTime;
            rb.MovePosition(rb.position + rollDir * rollDistance * Time.fixedDeltaTime);// Roll movement
            transform.rotation = startRotation * Quaternion.AngleAxis(t / duration * 360f, rotationAxis);// Roll rotation
            pivotPoint.transform.rotation = Quaternion.Euler(0, 0, angle);// Lock crossbow rotation
            if (t >= duration * iFrameMultiplier)
			{
                gameObject.layer = originalLayer;
            }
            if(isDead)
			{
                transform.rotation = startRotation;
                gameObject.layer = originalLayer;
                dustParticles.Stop();
                yield break;
			}
            yield return null;
        }
        // Post-roll
        actionInProgress = false;
        transform.rotation = startRotation;
        dustParticles.Stop();
        if (!isDead)
		{
            inputDisabled = false;
        }
    }
    public void OnMelee(InputAction.CallbackContext context)
	{
        if (context.ReadValue<float>() == 1 && Time.time > nextMelee && !inputDisabled)
        {
            nextMelee = Time.time + meleeRate;
            StartCoroutine(MeleeSlash());
        }
    }
    IEnumerator MeleeSlash()
	{
        // Pre-melee
        actionInProgress = true;
        inputDisabled = true;
        crossbow.SetActive(false);
        meleeImmune = true;
        // Variables
        Vector2 slashDir = (Vector2)pivotPoint.transform.right;
        GameObject sword;
        float t1 = 0f;
        float t2 = 0f;
        meleePauseDuration = 1;
        // Melee effects
        if (swipeDirection)
		{
            sword1.SetActive(true);
            meleeSpriteRenderer.flipX = false;
            sword = sword1;
        }
        else
		{
            sword2.SetActive(true);
            meleeSpriteRenderer.flipX = true;
            sword = sword2;
        }
        meleeAnimator.SetTrigger("Attack");
        audioSource.PlayOneShot(meleeSlash);
        // Melee movement
        while (t1 < meleeDuration)
        {
            t1 += Time.deltaTime;
            rb.MovePosition(rb.position + slashDir * meleeDistance * Time.fixedDeltaTime);
            yield return null;
        }
        // Post-melee
        actionInProgress = false;
        meleeImmune = false;
        while (t2 < meleePauseDuration)
		{
            t2 += Time.deltaTime;
            yield return null;
        }
        // Post-melee pause
        sword.SetActive(false);
        crossbow.SetActive(true);
        swipeDirection = !swipeDirection;
        if (!isDead)
        {
            inputDisabled = false;
        }
    }
    public void OnRotate(InputAction.CallbackContext context)
	{
        if (!inputDisabled)
		{
            Vector2 joystickInput = context.ReadValue<Vector2>();
            // Rotate crossbow
            if (joystickInput.magnitude > 0.5f)
			{
                angle = Mathf.Atan2(joystickInput.y, joystickInput.x) * Mathf.Rad2Deg;
                pivotPoint.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            // Flip player and crossbow
            if (firePoint.transform.position.x > transform.position.x)
            {
                spriteRenderer.flipX = false;
                crossbowSpriteRenderer.flipY = false;
            }
            else if (firePoint.transform.position.x < transform.position.x)
            {
                spriteRenderer.flipX = true;
                crossbowSpriteRenderer.flipY = true;
            }
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }
    public void OnRemovePlayer(InputAction.CallbackContext context)
	{
        if (SceneManager.GetActiveScene().buildIndex == 1)
		{
            transform.position = Vector2.zero;
            if (usingMouse)
			{
                Cursor.SetCursor((Texture2D)Resources.Load("Cursors/WhiteCursor"), cursorOffset, CursorMode.Auto);
                usingMouse = false;
            }
            Destroy(this.gameObject);
        }
	}
	void OnTriggerEnter2D(Collider2D other)
	{
        if (other.CompareTag("MovingPlatform"))
        {
            currentPlatform = other.GetComponent<MovingPlatform>();
        }
        if (other.CompareTag("Ice"))
        {
            rb.velocity = playerMovement * 0.5f;
            onIce = true;
        }
    }
	void OnTriggerExit2D(Collider2D other)
	{
        if (other.CompareTag("MovingPlatform"))
        {
            currentPlatform = null;
        }
        if (other.CompareTag("Ice"))
        {
            onIce = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0;
        }
    }
	void FixedUpdate()
    {
        if (!inputDisabled && movement.magnitude > 0.5f)
		{
            
            if (currentPlatform != null)
			{
               playerMovement = (movement * playerSpeed) + currentPlatform.platformMovement;
            }
			else
			{
                playerMovement = movement * playerSpeed;
            }
            if (onIce)
			{
                rb.AddForce(playerMovement * 2, ForceMode2D.Force);
			}
			else
			{
                rb.MovePosition(rb.position + playerMovement * Time.fixedDeltaTime);
            }
            animator.SetFloat("MovementSpeed", movement.magnitude);
        }
		else
		{
            if (currentPlatform != null && !actionInProgress)
			{
                rb.MovePosition(rb.position + currentPlatform.platformMovement * Time.fixedDeltaTime);
                animator.SetFloat("MovementSpeed", 0);
            }
			else if (onIce)
			{
                rb.velocity = rb.velocity * 0.95f;
                animator.SetFloat("MovementSpeed", 0);
            }
			else
			{
                rb.velocity = Vector3.zero;
                animator.SetFloat("MovementSpeed", 0);
            }
        }
    }
	void Update()
	{
        // Back to menu timer
        if(controls.Gameplay.RemovePlayer.ReadValue<float>() == 1)
		{
            if (SceneManager.GetActiveScene().buildIndex != 1 && SceneManager.GetActiveScene().buildIndex != 2)
			{
                if (playerInput.playerIndex == 0)
                {
                    countdownTimer -= Time.deltaTime;
                    if (countdownTimer <= 0)
                    {
                        gameManager.BackToMenu();
                    }
                }
            }

        }
		else
		{
            countdownTimer = timeToHold;
        }
        if (usingMouse && !inputDisabled)
        {
            // Reference camera
            if (cam == null) { cam = Camera.main; }
            // Mouse position
            Vector2 mouseScreenPosition = controls.Gameplay.MousePosition.ReadValue<Vector2>(); 
            Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(mouseScreenPosition);
            // Math for rotating crossbow
            Vector3 targetDirection = mouseWorldPosition - pivotPoint.transform.position;
            angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            // Point the crossbow towards cursor position
            pivotPoint.transform.rotation = Quaternion.Euler(0, 0, angle);
            // Flip player and crossbow to face cursor
            if (mouseWorldPosition.x > transform.position.x)
            {
                spriteRenderer.flipX = false;
                crossbowSpriteRenderer.flipY = false;
            }
            else if (mouseWorldPosition.x < transform.position.x)
            {
                spriteRenderer.flipX = true;
                crossbowSpriteRenderer.flipY = true;
            }
        }
        else if (!usingMouse)
		{           
            RaycastHit2D hitInfo = Physics2D.Raycast(laserOrigin.transform.position, laserOrigin.transform.right, 10f, layerMask);
            if (hitInfo.collider != null)
            {
                lineRenderer.SetPosition(0, laserOrigin.transform.position);
                lineRenderer.SetPosition(1, hitInfo.point);

                distancePercent = hitInfo.distance / 10f;
                laserAlpha[1].alpha = (1f - distancePercent);

                gradient.SetKeys(laserColor, laserAlpha);
                lineRenderer.colorGradient = gradient;
            }
            else
            {
                lineRenderer.SetPosition(0, laserOrigin.transform.position);
                lineRenderer.SetPosition(1, laserOrigin.transform.position + laserOrigin.transform.right * 10f);

                laserAlpha[1].alpha = 0f;

                gradient.SetKeys(laserColor, laserAlpha);
                lineRenderer.colorGradient = gradient;
            }
        }
    }
    public void SpawnRotation(float spawnAngle, bool direction)
	{
        angle = spawnAngle;
        pivotPoint.transform.rotation = Quaternion.Euler(0, 0, angle);
        if (direction)
		{
            spriteRenderer.flipX = true;
            crossbowSpriteRenderer.flipY = true;
        }
		else
		{
            spriteRenderer.flipX = false;
            crossbowSpriteRenderer.flipY = false;
        }
    }
	public void OnPlayerDeath(int killerID)
	{
        if (!invulnerable)
		{
            // Bools
            invulnerable = true;
            isDead = true;
            inputDisabled = true;
            actionInProgress = false;
            // Effects
            animator.SetBool("IsDead", true);
            meleeAnimator.ResetTrigger("Attack");
            audioSource.PlayOneShot(deathSound);
            hitbox.enabled = false;
            // Functions
            Invoke("DisablePlayer", 1.5f);
            gameManager.PlayerKnockout(killerID, playerID);
        } 
	}
    public void DisablePlayer()
	{
        // Reposition
        transform.position = new Vector2(100, 100);
        // Reset effects
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
        hitbox.enabled = true;
        animator.SetBool("IsDead", false);
        // Disable the player
        this.enabled = false;
	}
	void OnDisable()
    {
        controls.Disable();
        if (usingMouse) { Cursor.visible = false; }
        crossbow.SetActive(false);
    }
}