// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using System;

// public class GameManager : MonoBehaviour
// {
//     public static GameManager Instance;
    
//     [Header("Game Configuration")]
//     public bool enableFirebase = true;
//     public bool enableAnalytics = true;
//     public bool enableCloudDashboard = true;
//     public bool enableAdvancedAI = true;
//     public bool autoStartSession = true;
    
//     [Header("Game State")]
//     public GameState currentGameState = GameState.Menu;
//     public int currentScore = 0;
//     public int coinsCollected = 0;
//     public float gameStartTime = 0f;
//     public bool isPaused = false;
    
//     [Header("UI References")]
//     public GameObject menuUI;
//     public GameObject gameUI;
//     public GameObject pauseUI;
//     public GameObject gameOverUI;
//     public GameObject loadingUI;
    
//     [Header("Player References")]
//     public PlayerMovement playerMovement;
//     public GameObject playerObject;
    
//     [Header("AI References")]
//     public AdvancedAIBehavior advancedAI;
//     public ObstacleAI[] obstacleAIs;
    
//     // Game state management
//     public enum GameState
//     {
//         Loading,
//         Menu,
//         Playing,
//         Paused,
//         GameOver,
//         Restart
//     }
    
//     // Events
//     public event System.Action<GameState> OnGameStateChanged;
//     public event System.Action<int> OnScoreChanged;
//     public event System.Action<int> OnCoinsChanged;
//     public event System.Action OnGameStarted;
//     public event System.Action OnGameEnded;
    
//     // Private variables
//     private bool isInitialized = false;
//     [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used for future score update timing features")]
//     private float lastScoreUpdate = 0f;
//     private int lastKnownScore = 0;
    
//     private void Awake()
//     {
//         // Singleton pattern
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//             return;
//         }
        
//         // Initialize systems
//         StartCoroutine(InitializeGame());
//     }
    
//     private IEnumerator InitializeGame()
//     {
//         SetGameState(GameState.Loading);
        
//         Debug.Log("Initializing Game Manager...");
        
//         // Show loading UI
//         if (loadingUI != null)
//             loadingUI.SetActive(true);
            
//         // Wait for Firebase to initialize if enabled
//         if (enableFirebase && FirebaseManager.Instance != null)
//         {
//             Debug.Log("Waiting for Firebase initialization...");
//             float timeout = 10f;
//             float elapsed = 0f;
            
//             while (!FirebaseManager.Instance.IsAuthenticated && elapsed < timeout)
//             {
//                 yield return new WaitForSeconds(0.1f);
//                 elapsed += 0.1f;
//             }
            
//             if (!FirebaseManager.Instance.IsAuthenticated)
//             {
//                 Debug.LogWarning("Firebase authentication timeout - proceeding with anonymous signin");
//                 FirebaseManager.Instance.SignInAnonymously();
//             }
            
//             Debug.Log("Firebase initialized");
//         }
        
//         // Initialize Analytics
//         if (enableAnalytics && PlayerAnalyticsManager.Instance != null)
//         {
//             Debug.Log("Analytics system ready");
//             PlayerAnalyticsManager.Instance.OnSessionCompleted += OnAnalyticsSessionCompleted;
//         }
        
//         // Initialize Cloud Dashboard
//         if (enableCloudDashboard && CloudDashboardManager.Instance != null)
//         {
//             Debug.Log("Cloud Dashboard system ready");
//             CloudDashboardManager.Instance.OnConnectionStatusChanged += OnDashboardConnectionChanged;
//         }
        
//         // Initialize AI system
//         if (enableAdvancedAI)
//         {
//             SetupAdvancedAI();
//         }
        
//         // Setup player references
//         if (playerMovement == null)
//             playerMovement = FindObjectOfType<PlayerMovement>();
            
//         if (playerObject == null && playerMovement != null)
//             playerObject = playerMovement.gameObject;
            
//         // Hide loading UI
//         if (loadingUI != null)
//             loadingUI.SetActive(false);
            
//         isInitialized = true;
//         SetGameState(GameState.Menu);
        
//         Debug.Log("Game Manager initialization complete");
        
//         // Auto-start if enabled
//         if (autoStartSession)
//         {
//             StartGame();
//         }
//     }
    
//     private void SetupAdvancedAI()
//     {
//         // Find or create advanced AI
//         if (advancedAI == null)
//             advancedAI = FindObjectOfType<AdvancedAIBehavior>();
            
//         if (advancedAI == null)
//         {
//             // Create advanced AI if not found
//             var aiObject = new GameObject("AdvancedAI");
//             advancedAI = aiObject.AddComponent<AdvancedAIBehavior>();
            
//             if (playerMovement != null)
//                 advancedAI.player = playerMovement.transform;
//         }
        
//         // Find all obstacle AIs
//         obstacleAIs = FindObjectsOfType<ObstacleAI>();
        
//         Debug.Log($"Advanced AI setup complete. Found {obstacleAIs.Length} obstacle AIs");
//     }
    
//     private void Start()
//     {
//         // Subscribe to game events
//         if (playerMovement != null)
//         {
//             // Note: You'll need to add these events to PlayerMovement script
//             // playerMovement.OnObstacleHit += OnPlayerHitObstacle;
//             // playerMovement.OnCoinCollected += OnPlayerCoinCollected;
//         }
//     }
    
//     private void Update()
//     {
//         if (!isInitialized) return;
        
//         // Update game systems
//         UpdateScore();
//         UpdateGameLogic();
//         HandleInputs();
//     }
    
//     private void UpdateScore()
//     {
//         // Get score from LevelDistance component
//         var levelDistance = FindObjectOfType<LevelDistance>();
//         if (levelDistance != null)
//         {
//             int newScore = LevelDistance.latestRecord;
//             if (newScore != lastKnownScore)
//             {
//                 SetScore(newScore);
//                 lastKnownScore = newScore;
//             }
//         }
        
//         // Update coins from game systems
//         // This would need to be connected to your coin collection system
//     }
    
//     private void UpdateGameLogic()
//     {
//         switch (currentGameState)
//         {
//             case GameState.Playing:
//                 UpdatePlayingState();
//                 break;
//             case GameState.Paused:
//                 UpdatePausedState();
//                 break;
//             case GameState.GameOver:
//                 UpdateGameOverState();
//                 break;
//         }
//     }
    
//     private void UpdatePlayingState()
//     {
//         // Check for game over conditions
//         if (PlayerMovement.isPaused && currentGameState == GameState.Playing)
//         {
//             EndGame();
//         }
//     }
    
//     private void UpdatePausedState()
//     {
//         // Handle pause state logic
//         if (!PlayerMovement.isPaused && currentGameState == GameState.Paused)
//         {
//             ResumeGame();
//         }
//     }
    
//     private void UpdateGameOverState()
//     {
//         // Handle game over state logic
//         if (PlayerMovement.startAgain)
//         {
//             RestartGame();
//         }
//     }
    
//     private void HandleInputs()
//     {
//         // Handle global game inputs
//         if (Input.GetKeyDown(KeyCode.Escape))
//         {
//             if (currentGameState == GameState.Playing)
//                 PauseGame();
//             else if (currentGameState == GameState.Paused)
//                 ResumeGame();
//         }
        
//         // Debug inputs (remove in production)
//         if (Debug.isDebugBuild)
//         {
//             if (Input.GetKeyDown(KeyCode.R))
//                 RestartGame();
//         }
//     }
    
//     public void StartGame()
//     {
//         if (currentGameState == GameState.Playing) return;
        
//         Debug.Log("Starting new game...");
        
//         gameStartTime = Time.time;
//         currentScore = 0;
//         coinsCollected = 0;
//         isPaused = false;
        
//         // Reset player state
//         if (playerMovement != null)
//         {
//             PlayerMovement.canMove = true;
//             PlayerMovement.isPaused = false;
//             PlayerMovement.startAgain = false;
//         }
        
//         // Start analytics session
//         if (enableAnalytics && PlayerAnalyticsManager.Instance != null)
//         {
//             PlayerAnalyticsManager.Instance.StartGameSession();
//         }
        
//         // Reset AI systems
//         if (enableAdvancedAI && advancedAI != null)
//         {
//             // Reset AI state - you might need to add a Reset method to AdvancedAIBehavior
//         }
        
//         SetGameState(GameState.Playing);
//         OnGameStarted?.Invoke();
        
//         // Log game start events
//         LogGameEvent("game_started", new Dictionary<string, object>
//         {
//             {"timestamp", DateTime.Now.ToString()},
//             {"firebase_enabled", enableFirebase},
//             {"analytics_enabled", enableAnalytics},
//             {"ai_enabled", enableAdvancedAI}
//         });
//     }
    
//     public void PauseGame()
//     {
//         if (currentGameState != GameState.Playing) return;
        
//         Debug.Log("Game paused");
        
//         isPaused = true;
//         PlayerMovement.isPaused = true;
//         Time.timeScale = 0f;
        
//         SetGameState(GameState.Paused);
        
//         // Show pause UI
//         if (pauseUI != null)
//             pauseUI.SetActive(true);
            
//         LogGameEvent("game_paused", new Dictionary<string, object>
//         {
//             {"play_time", Time.time - gameStartTime},
//             {"score", currentScore}
//         });
//     }
    
//     public void ResumeGame()
//     {
//         if (currentGameState != GameState.Paused) return;
        
//         Debug.Log("Game resumed");
        
//         isPaused = false;
//         PlayerMovement.isPaused = false;
//         Time.timeScale = 1f;
        
//         SetGameState(GameState.Playing);
        
//         // Hide pause UI
//         if (pauseUI != null)
//             pauseUI.SetActive(false);
            
//         LogGameEvent("game_resumed");
//     }
    
//     public void EndGame()
//     {
//         if (currentGameState == GameState.GameOver) return;
        
//         Debug.Log($"Game ended. Final score: {currentScore}");
        
//         float playTime = Time.time - gameStartTime;
        
//         // Stop player movement
//         if (playerMovement != null)
//         {
//             PlayerMovement.canMove = false;
//             PlayerMovement.isPaused = true;
//         }
        
//         // End analytics session
//         if (enableAnalytics && PlayerAnalyticsManager.Instance != null)
//         {
//             PlayerAnalyticsManager.Instance.EndGameSession(currentScore, coinsCollected);
//         }
        
//         // Update cloud dashboard
//         if (enableCloudDashboard && CloudDashboardManager.Instance != null)
//         {
//             CloudDashboardManager.Instance.UpdateLeaderboard(currentScore);
//         }
        
//         SetGameState(GameState.GameOver);
//         OnGameEnded?.Invoke();
        
//         // Show game over UI
//         if (gameOverUI != null)
//             gameOverUI.SetActive(true);
            
//         LogGameEvent("game_ended", new Dictionary<string, object>
//         {
//             {"final_score", currentScore},
//             {"play_time", playTime},
//             {"coins_collected", coinsCollected}
//         });
//     }
    
//     public void RestartGame()
//     {
//         Debug.Log("Restarting game...");
        
//         SetGameState(GameState.Restart);
        
//         // Hide all UI
//         if (gameOverUI != null) gameOverUI.SetActive(false);
//         if (pauseUI != null) pauseUI.SetActive(false);
        
//         // Reset game state
//         Time.timeScale = 1f;
        
//         // Restart the game
//         StartGame();
//     }
    
//     public void QuitToMenu()
//     {
//         Debug.Log("Returning to menu...");
        
//         // End current session if playing
//         if (currentGameState == GameState.Playing || currentGameState == GameState.Paused)
//         {
//             EndGame();
//         }
        
//         Time.timeScale = 1f;
//         SetGameState(GameState.Menu);
        
//         // Show menu UI
//         if (menuUI != null) menuUI.SetActive(true);
//         if (gameUI != null) gameUI.SetActive(false);
//         if (pauseUI != null) pauseUI.SetActive(false);
//         if (gameOverUI != null) gameOverUI.SetActive(false);
//     }
    
//     public void QuitGame()
//     {
//         Debug.Log("Quitting game...");
        
//         // End current session
//         if (currentGameState == GameState.Playing || currentGameState == GameState.Paused)
//         {
//             EndGame();
//         }
        
//         LogGameEvent("game_quit");
        
//         #if UNITY_EDITOR
//             UnityEditor.EditorApplication.isPlaying = false;
//         #else
//             Application.Quit();
//         #endif
//     }
    
//     private void SetGameState(GameState newState)
//     {
//         if (currentGameState == newState) return;
        
//         GameState previousState = currentGameState;
//         currentGameState = newState;
        
//         Debug.Log($"Game state changed: {previousState} -> {newState}");
        
//         OnGameStateChanged?.Invoke(newState);
        
//         // Update UI based on state
//         UpdateUI();
        
//         LogGameEvent("game_state_changed", new Dictionary<string, object>
//         {
//             {"previous_state", previousState.ToString()},
//             {"new_state", newState.ToString()}
//         });
//     }
    
//     private void UpdateUI()
//     {
//         // Update UI visibility based on game state
//         switch (currentGameState)
//         {
//             case GameState.Loading:
//                 if (loadingUI != null) loadingUI.SetActive(true);
//                 break;
                
//             case GameState.Menu:
//                 if (menuUI != null) menuUI.SetActive(true);
//                 if (gameUI != null) gameUI.SetActive(false);
//                 if (pauseUI != null) pauseUI.SetActive(false);
//                 if (gameOverUI != null) gameOverUI.SetActive(false);
//                 if (loadingUI != null) loadingUI.SetActive(false);
//                 break;
                
//             case GameState.Playing:
//                 if (menuUI != null) menuUI.SetActive(false);
//                 if (gameUI != null) gameUI.SetActive(true);
//                 if (pauseUI != null) pauseUI.SetActive(false);
//                 if (gameOverUI != null) gameOverUI.SetActive(false);
//                 break;
                
//             case GameState.Paused:
//                 if (pauseUI != null) pauseUI.SetActive(true);
//                 break;
                
//             case GameState.GameOver:
//                 if (gameOverUI != null) gameOverUI.SetActive(true);
//                 if (gameUI != null) gameUI.SetActive(false);
//                 break;
//         }
//     }
    
//     public void SetScore(int newScore)
//     {
//         if (currentScore == newScore) return;
        
//         int previousScore = currentScore;
//         currentScore = newScore;
        
//         OnScoreChanged?.Invoke(currentScore);
        
//         // Track score milestones
//         if (currentScore > 0 && currentScore % 1000 == 0)
//         {
//             LogGameEvent("score_milestone", new Dictionary<string, object>
//             {
//                 {"milestone", currentScore},
//                 {"time_to_milestone", Time.time - gameStartTime}
//             });
//         }
//     }
    
//     public void AddCoins(int amount)
//     {
//         coinsCollected += amount;
//         OnCoinsChanged?.Invoke(coinsCollected);
        
//         if (enableAnalytics && PlayerAnalyticsManager.Instance != null)
//         {
//             PlayerAnalyticsManager.Instance.RecordCoinCollected(
//                 playerMovement != null ? playerMovement.transform.position : Vector3.zero
//             );
//         }
        
//         LogGameEvent("coin_collected", new Dictionary<string, object>
//         {
//             {"amount", amount},
//             {"total_coins", coinsCollected}
//         });
//     }
    
//     // Event handlers for player actions
//     public void OnPlayerHitObstacle(Vector3 obstaclePosition)
//     {
//         if (enableAnalytics && PlayerAnalyticsManager.Instance != null)
//         {
//             PlayerAnalyticsManager.Instance.RecordObstacleHit(obstaclePosition);
//         }
        
//         LogGameEvent("obstacle_hit", new Dictionary<string, object>
//         {
//             {"obstacle_position", obstaclePosition},
//             {"player_score", currentScore}
//         });
        
//         // Check if this should trigger game over
//         // This depends on your game's rules
//     }
    
//     public void OnPlayerJump()
//     {
//         if (enableAnalytics && PlayerAnalyticsManager.Instance != null)
//         {
//             PlayerAnalyticsManager.Instance.RecordJump();
//         }
        
//         LogGameEvent("player_jump");
//     }
    
//     public void OnPlayerSwipeLeft()
//     {
//         if (enableAnalytics && PlayerAnalyticsManager.Instance != null)
//         {
//             PlayerAnalyticsManager.Instance.RecordSwipeLeft();
//         }
        
//         LogGameEvent("player_swipe_left");
//     }
    
//     public void OnPlayerSwipeRight()
//     {
//         if (enableAnalytics && PlayerAnalyticsManager.Instance != null)
//         {
//             PlayerAnalyticsManager.Instance.RecordSwipeRight();
//         }
        
//         LogGameEvent("player_swipe_right");
//     }
    
//     private void LogGameEvent(string eventName, Dictionary<string, object> parameters = null)
//     {
//         // Log to Firebase Analytics
//         if (enableFirebase && FirebaseManager.Instance != null)
//         {
//             FirebaseManager.Instance.LogEvent(eventName, parameters);
//         }
        
//         // Log to Player Analytics
//         if (enableAnalytics && PlayerAnalyticsManager.Instance != null && playerMovement != null)
//         {
//             PlayerAnalyticsManager.Instance.LogGameEvent(eventName, playerMovement.transform.position, parameters);
//         }
//     }
    
//     // Event handlers for external systems
//     private void OnAnalyticsSessionCompleted(PlayerSessionData sessionData)
//     {
//         Debug.Log($"Analytics session completed: {sessionData.sessionId}");
        
//         // You can add additional logic here for session completion
//     }
    
//     private void OnDashboardConnectionChanged(bool isConnected)
//     {
//         Debug.Log($"Dashboard connection status: {(isConnected ? "Connected" : "Disconnected")}");
        
//         // You can update UI to show connection status
//     }
    
//     // Public getters for external systems
//     public GameState GetGameState() => currentGameState;
//     public int GetCurrentScore() => currentScore;
//     public int GetCoinsCollected() => coinsCollected;
//     public float GetPlayTime() => Time.time - gameStartTime;
//     public bool IsGamePlaying() => currentGameState == GameState.Playing;
    
//     private void OnDestroy()
//     {
//         // Clean up subscriptions
//         if (PlayerAnalyticsManager.Instance != null)
//         {
//             PlayerAnalyticsManager.Instance.OnSessionCompleted -= OnAnalyticsSessionCompleted;
//         }
        
//         if (CloudDashboardManager.Instance != null)
//         {
//             CloudDashboardManager.Instance.OnConnectionStatusChanged -= OnDashboardConnectionChanged;
//         }
//     }
// }
