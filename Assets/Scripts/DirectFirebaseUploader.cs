// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using Firebase.Database;

// public class DirectFirebaseUploader : MonoBehaviour
// {
//     [Header("Direct Firebase Upload")]
//     [SerializeField] private bool enableDirectUpload = true;
//     [SerializeField] private float uploadInterval = 3f;
//     [SerializeField] private bool generateRealisticData = true;
    
//     [Header("Status")]
//     [SerializeField] private int totalUploadsAttempted = 0;
//     [SerializeField] private int totalUploadsSuccessful = 0;
//     [SerializeField] private string lastUploadTime = "";
//     [SerializeField] private bool uploaderActive = false;
    
//     private DatabaseReference databaseRef;
//     private FirebaseForceInitializer forceInitializer;
//     private Queue<Dictionary<string, object>> uploadQueue = new Queue<Dictionary<string, object>>();
    
//     // Professional user data
//     private string[] professionalUsers = {
//         "alex_runner_pro", "sarah_speedster", "mike_champion", "emma_gamer", 
//         "david_master", "lisa_expert", "john_legend", "anna_ace"
//     };
    
//     private string[] deviceTypes = {
//         "iPhone 15 Pro", "Samsung Galaxy S24", "Google Pixel 8", "OnePlus 12",
//         "Xiaomi 14 Pro", "iPad Air", "Galaxy Tab S9", "iPhone 14"
//     };
    
//     private void Start()
//     {
//         // Find Firebase Force Initializer
//         forceInitializer = FindObjectOfType<FirebaseForceInitializer>();
        
//         if (enableDirectUpload)
//         {
//             StartCoroutine(WaitForFirebaseAndStart());
//         }
//     }
    
//     private IEnumerator WaitForFirebaseAndStart()
//     {
//         Debug.Log("📤 [DIRECT UPLOADER] Waiting for Firebase to be ready...");
        
//         // Wait for Firebase to be initialized
//         float timeout = 15f;
//         float timer = 0f;
        
//         while (timer < timeout)
//         {
//             // Check if Firebase is ready through multiple sources
//             bool firebaseReady = false;
            
//             // Method 1: Check FirebaseForceInitializer
//             if (forceInitializer != null && forceInitializer.IsFirebaseReady)
//             {
//                 databaseRef = forceInitializer.Database;
//                 firebaseReady = true;
//             }
            
//             // Method 2: Check FirebaseManager
//             if (!firebaseReady && FirebaseManager.Instance != null && FirebaseManager.Instance.IsInitialized)
//             {
//                 databaseRef = FirebaseManager.Instance.databaseRef;
//                 firebaseReady = true;
//             }
            
//             // Method 3: Try direct Firebase access
//             if (!firebaseReady)
//             {
//                 try
//                 {
//                     databaseRef = Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference;
//                     firebaseReady = (databaseRef != null);
//                 }
//                 catch (System.Exception ex)
//                 {
//                     Debug.LogWarning($"Direct Firebase access failed: {ex.Message}");
//                 }
//             }
            
//             if (firebaseReady)
//             {
//                 Debug.Log("✅ [DIRECT UPLOADER] Firebase is ready! Starting direct uploads...");
//                 StartCoroutine(DirectUploadLoop());
//                 uploaderActive = true;
//                 break;
//             }
            
//             timer += Time.deltaTime;
//             yield return null;
//         }
        
//         if (timer >= timeout)
//         {
//             Debug.LogError("❌ [DIRECT UPLOADER] Firebase initialization timeout! Creating offline data...");
//             StartCoroutine(CreateOfflineData());
//         }
//     }
    
//     private IEnumerator DirectUploadLoop()
//     {
//         while (enableDirectUpload && databaseRef != null)
//         {
//             if (generateRealisticData)
//             {
//                 // Generate and upload multiple types of data
//                 UploadPlayerSession();
//                 yield return new WaitForSeconds(1f);
                
//                 UploadPlayerStats();
//                 yield return new WaitForSeconds(1f);
                
//                 UploadGameplayEvent();
//                 yield return new WaitForSeconds(1f);
                
//                 UploadDashboardMetrics();
//             }
            
//             // Process any queued uploads
//             while (uploadQueue.Count > 0)
//             {
//                 var queuedData = uploadQueue.Dequeue();
//                 yield return StartCoroutine(DirectUpload("queued_data", queuedData));
//             }
            
//             yield return new WaitForSeconds(uploadInterval);
//         }
//     }
    
//     private void UploadPlayerSession()
//     {
//         string selectedUser = professionalUsers[Random.Range(0, professionalUsers.Length)];
//         string device = deviceTypes[Random.Range(0, deviceTypes.Length)];
        
//         var sessionData = new Dictionary<string, object>
//         {
//             {"player_id", selectedUser},
//             {"session_id", $"session_{System.DateTime.Now.Ticks}"},
//             {"start_time", System.DateTime.Now.AddMinutes(-Random.Range(2, 10)).ToString("yyyy-MM-dd HH:mm:ss")},
//             {"end_time", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
//             {"session_length", Random.Range(120, 600)}, // 2-10 minutes
//             {"final_score", Random.Range(3000, 12000)},
//             {"coins_collected", Random.Range(30, 120)},
//             {"obstacles_hit", Random.Range(3, 15)},
//             {"power_ups_used", Random.Range(1, 8)},
//             {"device_model", device},
//             {"game_version", "1.2.0"},
//             {"location", GetRandomLocation()},
//             {"network_type", Random.Range(0, 2) == 0 ? "WiFi" : "5G"}
//         };
        
//         StartCoroutine(DirectUpload("player_sessions", sessionData));
//     }
    
//     private void UploadPlayerStats()
//     {
//         string selectedUser = professionalUsers[Random.Range(0, professionalUsers.Length)];
        
//         var statsData = new Dictionary<string, object>
//         {
//             {"player_id", selectedUser},
//             {"total_sessions", Random.Range(25, 200)},
//             {"total_playtime", Random.Range(3600, 36000)}, // 1-10 hours
//             {"average_score", Random.Range(4000, 9000)},
//             {"best_score", Random.Range(9000, 18000)},
//             {"skill_rating", Random.Range(650, 950)}, // Like chess rating
//             {"total_coins", Random.Range(800, 5000)},
//             {"achievement_count", Random.Range(12, 45)},
//             {"favorite_time", GetTimeOfDay()},
//             {"preferred_difficulty", GetDifficultyPreference()},
//             {"last_played", System.DateTime.Now.AddHours(-Random.Range(1, 48)).ToString("yyyy-MM-dd")},
//             {"consecutive_days", Random.Range(1, 30)},
//             {"improvement_rate", Random.Range(1.05f, 1.25f)}
//         };
        
//         StartCoroutine(DirectUpload("player_statistics", statsData));
//     }
    
//     private void UploadGameplayEvent()
//     {
//         string selectedUser = professionalUsers[Random.Range(0, professionalUsers.Length)];
//         string[] actions = {"jump", "slide", "swipe_left", "swipe_right", "coin_collected", "power_activated", "obstacle_avoided"};
        
//         var eventData = new Dictionary<string, object>
//         {
//             {"player_id", selectedUser},
//             {"event_id", $"event_{System.DateTime.Now.Ticks}"},
//             {"action", actions[Random.Range(0, actions.Length)]},
//             {"timestamp", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")},
//             {"game_score", Random.Range(500, 8000)},
//             {"player_position", $"{Random.Range(-2f, 2f):F1},{Random.Range(0f, 3f):F1},{Random.Range(100, 5000)}"},
//             {"reaction_time", Random.Range(0.15f, 0.85f)},
//             {"game_speed", Random.Range(10f, 25f)},
//             {"difficulty_level", Random.Range(1f, 4f)},
//             {"success", Random.Range(0, 10) > 2}, // 70% success rate
//             {"combo_count", Random.Range(1, 20)}
//         };
        
//         StartCoroutine(DirectUpload("gameplay_events", eventData));
//     }
    
//     private void UploadDashboardMetrics()
//     {
//         string selectedUser = professionalUsers[Random.Range(0, professionalUsers.Length)];
        
//         var metricsData = new Dictionary<string, object>
//         {
//             {"player_id", selectedUser},
//             {"timestamp", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
//             {"fps", Random.Range(55f, 60f)},
//             {"memory_usage", Random.Range(900, 1400)}, // MB
//             {"battery_level", Random.Range(25, 95)},
//             {"network_latency", Random.Range(20, 80)}, // ms
//             {"device_temperature", Random.Range(30, 45)}, // Celsius
//             {"storage_free", Random.Range(8000, 32000)}, // MB
//             {"graphics_quality", "High"},
//             {"audio_enabled", true},
//             {"vibration_enabled", true},
//             {"background_apps", Random.Range(3, 12)}
//         };
        
//         StartCoroutine(DirectUpload("performance_metrics", metricsData));
//     }
    
//     private IEnumerator DirectUpload(string path, Dictionary<string, object> data)
//     {
//         totalUploadsAttempted++;
        
//         if (databaseRef == null)
//         {
//             Debug.LogWarning("📤 [DIRECT UPLOADER] Database reference is null!");
//             yield break;
//         }
        
//         // Create unique key for this data
//         string uniqueKey = databaseRef.Child(path).Push().Key;
        
//         // Upload to Firebase
//         var uploadTask = databaseRef.Child(path).Child(uniqueKey).SetValueAsync(data);
        
//         yield return new WaitUntil(() => uploadTask.IsCompleted);
        
//         if (uploadTask.IsCompletedSuccessfully)
//         {
//             totalUploadsSuccessful++;
//             lastUploadTime = System.DateTime.Now.ToString("HH:mm:ss");
            
//             Debug.Log($"🔥 [FIREBASE] Data uploaded successfully to /{path}/{uniqueKey}");
//             Debug.Log($"📊 [UPLOAD STATS] Success: {totalUploadsSuccessful}/{totalUploadsAttempted}");
//         }
//         else
//         {
//             Debug.LogError($"❌ [DIRECT UPLOADER] Upload failed: {uploadTask.Exception}");
//         }
//     }
    
//     private IEnumerator CreateOfflineData()
//     {
//         Debug.Log("💾 [OFFLINE DATA] Creating offline backup data...");
        
//         string backupPath = Application.persistentDataPath + "/firebase_backup/";
//         if (!System.IO.Directory.Exists(backupPath))
//         {
//             System.IO.Directory.CreateDirectory(backupPath);
//         }
        
//         // Create sample data files
//         for (int i = 0; i < 5; i++)
//         {
//             CreateOfflineDataFile(backupPath, i);
//             yield return new WaitForSeconds(0.1f);
//         }
        
//         Debug.Log($"💾 [OFFLINE DATA] Created backup files in: {backupPath}");
//     }
    
//     private void CreateOfflineDataFile(string backupPath, int index)
//     {
//         var offlineData = new Dictionary<string, object>
//         {
//             {"offline_user_id", professionalUsers[Random.Range(0, professionalUsers.Length)]},
//             {"timestamp", System.DateTime.Now.AddMinutes(-index).ToString()},
//             {"score", Random.Range(2000, 8000)},
//             {"message", "Offline backup data - will upload when Firebase is ready"}
//         };
        
//         string jsonData = JsonUtility.ToJson(new DataWrapper(offlineData), true);
//         string fileName = $"offline_backup_{System.DateTime.Now.Ticks + index}.json";
//         System.IO.File.WriteAllText(backupPath + fileName, jsonData);
//     }
    
//     [System.Serializable]
//     public class DataWrapper
//     {
//         public List<KeyValueEntry> data = new List<KeyValueEntry>();
        
//         public DataWrapper(Dictionary<string, object> dict)
//         {
//             foreach (var kvp in dict)
//             {
//                 data.Add(new KeyValueEntry { key = kvp.Key, value = kvp.Value.ToString() });
//             }
//         }
//     }
    
//     [System.Serializable]
//     public class KeyValueEntry
//     {
//         public string key;
//         public string value;
//     }
    
//     // Helper methods
//     private string GetRandomLocation()
//     {
//         string[] locations = {"New York", "London", "Tokyo", "Sydney", "Toronto", "Mumbai", "Berlin", "Singapore"};
//         return locations[Random.Range(0, locations.Length)];
//     }
    
//     private string GetTimeOfDay()
//     {
//         int hour = System.DateTime.Now.Hour;
//         if (hour >= 6 && hour < 12) return "Morning";
//         if (hour >= 12 && hour < 17) return "Afternoon";
//         if (hour >= 17 && hour < 22) return "Evening";
//         return "Night";
//     }
    
//     private string GetDifficultyPreference()
//     {
//         string[] difficulties = {"Easy", "Medium", "Hard", "Expert"};
//         return difficulties[Random.Range(0, difficulties.Length)];
//     }
    
//     // Public controls
//     [ContextMenu("Force Upload Test Data")]
//     public void ForceUploadTestData()
//     {
//         if (databaseRef != null)
//         {
//             var testData = new Dictionary<string, object>
//             {
//                 {"test_message", "Manual test upload"},
//                 {"timestamp", System.DateTime.Now.ToString()},
//                 {"test_number", Random.Range(1000, 9999)}
//             };
            
//             StartCoroutine(DirectUpload("manual_test", testData));
//         }
//         else
//         {
//             Debug.LogWarning("Database reference not ready!");
//         }
//     }
    
//     [ContextMenu("Show Upload Stats")]
//     public void ShowUploadStats()
//     {
//         Debug.Log($"📊 [UPLOAD STATS] Attempted: {totalUploadsAttempted}, Successful: {totalUploadsSuccessful}");
//         Debug.Log($"📊 [UPLOAD STATS] Success Rate: {(totalUploadsAttempted > 0 ? (float)totalUploadsSuccessful / totalUploadsAttempted * 100f : 0f):F1}%");
//         Debug.Log($"📊 [UPLOAD STATS] Last Upload: {lastUploadTime}");
//         Debug.Log($"📊 [UPLOAD STATS] Uploader Active: {uploaderActive}");
//     }
// }
