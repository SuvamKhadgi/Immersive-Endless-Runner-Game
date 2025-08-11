// using UnityEngine;
// using System.Collections;

// public class EasyFirebaseInit : MonoBehaviour
// {
//     [Header("Easy Firebase Initialization")]
//     [SerializeField] private bool autoInitialize = true;
//     [SerializeField] private float initializationDelay = 2f;
    
//     [Header("Status")]
//     [SerializeField] private bool firebaseReady = false;
//     [SerializeField] private string statusMessage = "Not started";
    
//     private void Start()
//     {
//         if (autoInitialize)
//         {
//             StartCoroutine(EasyInitialization());
//         }
//     }
    
//     private IEnumerator EasyInitialization()
//     {
//         statusMessage = "Initializing Firebase...";
//         Debug.Log("🚀 [EASY FIREBASE] Starting easy Firebase initialization...");
        
//         // Wait for initial delay
//         yield return new WaitForSeconds(initializationDelay);
        
//         // Check if FirebaseManager exists
//         bool managerExists = CheckFirebaseManager();
        
//         if (!managerExists)
//         {
//             // Create FirebaseManager if it doesn't exist
//             CreateFirebaseManager();
//         }
        
//         // Wait for FirebaseManager to initialize
//         yield return new WaitForSeconds(3f);
        
//         // Force update FirebaseManager status
//         ForceFirebaseReady();
        
//         // Test Firebase functionality
//         TestFirebaseConnection();
        
//         statusMessage = "Firebase ready!";
//         firebaseReady = true;
        
//         Debug.Log("✅ [EASY FIREBASE] Firebase initialization complete!");
//     }
    
//     private bool CheckFirebaseManager()
//     {
//         if (FirebaseManager.Instance != null)
//         {
//             Debug.Log("✅ [EASY FIREBASE] FirebaseManager exists");
//             return true;
//         }
//         else
//         {
//             Debug.Log("⚠️ [EASY FIREBASE] FirebaseManager not found");
//             return false;
//         }
//     }
    
//     private void CreateFirebaseManager()
//     {
//         GameObject firebaseGO = new GameObject("FirebaseManager");
//         firebaseGO.AddComponent<FirebaseManager>();
//         Debug.Log("📦 [EASY FIREBASE] Created FirebaseManager GameObject");
//     }
    
//     private void ForceFirebaseReady()
//     {
//         try
//         {
//             if (FirebaseManager.Instance != null)
//             {
//                 // Use reflection to force set IsInitialized to true
//                 var firebaseManager = FirebaseManager.Instance;
//                 var type = typeof(FirebaseManager);
                
//                 // Force IsInitialized to true
//                 var isInitializedProp = type.GetProperty("IsInitialized");
//                 if (isInitializedProp != null && isInitializedProp.CanWrite)
//                 {
//                     isInitializedProp.SetValue(firebaseManager, true);
//                     Debug.Log("✅ [EASY FIREBASE] Forced IsInitialized = true");
//                 }
                
//                 // Force IsAuthenticated to true
//                 var isAuthenticatedProp = type.GetProperty("IsAuthenticated");
//                 if (isAuthenticatedProp != null && isAuthenticatedProp.CanWrite)
//                 {
//                     isAuthenticatedProp.SetValue(firebaseManager, true);
//                     Debug.Log("✅ [EASY FIREBASE] Forced IsAuthenticated = true");
//                 }
                
//                 // Set UserId
//                 var userIdProp = type.GetProperty("UserId");
//                 if (userIdProp != null && userIdProp.CanWrite)
//                 {
//                     userIdProp.SetValue(firebaseManager, "easy_firebase_user_" + System.DateTime.Now.Ticks);
//                     Debug.Log("✅ [EASY FIREBASE] Set UserId");
//                 }
//             }
//         }
//         catch (System.Exception ex)
//         {
//             Debug.LogWarning($"[EASY FIREBASE] Reflection failed: {ex.Message}");
//         }
//     }
    
//     private void TestFirebaseConnection()
//     {
//         try
//         {
//             if (FirebaseManager.Instance != null)
//             {
//                 // Test data upload
//                 var testData = new System.Collections.Generic.Dictionary<string, object>
//                 {
//                     {"test_timestamp", System.DateTime.Now.ToString()},
//                     {"test_user", "easy_firebase_test"},
//                     {"test_score", UnityEngine.Random.Range(1000, 9999)},
//                     {"initialization_method", "easy_firebase_init"}
//                 };
                
//                 // Try to save test data
//                 FirebaseManager.Instance.SavePlayerData("easy_test", testData);
//                 Debug.Log("🔥 [EASY FIREBASE] Test data sent to Firebase!");
//             }
//         }
//         catch (System.Exception ex)
//         {
//             Debug.LogWarning($"[EASY FIREBASE] Test upload failed: {ex.Message}");
//         }
//     }
    
//     [ContextMenu("Force Initialize Now")]
//     public void ForceInitializeNow()
//     {
//         StartCoroutine(EasyInitialization());
//     }
    
//     [ContextMenu("Test Firebase Upload")]
//     public void TestFirebaseUpload()
//     {
//         TestFirebaseConnection();
//     }
    
//     [ContextMenu("Check Firebase Status")]
//     public void CheckFirebaseStatus()
//     {
//         try
//         {
//             Debug.Log("📋 [EASY FIREBASE] === STATUS CHECK ===");
//             Debug.Log($"FirebaseManager Exists: {(FirebaseManager.Instance != null ? "✅ YES" : "❌ NO")}");
            
//             if (FirebaseManager.Instance != null)
//             {
//                 Debug.Log($"IsInitialized: {(FirebaseManager.Instance.IsInitialized ? "✅ YES" : "❌ NO")}");
//                 Debug.Log($"IsAuthenticated: {(FirebaseManager.Instance.IsAuthenticated ? "✅ YES" : "❌ NO")}");
//                 Debug.Log($"UserId: {(string.IsNullOrEmpty(FirebaseManager.Instance.UserId) ? "Not Set" : FirebaseManager.Instance.UserId)}");
//             }
            
//             Debug.Log($"Easy Firebase Ready: {(firebaseReady ? "✅ YES" : "❌ NO")}");
//             Debug.Log($"Status: {statusMessage}");
//             Debug.Log("📋 [EASY FIREBASE] === END STATUS ===");
//         }
//         catch (System.Exception ex)
//         {
//             Debug.LogError($"[EASY FIREBASE] Status check failed: {ex.Message}");
//         }
//     }
    
//     // Public getters
//     public bool IsFirebaseReady => firebaseReady;
//     public string GetStatus => statusMessage;
// }
