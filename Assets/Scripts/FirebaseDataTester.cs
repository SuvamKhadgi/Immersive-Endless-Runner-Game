// using UnityEngine;
// using System.Collections;

// public class FirebaseDataTester : MonoBehaviour
// {
//     [Header("Testing Controls")]
//     public bool enableDebugLogging = true;
//     public bool testDataWrite = false;
    
//     void Start()
//     {
//         if (enableDebugLogging)
//         {
//             InvokeRepeating("LogSystemStatus", 2f, 10f);
//         }
        
//         if (testDataWrite)
//         {
//             StartCoroutine(TestDataWriting());
//         }
//     }
    
//     void LogSystemStatus()
//     {
//         Debug.Log("=== FIREBASE SYSTEM STATUS ===");
        
//         // Check FirebaseManager
//         if (FirebaseManager.Instance != null)
//         {
//             Debug.Log($"✅ FirebaseManager: EXISTS");
//             Debug.Log($"   - Initialized: {FirebaseManager.Instance.IsInitialized}");
//             Debug.Log($"   - Authenticated: {FirebaseManager.Instance.IsAuthenticated}");
//             Debug.Log($"   - User ID: {FirebaseManager.Instance.UserId ?? "NULL"}");
//             Debug.Log($"   - Database Ref: {(FirebaseManager.Instance.databaseRef != null ? "EXISTS" : "NULL")}");
//         }
//         else
//         {
//             Debug.Log("❌ FirebaseManager: NOT FOUND");
//         }
        
//         // Check PlayerAnalyticsManager
//         if (PlayerAnalyticsManager.Instance != null)
//         {
//             Debug.Log($"✅ PlayerAnalyticsManager: EXISTS");
//             Debug.Log($"   - Current Session: {(PlayerAnalyticsManager.Instance.CurrentSession != null ? "ACTIVE" : "NULL")}");
//             if (PlayerAnalyticsManager.Instance.CurrentSession != null)
//             {
//                 Debug.Log($"   - Session ID: {PlayerAnalyticsManager.Instance.CurrentSession.sessionId}");
//             }
//         }
//         else
//         {
//             Debug.Log("❌ PlayerAnalyticsManager: NOT FOUND");
//         }
        
//         // Check CloudDashboardManager
//         if (CloudDashboardManager.Instance != null)
//         {
//             Debug.Log($"✅ CloudDashboardManager: EXISTS");
//         }
//         else
//         {
//             Debug.Log("❌ CloudDashboardManager: NOT FOUND");
//         }
        
//         Debug.Log("================================");
//     }
    
//     IEnumerator TestDataWriting()
//     {
//         Debug.Log("Starting Firebase data write test...");
        
//         // Wait for Firebase to initialize
//         yield return new WaitForSeconds(5f);
        
//         if (FirebaseManager.Instance?.IsInitialized == true && FirebaseManager.Instance.databaseRef != null)
//         {
//             Debug.Log("Attempting to write test data to Firebase...");
            
//             // Try to write test data using FirebaseManager
//             var testData = new System.Collections.Generic.Dictionary<string, object>
//             {
//                 {"test_timestamp", System.DateTime.Now.ToString()},
//                 {"test_value", Random.Range(1, 100)},
//                 {"unity_test", true}
//             };
            
//             FirebaseManager.Instance.SavePlayerData("test_data", testData);
//             Debug.Log("Test data write attempt completed - check Firebase Console");
//         }
//         else
//         {
//             Debug.LogError("Cannot test data writing - Firebase not initialized");
//             Debug.Log("TRYING OFFLINE MODE - Writing to JSON file instead...");
            
//             // Fallback: Write to local file for testing
//             var testData = new System.Collections.Generic.Dictionary<string, object>
//             {
//                 {"test_timestamp", System.DateTime.Now.ToString()},
//                 {"test_value", Random.Range(1, 100)},
//                 {"unity_test", true},
//                 {"offline_mode", true}
//             };
            
//             string jsonData = JsonUtility.ToJson(testData, true);
//             string filePath = Application.persistentDataPath + "/firebase_test.json";
//             System.IO.File.WriteAllText(filePath, jsonData);
//             Debug.Log($"✅ TEST DATA WRITTEN TO: {filePath}");
//             Debug.Log($"Content: {jsonData}");
            
//             // Also test analytics directly
//             if (PlayerAnalyticsManager.Instance != null)
//             {
//                 PlayerAnalyticsManager.Instance.LogGameEvent("offline_test", transform.position);
//                 Debug.Log("✅ Analytics event logged in offline mode");
//             }
//         }
//     }
    
//     // Manual test buttons for inspector
//     [ContextMenu("Test Firebase Connection")]
//     void TestFirebaseConnection()
//     {
//         LogSystemStatus();
//     }
    
//     [ContextMenu("Force Start Analytics Session")]
//     void ForceStartSession()
//     {
//         if (PlayerAnalyticsManager.Instance != null)
//         {
//             PlayerAnalyticsManager.Instance.StartGameSession();
//             Debug.Log("Manually started analytics session");
//         }
//         else
//         {
//             Debug.LogError("PlayerAnalyticsManager not found!");
//         }
//     }
    
//     [ContextMenu("Simulate Player Movement")]
//     void SimulateMovement()
//     {
//         if (PlayerAnalyticsManager.Instance != null)
//         {
//             // Simulate some player actions
//             PlayerAnalyticsManager.Instance.TrackPlayerMovement(transform.position + Vector3.right);
//             PlayerAnalyticsManager.Instance.TrackPlayerInput("test_move", 0.5f);
//             PlayerAnalyticsManager.Instance.LogGameEvent("test_event", transform.position);
//             Debug.Log("Simulated player movement and events");
//         }
//         else
//         {
//             Debug.LogError("PlayerAnalyticsManager not found!");
//         }
//     }
// }
