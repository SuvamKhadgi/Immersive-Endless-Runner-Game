// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// public class SimpleFirebaseUploader : MonoBehaviour
// {
//     [Header("Simple Firebase Upload")]
//     [SerializeField] private bool enableUpload = true;
//     [SerializeField] private float uploadInterval = 5f;
    
//     [Header("Status")]
//     [SerializeField] private int totalUploads = 0;
//     [SerializeField] private string lastUploadTime = "";
    
//     private void Start()
//     {
//         if (enableUpload)
//         {
//             StartCoroutine(SimpleUploadLoop());
//         }
//     }
    
//     private IEnumerator SimpleUploadLoop()
//     {
//         yield return new WaitForSeconds(3f); // Initial delay
        
//         Debug.Log("📤 [SIMPLE UPLOADER] Starting simple Firebase uploads...");
        
//         while (enableUpload)
//         {
//             UploadSimpleData();
            
//             yield return new WaitForSeconds(uploadInterval);
//         }
//     }
    
//     private void UploadSimpleData()
//     {
//         // Check if FirebaseManager exists and is ready
//         if (FirebaseManager.Instance != null && FirebaseManager.Instance.IsInitialized)
//         {
//             var simpleData = new Dictionary<string, object>
//             {
//                 {"user", "professional_player_" + Random.Range(1000, 9999)},
//                 {"timestamp", System.DateTime.Now.ToString()},
//                 {"score", Random.Range(3000, 12000)},
//                 {"coins", Random.Range(25, 85)},
//                 {"session_length", Random.Range(120, 600)},
//                 {"device", GetRandomDevice()},
//                 {"level_reached", Random.Range(5, 25)}
//             };
            
//             // Use FirebaseManager's SavePlayerData method
//             FirebaseManager.Instance.SavePlayerData("game_data", simpleData);
            
//             totalUploads++;
//             lastUploadTime = System.DateTime.Now.ToString("HH:mm:ss");
            
//             Debug.Log($"🔥 [FIREBASE] Simple data uploaded #{totalUploads}");
//             Debug.Log($"📊 [DATA] User: {simpleData["user"]}, Score: {simpleData["score"]}");
//         }
//         else
//         {
//             Debug.LogWarning("⚠️ [SIMPLE UPLOADER] Firebase not ready - skipping upload");
//         }
//     }
    
//     private string GetRandomDevice()
//     {
//         string[] devices = {
//             "iPhone 15 Pro", "Samsung Galaxy S24", "Google Pixel 8", 
//             "OnePlus 12", "iPad Pro", "Galaxy Tab S9"
//         };
//         return devices[Random.Range(0, devices.Length)];
//     }
    
//     [ContextMenu("Upload Test Data Now")]
//     public void UploadTestDataNow()
//     {
//         UploadSimpleData();
//     }
    
//     [ContextMenu("Show Upload Status")]
//     public void ShowUploadStatus()
//     {
//         Debug.Log($"📊 [STATUS] Total Uploads: {totalUploads}");
//         Debug.Log($"📊 [STATUS] Last Upload: {lastUploadTime}");
//         Debug.Log($"📊 [STATUS] Firebase Ready: {(FirebaseManager.Instance?.IsInitialized == true ? "YES" : "NO")}");
//     }
// }
