// using UnityEngine;

// public class TestFakeFirebase : MonoBehaviour
// {
//     [Header("Quick Test")]
//     [SerializeField] private bool runTestOnStart = true;
    
//     private void Start()
//     {
//         if (runTestOnStart)
//         {
//             Invoke("RunQuickTest", 1f); // Wait 1 second then test
//         }
//     }
    
//     public void RunQuickTest()
//     {
//         Debug.Log("🧪 [TEST] Testing Fake Firebase setup...");
        
//         // Check if FakeFirebaseData exists
//         FakeFirebaseData fakeData = FindObjectOfType<FakeFirebaseData>();
        
//         if (fakeData == null)
//         {
//             Debug.LogError("❌ [TEST] FAILED: No FakeFirebaseData GameObject found!");
//             Debug.LogError("❌ [TEST] SOLUTION: Create GameObject → Add FakeFirebaseData script → Check 'Generate Fake Data'");
//             ShowSetupInstructions();
//             return;
//         }
        
//         Debug.Log("✅ [TEST] FakeFirebaseData found on: " + fakeData.gameObject.name);
        
//         if (!fakeData.generateFakeData)
//         {
//             Debug.LogWarning("⚠️ [TEST] Data generation is DISABLED on " + fakeData.gameObject.name);
//             Debug.LogWarning("⚠️ [TEST] SOLUTION: Check the 'Generate Fake Data' checkbox in Inspector");
//         }
//         else
//         {
//             Debug.Log("✅ [TEST] Data generation is ENABLED");
            
//             // Force generate test data
//             fakeData.GenerateSingleFakeSession();
//             Debug.Log("✅ [TEST] Generated test data - check console for Firebase messages!");
//         }
        
//         Debug.Log("🧪 [TEST] Test complete!");
//     }
    
//     private void ShowSetupInstructions()
//     {
//         Debug.Log("📋 [INSTRUCTIONS] How to fix:");
//         Debug.Log("1. Right-click in Hierarchy → Create Empty");
//         Debug.Log("2. Rename to 'FakeFirebaseData'");
//         Debug.Log("3. Add Component → FakeFirebaseData script");
//         Debug.Log("4. Check 'Generate Fake Data' checkbox ✅");
//         Debug.Log("5. Press Play to see Firebase-like messages");
//     }
    
//     [ContextMenu("Run Manual Test")]
//     public void RunManualTest()
//     {
//         RunQuickTest();
//     }
// }
