using UnityEngine;

public class ObstacleAI : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float chaseSpeed = 3f; // Speed when chasing
    public float evadeSpeed = 4f; // Speed when evading
    public float wanderSpeed = 2f; // Speed when wandering
    public float detectionRange = 15f; // Range to detect player for chasing
    public float evadeRange = 3f; // Range to start evading
    public float wanderChangeInterval = 2f; // Time before changing wander direction
    public float[] laneXPositions = { -5f, 0f, 5f }; // X-positions of the three lanes
    public bool useRaycastForGround = true; // Use raycast to detect ground Y
    public float groundY = 0f; // Manual Y-position if raycast is disabled
    public float raycastDistance = 10f; // Distance to raycast downward
    public float laneSwitchDelay = 0.5f; // Delay before switching to predicted lane
    public float stealthHideRange = 20f; // Range beyond which obstacle hides
    public float environmentAvoidRange = 2f; // Range to detect environmental obstacles
    public LayerMask environmentLayer; // Layer for environmental objects
    public float lingerTimeThreshold = 3f; // Time to detect player lingering in a lane
    public float lingerLaneBias = 0.6f; // Probability to choose player's lane when lingering
    private Vector3 wanderDirection;
    private float wanderTimer;
    private float currentLaneX; // Current lane X-position
    private float targetLaneX; // Predicted lane X-position
    private float lastPlayerLaneX; // Last known player lane X-position
    private float laneSwitchTimer; // Timer for lane switch delay
    private Vector3 lastPlayerPosition; // Last known player position for mimicking
    private Renderer obstacleRenderer; // For hiding in stealth mode
    private float lingerTimer; // Timer for tracking player linger time
    private bool isLingering; // Whether player is lingering in a lane
    private enum AIState { Wander, Chase, Evade, Stealth }
    private AIState currentState = AIState.Wander;

    void Start()
    {
        // Find the player by tag
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        // Initialize components and variables
        obstacleRenderer = GetComponent<Renderer>();
        wanderDirection = new Vector3(0, 0, Random.Range(-1f, 1f)).normalized;
        wanderTimer = wanderChangeInterval;
        // Randomly select a lane at start with weighted distribution
        float randomValue = Random.value;
        if (randomValue < 0.4f) // 40% chance for left lane (-5)
            currentLaneX = laneXPositions[0];
        else if (randomValue < 0.7f) // 30% chance for center lane (0)
            currentLaneX = laneXPositions[1];
        else // 30% chance for right lane (5)
            currentLaneX = laneXPositions[2];
        targetLaneX = currentLaneX;
        lastPlayerLaneX = GetClosestLaneX(player != null ? player.position.x : 0f);
        lastPlayerPosition = player != null ? player.position : Vector3.zero;
        lingerTimer = 0f;
        isLingering = false;
        transform.position = new Vector3(currentLaneX, GetGroundY(), transform.position.z);
    }

    void Update()
    {
        if (player == null) return;

        // Get ground Y-position
        float targetY = useRaycastForGround ? GetGroundY() : groundY;

        // Calculate distance to player (ignore Y for consistency)
        float distanceToPlayer = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.position.x, 0, player.position.z));

        // Track player lingering in a lane
        float playerLaneX = GetClosestLaneX(player.position.x);
        if (playerLaneX == lastPlayerLaneX)
        {
            lingerTimer += Time.deltaTime;
            if (lingerTimer >= lingerTimeThreshold)
            {
                isLingering = true;
            }
        }
        else
        {
            lingerTimer = 0f;
            isLingering = false;
            lastPlayerLaneX = playerLaneX;
        }

        // Predict player's lane
        if (playerLaneX != lastPlayerLaneX)
        {
            targetLaneX = playerLaneX;
            laneSwitchTimer = laneSwitchDelay;
        }

        // Update lane switch timer
        if (laneSwitchTimer > 0)
        {
            laneSwitchTimer -= Time.deltaTime;
            if (laneSwitchTimer <= 0)
            {
                currentLaneX = targetLaneX;
            }
        }

        // Determine state
        if (distanceToPlayer > stealthHideRange)
        {
            currentState = AIState.Stealth;
        }
        else if (distanceToPlayer <= evadeRange)
        {
            currentState = AIState.Evade;
        }
        else if (distanceToPlayer <= detectionRange)
        {
            currentState = AIState.Chase;
        }
        else
        {
            currentState = AIState.Wander;
        }

        // Handle stealth visibility
        obstacleRenderer.enabled = currentState != AIState.Stealth;

        // Execute behavior
        Vector3 newPosition = transform.position;
        switch (currentState)
        {
            case AIState.Chase:
                // Move toward player's Z and predicted lane X, avoid environment
                Vector3 chaseTarget = new Vector3(targetLaneX, targetY, player.position.z);
                Vector3 chaseDirection = (chaseTarget - newPosition).normalized;
                chaseDirection = AvoidEnvironment(chaseDirection);
                chaseDirection.y = 0;
                newPosition += chaseDirection * chaseSpeed * Time.deltaTime;
                transform.LookAt(new Vector3(targetLaneX, targetY, newPosition.z + chaseDirection.z));
                currentLaneX = GetClosestLaneX(newPosition.x);
                break;

            case AIState.Evade:
                // Move away from player's Z in current lane, avoid environment
                Vector3 evadeTarget = new Vector3(currentLaneX, targetY, player.position.z);
                Vector3 evadeDirection = (newPosition - evadeTarget).normalized;
                evadeDirection = AvoidEnvironment(evadeDirection);
                evadeDirection.y = 0;
                newPosition += evadeDirection * evadeSpeed * Time.deltaTime;
                transform.LookAt(new Vector3(currentLaneX, targetY, newPosition.z + evadeDirection.z));
                break;

            case AIState.Wander:
                // Wander along current lane, mimic player movement, avoid environment
                wanderTimer -= Time.deltaTime;
                if (wanderTimer <= 0)
                {
                    wanderDirection = new Vector3(0, 0, Random.Range(-1f, 1f)).normalized;
                    wanderTimer = wanderChangeInterval;
                    if (Random.value < 0.3f && laneSwitchTimer <= 0)
                    {
                        // Weighted lane selection with linger bias
                        float laneRandom = Random.value;
                        if (isLingering && laneRandom < lingerLaneBias)
                            targetLaneX = playerLaneX; // Bias toward player's lane
                        else if (laneRandom < 0.4f)
                            targetLaneX = laneXPositions[0]; // Left lane
                        else if (laneRandom < 0.7f)
                            targetLaneX = laneXPositions[1]; // Center lane
                        else
                            targetLaneX = laneXPositions[2]; // Right lane
                        laneSwitchTimer = laneSwitchDelay;
                    }
                }
                // Mimic player's Z movement
                float playerZDelta = player.position.z - lastPlayerPosition.z;
                newPosition.z += playerZDelta * 0.5f;
                newPosition += wanderDirection * wanderSpeed * Time.deltaTime;
                newPosition = AvoidEnvironmentPosition(newPosition);
                transform.LookAt(new Vector3(currentLaneX, targetY, newPosition.z + wanderDirection.z));
                break;

            case AIState.Stealth:
                // Stay in place, hidden
                newPosition = new Vector3(currentLaneX, targetY, newPosition.z);
                break;
        }

        // Snap to lane and ground
        newPosition.x = currentLaneX;
        newPosition.y = targetY;
        transform.position = newPosition;
        lastPlayerPosition = player.position;
    }

    private float GetClosestLaneX(float currentX)
    {
        float closestX = laneXPositions[0];
        float minDistance = Mathf.Abs(currentX - closestX);
        foreach (float laneX in laneXPositions)
        {
            float distance = Mathf.Abs(currentX - laneX);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestX = laneX;
            }
        }
        return closestX;
    }

    private float GetGroundY()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, raycastDistance))
        {
            return hit.point.y + 0.1f;
        }
        return groundY;
    }

    private Vector3 AvoidEnvironment(Vector3 desiredDirection)
    {
        RaycastHit hit;
        Vector3 forward = transform.position + desiredDirection * environmentAvoidRange;
        if (Physics.Raycast(transform.position, desiredDirection, out hit, environmentAvoidRange, environmentLayer))
        {
            Vector3 avoidDirection = Vector3.Reflect(desiredDirection, hit.normal);
            avoidDirection.y = 0;
            return avoidDirection.normalized;
        }
        return desiredDirection;
    }

    private Vector3 AvoidEnvironmentPosition(Vector3 desiredPosition)
    {
        RaycastHit hit;
        Vector3 direction = (desiredPosition - transform.position).normalized;
        if (Physics.Raycast(transform.position, direction, out hit, environmentAvoidRange, environmentLayer))
        {
            Vector3 avoidPosition = transform.position + hit.normal * (environmentAvoidRange - hit.distance);
            avoidPosition.x = currentLaneX;
            avoidPosition.y = GetGroundY();
            return avoidPosition;
        }
        return desiredPosition;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, evadeRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stealthHideRange);
        Gizmos.color = Color.green;
        foreach (float laneX in laneXPositions)
        {
            Gizmos.DrawLine(new Vector3(laneX, transform.position.y - 1, transform.position.z - 10), new Vector3(laneX, transform.position.y + 1, transform.position.z + 10));
        }
    }
}