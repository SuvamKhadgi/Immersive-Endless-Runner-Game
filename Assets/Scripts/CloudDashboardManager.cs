// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Networking;
// using System;
// using System.Text;

// [System.Serializable]
// public class DashboardMetrics
// {
//     public string userId;
//     public string sessionId;
//     public DateTime timestamp;
//     public GameMetrics gameMetrics;
//     public AIMetrics aiMetrics;
//     public PerformanceMetrics performanceMetrics;
//     public PlayerBehaviorMetrics behaviorMetrics;
    
//     public DashboardMetrics()
//     {
//         timestamp = DateTime.Now;
//         gameMetrics = new GameMetrics();
//         aiMetrics = new AIMetrics();
//         performanceMetrics = new PerformanceMetrics();
//         behaviorMetrics = new PlayerBehaviorMetrics();
//     }
// }

// [System.Serializable]
// public class GameMetrics
// {
//     public int currentScore;
//     public int highScore;
//     public float playTime;
//     public int coinsCollected;
//     public int obstaclesHit;
//     public int powerUpsUsed;
//     public float averageSpeed;
//     public string currentLevel;
//     public int totalJumps;
//     public int totalSwipes;
// }

// [System.Serializable]
// public class AIMetrics
// {
//     public string currentAIState;
//     public float predictionAccuracy;
//     public float difficultyMultiplier;
//     public int totalPredictions;
//     public int correctPredictions;
//     public string currentBehaviorPattern;
//     public float interceptionSuccessRate;
//     public Dictionary<string, float> playerPatternWeights;
// }

// [System.Serializable]
// public class PerformanceMetrics
// {
//     public float currentFPS;
//     public float averageFPS;
//     public float minFPS;
//     public float maxFPS;
//     public long memoryUsage;
//     public float batteryLevel;
//     public string deviceInfo;
//     public float networkLatency;
// }

// [System.Serializable]
// public class PlayerBehaviorMetrics
// {
//     public float averageReactionTime;
//     public string favoriteLane;
//     public int laneChanges;
//     public float timeInEachLane;
//     public string playStyle; // "Aggressive", "Defensive", "Balanced"
//     public float adaptabilityScore;
//     public int streakCount;
//     public float focusLevel; // Based on input consistency
// }

// public class CloudDashboardManager : MonoBehaviour
// {
//     public static CloudDashboardManager Instance;
    
//     [Header("Dashboard Configuration")]
//     public string dashboardEndpoint = "https://your-dashboard-api.com";
//     public string apiKey = "your-api-key";
//     public bool enableRealTimeDashboard = true;
//     public float dashboardUpdateInterval = 30f; // Send data every 30 seconds
//     public bool enableOfflineMode = true; // Store data when offline
    
//     [Header("Analytics Endpoints")]
//     public string metricsEndpoint = "/api/metrics";
//     public string heatmapEndpoint = "/api/heatmap";
//     public string leaderboardEndpoint = "/api/leaderboard";
//     public string aiInsightsEndpoint = "/api/ai-insights";
    
//     // Current metrics
//     private DashboardMetrics currentMetrics;
//     private Queue<DashboardMetrics> offlineDataQueue;
//     private bool isOnline = true;
    
//     // Performance tracking
//     private List<float> fpsHistory;
//     private long totalMemoryUsage = 0L;
//     private int memoryReadings = 0;
    
//     // Network monitoring
//     [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used for future network latency features")]
//     private float lastPingTime = 0f;
//     private float networkLatency = 0f;
    
//     // Events
//     public event System.Action<DashboardMetrics> OnMetricsUpdated;
//     public event System.Action<bool> OnConnectionStatusChanged;
    
//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//             InitializeDashboard();
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }
    
//     private void InitializeDashboard()
//     {
//         currentMetrics = new DashboardMetrics();
//         offlineDataQueue = new Queue<DashboardMetrics>();
//         fpsHistory = new List<float>();
        
//         // Start coroutines
//         if (enableRealTimeDashboard)
//         {
//             StartCoroutine(DashboardUpdateLoop());
//             StartCoroutine(NetworkMonitorLoop());
//         }
        
//         Debug.Log("Cloud Dashboard Manager initialized");
//     }
    
//     private void Start()
//     {
//         // Subscribe to analytics events
//         if (PlayerAnalyticsManager.Instance != null)
//         {
//             PlayerAnalyticsManager.Instance.OnSessionCompleted += OnSessionCompleted;
//             PlayerAnalyticsManager.Instance.OnBehaviorPredicted += OnBehaviorPredicted;
//         }
        
//         // Test connection to dashboard
//         StartCoroutine(TestConnection());
//     }
    
//     private void Update()
//     {
//         UpdatePerformanceMetrics();
//         UpdateGameMetrics();
//         UpdateBehaviorMetrics();
        
//         // Update AI metrics if available
//         var aiComponent = FindObjectOfType<AdvancedAIBehavior>();
//         if (aiComponent != null)
//         {
//             UpdateAIMetrics(aiComponent);
//         }
//     }
    
//     private void UpdateGameMetrics()
//     {
//         // Get current game state
//         if (PlayerAnalyticsManager.Instance?.CurrentSession != null)
//         {
//             var session = PlayerAnalyticsManager.Instance.CurrentSession;
//             currentMetrics.gameMetrics.playTime = Time.time;
//             currentMetrics.gameMetrics.highScore = session.highScore;
//             currentMetrics.gameMetrics.coinsCollected = session.totalCoinsCollected;
//             currentMetrics.gameMetrics.obstaclesHit = session.totalObstaclesHit;
//             currentMetrics.gameMetrics.totalJumps = session.movementPattern.jumpCount;
//             currentMetrics.gameMetrics.totalSwipes = session.movementPattern.leftSwipes + session.movementPattern.rightSwipes;
//         }
        
//         // Get current score from game systems
//         var distanceComponent = FindObjectOfType<LevelDistance>();
//         if (distanceComponent != null)
//         {
//             currentMetrics.gameMetrics.currentScore = LevelDistance.latestRecord;
//         }
        
//         // Calculate average speed
//         if (PlayerMovement.canMove)
//         {
//             currentMetrics.gameMetrics.averageSpeed = PlayerMovement.moveSpeed;
//         }
//     }
    
//     private void UpdateAIMetrics(AdvancedAIBehavior aiComponent)
//     {
//         currentMetrics.aiMetrics.currentAIState = aiComponent.GetCurrentAIState();
//         currentMetrics.aiMetrics.predictionAccuracy = aiComponent.GetPredictionAccuracy();
//         currentMetrics.aiMetrics.difficultyMultiplier = aiComponent.GetCurrentDifficulty();
//         currentMetrics.aiMetrics.playerPatternWeights = aiComponent.GetPlayerPatternWeights();
        
//         // Calculate success rates
//         // These would be tracked by the AI component
//     }
    
//     private void UpdatePerformanceMetrics()
//     {
//         // FPS tracking
//         float currentFPS = 1f / Time.unscaledDeltaTime;
//         fpsHistory.Add(currentFPS);
        
//         if (fpsHistory.Count > 60) // Keep last 60 frames
//             fpsHistory.RemoveAt(0);
            
//         currentMetrics.performanceMetrics.currentFPS = currentFPS;
        
//         if (fpsHistory.Count > 0)
//         {
//             float sum = 0f;
//             float min = float.MaxValue;
//             float max = float.MinValue;
            
//             foreach (float fps in fpsHistory)
//             {
//                 sum += fps;
//                 if (fps < min) min = fps;
//                 if (fps > max) max = fps;
//             }
            
//             currentMetrics.performanceMetrics.averageFPS = sum / fpsHistory.Count;
//             currentMetrics.performanceMetrics.minFPS = min;
//             currentMetrics.performanceMetrics.maxFPS = max;
//         }
        
//         // Memory usage
//         long currentMemory = System.GC.GetTotalMemory(false);
//         totalMemoryUsage += currentMemory;
//         memoryReadings++;
//         currentMetrics.performanceMetrics.memoryUsage = (long)(totalMemoryUsage / memoryReadings);
        
//         // Device info
//         currentMetrics.performanceMetrics.deviceInfo = $"{SystemInfo.deviceModel} - {SystemInfo.operatingSystem}";
        
//         // Battery level (if available)
//         currentMetrics.performanceMetrics.batteryLevel = SystemInfo.batteryLevel;
        
//         // Network latency
//         currentMetrics.performanceMetrics.networkLatency = networkLatency;
//     }
    
//     private void UpdateBehaviorMetrics()
//     {
//         if (PlayerAnalyticsManager.Instance?.CurrentSession?.movementPattern != null)
//         {
//             var pattern = PlayerAnalyticsManager.Instance.CurrentSession.movementPattern;
            
//             currentMetrics.behaviorMetrics.averageReactionTime = pattern.averageReactionTime;
//             currentMetrics.behaviorMetrics.laneChanges = pattern.leftSwipes + pattern.rightSwipes;
            
//             // Determine favorite lane
//             if (pattern.timeInLeftLane > pattern.timeInCenterLane && pattern.timeInLeftLane > pattern.timeInRightLane)
//                 currentMetrics.behaviorMetrics.favoriteLane = "Left";
//             else if (pattern.timeInRightLane > pattern.timeInCenterLane)
//                 currentMetrics.behaviorMetrics.favoriteLane = "Right";
//             else
//                 currentMetrics.behaviorMetrics.favoriteLane = "Center";
                
//             // Determine play style
//             if (pattern.averageReactionTime < 0.3f && currentMetrics.behaviorMetrics.laneChanges > 20)
//                 currentMetrics.behaviorMetrics.playStyle = "Aggressive";
//             else if (pattern.averageReactionTime > 0.6f)
//                 currentMetrics.behaviorMetrics.playStyle = "Defensive";
//             else
//                 currentMetrics.behaviorMetrics.playStyle = "Balanced";
                
//             // Calculate adaptability score
//             float reactionTimeVariance = CalculateReactionTimeVariance(pattern.reactionTimes);
//             currentMetrics.behaviorMetrics.adaptabilityScore = Mathf.Clamp01(1f - reactionTimeVariance);
//         }
//     }
    
//     private float CalculateReactionTimeVariance(List<float> reactionTimes)
//     {
//         if (reactionTimes.Count <= 1) return 0f;
        
//         float mean = 0f;
//         foreach (float time in reactionTimes)
//         {
//             mean += time;
//         }
//         mean /= reactionTimes.Count;
        
//         float variance = 0f;
//         foreach (float time in reactionTimes)
//         {
//             variance += Mathf.Pow(time - mean, 2);
//         }
//         variance /= reactionTimes.Count;
        
//         return Mathf.Sqrt(variance);
//     }
    
//     private IEnumerator DashboardUpdateLoop()
//     {
//         while (true)
//         {
//             yield return new WaitForSeconds(dashboardUpdateInterval);
            
//             if (isOnline)
//             {
//                 SendMetricsToDashboard();
                
//                 // Send any offline data if available
//                 while (offlineDataQueue.Count > 0)
//                 {
//                     var offlineMetrics = offlineDataQueue.Dequeue();
//                     yield return SendMetricsCoroutine(offlineMetrics);
//                 }
//             }
//             else if (enableOfflineMode)
//             {
//                 // Store data for later when back online
//                 offlineDataQueue.Enqueue(new DashboardMetrics
//                 {
//                     userId = currentMetrics.userId,
//                     sessionId = currentMetrics.sessionId,
//                     timestamp = DateTime.Now,
//                     gameMetrics = currentMetrics.gameMetrics,
//                     aiMetrics = currentMetrics.aiMetrics,
//                     performanceMetrics = currentMetrics.performanceMetrics,
//                     behaviorMetrics = currentMetrics.behaviorMetrics
//                 });
                
//                 Debug.Log("Stored metrics offline for later transmission");
//             }
//         }
//     }
    
//     private IEnumerator NetworkMonitorLoop()
//     {
//         while (true)
//         {
//             yield return new WaitForSeconds(10f); // Check every 10 seconds
//             yield return StartCoroutine(PingDashboard());
//         }
//     }
    
//     private IEnumerator TestConnection()
//     {
//         // For local HTML dashboard, we'll skip the connection test
//         // and assume we can write to Firebase directly
//         if (FirebaseManager.Instance?.IsInitialized == true)
//         {
//             isOnline = true;
//             Debug.Log("Dashboard connection established (using Firebase)");
//         }
//         else
//         {
//             isOnline = false;
//             Debug.LogWarning("Dashboard offline - Firebase not initialized");
//         }
        
//         OnConnectionStatusChanged?.Invoke(isOnline);
//         yield break;
//     }
    
//     private IEnumerator PingDashboard()
//     {
//         float startTime = Time.realtimeSinceStartup;
        
//         string pingUrl = dashboardEndpoint + "/api/ping";
//         UnityWebRequest request = UnityWebRequest.Get(pingUrl);
//         request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
//         request.timeout = 5;
        
//         yield return request.SendWebRequest();
        
//         float endTime = Time.realtimeSinceStartup;
//         networkLatency = (endTime - startTime) * 1000f; // Convert to milliseconds
        
//         bool wasOnline = isOnline;
//         isOnline = request.result == UnityWebRequest.Result.Success;
        
//         if (wasOnline != isOnline)
//         {
//             OnConnectionStatusChanged?.Invoke(isOnline);
//             Debug.Log($"Network status changed: {(isOnline ? "Online" : "Offline")}");
//         }
//     }
    
//     public void SendMetricsToDashboard()
//     {
//         if (!isOnline) return;
        
//         // Update user and session IDs
//         if (FirebaseManager.Instance != null)
//         {
//             currentMetrics.userId = FirebaseManager.Instance.UserId ?? "anonymous";
//         }
        
//         if (PlayerAnalyticsManager.Instance?.CurrentSession != null)
//         {
//             currentMetrics.sessionId = PlayerAnalyticsManager.Instance.CurrentSession.sessionId;
//         }
        
//         StartCoroutine(SendMetricsCoroutine(currentMetrics));
//     }
    
//     private IEnumerator SendMetricsCoroutine(DashboardMetrics metrics)
//     {
//         // Save to Firebase instead of sending to web server
//         if (FirebaseManager.Instance?.IsInitialized == true)
//         {
//             var metricsData = new System.Collections.Generic.Dictionary<string, object>
//             {
//                 {"timestamp", System.DateTime.Now.ToString()},
//                 {"userId", metrics.userId},
//                 {"sessionId", metrics.sessionId},
//                 {"currentFPS", metrics.performanceMetrics.currentFPS},
//                 {"memoryUsage", metrics.performanceMetrics.memoryUsage},
//                 {"networkLatency", metrics.performanceMetrics.networkLatency},
//                 {"gameTime", metrics.gameMetrics.playTime},
//                 {"highScore", metrics.gameMetrics.highScore},
//                 {"coinsCollected", metrics.gameMetrics.coinsCollected},
//                 {"obstaclesHit", metrics.gameMetrics.obstaclesHit}
//             };
            
//             FirebaseManager.Instance.SavePlayerData("dashboard_metrics", metricsData);
//             Debug.Log("Metrics saved to Firebase for dashboard");
//             OnMetricsUpdated?.Invoke(metrics);
//         }
//         else
//         {
//             Debug.LogWarning("Cannot send metrics - Firebase not available");
            
//             // Add to offline queue if enabled
//             if (enableOfflineMode)
//             {
//                 offlineDataQueue.Enqueue(metrics);
//             }
//         }
        
//         yield break;
//     }
    
//     public void SendHeatmapData(List<Vector3> playerPositions, List<Vector3> obstaclePositions)
//     {
//         if (!isOnline) return;
        
//         var heatmapData = new
//         {
//             sessionId = currentMetrics.sessionId,
//             timestamp = DateTime.Now.ToString(),
//             playerPositions = playerPositions,
//             obstaclePositions = obstaclePositions
//         };
        
//         StartCoroutine(SendHeatmapCoroutine(heatmapData));
//     }
    
//     private IEnumerator SendHeatmapCoroutine(object heatmapData)
//     {
//         string url = dashboardEndpoint + heatmapEndpoint;
//         string jsonData = JsonUtility.ToJson(heatmapData);
        
//         UnityWebRequest request = new UnityWebRequest(url, "POST");
//         byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);
//         request.uploadHandler = new UploadHandlerRaw(jsonToSend);
//         request.downloadHandler = new DownloadHandlerBuffer();
//         request.SetRequestHeader("Content-Type", "application/json");
//         request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
        
//         yield return request.SendWebRequest();
        
//         if (request.result == UnityWebRequest.Result.Success)
//         {
//             Debug.Log("Heatmap data sent successfully");
//         }
//         else
//         {
//             Debug.LogError($"Failed to send heatmap data: {request.error}");
//         }
//     }
    
//     public void UpdateLeaderboard(int score, string playerName = "")
//     {
//         if (!isOnline) return;
        
//         var leaderboardEntry = new
//         {
//             userId = currentMetrics.userId,
//             playerName = string.IsNullOrEmpty(playerName) ? "Anonymous" : playerName,
//             score = score,
//             timestamp = DateTime.Now.ToString(),
//             sessionId = currentMetrics.sessionId
//         };
        
//         StartCoroutine(UpdateLeaderboardCoroutine(leaderboardEntry));
//     }
    
//     private IEnumerator UpdateLeaderboardCoroutine(object leaderboardEntry)
//     {
//         string url = dashboardEndpoint + leaderboardEndpoint;
//         string jsonData = JsonUtility.ToJson(leaderboardEntry);
        
//         UnityWebRequest request = new UnityWebRequest(url, "POST");
//         byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);
//         request.uploadHandler = new UploadHandlerRaw(jsonToSend);
//         request.downloadHandler = new DownloadHandlerBuffer();
//         request.SetRequestHeader("Content-Type", "application/json");
//         request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
        
//         yield return request.SendWebRequest();
        
//         if (request.result == UnityWebRequest.Result.Success)
//         {
//             Debug.Log("Leaderboard updated successfully");
//         }
//         else
//         {
//             Debug.LogError($"Failed to update leaderboard: {request.error}");
//         }
//     }
    
//     public void SendAIInsights(Dictionary<string, object> insights)
//     {
//         if (!isOnline) return;
        
//         var aiInsightsData = new
//         {
//             sessionId = currentMetrics.sessionId,
//             userId = currentMetrics.userId,
//             timestamp = DateTime.Now.ToString(),
//             insights = insights
//         };
        
//         StartCoroutine(SendAIInsightsCoroutine(aiInsightsData));
//     }
    
//     private IEnumerator SendAIInsightsCoroutine(object aiInsightsData)
//     {
//         string url = dashboardEndpoint + aiInsightsEndpoint;
//         string jsonData = JsonUtility.ToJson(aiInsightsData);
        
//         UnityWebRequest request = new UnityWebRequest(url, "POST");
//         byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);
//         request.uploadHandler = new UploadHandlerRaw(jsonToSend);
//         request.downloadHandler = new DownloadHandlerBuffer();
//         request.SetRequestHeader("Content-Type", "application/json");
//         request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
        
//         yield return request.SendWebRequest();
        
//         if (request.result == UnityWebRequest.Result.Success)
//         {
//             Debug.Log("AI insights sent successfully");
//         }
//         else
//         {
//             Debug.LogError($"Failed to send AI insights: {request.error}");
//         }
//     }
    
//     // Event handlers
//     private void OnSessionCompleted(PlayerSessionData sessionData)
//     {
//         // Send final session data to dashboard
//         var finalMetrics = new DashboardMetrics
//         {
//             userId = FirebaseManager.Instance?.UserId ?? "anonymous",
//             sessionId = sessionData.sessionId,
//             timestamp = sessionData.sessionEnd
//         };
        
//         // Copy session data to metrics
//         finalMetrics.gameMetrics.playTime = sessionData.totalPlayTime;
//         finalMetrics.gameMetrics.highScore = sessionData.highScore;
//         finalMetrics.gameMetrics.coinsCollected = sessionData.totalCoinsCollected;
//         finalMetrics.gameMetrics.obstaclesHit = sessionData.totalObstaclesHit;
//         finalMetrics.gameMetrics.totalJumps = sessionData.movementPattern.jumpCount;
//         finalMetrics.gameMetrics.totalSwipes = sessionData.movementPattern.leftSwipes + sessionData.movementPattern.rightSwipes;
        
//         StartCoroutine(SendMetricsCoroutine(finalMetrics));
        
//         // Update leaderboard
//         UpdateLeaderboard(sessionData.highScore);
        
//         // Send heatmap data
//         SendHeatmapData(sessionData.movementPattern.positionHistory, new List<Vector3>());
//     }
    
//     private void OnBehaviorPredicted(Dictionary<string, object> prediction)
//     {
//         // Send AI insights to dashboard
//         SendAIInsights(prediction);
//     }
    
//     // Public getters for dashboard data
//     public DashboardMetrics GetCurrentMetrics() => currentMetrics;
//     public bool IsOnline() => isOnline;
//     public int GetOfflineDataCount() => offlineDataQueue.Count;
//     public float GetNetworkLatency() => networkLatency;
    
//     private void OnDestroy()
//     {
//         // Clean up subscriptions
//         if (PlayerAnalyticsManager.Instance != null)
//         {
//             PlayerAnalyticsManager.Instance.OnSessionCompleted -= OnSessionCompleted;
//             PlayerAnalyticsManager.Instance.OnBehaviorPredicted -= OnBehaviorPredicted;
//         }
//     }
// } 
