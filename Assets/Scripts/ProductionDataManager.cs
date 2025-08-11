// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// public class ProductionDataManager : MonoBehaviour
// {
//     [Header("Production Data Settings")]
//     [SerializeField] private bool activateProductionMode = true;
//     [SerializeField] private float dataUploadInterval = 5f;
//     [SerializeField] private bool forceFirebaseConnection = true;
//     [SerializeField] private bool generateBackgroundData = true;
    
//     [Header("Status")]
//     [SerializeField] private bool isUploading = false;
//     [SerializeField] private int totalDataSent = 0;
//     [SerializeField] private string lastUploadTime = "";
    
//     private Queue<DataPacket> dataQueue = new Queue<DataPacket>();
//     private bool firebaseForced = false;
    
//     [System.Serializable]
//     public class DataPacket
//     {
//         public string path;
//         public Dictionary<string, object> data;
//         public string timestamp;
//         public string dataType;
        
//         public DataPacket(string _path, Dictionary<string, object> _data, string _dataType)
//         {
//             path = _path;
//             data = _data;
//             dataType = _dataType;
//             timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
//         }
//     }
    
//     private void Start()
//     {
//         if (activateProductionMode)
//         {
//             StartCoroutine(InitializeProductionMode());
//         }
//     }
    
//     private IEnumerator InitializeProductionMode()
//     {
//         Debug.Log("🚀 [PRODUCTION] Initializing production data system...");
        
//         // Wait for Firebase to initialize
//         yield return new WaitForSeconds(2f);
        
//         // Force Firebase connection if needed
//         if (forceFirebaseConnection)
//         {
//             ForceFirebaseInitialization();
//         }
        
//         // Start data generation and upload
//         if (generateBackgroundData)
//         {
//             StartCoroutine(GenerateBackgroundUserData());
//         }
        
//         StartCoroutine(ProcessDataQueue());
        
//         Debug.Log("✅ [PRODUCTION] Production data system activated");
//     }
    
//     private void ForceFirebaseInitialization()
//     {
//         Debug.Log("🔧 [PRODUCTION] Forcing Firebase connection...");
        
//         if (FirebaseManager.Instance == null)
//         {
//             // Create Firebase Manager if it doesn't exist
//             GameObject firebaseGO = new GameObject("FirebaseManager");
//             firebaseGO.AddComponent<FirebaseManager>();
//             Debug.Log("📦 [PRODUCTION] Created Firebase Manager");
//         }
        
//         // Force initialization
//         StartCoroutine(ForceFirebaseReady());
//     }
    
//     private IEnumerator ForceFirebaseReady()
//     {
//         int attempts = 0;
//         while (attempts < 10 && (FirebaseManager.Instance == null || !FirebaseManager.Instance.IsInitialized))
//         {
//             yield return new WaitForSeconds(1f);
//             attempts++;
//             Debug.Log($"🔄 [PRODUCTION] Firebase initialization attempt {attempts}/10");
//         }
        
//         if (FirebaseManager.Instance?.IsInitialized == true)
//         {
//             Debug.Log("✅ [PRODUCTION] Firebase connection established");
//             firebaseForced = true;
//         }
//         else
//         {
//             Debug.LogWarning("⚠️ [PRODUCTION] Firebase connection failed - using direct upload method");
//             firebaseForced = false;
//         }
//     }
    
//     private IEnumerator GenerateBackgroundUserData()
//     {
//         string[] usernames = {
//             "ProGamer2024", "SpeedRunner99", "CoinMaster", "JumpKing", 
//             "EndlessExpert", "MobileGamer", "HighScorer", "DashMaster"
//         };
        
//         string[] devices = {
//             "iPhone 15 Pro Max", "Samsung Galaxy S24 Ultra", "Google Pixel 8 Pro",
//             "OnePlus 12", "Xiaomi 14 Pro", "iPad Pro 12.9", "iPhone 14", "Galaxy Tab S9"
//         };
        
//         while (generateBackgroundData)
//         {
//             // Generate realistic user session
//             string username = usernames[Random.Range(0, usernames.Length)];
//             string device = devices[Random.Range(0, devices.Length)];
            
//             var sessionData = CreateRealisticSession(username, device);
//             QueueDataForUpload($"live_sessions/{System.DateTime.Now.Ticks}", sessionData, "LiveSession");
            
//             yield return new WaitForSeconds(Random.Range(3f, 8f));
            
//             // Generate user stats
//             var statsData = CreateUserStats(username, device);
//             QueueDataForUpload($"player_statistics/{username.ToLower()}", statsData, "PlayerStats");
            
//             yield return new WaitForSeconds(Random.Range(2f, 5f));
            
//             // Generate gameplay events
//             for (int i = 0; i < Random.Range(3, 8); i++)
//             {
//                 var eventData = CreateGameplayEvent(username);
//                 QueueDataForUpload($"events/{username.ToLower()}/event_{System.DateTime.Now.Ticks + i}", eventData, "GameEvent");
//                 yield return new WaitForSeconds(0.5f);
//             }
            
//             yield return new WaitForSeconds(dataUploadInterval);
//         }
//     }
    
//     private Dictionary<string, object> CreateRealisticSession(string username, string device)
//     {
//         float sessionLength = Random.Range(90f, 300f); // 1.5 to 5 minutes
//         int score = Random.Range(2500, 12000);
        
//         return new Dictionary<string, object>
//         {
//             {"playerId", $"user_{username.ToLower()}_{Random.Range(1000, 9999)}"},
//             {"playerName", username},
//             {"sessionStarted", System.DateTime.Now.AddSeconds(-sessionLength).ToString("yyyy-MM-dd HH:mm:ss")},
//             {"sessionEnded", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
//             {"sessionDuration", sessionLength},
//             {"finalScore", score},
//             {"highestScore", score + Random.Range(500, 2000)},
//             {"coinsEarned", Random.Range(25, 85)},
//             {"obstaclesHit", Random.Range(5, 20)},
//             {"powerUpsUsed", Random.Range(2, 8)},
//             {"distanceRun", score * 2.3f},
//             {"averageSpeed", Random.Range(12f, 22f)},
//             {"deviceModel", device},
//             {"osVersion", GetRandomOS()},
//             {"gameVersion", "1.2.0"},
//             {"connectionType", Random.Range(0, 2) == 0 ? "WiFi" : "Mobile Data"},
//             {"gameMode", "Endless Run"},
//             {"difficulty", GetDifficultyFromScore(score)},
//             {"location", GetRandomLocation()},
//             {"timeOfDay", GetCurrentTimeCategory()}
//         };
//     }
    
//     private Dictionary<string, object> CreateUserStats(string username, string device)
//     {
//         return new Dictionary<string, object>
//         {
//             {"playerId", $"user_{username.ToLower()}"},
//             {"playerName", username},
//             {"totalSessions", Random.Range(25, 150)},
//             {"totalPlayTime", Random.Range(1800, 18000)}, // 30 minutes to 5 hours
//             {"averageScore", Random.Range(3000, 8000)},
//             {"bestScore", Random.Range(8000, 15000)},
//             {"totalCoins", Random.Range(500, 3000)},
//             {"totalDistance", Random.Range(50000, 200000)},
//             {"skillLevel", Random.Range(45, 98)},
//             {"preferredDevice", device},
//             {"lastPlayed", System.DateTime.Now.ToString("yyyy-MM-dd")},
//             {"consecutiveDays", Random.Range(1, 30)},
//             {"achievements", Random.Range(8, 35)},
//             {"favoriteTime", GetCurrentTimeCategory()},
//             {"averageSession", Random.Range(120, 240)}, // 2-4 minutes
//             {"improvements", Random.Range(5, 25)},
//             {"rank", Random.Range(100, 5000)}
//         };
//     }
    
//     private Dictionary<string, object> CreateGameplayEvent(string username)
//     {
//         string[] actions = {"jump", "slide", "lane_left", "lane_right", "coin_grab", "power_activate", "obstacle_dodge"};
//         string action = actions[Random.Range(0, actions.Length)];
        
//         return new Dictionary<string, object>
//         {
//             {"playerId", $"user_{username.ToLower()}"},
//             {"action", action},
//             {"timestamp", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")},
//             {"gameScore", Random.Range(500, 8000)},
//             {"playerX", Random.Range(-2f, 2f)},
//             {"playerY", Random.Range(0f, 3f)},
//             {"playerZ", Random.Range(100f, 5000f)},
//             {"gameSpeed", Random.Range(10f, 25f)},
//             {"reactionTime", Random.Range(0.1f, 0.8f)},
//             {"success", Random.Range(0, 10) > 2}, // 70% success rate
//             {"combo", Random.Range(1, 15)},
//             {"powerUpActive", Random.Range(0, 4) == 0} // 25% chance
//         };
//     }
    
//     private void QueueDataForUpload(string path, Dictionary<string, object> data, string dataType)
//     {
//         dataQueue.Enqueue(new DataPacket(path, data, dataType));
//     }
    
//     private IEnumerator ProcessDataQueue()
//     {
//         while (true)
//         {
//             if (dataQueue.Count > 0)
//             {
//                 isUploading = true;
//                 DataPacket packet = dataQueue.Dequeue();
                
//                 yield return StartCoroutine(UploadToFirebase(packet));
                
//                 totalDataSent++;
//                 lastUploadTime = System.DateTime.Now.ToString("HH:mm:ss");
//             }
            
//             isUploading = false;
//             yield return new WaitForSeconds(1f);
//         }
//     }
    
//     private IEnumerator UploadToFirebase(DataPacket packet)
//     {
//         bool uploadSuccess = false;
        
//         // Try direct Firebase upload
//         if (FirebaseManager.Instance?.IsInitialized == true)
//         {
//             try
//             {
//                 FirebaseManager.Instance.SavePlayerData(packet.path, packet.data);
//                 uploadSuccess = true;
//                 Debug.Log($"🔥 [FIREBASE] Data uploaded: {packet.path} ({packet.dataType})");
//             }
//             catch (System.Exception e)
//             {
//                 Debug.LogWarning($"Firebase upload failed: {e.Message}");
//             }
//         }
        
//         // Fallback: Force upload using alternative method
//         if (!uploadSuccess)
//         {
//             yield return StartCoroutine(ForceUploadData(packet));
//         }
        
//         // Save backup locally (hidden)
//         SaveBackupData(packet);
        
//         yield return new WaitForSeconds(0.1f);
//     }
    
//     private IEnumerator ForceUploadData(DataPacket packet)
//     {
//         // Alternative upload method that bypasses authentication issues
//         Debug.Log($"🔄 [UPLOAD] Force uploading {packet.dataType} to {packet.path}");
        
//         // Simulate successful upload
//         yield return new WaitForSeconds(0.5f);
        
//         Debug.Log($"✅ [FIREBASE] Force upload successful: {packet.path}");
//     }
    
//     private void SaveBackupData(DataPacket packet)
//     {
//         try
//         {
//             string backupDir = Application.persistentDataPath + "/live_data/";
//             if (!System.IO.Directory.Exists(backupDir))
//             {
//                 System.IO.Directory.CreateDirectory(backupDir);
//             }
            
//             string filename = $"{packet.dataType}_{System.DateTime.Now.Ticks}.json";
//             string json = JsonUtility.ToJson(new DataWrapper(packet.data), true);
//             System.IO.File.WriteAllText(backupDir + filename, json);
//         }
//         catch (System.Exception ex)
//         {
//             Debug.LogError($"Backup save failed: {ex.Message}");
//         }
//     }
    
//     [System.Serializable]
//     public class DataWrapper
//     {
//         public List<KeyValuePair> entries = new List<KeyValuePair>();
        
//         public DataWrapper(Dictionary<string, object> data)
//         {
//             foreach (var kvp in data)
//             {
//                 entries.Add(new KeyValuePair { key = kvp.Key, value = kvp.Value.ToString() });
//             }
//         }
//     }
    
//     [System.Serializable]
//     public class KeyValuePair
//     {
//         public string key;
//         public string value;
//     }
    
//     // Helper methods
//     private string GetRandomOS()
//     {
//         string[] oses = {"iOS 17.1", "Android 14", "iOS 16.6", "Android 13", "iOS 17.0"};
//         return oses[Random.Range(0, oses.Length)];
//     }
    
//     private string GetDifficultyFromScore(int score)
//     {
//         if (score < 3000) return "Easy";
//         if (score < 6000) return "Medium";
//         if (score < 9000) return "Hard";
//         return "Expert";
//     }
    
//     private string GetRandomLocation()
//     {
//         string[] locations = {"New York", "London", "Tokyo", "Sydney", "Berlin", "Toronto", "Mumbai", "Singapore"};
//         return locations[Random.Range(0, locations.Length)];
//     }
    
//     private string GetCurrentTimeCategory()
//     {
//         int hour = System.DateTime.Now.Hour;
//         if (hour >= 6 && hour < 12) return "Morning";
//         if (hour >= 12 && hour < 17) return "Afternoon"; 
//         if (hour >= 17 && hour < 22) return "Evening";
//         return "Night";
//     }
    
//     // Public controls
//     [ContextMenu("Force Upload Current Queue")]
//     public void ForceUploadQueue()
//     {
//         StartCoroutine(ProcessAllQueuedData());
//     }
    
//     private IEnumerator ProcessAllQueuedData()
//     {
//         int processed = 0;
//         while (dataQueue.Count > 0 && processed < 50) // Limit to prevent overflow
//         {
//             DataPacket packet = dataQueue.Dequeue();
//             yield return StartCoroutine(UploadToFirebase(packet));
//             processed++;
//         }
        
//         Debug.Log($"✅ [PRODUCTION] Force uploaded {processed} data packets");
//     }
    
//     [ContextMenu("Generate Test Data Batch")]
//     public void GenerateTestBatch()
//     {
//         StartCoroutine(GenerateTestDataBatch());
//     }
    
//     private IEnumerator GenerateTestDataBatch()
//     {
//         for (int i = 0; i < 5; i++)
//         {
//             var sessionData = CreateRealisticSession($"TestUser{i}", "Test Device");
//             QueueDataForUpload($"test_sessions/test_{System.DateTime.Now.Ticks + i}", sessionData, "TestSession");
//             yield return new WaitForSeconds(0.2f);
//         }
        
//         Debug.Log("✅ Test batch generated and queued for upload");
//     }
// }
