// using UnityEngine;
// using System.Collections.Generic;

// public class SystemDiagnostic : MonoBehaviour
// {
//     [Header("Diagnostic Settings")]
//     [SerializeField] private bool runDiagnosticOnStart = true;
//     [SerializeField] private bool verboseLogging = true;
    
//     [Header("System Status")]
//     [SerializeField] private bool allSystemsReady = false;
//     [SerializeField] private List<string> missingComponents = new List<string>();
//     [SerializeField] private List<string> foundComponents = new List<string>();
    
//     private void Start()
//     {
//         if (runDiagnosticOnStart)
//         {
//             Invoke("RunCompleteDiagnostic", 1f);
//         }
//     }
    
//     [ContextMenu("Run Complete Diagnostic")]
//     public void RunCompleteDiagnostic()
//     {
//         Debug.Log("🔍 [DIAGNOSTIC] Starting complete system diagnostic...");
        
//         missingComponents.Clear();
//         foundComponents.Clear();
        
//         CheckGameObjectIntegrity();
//         CheckScriptReferences();
//         CheckFirebaseConfiguration();
//         CheckSystemComponents();
        
//         GenerateReport();
//     }
    
//     private void CheckGameObjectIntegrity()
//     {
//         Debug.Log("🔍 [DIAGNOSTIC] Checking GameObject integrity...");
        
//         // Find all GameObjects with missing scripts
//         GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
//         int missingScriptCount = 0;
        
//         foreach (GameObject go in allGameObjects)
//         {
//             Component[] components = go.GetComponents<Component>();
//             for (int i = 0; i < components.Length; i++)
//             {
//                 if (components[i] == null)
//                 {
//                     Debug.LogError($"❌ [DIAGNOSTIC] Missing script on GameObject: {go.name} at component index {i}");
//                     missingScriptCount++;
//                 }
//             }
//         }
        
//         if (missingScriptCount == 0)
//         {
//             Debug.Log("✅ [DIAGNOSTIC] No missing script references found");
//         }
//         else
//         {
//             Debug.LogWarning($"⚠️ [DIAGNOSTIC] Found {missingScriptCount} missing script references");
//         }
//     }
    
//     private void CheckScriptReferences()
//     {
//         Debug.Log("🔍 [DIAGNOSTIC] Checking required system scripts...");
        
//         CheckComponent<FirebaseManager>("FirebaseManager");
//         CheckComponent<PlayerAnalyticsManager>("PlayerAnalyticsManager");
//         CheckComponent<AdvancedAIBehavior>("AdvancedAIBehavior");
//         CheckComponent<CloudDashboardManager>("CloudDashboardManager");
//         CheckComponent<GameManager>("GameManager");
//         CheckComponent<FakeFirebaseData>("FakeFirebaseData");
//         CheckComponent<SystemIntegration>("SystemIntegration");
//         CheckComponent<PlayerMovement>("PlayerMovement");
//         CheckComponent<LevelDistance>("LevelDistance");
//     }
    
//     private void CheckComponent<T>(string componentName) where T : Component
//     {
//         T component = FindObjectOfType<T>();
//         if (component != null)
//         {
//             foundComponents.Add(componentName);
//             if (verboseLogging)
//                 Debug.Log($"✅ [DIAGNOSTIC] {componentName} found on: {component.gameObject.name}");
//         }
//         else
//         {
//             missingComponents.Add(componentName);
//             Debug.LogWarning($"❌ [DIAGNOSTIC] {componentName} NOT FOUND - need to create GameObject with this script");
//         }
//     }
    
//     private void CheckFirebaseConfiguration()
//     {
//         Debug.Log("🔍 [DIAGNOSTIC] Checking Firebase configuration...");
        
//         // Check config files
//         string googleServicesPath = Application.streamingAssetsPath + "/google-services.json";
//         string firebaseConfigPath = Application.streamingAssetsPath + "/FirebaseConfig.json";
        
//         bool googleServicesExists = System.IO.File.Exists(googleServicesPath);
//         bool firebaseConfigExists = System.IO.File.Exists(firebaseConfigPath);
        
//         Debug.Log($"Google Services JSON: {(googleServicesExists ? "✅ Found" : "❌ Missing")}");
//         Debug.Log($"Firebase Config JSON: {(firebaseConfigExists ? "✅ Found" : "❌ Missing")}");
        
//         // Check Firebase Manager status
//         FirebaseManager firebaseManager = FindObjectOfType<FirebaseManager>();
//         if (firebaseManager != null)
//         {
//             Debug.Log($"Firebase Initialized: {(FirebaseManager.Instance?.IsInitialized == true ? "✅ Yes" : "❌ No")}");
//             Debug.Log($"Firebase Authenticated: {(FirebaseManager.Instance?.IsAuthenticated == true ? "✅ Yes" : "❌ No")}");
//         }
//     }
    
//     private void CheckSystemComponents()
//     {
//         Debug.Log("🔍 [DIAGNOSTIC] Checking system component status...");
        
//         // Check Analytics Manager
//         PlayerAnalyticsManager analytics = FindObjectOfType<PlayerAnalyticsManager>();
//         if (analytics != null)
//         {
//             Debug.Log($"Analytics Current Session: {(analytics.CurrentSession != null ? "✅ Active" : "❌ Not Started")}");
//         }
        
//         // Check Fake Firebase Data
//         FakeFirebaseData fakeData = FindObjectOfType<FakeFirebaseData>();
//         if (fakeData != null)
//         {
//             Debug.Log($"Fake Data Generation: {(fakeData.generateFakeData ? "✅ Enabled" : "❌ Disabled")}");
//         }
        
//         // Check System Integration
//         SystemIntegration integration = FindObjectOfType<SystemIntegration>();
//         if (integration != null)
//         {
//             Debug.Log($"Systems Initialized: {(integration.AreSystemsInitialized() ? "✅ Yes" : "❌ No")}");
//             Debug.Log($"Firebase Connected: {(integration.IsFirebaseConnected() ? "✅ Yes" : "❌ No")}");
//             Debug.Log($"Analytics Active: {(integration.IsAnalyticsActive() ? "✅ Yes" : "❌ No")}");
//         }
//     }
    
//     private void GenerateReport()
//     {
//         Debug.Log("📋 [DIAGNOSTIC REPORT] ================");
//         Debug.Log($"Found Components ({foundComponents.Count}): {string.Join(", ", foundComponents)}");
        
//         if (missingComponents.Count > 0)
//         {
//             Debug.LogWarning($"Missing Components ({missingComponents.Count}): {string.Join(", ", missingComponents)}");
//             Debug.LogWarning("🔧 [SOLUTION] Use AutoSetupManager to create missing GameObjects automatically");
//         }
        
//         allSystemsReady = (missingComponents.Count == 0);
        
//         if (allSystemsReady)
//         {
//             Debug.Log("🎉 [DIAGNOSTIC] All systems are properly configured!");
//         }
//         else
//         {
//             Debug.LogWarning("⚠️ [DIAGNOSTIC] Some systems need attention. Check the missing components list above.");
//         }
        
//         Debug.Log("📋 [DIAGNOSTIC REPORT END] ============");
//     }
    
//     [ContextMenu("Fix Missing References")]
//     public void FixMissingReferences()
//     {
//         Debug.Log("🔧 [FIX] Attempting to fix missing references...");
        
//         // Try to find and create AutoSetupManager if it doesn't exist
//         AutoSetupManager autoSetup = FindObjectOfType<AutoSetupManager>();
//         if (autoSetup == null)
//         {
//             GameObject setupGO = new GameObject("AutoSetupManager");
//             autoSetup = setupGO.AddComponent<AutoSetupManager>();
//             Debug.Log("✅ [FIX] Created AutoSetupManager");
//         }
        
//         // Run auto setup
//         autoSetup.SetupAllSystemsManually();
        
//         // Re-run diagnostic after setup
//         Invoke("RunCompleteDiagnostic", 2f);
//     }
    
//     [ContextMenu("Show Quick Status")]
//     public void ShowQuickStatus()
//     {
//         Debug.Log("⚡ [QUICK STATUS] ================");
//         Debug.Log($"Firebase: {(FindObjectOfType<FirebaseManager>() != null ? "✅" : "❌")}");
//         Debug.Log($"Analytics: {(FindObjectOfType<PlayerAnalyticsManager>() != null ? "✅" : "❌")}");
//         Debug.Log($"AI Behavior: {(FindObjectOfType<AdvancedAIBehavior>() != null ? "✅" : "❌")}");
//         Debug.Log($"Dashboard: {(FindObjectOfType<CloudDashboardManager>() != null ? "✅" : "❌")}");
//         Debug.Log($"Game Manager: {(FindObjectOfType<GameManager>() != null ? "✅" : "❌")}");
//         Debug.Log($"Fake Data: {(FindObjectOfType<FakeFirebaseData>() != null ? "✅" : "❌")}");
//         Debug.Log($"Integration: {(FindObjectOfType<SystemIntegration>() != null ? "✅" : "❌")}");
//         Debug.Log("⚡ [QUICK STATUS END] ============");
//     }
// }
