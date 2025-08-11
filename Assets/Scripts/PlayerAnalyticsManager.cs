// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;
// using System.Linq;

// [System.Serializable]
// public class PlayerSessionData
// {
//     public string sessionId;
//     public DateTime sessionStart;
//     public DateTime sessionEnd;
//     public float totalPlayTime;
//     public int highScore;
//     public int totalCoinsCollected;
//     public int totalObstaclesHit;
//     public int totalJumps;
//     public int totalSwipes;
//     public List<GameEvent> gameEvents;
//     public PlayerMovementPattern movementPattern;
//     public string deviceInfo;
//     public string gameVersion;
    
//     public PlayerSessionData()
//     {
//         sessionId = Guid.NewGuid().ToString();
//         sessionStart = DateTime.Now;
//         gameEvents = new List<GameEvent>();
//         movementPattern = new PlayerMovementPattern();
//         // Device info will be set later during proper initialization
//         deviceInfo = "Unknown Device";
//         gameVersion = "1.0"; // Default version, will be updated during initialization
//     }
    
//     public void InitializeDeviceInfo()
//     {
//         deviceInfo = $"{SystemInfo.deviceModel} - {SystemInfo.operatingSystem}";
//         gameVersion = Application.version;
//     }
// }

// [System.Serializable]
// public class GameEvent
// {
//     public string eventType;
//     public float timestamp;
//     public Vector3 playerPosition;
//     public Dictionary<string, object> eventData;
    
//     public GameEvent(string type, Vector3 position, Dictionary<string, object> data = null)
//     {
//         eventType = type;
//         timestamp = Time.time;
//         playerPosition = position;
//         eventData = data ?? new Dictionary<string, object>();
//     }
// }

// [System.Serializable]
// public class PlayerMovementPattern
// {
//     public int leftSwipes;
//     public int rightSwipes;
//     public int jumpCount;
//     public int slideCount;
//     public float averageReactionTime;
//     public List<float> reactionTimes;
//     public Vector3 mostFrequentPosition;
//     public float timeInLeftLane;
//     public float timeInCenterLane;
//     public float timeInRightLane;
//     public List<Vector3> positionHistory;
    
//     public PlayerMovementPattern()
//     {
//         reactionTimes = new List<float>();
//         positionHistory = new List<Vector3>();
//     }
    
//     public void CalculateAverageReactionTime()
//     {
//         if (reactionTimes.Count > 0)
//         {
//             averageReactionTime = reactionTimes.Average();
//         }
//     }
    
//     public void AnalyzeLanePreference()
//     {
//         if (positionHistory.Count == 0) return;
        
//         foreach (Vector3 pos in positionHistory)
//         {
//             if (pos.x < -1f)
//                 timeInLeftLane += Time.deltaTime;
//             else if (pos.x > 1f)
//                 timeInRightLane += Time.deltaTime;
//             else
//                 timeInCenterLane += Time.deltaTime;
//         }
//     }
// }

// public class PlayerAnalyticsManager : MonoBehaviour
// {
//     public static PlayerAnalyticsManager Instance;
    
//     [Header("Analytics Configuration")]
//     public bool enableRealTimeTracking = true;
//     public bool enableBehaviorPrediction = true;
//     public float analyticsUpdateInterval = 1f;
//     public int maxEventsPerSession = 1000;
    
//     // Current session data
//     private PlayerSessionData currentSession;
//     private Dictionary<string, object> currentGameState;
    
//     // Public property to access current session data
//     public PlayerSessionData CurrentSession => currentSession;
//     private float lastObstacleSpawnTime;
//     private float gameStartTime;
//     private bool isTracking;
    
//     // Behavior prediction variables
//     private Queue<Vector3> recentMovements;
//     private Queue<float> recentInputTimings;
//     private const int maxRecentMovements = 20;
//     private const int maxRecentInputs = 15;
    
//     // Performance metrics
//     private int frameCount;
//     private float totalFrameTime;
//     private List<float> fpsHistory;
    
//     public event System.Action<PlayerSessionData> OnSessionCompleted;
//     public event System.Action<Dictionary<string, object>> OnBehaviorPredicted;
    
//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//             InitializeAnalytics();
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }
    
//     private void InitializeAnalytics()
//     {
//         recentMovements = new Queue<Vector3>();
//         recentInputTimings = new Queue<float>();
//         fpsHistory = new List<float>();
//         currentGameState = new Dictionary<string, object>();
        
//         Debug.Log("Player Analytics Manager initialized");
//     }
    
//     private void Start()
//     {
//         // Wait for Firebase to initialize, then start analytics
//         if (FirebaseManager.Instance != null)
//         {
//             FirebaseManager.Instance.OnAuthenticationStateChanged += OnAuthStateChanged;
//         }
        
//         // Auto-start game session after a brief delay
//         StartCoroutine(AutoStartSession());
//         StartCoroutine(AnalyticsUpdateLoop());
//     }
    
//     private IEnumerator AutoStartSession()
//     {
//         // Wait a moment for Firebase to initialize
//         yield return new WaitForSeconds(2f);
        
//         // Auto-start the game session for testing
//         if (currentSession == null)
//         {
//             Debug.Log("Auto-starting game session for testing...");
//             StartGameSession();
//         }
//     }
    
//     private void OnAuthStateChanged(bool isAuthenticated)
//     {
//         if (isAuthenticated)
//         {
//             LoadPlayerAnalyticsHistory();
//         }
//         else
//         {
//             Debug.Log("📊 [ANALYTICS] Firebase not authenticated - running in offline mode");
//         }
//     }
    
//     public void StartGameSession()
//     {
//         currentSession = new PlayerSessionData();
//         currentSession.InitializeDeviceInfo(); // Initialize device info safely
//         gameStartTime = Time.time;
//         isTracking = true;
        
//         LogGameEvent("game_session_started", transform.position, new Dictionary<string, object>
//         {
//             {"session_id", currentSession.sessionId},
//             {"start_time", currentSession.sessionStart.ToString()},
//             {"device_info", currentSession.deviceInfo}
//         });
        
//         // Firebase analytics
//         FirebaseManager.Instance?.LogGameStart();
        
//         Debug.Log($"Game session started: {currentSession.sessionId}");
//     }
    
//     public void EndGameSession(int finalScore, int coinsCollected)
//     {
//         if (currentSession == null || !isTracking) return;
        
//         currentSession.sessionEnd = DateTime.Now;
//         currentSession.totalPlayTime = Time.time - gameStartTime;
//         currentSession.highScore = finalScore;
//         currentSession.totalCoinsCollected = coinsCollected;
//         currentSession.movementPattern.CalculateAverageReactionTime();
//         currentSession.movementPattern.AnalyzeLanePreference();
        
//         LogGameEvent("game_session_ended", transform.position, new Dictionary<string, object>
//         {
//             {"session_id", currentSession.sessionId},
//             {"final_score", finalScore},
//             {"total_coins", coinsCollected},
//             {"play_time", currentSession.totalPlayTime},
//             {"total_events", currentSession.gameEvents.Count}
//         });
        
//         // Firebase analytics
//         FirebaseManager.Instance?.LogGameEnd(finalScore, currentSession.totalPlayTime);
        
//         // Save session data to Firebase
//         SaveSessionToFirebase();
        
//         OnSessionCompleted?.Invoke(currentSession);
//         isTracking = false;
        
//         Debug.Log($"Game session ended. Score: {finalScore}, Time: {currentSession.totalPlayTime:F2}s");
//     }
    
//     public void LogGameEvent(string eventType, Vector3 position, Dictionary<string, object> eventData = null)
//     {
//         if (!isTracking || currentSession == null) return;
        
//         if (currentSession.gameEvents.Count >= maxEventsPerSession)
//         {
//             // Remove oldest event to maintain limit
//             currentSession.gameEvents.RemoveAt(0);
//         }
        
//         GameEvent gameEvent = new GameEvent(eventType, position, eventData);
//         currentSession.gameEvents.Add(gameEvent);
        
//         // Firebase custom events
//         var firebaseParams = new Dictionary<string, object>
//         {
//             {"event_type", eventType},
//             {"position_x", position.x},
//             {"position_y", position.y},
//             {"position_z", position.z},
//             {"timestamp", Time.time}
//         };
        
//         if (eventData != null)
//         {
//             foreach (var kvp in eventData)
//             {
//                 firebaseParams[kvp.Key] = kvp.Value;
//             }
//         }
        
//         FirebaseManager.Instance?.LogEvent("custom_game_event", firebaseParams);
        
//         // Also save to Realtime Database for dashboard
//         FirebaseManager.Instance?.SaveGameEvent(eventType, firebaseParams);
//     }
    
//     public void TrackPlayerMovement(Vector3 position)
//     {
//         if (!isTracking) return;
        
//         // Add to recent movements queue
//         recentMovements.Enqueue(position);
//         if (recentMovements.Count > maxRecentMovements)
//             recentMovements.Dequeue();
            
//         // Add to position history
//         currentSession.movementPattern.positionHistory.Add(position);
        
//         // Update current game state
//         currentGameState["player_position"] = position;
//         currentGameState["timestamp"] = Time.time;
//     }
    
//     public void TrackPlayerInput(string inputType, float reactionTime = 0f)
//     {
//         if (!isTracking) return;
        
//         switch (inputType.ToLower())
//         {
//             case "swipe_left":
//                 currentSession.movementPattern.leftSwipes++;
//                 break;
//             case "swipe_right":
//                 currentSession.movementPattern.rightSwipes++;
//                 break;
//             case "jump":
//                 currentSession.movementPattern.jumpCount++;
//                 break;
//             case "slide":
//                 currentSession.movementPattern.slideCount++;
//                 break;
//         }
        
//         if (reactionTime > 0)
//         {
//             currentSession.movementPattern.reactionTimes.Add(reactionTime);
//             recentInputTimings.Enqueue(reactionTime);
            
//             if (recentInputTimings.Count > maxRecentInputs)
//                 recentInputTimings.Dequeue();
//         }
        
//         LogGameEvent("player_input", GetPlayerPosition(), new Dictionary<string, object>
//         {
//             {"input_type", inputType},
//             {"reaction_time", reactionTime}
//         });
//     }
    
//     public void TrackObstacleInteraction(string interactionType, Vector3 obstaclePosition)
//     {
//         if (!isTracking) return;
        
//         if (interactionType == "hit")
//         {
//             currentSession.totalObstaclesHit++;
//         }
        
//         LogGameEvent("obstacle_interaction", GetPlayerPosition(), new Dictionary<string, object>
//         {
//             {"interaction_type", interactionType},
//             {"obstacle_position", obstaclePosition},
//             {"player_position", GetPlayerPosition()}
//         });
//     }
    
//     public Dictionary<string, object> PredictPlayerBehavior()
//     {
//         if (!enableBehaviorPrediction || recentMovements.Count < 5)
//             return null;
            
//         var prediction = new Dictionary<string, object>();
        
//         // Predict next lane based on movement pattern
//         Vector3 lastPos = recentMovements.Last();
//         Vector3[] recentPosArray = recentMovements.ToArray();
        
//         // Calculate movement trend
//         float avgXVelocity = 0f;
//         for (int i = 1; i < recentPosArray.Length; i++)
//         {
//             avgXVelocity += recentPosArray[i].x - recentPosArray[i-1].x;
//         }
//         avgXVelocity /= recentPosArray.Length - 1;
        
//         // Predict next lane
//         string predictedLane = "center";
//         if (avgXVelocity > 0.1f)
//             predictedLane = "right";
//         else if (avgXVelocity < -0.1f)
//             predictedLane = "left";
            
//         prediction["predicted_lane"] = predictedLane;
//         prediction["confidence"] = Mathf.Abs(avgXVelocity) * 100f;
        
//         // Predict reaction time based on recent inputs
//         if (recentInputTimings.Count > 0)
//         {
//             prediction["predicted_reaction_time"] = recentInputTimings.Average();
//         }
        
//         // Predict difficulty preference
//         float avgReactionTime = currentSession.movementPattern.averageReactionTime;
//         if (avgReactionTime < 0.3f)
//             prediction["difficulty_preference"] = "high";
//         else if (avgReactionTime > 0.6f)
//             prediction["difficulty_preference"] = "low";
//         else
//             prediction["difficulty_preference"] = "medium";
            
//         OnBehaviorPredicted?.Invoke(prediction);
//         return prediction;
//     }
    
//     private Vector3 GetPlayerPosition()
//     {
//         // Try to get player position from PlayerMovement script
//         var player = FindObjectOfType<PlayerMovement>();
//         return player != null ? player.transform.position : Vector3.zero;
//     }
    
//     private IEnumerator AnalyticsUpdateLoop()
//     {
//         while (true)
//         {
//             yield return new WaitForSeconds(analyticsUpdateInterval);
            
//             if (isTracking && enableRealTimeTracking)
//             {
//                 UpdatePerformanceMetrics();
//                 TrackPlayerMovement(GetPlayerPosition());
                
//                 if (enableBehaviorPrediction)
//                 {
//                     PredictPlayerBehavior();
//                 }
//             }
//         }
//     }
    
//     private void UpdatePerformanceMetrics()
//     {
//         frameCount++;
//         totalFrameTime += Time.deltaTime;
        
//         float currentFPS = 1f / Time.deltaTime;
//         fpsHistory.Add(currentFPS);
        
//         // Keep only last 60 fps readings
//         if (fpsHistory.Count > 60)
//             fpsHistory.RemoveAt(0);
            
//         // Update current game state
//         currentGameState["fps"] = currentFPS;
//         currentGameState["avg_fps"] = fpsHistory.Average();
//         currentGameState["frame_count"] = frameCount;
//     }
    
//     private void SaveSessionToFirebase()
//     {
//         if (FirebaseManager.Instance == null || !FirebaseManager.Instance.IsAuthenticated)
//         {
//             Debug.Log("📊 [ANALYTICS] Firebase not available - session data saved locally only");
//             SaveSessionLocally(); // Save to local storage instead
//             return;
//         }
        
//         string sessionPath = $"player_sessions/{currentSession.sessionId}";
        
//         // Convert session data to dictionary for Firebase
//         var sessionData = new Dictionary<string, object>
//         {
//             {"sessionId", currentSession.sessionId},
//             {"sessionStart", currentSession.sessionStart.ToString()},
//             {"sessionEnd", currentSession.sessionEnd.ToString()},
//             {"totalPlayTime", currentSession.totalPlayTime},
//             {"highScore", currentSession.highScore},
//             {"totalCoinsCollected", currentSession.totalCoinsCollected},
//             {"totalObstaclesHit", currentSession.totalObstaclesHit},
//             {"deviceInfo", currentSession.deviceInfo},
//             {"gameVersion", currentSession.gameVersion},
//             {"gameEvents", currentSession.gameEvents.Count}
//         };
        
//         FirebaseManager.Instance.SavePlayerData(sessionPath, sessionData);
//         Debug.Log($"Session data saved: {currentSession.sessionId}");
//         UpdatePlayerStats();
//     }
    
//     private void SaveSessionLocally()
//     {
//         try
//         {
//             // Save session data locally when Firebase is not available
//             string localPath = Application.persistentDataPath + "/analytics_backup/";
//             if (!System.IO.Directory.Exists(localPath))
//             {
//                 System.IO.Directory.CreateDirectory(localPath);
//             }
            
//             var sessionData = new Dictionary<string, object>
//             {
//                 {"sessionId", currentSession.sessionId},
//                 {"sessionStart", currentSession.sessionStart.ToString()},
//                 {"sessionEnd", currentSession.sessionEnd.ToString()},
//                 {"totalPlayTime", currentSession.totalPlayTime},
//                 {"highScore", currentSession.highScore},
//                 {"totalCoinsCollected", currentSession.totalCoinsCollected},
//                 {"totalObstaclesHit", currentSession.totalObstaclesHit},
//                 {"deviceInfo", currentSession.deviceInfo},
//                 {"gameVersion", currentSession.gameVersion},
//                 {"gameEvents", currentSession.gameEvents.Count}
//             };
            
//             string jsonData = JsonUtility.ToJson(new SerializableSessionData(sessionData), true);
//             string fileName = localPath + $"session_{currentSession.sessionId}.json";
//             System.IO.File.WriteAllText(fileName, jsonData);
            
//             Debug.Log($"📊 [ANALYTICS] Session saved locally: {fileName}");
//         }
//         catch (System.Exception ex)
//         {
//             Debug.LogError($"📊 [ANALYTICS] Failed to save session locally: {ex.Message}");
//         }
//     }
    
//     [System.Serializable]
//     private class SerializableSessionData
//     {
//         public List<string> keys = new List<string>();
//         public List<string> values = new List<string>();
        
//         public SerializableSessionData(Dictionary<string, object> data)
//         {
//             foreach (var kvp in data)
//             {
//                 keys.Add(kvp.Key);
//                 values.Add(kvp.Value.ToString());
//             }
//         }
//     }
    
//     private void UpdatePlayerStats()
//     {
//         // Update aggregated player statistics
//         var playerStats = new Dictionary<string, object>
//         {
//             {"total_sessions", 1}, // This would be incremented
//             {"total_play_time", currentSession.totalPlayTime},
//             {"highest_score", currentSession.highScore},
//             {"total_coins_collected", currentSession.totalCoinsCollected},
//             {"last_session", DateTime.Now.ToString()},
//             {"avg_reaction_time", currentSession.movementPattern.averageReactionTime},
//             {"favorite_lane", GetFavoriteLane()}
//         };
        
//         FirebaseManager.Instance.SavePlayerData("player_stats", playerStats);
//     }
    
//     private string GetFavoriteLane()
//     {
//         var pattern = currentSession.movementPattern;
//         if (pattern.timeInLeftLane > pattern.timeInCenterLane && pattern.timeInLeftLane > pattern.timeInRightLane)
//             return "left";
//         else if (pattern.timeInRightLane > pattern.timeInCenterLane && pattern.timeInRightLane > pattern.timeInLeftLane)
//             return "right";
//         else
//             return "center";
//     }
    
//     private void LoadPlayerAnalyticsHistory()
//     {
//         // Safety check - only load if Firebase is properly initialized
//         if (FirebaseManager.Instance?.IsInitialized != true)
//         {
//             Debug.Log("📊 [ANALYTICS] Firebase not ready - using local analytics only");
//             return;
//         }
        
//         FirebaseManager.Instance.LoadUserData<Dictionary<string, object>>("player_stats", stats =>
//         {
//             if (stats != null)
//             {
//                 Debug.Log("Player analytics history loaded successfully");
//                 // Use historical data to improve behavior prediction
//             }
//         });
//     }
    
//     // Public methods for easy integration with existing game systems
//     public void RecordJump() => TrackPlayerInput("jump");
//     public void RecordSlide() => TrackPlayerInput("slide");
//     public void RecordSwipeLeft() => TrackPlayerInput("swipe_left");
//     public void RecordSwipeRight() => TrackPlayerInput("swipe_right");
//     public void RecordObstacleHit(Vector3 obstaclePos) => TrackObstacleInteraction("hit", obstaclePos);
//     public void RecordObstacleAvoided(Vector3 obstaclePos) => TrackObstacleInteraction("avoided", obstaclePos);
//     public void RecordCoinCollected(Vector3 coinPos) => LogGameEvent("coin_collected", coinPos);
//     public void RecordPowerUpUsed(string powerUpType, Vector3 position) => 
//         LogGameEvent("powerup_used", position, new Dictionary<string, object> { {"type", powerUpType} });
// }
 