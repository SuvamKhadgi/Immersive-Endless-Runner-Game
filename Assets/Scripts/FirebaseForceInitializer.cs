// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using Firebase;
// using Firebase.Auth;
// using Firebase.Database;

// public class FirebaseForceInitializer : MonoBehaviour
// {
//     [Header("Force Firebase Setup")]
//     [SerializeField] private bool forceInitialization = true;
//     [SerializeField] private bool bypassDependencyCheck = false;
//     [SerializeField] private bool useManualConfig = true;
//     [SerializeField] private float initializationTimeout = 10f;
    
//     [Header("Manual Configuration")]
//     [SerializeField] private string projectId = "myendlessrunner";
//     [SerializeField] private string databaseUrl = "https://myendlessrunner-default-rtdb.firebaseio.com";
//     [SerializeField] private string apiKey = "AIzaSyAT7AAttMiTaC_VPvqq5vdFImRLwLpkOcg";
//     [SerializeField] private string appId = "1:553888468732:android:75339f88cef8d262831179";
    
//     [Header("Status")]
//     [SerializeField] private bool initializationComplete = false;
//     [SerializeField] private bool authenticationComplete = false;
//     [SerializeField] private bool databaseReady = false;
//     [SerializeField] private string lastError = "";
    
//     private FirebaseApp app;
//     private FirebaseAuth auth;
//     private DatabaseReference database;
//     private bool forcedInitialization = false;
    
//     private void Start()
//     {
//         if (forceInitialization)
//         {
//             StartCoroutine(ForceFirebaseInitialization());
//         }
//     }
    
//     private IEnumerator ForceFirebaseInitialization()
//     {
//         Debug.Log("🚀 [FORCE FIREBASE] Starting forced Firebase initialization...");
        
//         // Method 1: Standard initialization with timeout
//         bool standardSuccess = false;
//         float timeoutTimer = 0f;
        
//         FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
//         {
//             var dependencyStatus = task.Result;
//             if (dependencyStatus == DependencyStatus.Available)
//             {
//                 standardSuccess = true;
//                 Debug.Log("✅ [FORCE FIREBASE] Standard Firebase dependencies OK");
//             }
//             else
//             {
//                 Debug.LogWarning($"⚠️ [FORCE FIREBASE] Dependency status: {dependencyStatus}");
//             }
//         });
        
//         // Wait for standard initialization or timeout
//         while (!standardSuccess && timeoutTimer < initializationTimeout)
//         {
//             timeoutTimer += Time.deltaTime;
//             yield return null;
//         }
        
//         if (standardSuccess)
//         {
//             yield return StartCoroutine(StandardFirebaseSetup());
//         }
//         else
//         {
//             Debug.LogWarning("🔄 [FORCE FIREBASE] Standard init failed, trying forced method...");
//             yield return StartCoroutine(ForceFirebaseSetup());
//         }
        
//         // Verify everything is working
//         yield return StartCoroutine(VerifyFirebaseSetup());
//     }
    
//     private IEnumerator StandardFirebaseSetup()
//     {
//         try
//         {
//             app = FirebaseApp.DefaultInstance;
            
//             // Initialize Auth
//             auth = FirebaseAuth.DefaultInstance;
//             Debug.Log("✅ [FORCE FIREBASE] Auth initialized");
            
//             // Initialize Database
//             database = FirebaseDatabase.DefaultInstance.RootReference;
//             Debug.Log("✅ [FORCE FIREBASE] Database initialized");
            
//             // Update FirebaseManager if it exists
//             if (FirebaseManager.Instance != null)
//             {
//                 // Force set the references
//                 var firebaseManager = FirebaseManager.Instance;
//                 System.Reflection.FieldInfo appField = typeof(FirebaseManager).GetField("app", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
//                 System.Reflection.FieldInfo authField = typeof(FirebaseManager).GetField("auth", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
//                 if (appField != null) appField.SetValue(firebaseManager, app);
//                 if (authField != null) authField.SetValue(firebaseManager, auth);
                
//                 // Force set public properties
//                 System.Reflection.PropertyInfo isInitializedProp = typeof(FirebaseManager).GetProperty("IsInitialized");
//                 if (isInitializedProp != null && isInitializedProp.CanWrite)
//                 {
//                     isInitializedProp.SetValue(firebaseManager, true);
//                 }
                
//                 Debug.Log("✅ [FORCE FIREBASE] FirebaseManager references updated");
//             }
            
//             initializationComplete = true;
            
//         }
//         catch (System.Exception ex)
//         {
//             Debug.LogError($"❌ [FORCE FIREBASE] Standard setup failed: {ex.Message}");
//             lastError = ex.Message;
//         }
        
//         yield return null;
//     }
    
//     private IEnumerator ForceFirebaseSetup()
//     {
//         Debug.Log("🔧 [FORCE FIREBASE] Attempting manual Firebase configuration...");
        
//         bool setupSuccess = InitializeFirebaseManually();
        
//         if (setupSuccess)
//         {
//             yield return new WaitForSeconds(1f);
            
//             // Force initialize Auth
//             InitializeAuthSafely();
            
//             yield return new WaitForSeconds(1f);
            
//             // Force initialize Database
//             InitializeDatabaseSafely();
            
//             forcedInitialization = true;
//             initializationComplete = true;
//         }
//         else
//         {
//             Debug.LogError("❌ [FORCE FIREBASE] Manual setup failed completely");
//         }
        
//         yield return null;
//     }
    
//     private bool InitializeFirebaseManually()
//     {
//         try
//         {
//             // Create Firebase app options manually
//             var options = new AppOptions
//             {
//                 ProjectId = projectId,
//                 DatabaseUrl = new System.Uri(databaseUrl),
//                 ApiKey = apiKey,
//                 AppId = appId
//             };
            
//             // Try to create Firebase app with manual config
//             if (FirebaseApp.DefaultInstance == null)
//             {
//                 app = FirebaseApp.Create(options);
//                 Debug.Log("✅ [FORCE FIREBASE] Manual app creation successful");
//             }
//             else
//             {
//                 app = FirebaseApp.DefaultInstance;
//                 Debug.Log("✅ [FORCE FIREBASE] Using existing app instance");
//             }
            
//             return true;
//         }
//         catch (System.Exception ex)
//         {
//             Debug.LogError($"❌ [FORCE FIREBASE] Force setup failed: {ex.Message}");
//             lastError = ex.Message;
//             return false;
//         }
//     }
    
//     private void InitializeAuthSafely()
//     {
//         try
//         {
//             auth = FirebaseAuth.GetAuth(app);
//             Debug.Log("✅ [FORCE FIREBASE] Auth force-initialized");
            
//             // Sign in anonymously
//             auth.SignInAnonymouslyAsync().ContinueWith(task =>
//             {
//                 if (task.IsCompletedSuccessfully)
//                 {
//                     Debug.Log("✅ [FORCE FIREBASE] Anonymous auth successful");
//                     authenticationComplete = true;
//                 }
//                 else
//                 {
//                     Debug.LogWarning($"⚠️ [FORCE FIREBASE] Auth failed: {task.Exception}");
//                 }
//             });
//         }
//         catch (System.Exception ex)
//         {
//             Debug.LogWarning($"⚠️ [FORCE FIREBASE] Auth init warning: {ex.Message}");
//         }
//     }
    
//     private void InitializeDatabaseSafely()
//     {
//         try
//         {
//             database = FirebaseDatabase.GetInstance(app).RootReference;
//             Debug.Log("✅ [FORCE FIREBASE] Database force-initialized");
//             databaseReady = true;
//         }
//         catch (System.Exception ex)
//         {
//             Debug.LogWarning($"⚠️ [FORCE FIREBASE] Database init warning: {ex.Message}");
//         }
//     }
    
//     private void TestDatabaseWrite()
//     {
//         try
//         {
//             var testData = new Dictionary<string, object>
//             {
//                 {"test_timestamp", System.DateTime.Now.ToString()},
//                 {"test_message", "Firebase force initialization test"},
//                 {"initialization_method", forcedInitialization ? "forced" : "standard"}
//             };
            
//             database.Child("initialization_test").SetValueAsync(testData).ContinueWith(task =>
//             {
//                 if (task.IsCompletedSuccessfully)
//                 {
//                     Debug.Log("✅ [FORCE FIREBASE] Database write test SUCCESSFUL!");
//                     databaseReady = true;
//                 }
//                 else
//                 {
//                     Debug.LogError($"❌ [FORCE FIREBASE] Database write test FAILED: {task.Exception}");
//                 }
//             });
//         }
//         catch (System.Exception ex)
//         {
//             Debug.LogError($"❌ [FORCE FIREBASE] Database test failed: {ex.Message}");
//         }
//     }
    
//     private IEnumerator VerifyFirebaseSetup()
//     {
//         Debug.Log("🔍 [FORCE FIREBASE] Verifying Firebase setup...");
        
//         // Test database write
//         if (database != null)
//         {
//             TestDatabaseWrite();
//             yield return new WaitForSeconds(2f);
//         }
        
//         // Update FirebaseManager status
//         UpdateFirebaseManagerStatus();
        
//         // Generate summary
//         GenerateSetupSummary();
//     }
    
//     private void UpdateFirebaseManagerStatus()
//     {
//         if (FirebaseManager.Instance != null && initializationComplete)
//         {
//             try
//             {
//                 // Use reflection to force update the FirebaseManager
//                 var firebaseManager = FirebaseManager.Instance;
//                 var type = typeof(FirebaseManager);
                
//                 // Force set IsInitialized to true
//                 var isInitializedField = type.GetField("IsInitialized", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
//                 if (isInitializedField == null)
//                 {
//                     var isInitializedProp = type.GetProperty("IsInitialized");
//                     if (isInitializedProp != null)
//                     {
//                         // Set via backing field
//                         var backingField = type.GetField("<IsInitialized>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
//                         if (backingField != null)
//                         {
//                             backingField.SetValue(firebaseManager, true);
//                             Debug.Log("✅ [FORCE FIREBASE] FirebaseManager.IsInitialized set to true");
//                         }
//                     }
//                 }
                
//                 // Force set IsAuthenticated to true
//                 var isAuthenticatedProp = type.GetProperty("IsAuthenticated");
//                 if (isAuthenticatedProp != null)
//                 {
//                     var backingField = type.GetField("<IsAuthenticated>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
//                     if (backingField != null)
//                     {
//                         backingField.SetValue(firebaseManager, authenticationComplete);
//                         Debug.Log($"✅ [FORCE FIREBASE] FirebaseManager.IsAuthenticated set to {authenticationComplete}");
//                     }
//                 }
                
//                 // Set database reference
//                 var databaseRefField = type.GetField("databaseRef", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
//                 if (databaseRefField != null && database != null)
//                 {
//                     databaseRefField.SetValue(firebaseManager, database);
//                     Debug.Log("✅ [FORCE FIREBASE] FirebaseManager.databaseRef updated");
//                 }
                
//             }
//             catch (System.Exception ex)
//             {
//                 Debug.LogWarning($"⚠️ [FORCE FIREBASE] FirebaseManager update warning: {ex.Message}");
//             }
//         }
//     }
    
//     private void GenerateSetupSummary()
//     {
//         Debug.Log("📋 [FORCE FIREBASE] === SETUP SUMMARY ===");
//         Debug.Log($"Initialization Complete: {(initializationComplete ? "✅ YES" : "❌ NO")}");
//         Debug.Log($"Authentication Complete: {(authenticationComplete ? "✅ YES" : "⚠️ PARTIAL")}");
//         Debug.Log($"Database Ready: {(databaseReady ? "✅ YES" : "❌ NO")}");
//         Debug.Log($"Method Used: {(forcedInitialization ? "🔧 FORCED" : "📦 STANDARD")}");
        
//         if (!string.IsNullOrEmpty(lastError))
//         {
//             Debug.Log($"Last Error: {lastError}");
//         }
        
//         if (initializationComplete && databaseReady)
//         {
//             Debug.Log("🎉 [FORCE FIREBASE] Firebase is ready for data upload!");
            
//             // Test data upload
//             TestDataUpload();
//         }
//         else
//         {
//             Debug.LogWarning("⚠️ [FORCE FIREBASE] Firebase setup incomplete - some features may not work");
//         }
        
//         Debug.Log("📋 [FORCE FIREBASE] === END SUMMARY ===");
//     }
    
//     private void TestDataUpload()
//     {
//         if (database != null)
//         {
//             var testUploadData = new Dictionary<string, object>
//             {
//                 {"timestamp", System.DateTime.Now.ToString()},
//                 {"test_user", "force_init_test_user"},
//                 {"test_score", 12345},
//                 {"message", "This confirms Firebase is working!"}
//             };
            
//             database.Child("force_test_data").Push().SetValueAsync(testUploadData).ContinueWith(task =>
//             {
//                 if (task.IsCompletedSuccessfully)
//                 {
//                     Debug.Log("🎯 [FORCE FIREBASE] TEST DATA UPLOAD SUCCESSFUL!");
//                     Debug.Log("🎯 Check your Firebase Console - you should see data in 'force_test_data' node");
//                 }
//                 else
//                 {
//                     Debug.LogError($"❌ [FORCE FIREBASE] Test upload failed: {task.Exception}");
//                 }
//             });
//         }
//     }
    
//     // Public methods for external testing
//     [ContextMenu("Force Initialize Now")]
//     public void ForceInitializeNow()
//     {
//         StartCoroutine(ForceFirebaseInitialization());
//     }
    
//     [ContextMenu("Test Data Upload")]
//     public void TestDataUploadManual()
//     {
//         TestDataUpload();
//     }
    
//     [ContextMenu("Check Status")]
//     public void CheckStatus()
//     {
//         GenerateSetupSummary();
//     }
    
//     // Public getters
//     public bool IsFirebaseReady => initializationComplete && databaseReady;
//     public DatabaseReference Database => database;
//     public FirebaseAuth Auth => auth;
// }
