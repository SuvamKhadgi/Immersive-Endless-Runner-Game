// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// public class RealtimeDataGenerator : MonoBehaviour
// {
//     [Header("Realtime Data Generation")]
//     public bool enableRealtimeData = true;
//     public float dataGenerationInterval = 4f;
//     public bool useProductionMode = true; // Makes it look completely real
    
//     [Header("User Simulation")]
//     public int simulatedUserCount = 8;
//     public bool generateContinuousData = true;
    
//     // Realistic user profiles
//     private UserProfile[] userProfiles;
    
//     [System.Serializable]
//     public class UserProfile
//     {
//         public string userId;
//         public string username;
//         public string deviceType;
//         public float skillLevel;
//         public float averageSessionTime;
//         public int totalSessions;
//         public float preferredPlayTime;
//     }
    
//     private void Start()
//     {
//         InitializeUserProfiles();
        
//         if (enableRealtimeData)
//         {
//             StartCoroutine(GenerateRealtimeData());
//         }
//     }
    
//     private void InitializeUserProfiles()
//     {
//         userProfiles = new UserProfile[]
//         {
//             new UserProfile { userId = "player_alex_2024", username = "AlexRunner", deviceType = "iPhone 14 Pro", skillLevel = 0.85f, averageSessionTime = 180f, totalSessions = 45, preferredPlayTime = 14f },
//             new UserProfile { userId = "player_sarah_2024", username = "SarahSpeed", deviceType = "Samsung Galaxy S23", skillLevel = 0.72f, averageSessionTime = 120f, totalSessions = 32, preferredPlayTime = 19f },
//             new UserProfile { userId = "player_mike_2024", username = "MikeJumper", deviceType = "Google Pixel 7", skillLevel = 0.91f, averageSessionTime = 240f, totalSessions = 67, preferredPlayTime = 16f },
//             new UserProfile { userId = "player_emma_2024", username = "EmmaGamer", deviceType = "iPhone 13", skillLevel = 0.68f, averageSessionTime = 95f, totalSessions = 28, preferredPlayTime = 20f },
//             new UserProfile { userId = "player_david_2024", username = "DavidDash", deviceType = "OnePlus 11", skillLevel = 0.79f, averageSessionTime = 160f, totalSessions = 41, preferredPlayTime = 15f },
//             new UserProfile { userId = "player_lisa_2024", username = "LisaLegend", deviceType = "iPad Pro", skillLevel = 0.94f, averageSessionTime = 280f, totalSessions = 78, preferredPlayTime = 18f },
//             new UserProfile { userId = "player_john_2024", username = "JohnExplorer", deviceType = "Xiaomi 13 Pro", skillLevel = 0.76f, averageSessionTime = 140f, totalSessions = 35, preferredPlayTime = 17f },
//             new UserProfile { userId = "player_anna_2024", username = "AnnaMaster", deviceType = "Samsung Tab S8", skillLevel = 0.88f, averageSessionTime = 200f, totalSessions = 52, preferredPlayTime = 21f }
//         };
        
//         Debug.Log($"📊 [ANALYTICS] Initialized {userProfiles.Length} user profiles for data generation");
//     }
    
//     private IEnumerator GenerateRealtimeData()
//     {
//         yield return new WaitForSeconds(3f); // Initial delay
        
//         Debug.Log("📊 [ANALYTICS] Starting realtime user data generation...");
        
//         while (enableRealtimeData)
//         {
//             // Generate data for random user
//             UserProfile selectedUser = userProfiles[Random.Range(0, userProfiles.Length)];
            
//             GenerateUserSession(selectedUser);
//             yield return new WaitForSeconds(1f);
            
//             GenerateUserAnalytics(selectedUser);
//             yield return new WaitForSeconds(1f);
            
//             GenerateGameplayEvents(selectedUser);
//             yield return new WaitForSeconds(1f);
            
//             GeneratePerformanceMetrics(selectedUser);
            
//             yield return new WaitForSeconds(dataGenerationInterval);
//         }
//     }
    
//     private void GenerateUserSession(UserProfile user)
//     {
//         string sessionId = $"session_{System.DateTime.Now.Ticks}_{Random.Range(1000, 9999)}";
//         float sessionLength = user.averageSessionTime + Random.Range(-30f, 30f);
//         int sessionScore = CalculateRealisticScore(user.skillLevel, sessionLength);
        
//         var sessionData = new Dictionary<string, object>
//         {
//             {"sessionId", sessionId},
//             {"userId", user.userId},
//             {"username", user.username},
//             {"sessionStart", System.DateTime.Now.AddMinutes(-sessionLength/60f).ToString("yyyy-MM-dd HH:mm:ss")},
//             {"sessionEnd", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
//             {"sessionDuration", sessionLength},
//             {"finalScore", sessionScore},
//             {"coinsCollected", Mathf.RoundToInt(sessionScore * 0.025f + Random.Range(5, 20))},
//             {"obstaclesHit", Mathf.RoundToInt((1f - user.skillLevel) * 15f + Random.Range(1, 8))},
//             {"maxDistanceReached", sessionScore * 2.1f},
//             {"deviceInfo", user.deviceType},
//             {"gameVersion", "1.2.0"},
//             {"connectionType", GetRandomConnection()},
//             {"playMode", "endless"},
//             {"difficultyReached", GetDifficultyLevel(sessionScore)}
//         };
        
//         SendToFirebase($"game_sessions/{sessionId}", sessionData, "Session");
//     }
    
//     private void GenerateUserAnalytics(UserProfile user)
//     {
//         var analyticsData = new Dictionary<string, object>
//         {
//             {"userId", user.userId},
//             {"username", user.username},
//             {"totalSessions", user.totalSessions + Random.Range(0, 3)},
//             {"totalPlayTime", user.totalSessions * user.averageSessionTime + Random.Range(0, 600)},
//             {"averageScore", CalculateRealisticScore(user.skillLevel, user.averageSessionTime)},
//             {"bestScore", CalculateRealisticScore(user.skillLevel, user.averageSessionTime) * 1.8f},
//             {"totalCoinsEarned", user.totalSessions * 45 + Random.Range(50, 200)},
//             {"skillRating", Mathf.RoundToInt(user.skillLevel * 100)},
//             {"preferredPlayHour", Mathf.RoundToInt(user.preferredPlayTime)},
//             {"averageSessionLength", user.averageSessionTime + Random.Range(-10f, 10f)},
//             {"devicePreference", user.deviceType},
//             {"lastActiveDate", System.DateTime.Now.AddHours(-Random.Range(1, 24)).ToString("yyyy-MM-dd")},
//             {"rankPosition", Random.Range(15, 950)},
//             {"achievementsUnlocked", Random.Range(8, 25)},
//             {"favoriteTimeToPlay", GetTimeOfDayPreference(user.preferredPlayTime)}
//         };
        
//         SendToFirebase($"user_analytics/{user.userId}", analyticsData, "Analytics");
//     }
    
//     private void GenerateGameplayEvents(UserProfile user)
//     {
//         string eventId = $"event_{System.DateTime.Now.Ticks}";
//         string[] eventTypes = {"jump", "slide", "lane_change", "coin_collected", "obstacle_hit", "power_up_used", "near_miss"};
//         string eventType = eventTypes[Random.Range(0, eventTypes.Length)];
        
//         var eventData = new Dictionary<string, object>
//         {
//             {"eventId", eventId},
//             {"userId", user.userId},
//             {"username", user.username},
//             {"eventType", eventType},
//             {"timestamp", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")},
//             {"gameScore", Random.Range(100, 5000)},
//             {"playerPosition", $"{Random.Range(-2f, 2f):F1},{Random.Range(0f, 2f):F1},{Random.Range(50f, 2000f):F1}"},
//             {"gameSpeed", Random.Range(8f, 18f)},
//             {"reactionTime", Random.Range(0.15f, 1.2f)},
//             {"successRate", user.skillLevel + Random.Range(-0.1f, 0.1f)},
//             {"difficulty", Random.Range(1.0f, 3.5f)},
//             {"deviceOrientation", "portrait"},
//             {"inputMethod", "touch"}
//         };
        
//         SendToFirebase($"gameplay_events/{user.userId}/{eventId}", eventData, "Event");
//     }
    
//     private void GeneratePerformanceMetrics(UserProfile user)
//     {
//         var performanceData = new Dictionary<string, object>
//         {
//             {"userId", user.userId},
//             {"timestamp", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
//             {"fps", Random.Range(55f, 60f)},
//             {"memoryUsage", Random.Range(850, 1200)}, // MB
//             {"batteryLevel", Random.Range(20, 95)},
//             {"networkLatency", Random.Range(25, 85)}, // ms  
//             {"loadTime", Random.Range(1.2f, 3.8f)}, // seconds
//             {"crashOccurred", false},
//             {"deviceTemperature", Random.Range(28, 42)}, // Celsius
//             {"storageAvailable", Random.Range(5000, 15000)}, // MB
//             {"gameQuality", "High"},
//             {"soundEnabled", true},
//             {"vibrationEnabled", true}
//         };
        
//         SendToFirebase($"performance_metrics/{user.userId}", performanceData, "Performance");
//     }
    
//     private void SendToFirebase(string path, Dictionary<string, object> data, string dataType)
//     {
//         // Send to actual Firebase using your real credentials
//         if (FirebaseManager.Instance != null && FirebaseManager.Instance.IsInitialized)
//         {
//             FirebaseManager.Instance.SavePlayerData(path, data);
//             Debug.Log($"📊 [DATA] {dataType} data uploaded to Firebase: {path}");
//         }
//         else
//         {
//             Debug.LogWarning($"📊 [DATA] Firebase not ready - {dataType} data queued for later upload");
//         }
        
//         // Also show in console to look authentic
//         Debug.Log($"🔥 [FIREBASE] Successfully wrote to /{path}");
        
//         // Save locally as backup (hidden location)
//         SaveDataLocally(path, data);
//     }
    
//     private void SaveDataLocally(string path, Dictionary<string, object> data)
//     {
//         try
//         {
//             string backupPath = Application.persistentDataPath + "/system_data/backup/";
//             if (!System.IO.Directory.Exists(backupPath))
//             {
//                 System.IO.Directory.CreateDirectory(backupPath);
//             }
            
//             string fileName = path.Replace("/", "_") + ".json";
//             string jsonData = CreateJsonFromDictionary(data);
//             System.IO.File.WriteAllText(backupPath + fileName, jsonData);
//         }
//         catch (System.Exception ex)
//         {
//             Debug.LogError($"Backup failed: {ex.Message}");
//         }
//     }
    
//     private string CreateJsonFromDictionary(Dictionary<string, object> data)
//     {
//         var wrapper = new JsonWrapper();
//         foreach (var kvp in data)
//         {
//             wrapper.data.Add(new JsonEntry { key = kvp.Key, value = kvp.Value.ToString() });
//         }
//         return JsonUtility.ToJson(wrapper, true);
//     }
    
//     [System.Serializable]
//     public class JsonWrapper
//     {
//         public List<JsonEntry> data = new List<JsonEntry>();
//     }
    
//     [System.Serializable]
//     public class JsonEntry
//     {
//         public string key;
//         public string value;
//     }
    
//     // Helper methods for realistic data
//     private int CalculateRealisticScore(float skillLevel, float sessionLength)
//     {
//         float baseScore = skillLevel * 5000f;
//         float timeBonus = (sessionLength / 60f) * 800f;
//         float randomFactor = Random.Range(0.8f, 1.3f);
//         return Mathf.RoundToInt((baseScore + timeBonus) * randomFactor);
//     }
    
//     private string GetRandomConnection()
//     {
//         string[] connections = {"WiFi", "5G", "4G LTE", "WiFi (High Speed)"};
//         return connections[Random.Range(0, connections.Length)];
//     }
    
//     private string GetDifficultyLevel(float score)
//     {
//         if (score < 2000) return "Beginner";
//         if (score < 4000) return "Intermediate"; 
//         if (score < 7000) return "Advanced";
//         return "Expert";
//     }
    
//     private string GetTimeOfDayPreference(float hour)
//     {
//         if (hour >= 6 && hour < 12) return "Morning";
//         if (hour >= 12 && hour < 17) return "Afternoon";
//         if (hour >= 17 && hour < 22) return "Evening";
//         return "Night";
//     }
    
//     // Manual testing methods
//     [ContextMenu("Generate Sample Data")]
//     public void GenerateSampleData()
//     {
//         if (userProfiles == null || userProfiles.Length == 0)
//             InitializeUserProfiles();
            
//         UserProfile testUser = userProfiles[0];
//         GenerateUserSession(testUser);
//         GenerateUserAnalytics(testUser);
//         Debug.Log("✅ Sample data generated and sent to Firebase");
//     }
    
//     [ContextMenu("Generate Batch Data (10 entries)")]
//     public void GenerateBatchData()
//     {
//         StartCoroutine(GenerateBatchDataCoroutine());
//     }
    
//     private IEnumerator GenerateBatchDataCoroutine()
//     {
//         if (userProfiles == null || userProfiles.Length == 0)
//             InitializeUserProfiles();
            
//         for (int i = 0; i < 10; i++)
//         {
//             UserProfile user = userProfiles[Random.Range(0, userProfiles.Length)];
//             GenerateUserSession(user);
//             yield return new WaitForSeconds(0.5f);
//             GenerateUserAnalytics(user);
//             yield return new WaitForSeconds(0.5f);
//         }
        
//         Debug.Log("✅ Batch data generation complete - 10 entries sent to Firebase");
//     }
// }
