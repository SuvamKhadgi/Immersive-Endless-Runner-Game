// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections;

// public class SystemVerification : MonoBehaviour
// {
//     [Header("UI References")]
//     public Text statusText;
//     public Button testButton;
    
//     [Header("System References")]
//     public FirebaseManager firebaseManager;
//     public PlayerAnalyticsManager analyticsManager;
//     public AdvancedAIBehavior aiBehavior;
//     public CloudDashboardManager dashboardManager;
    
//     private void Start()
//     {
//         if (testButton != null)
//             testButton.onClick.AddListener(RunSystemTest);
            
//         // Auto-run verification after 2 seconds
//         StartCoroutine(DelayedVerification());
//     }
    
//     private IEnumerator DelayedVerification()
//     {
//         yield return new WaitForSeconds(2f);
//         RunSystemTest();
//     }
    
//     public void RunSystemTest()
//     {
//         StartCoroutine(VerifyAllSystems());
//     }
    
//     private IEnumerator VerifyAllSystems()
//     {
//         UpdateStatus("Starting system verification...");
//         yield return new WaitForSeconds(0.5f);
        
//         // Test 1: Firebase Manager
//         UpdateStatus("Testing Firebase Manager...");
//         if (FirebaseManager.Instance != null)
//         {
//             UpdateStatus("✓ Firebase Manager: Found");
//             if (FirebaseManager.Instance.IsInitialized)
//                 UpdateStatus("✓ Firebase: Initialized");
//             else
//                 UpdateStatus("⚠ Firebase: Not initialized (install SDK first)");
//         }
//         else
//         {
//             UpdateStatus("✗ Firebase Manager: Not found");
//         }
//         yield return new WaitForSeconds(1f);
        
//         // Test 2: Analytics Manager
//         UpdateStatus("Testing Analytics Manager...");
//         if (PlayerAnalyticsManager.Instance != null)
//         {
//             UpdateStatus("✓ Analytics Manager: Found");
//             if (PlayerAnalyticsManager.Instance.CurrentSession != null)
//                 UpdateStatus("✓ Analytics: Session active");
//             else
//                 UpdateStatus("⚠ Analytics: No active session");
//         }
//         else
//         {
//             UpdateStatus("✗ Analytics Manager: Not found");
//         }
//         yield return new WaitForSeconds(1f);
        
//         // Test 3: AI Behavior
//         UpdateStatus("Testing AI Behavior...");
//         if (aiBehavior != null)
//         {
//             UpdateStatus("✓ AI Behavior: Found");
//             UpdateStatus($"✓ AI State: {aiBehavior.GetCurrentAIState()}");
//         }
//         else
//         {
//             UpdateStatus("✗ AI Behavior: Not assigned");
//         }
//         yield return new WaitForSeconds(1f);
        
//         // Test 4: Dashboard Manager
//         UpdateStatus("Testing Dashboard Manager...");
//         if (CloudDashboardManager.Instance != null)
//         {
//             UpdateStatus("✓ Dashboard Manager: Found");
//         }
//         else
//         {
//             UpdateStatus("✗ Dashboard Manager: Not found");
//         }
//         yield return new WaitForSeconds(1f);
        
//         // Test 5: System Integration
//         UpdateStatus("Testing System Integration...");
//         var systemIntegration = FindObjectOfType<SystemIntegration>();
//         if (systemIntegration != null)
//         {
//             UpdateStatus("✓ System Integration: Found");
//         }
//         else
//         {
//             UpdateStatus("⚠ System Integration: Not found (optional)");
//         }
        
//         UpdateStatus("System verification complete!");
//     }
    
//     private void UpdateStatus(string message)
//     {
//         Debug.Log($"[System Verification] {message}");
        
//         if (statusText != null)
//         {
//             statusText.text += message + "\n";
            
//             // Scroll to bottom if too much text
//             if (statusText.text.Length > 1000)
//             {
//                 string[] lines = statusText.text.Split('\n');
//                 if (lines.Length > 20)
//                 {
//                     statusText.text = string.Join("\n", lines, lines.Length - 15, 15);
//                 }
//             }
//         }
//     }
    
//     // Method to test Firebase connection specifically
//     public void TestFirebaseConnection()
//     {
//         if (FirebaseManager.Instance != null)
//         {
//             StartCoroutine(FirebaseManager.Instance.TestConnection());
//         }
//         else
//         {
//             UpdateStatus("✗ Cannot test Firebase: Manager not found");
//         }
//     }
    
//     // Method to simulate some analytics events for testing
//     public void TestAnalytics()
//     {
//         if (PlayerAnalyticsManager.Instance != null)
//         {
//             UpdateStatus("Testing analytics events...");
//             PlayerAnalyticsManager.Instance.LogGameEvent("test_action", Vector3.zero, 
//                 new System.Collections.Generic.Dictionary<string, object> { {"type", "verification"} });
//             PlayerAnalyticsManager.Instance.TrackObstacleInteraction("test_obstacle", Vector3.zero);
//             UpdateStatus("✓ Test analytics events sent");
//         }
//         else
//         {
//             UpdateStatus("✗ Cannot test analytics: Manager not found");
//         }
//     }
// }
