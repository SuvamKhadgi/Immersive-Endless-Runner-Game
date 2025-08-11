// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class SystemIntegration : MonoBehaviour
// {
//     [Header("Integration Settings")]
//     public bool enableFullIntegration = true;
//     public bool debugMode = true;
    
//     [Header("Existing System References")]
//     public PlayerMovement playerMovement;
//     public LevelDistance levelDistance;
//     public ObstacleAI[] existingObstacleAIs;
    
//     [Header("New System References")]
//     public FirebaseManager firebaseManager;
//     public PlayerAnalyticsManager analyticsManager;
//     public AdvancedAIBehavior advancedAI;
//     public CloudDashboardManager dashboardManager;
//     public GameManager gameManager;
    
//     private bool systemsInitialized = false;
//     private float lastInputTime = 0f;
//     private Vector3 lastPlayerPosition;
    
//     private void Start()
//     {
//         if (enableFullIntegration)
//         {
//             StartCoroutine(InitializeIntegration());
//         }
//     }
    
//     private IEnumerator InitializeIntegration()
//     {
//         Debug.Log("Starting system integration...");
        
//         // Wait a moment for all systems to initialize
//         yield return new WaitForSeconds(1f);
        
//         // Find and assign missing references
//         FindAndAssignReferences();
        
//         // Connect existing systems to new analytics
//         IntegratePlayerMovement();
//         IntegrateObstacleAI();
//         IntegrateLevelDistance();
        
//         // Start monitoring loops
//         StartCoroutine(MonitorPlayerBehavior());
//         StartCoroutine(MonitorGameState());
        
//         systemsInitialized = true;
        
//         Debug.Log("System integration completed successfully!");
        
//         if (debugMode)
//         {
//             StartCoroutine(DebugSystemStatus());
//         }
//     }
    
//     private void FindAndAssignReferences()
//     {
//         // Find existing systems
//         if (playerMovement == null)
//             playerMovement = FindObjectOfType<PlayerMovement>();
            
//         if (levelDistance == null)
//             levelDistance = FindObjectOfType<LevelDistance>();
            
//         if (existingObstacleAIs == null || existingObstacleAIs.Length == 0)
//             existingObstacleAIs = FindObjectsOfType<ObstacleAI>();
            
//         // Find new systems
//         if (firebaseManager == null)
//             firebaseManager = FindObjectOfType<FirebaseManager>();
            
//         if (analyticsManager == null)
//             analyticsManager = FindObjectOfType<PlayerAnalyticsManager>();
            
//         if (advancedAI == null)
//             advancedAI = FindObjectOfType<AdvancedAIBehavior>();
            
//         if (dashboardManager == null)
//             dashboardManager = FindObjectOfType<CloudDashboardManager>();
            
//         if (gameManager == null)
//             gameManager = FindObjectOfType<GameManager>();
            
//         Debug.Log($"References found - Player: {playerMovement != null}, Analytics: {analyticsManager != null}, AI: {advancedAI != null}");
//     }
    
//     private void IntegratePlayerMovement()
//     {
//         if (playerMovement == null || analyticsManager == null) return;
        
//         // We'll monitor PlayerMovement through Update since we can't modify the original script
//         Debug.Log("PlayerMovement integration setup complete");
//     }
    
//     private void IntegrateObstacleAI()
//     {
//         if (existingObstacleAIs == null || advancedAI == null) return;
        
//         // Connect existing ObstacleAI with new AdvancedAIBehavior
//         foreach (var obstacleAI in existingObstacleAIs)
//         {
//             if (obstacleAI != null)
//             {
//                 // The AdvancedAIBehavior will work alongside existing ObstacleAI
//                 Debug.Log($"Integrated ObstacleAI: {obstacleAI.name}");
//             }
//         }
        
//         Debug.Log($"ObstacleAI integration complete for {existingObstacleAIs.Length} obstacles");
//     }
    
//     private void IntegrateLevelDistance()
//     {
//         if (levelDistance == null || analyticsManager == null) return;
        
//         // Monitor score changes through LevelDistance.latestRecord
//         Debug.Log("LevelDistance integration setup complete");
//     }
    
//     private IEnumerator MonitorPlayerBehavior()
//     {
//         while (true)
//         {
//             yield return new WaitForSeconds(0.1f); // Monitor 10 times per second
            
//             if (!systemsInitialized || playerMovement == null || analyticsManager == null)
//                 continue;
                
//             Vector3 currentPosition = playerMovement.transform.position;
            
//             // Track position changes
//             if (Vector3.Distance(currentPosition, lastPlayerPosition) > 0.1f)
//             {
//                 analyticsManager.TrackPlayerMovement(currentPosition);
//                 lastPlayerPosition = currentPosition;
//             }
            
//             // Detect player inputs by monitoring position and movement changes
//             DetectPlayerInputs(currentPosition);
//         }
//     }
    
//     private void DetectPlayerInputs(Vector3 currentPosition)
//     {
//         // Detect lane changes
//         float xDifference = currentPosition.x - lastPlayerPosition.x;
//         if (Mathf.Abs(xDifference) > 1.5f) // Lane change threshold
//         {
//             float reactionTime = Time.time - lastInputTime;
            
//             if (xDifference > 0)
//                 analyticsManager?.TrackPlayerInput("swipe_right", reactionTime);
//             else
//                 analyticsManager?.TrackPlayerInput("swipe_left", reactionTime);
                
//             lastInputTime = Time.time;
            
//             // Notify game manager
//             if (xDifference > 0)
//                 gameManager?.OnPlayerSwipeRight();
//             else
//                 gameManager?.OnPlayerSwipeLeft();
//         }
        
//         // Detect jumps (Y position changes)
//         if (currentPosition.y > 2.5f && lastPlayerPosition.y <= 2.5f)
//         {
//             float reactionTime = Time.time - lastInputTime;
//             analyticsManager?.TrackPlayerInput("jump", reactionTime);
//             lastInputTime = Time.time;
            
//             gameManager?.OnPlayerJump();
//         }
        
//         // Detect slides (low Y position)
//         if (currentPosition.y < 1.2f && lastPlayerPosition.y >= 1.2f)
//         {
//             float reactionTime = Time.time - lastInputTime;
//             analyticsManager?.TrackPlayerInput("slide", reactionTime);
//             lastInputTime = Time.time;
//         }
//     }
    
//     private IEnumerator MonitorGameState()
//     {
//         int lastKnownScore = 0;
//         bool wasGameRunning = false;
        
//         while (true)
//         {
//             yield return new WaitForSeconds(1f); // Check every second
            
//             if (!systemsInitialized) continue;
            
//             // Monitor score changes
//             int currentScore = LevelDistance.latestRecord;
//             if (currentScore != lastKnownScore)
//             {
//                 if (gameManager != null)
//                     gameManager.SetScore(currentScore);
                    
//                 lastKnownScore = currentScore;
//             }
            
//             // Monitor game running state
//             bool isGameRunning = PlayerMovement.canMove && !PlayerMovement.isPaused;
            
//             if (isGameRunning && !wasGameRunning)
//             {
//                 // Game started
//                 if (analyticsManager != null)
//                     analyticsManager.StartGameSession();
                    
//                 if (gameManager != null && gameManager.GetGameState() != GameManager.GameState.Playing)
//                     gameManager.StartGame();
//             }
//             else if (!isGameRunning && wasGameRunning)
//             {
//                 // Game ended
//                 if (analyticsManager != null)
//                     analyticsManager.EndGameSession(currentScore, 0); // You can track coins separately
                    
//                 if (gameManager != null && gameManager.GetGameState() == GameManager.GameState.Playing)
//                     gameManager.EndGame();
//             }
            
//             wasGameRunning = isGameRunning;
//         }
//     }
    
//     private IEnumerator DebugSystemStatus()
//     {
//         while (debugMode)
//         {
//             yield return new WaitForSeconds(5f); // Debug every 5 seconds
            
//             Debug.Log("=== System Status ===");
//             Debug.Log($"Firebase: {(firebaseManager?.IsAuthenticated == true ? "Connected" : "Disconnected")}");
//             Debug.Log($"Analytics: {(analyticsManager != null ? "Active" : "Inactive")}");
//             Debug.Log($"Advanced AI: {(advancedAI != null ? advancedAI.GetCurrentAIState() : "Inactive")}");
//             Debug.Log($"Dashboard: {(dashboardManager?.IsOnline() == true ? "Online" : "Offline")}");
//             Debug.Log($"Game State: {(gameManager != null ? gameManager.GetGameState().ToString() : "Unknown")}");
//             Debug.Log($"Current Score: {LevelDistance.latestRecord}");
//             Debug.Log($"Player Moving: {PlayerMovement.canMove}");
//             Debug.Log("==================");
//         }
//     }
    
//     // Public methods for manual triggering (useful for testing)
//     public void TriggerObstacleHit()
//     {
//         if (analyticsManager != null && playerMovement != null)
//         {
//             Vector3 obstaclePos = playerMovement.transform.position + Vector3.forward * 2f;
//             analyticsManager.RecordObstacleHit(obstaclePos);
//             gameManager?.OnPlayerHitObstacle(obstaclePos);
//         }
//     }
    
//     public void TriggerCoinCollection()
//     {
//         if (analyticsManager != null && playerMovement != null)
//         {
//             analyticsManager.RecordCoinCollected(playerMovement.transform.position);
//             gameManager?.AddCoins(1);
//         }
//     }
    
//     public void TriggerPowerUpUsage(string powerUpType)
//     {
//         if (analyticsManager != null && playerMovement != null)
//         {
//             analyticsManager.RecordPowerUpUsed(powerUpType, playerMovement.transform.position);
//         }
//     }
    
//     // Integration status getters
//     public bool AreSystemsInitialized() => systemsInitialized;
//     public bool IsFirebaseConnected() => firebaseManager?.IsAuthenticated == true;
//     public bool IsAnalyticsActive() => analyticsManager != null;
//     public bool IsDashboardOnline() => dashboardManager?.IsOnline() == true;
//     public bool IsAdvancedAIActive() => advancedAI != null;
    
//     // Manual system restart (useful for debugging)
//     public void RestartIntegration()
//     {
//         if (enableFullIntegration)
//         {
//             StopAllCoroutines();
//             systemsInitialized = false;
//             StartCoroutine(InitializeIntegration());
//         }
//     }
// }
