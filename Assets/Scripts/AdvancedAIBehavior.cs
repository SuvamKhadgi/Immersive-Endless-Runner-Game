// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Linq;
// using System;

// [System.Serializable]
// public class AIBehaviorPattern
// {
//     public string patternName;
//     public float aggressiveness;
//     public float predictiveness;
//     public float adaptability;
//     public List<AIDecision> recentDecisions;
//     public Dictionary<string, float> playerBehaviorWeights;
    
//     public AIBehaviorPattern()
//     {
//         recentDecisions = new List<AIDecision>();
//         playerBehaviorWeights = new Dictionary<string, float>();
//     }
// }

// [System.Serializable]
// public class AIDecision
// {
//     public float timestamp;
//     public string decisionType;
//     public Vector3 aiPosition;
//     public Vector3 playerPosition;
//     public float playerSpeed;
//     public string playerAction;
//     public float successRate;
//     public Dictionary<string, object> contextData;
    
//     public AIDecision()
//     {
//         timestamp = Time.time;
//         contextData = new Dictionary<string, object>();
//     }
// }

// public class AdvancedAIBehavior : MonoBehaviour
// {
//     [Header("AI Configuration")]
//     public Transform player;
//     public float baseSpeed = 3f;
//     public float detectionRange = 15f;
//     public float[] laneXPositions = { -5f, 0f, 5f };
//     public bool enableMachineLearning = true;
//     public bool enableBehaviorPrediction = true;
    
//     [Header("Behavior Patterns")]
//     public AIBehaviorPattern aggressivePattern;
//     public AIBehaviorPattern defensivePattern;
//     public AIBehaviorPattern adaptivePattern;
//     public AIBehaviorPattern currentPattern;
    
//     [Header("Prediction Settings")]
//     public int maxDecisionHistory = 50;
//     public float predictionAccuracy = 0.75f;
//     public float learningRate = 0.1f;
//     public float adaptationThreshold = 0.3f;
    
//     // AI State Management
//     private enum AIState { Learning, Pursuing, Intercepting, Ambushing, Retreating }
//     private AIState currentState = AIState.Learning;
//     private AIState previousState;
    
//     // Behavior Prediction Variables
//     private Queue<Vector3> playerPositionHistory;
//     private Queue<float> playerSpeedHistory;
//     private Queue<string> playerActionHistory;
//     private const int maxHistorySize = 20;
    
//     // Machine Learning Variables
//     private Dictionary<string, float> playerPatternWeights;
//     private List<float> predictionSuccessRates;
//     private float currentPredictionAccuracy = 0.5f;
    
//     // Decision Making
//     private float lastDecisionTime;
//     private float decisionInterval = 0.5f;
//     private Vector3 predictedPlayerPosition;
//     private string predictedPlayerAction;
//     private float predictionConfidence;
    
//     // Performance Metrics
//     private int successfulInterceptions = 0;
//     private int totalInterceptionAttempts = 0;
//     private int correctPredictions = 0;
//     private int totalPredictions = 0;
    
//     // Real-time adaptation
//     private float difficultyMultiplier = 1f;
//     private bool isPlayerStruggling = false;
//     [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used for future difficulty adjustment features")]
//     private int recentPlayerHits = 0;
//     private float playerPerformanceWindow = 10f;
    
//     private void Start()
//     {
//         InitializeAI();
//         if (player == null)
//             player = FindObjectOfType<PlayerMovement>()?.transform;
//     }
    
//     private void InitializeAI()
//     {
//         // Initialize behavior patterns
//         aggressivePattern = new AIBehaviorPattern
//         {
//             patternName = "Aggressive",
//             aggressiveness = 0.8f,
//             predictiveness = 0.9f,
//             adaptability = 0.6f
//         };
        
//         defensivePattern = new AIBehaviorPattern
//         {
//             patternName = "Defensive",
//             aggressiveness = 0.3f,
//             predictiveness = 0.5f,
//             adaptability = 0.8f
//         };
        
//         adaptivePattern = new AIBehaviorPattern
//         {
//             patternName = "Adaptive",
//             aggressiveness = 0.6f,
//             predictiveness = 0.7f,
//             adaptability = 0.9f
//         };
        
//         currentPattern = adaptivePattern;
        
//         // Initialize history queues
//         playerPositionHistory = new Queue<Vector3>();
//         playerSpeedHistory = new Queue<float>();
//         playerActionHistory = new Queue<string>();
        
//         // Initialize pattern weights
//         playerPatternWeights = new Dictionary<string, float>
//         {
//             {"prefers_left_lane", 0.33f},
//             {"prefers_center_lane", 0.33f},
//             {"prefers_right_lane", 0.33f},
//             {"quick_reactions", 0.5f},
//             {"predictable_patterns", 0.5f},
//             {"adapts_quickly", 0.5f}
//         };
        
//         predictionSuccessRates = new List<float>();
        
//         Debug.Log("Advanced AI Behavior initialized");
//     }
    
//     private void Update()
//     {
//         if (player == null) return;
        
//         UpdatePlayerHistory();
        
//         if (Time.time - lastDecisionTime >= decisionInterval)
//         {
//             MakeAIDecision();
//             lastDecisionTime = Time.time;
//         }
        
//         ExecuteCurrentBehavior();
        
//         if (enableBehaviorPrediction)
//         {
//             UpdateBehaviorPrediction();
//         }
//     }
    
//     private void UpdateBehaviorPrediction()
//     {
//         // This method updates the AI's behavior prediction in real-time
//         if (playerPositionHistory.Count < 5) return;
        
//         var prediction = PredictPlayerBehavior();
//         predictedPlayerPosition = prediction.position;
//         predictedPlayerAction = prediction.action;
//         predictionConfidence = prediction.confidence;
        
//         // Log prediction accuracy over time
//         if (enableMachineLearning && totalPredictions > 0)
//         {
//             float currentAccuracy = (float)correctPredictions / totalPredictions;
//             if (Mathf.Abs(currentAccuracy - currentPredictionAccuracy) > 0.05f)
//             {
//                 currentPredictionAccuracy = currentAccuracy;
//                 Debug.Log($"AI Prediction accuracy updated: {currentAccuracy:P1}");
//             }
//         }
//     }
    
//     private void UpdatePlayerHistory()
//     {
//         // Update position history
//         playerPositionHistory.Enqueue(player.position);
//         if (playerPositionHistory.Count > maxHistorySize)
//             playerPositionHistory.Dequeue();
            
//         // Update speed history
//         if (playerPositionHistory.Count >= 2)
//         {
//             Vector3[] positions = playerPositionHistory.ToArray();
//             float speed = Vector3.Distance(positions[positions.Length-1], positions[positions.Length-2]) / Time.deltaTime;
//             playerSpeedHistory.Enqueue(speed);
            
//             if (playerSpeedHistory.Count > maxHistorySize)
//                 playerSpeedHistory.Dequeue();
//         }
        
//         // Detect player action
//         string currentAction = DetectPlayerAction();
//         if (!string.IsNullOrEmpty(currentAction))
//         {
//             playerActionHistory.Enqueue(currentAction);
//             if (playerActionHistory.Count > maxHistorySize)
//                 playerActionHistory.Dequeue();
//         }
//     }
    
//     private string DetectPlayerAction()
//     {
//         if (playerPositionHistory.Count < 2) return null;
        
//         Vector3[] recentPositions = playerPositionHistory.TakeLast(2).ToArray();
//         Vector3 movement = recentPositions[1] - recentPositions[0];
        
//         // Detect lane changes
//         if (Mathf.Abs(movement.x) > 0.5f)
//         {
//             return movement.x > 0 ? "move_right" : "move_left";
//         }
        
//         // Detect jumping (Y movement)
//         if (movement.y > 0.3f)
//         {
//             return "jump";
//         }
        
//         // Detect sliding (staying low)
//         if (recentPositions[1].y < 1f)
//         {
//             return "slide";
//         }
        
//         return "forward";
//     }
    
//     private void MakeAIDecision()
//     {
//         // Predict player's next move
//         if (enableBehaviorPrediction)
//         {
//             var prediction = PredictPlayerBehavior();
//             predictedPlayerPosition = prediction.position;
//             predictedPlayerAction = prediction.action;
//             predictionConfidence = prediction.confidence;
//         }
//         else
//         {
//             predictedPlayerPosition = player.position;
//             predictedPlayerAction = "forward";
//             predictionConfidence = 0.5f;
//         }
        
//         // Determine AI state based on prediction and current pattern
//         DetermineAIState();
        
//         // Create decision record
//         var decision = new AIDecision
//         {
//             decisionType = currentState.ToString(),
//             aiPosition = transform.position,
//             playerPosition = player.position,
//             playerSpeed = playerSpeedHistory.Count > 0 ? playerSpeedHistory.Average() : 0f,
//             playerAction = predictedPlayerAction
//         };
        
//         decision.contextData["prediction_confidence"] = predictionConfidence;
//         decision.contextData["difficulty_multiplier"] = difficultyMultiplier;
//         decision.contextData["player_struggling"] = isPlayerStruggling;
        
//         currentPattern.recentDecisions.Add(decision);
        
//         // Maintain decision history limit
//         if (currentPattern.recentDecisions.Count > maxDecisionHistory)
//             currentPattern.recentDecisions.RemoveAt(0);
            
//         // Log decision for analytics
//         if (PlayerAnalyticsManager.Instance != null)
//         {
//             PlayerAnalyticsManager.Instance.LogGameEvent("ai_decision", transform.position, 
//                 new Dictionary<string, object>
//                 {
//                     {"ai_state", currentState.ToString()},
//                     {"predicted_action", predictedPlayerAction},
//                     {"confidence", predictionConfidence},
//                     {"pattern", currentPattern.patternName}
//                 });
//         }
//     }
    
//     private (Vector3 position, string action, float confidence) PredictPlayerBehavior()
//     {
//         if (playerPositionHistory.Count < 5)
//             return (player.position, "forward", 0.1f);
            
//         Vector3 predictedPos = player.position;
//         string predictedAction = "forward";
//         float confidence = 0.5f;
        
//         // Analyze movement patterns
//         Vector3[] positions = playerPositionHistory.ToArray();
//         float[] speeds = playerSpeedHistory.ToArray();
//         string[] actions = playerActionHistory.ToArray();
        
//         // Predict next position based on movement trend
//         if (positions.Length >= 3)
//         {
//             Vector3 velocity = (positions[positions.Length-1] - positions[positions.Length-2]) / Time.deltaTime;
//             Vector3 acceleration = velocity - ((positions[positions.Length-2] - positions[positions.Length-3]) / Time.deltaTime);
            
//             // Predict position 1 second ahead
//             predictedPos = positions[positions.Length-1] + velocity * 1f + 0.5f * acceleration * 1f * 1f;
            
//             // Clamp to valid lanes
//             float targetLaneX = GetClosestLaneX(predictedPos.x);
//             predictedPos.x = targetLaneX;
//         }
        
//         // Predict next action based on pattern analysis
//         if (actions.Length >= 3)
//         {
//             // Look for patterns in recent actions
//             var actionCounts = new Dictionary<string, int>();
//             foreach (string action in actions)
//             {
//                 actionCounts[action] = actionCounts.ContainsKey(action) ? actionCounts[action] + 1 : 1;
//             }
            
//             // Find most common action
//             predictedAction = actionCounts.OrderByDescending(kvp => kvp.Value).First().Key;
            
//             // Calculate confidence based on pattern consistency
//             confidence = (float)actionCounts[predictedAction] / actions.Length;
//         }
        
//         // Apply machine learning adjustments
//         if (enableMachineLearning)
//         {
//             ApplyLearningAdjustments(ref predictedPos, ref predictedAction, ref confidence);
//         }
        
//         return (predictedPos, predictedAction, confidence);
//     }
    
//     private void ApplyLearningAdjustments(ref Vector3 predictedPos, ref string predictedAction, ref float confidence)
//     {
//         // Adjust prediction based on learned player patterns
//         foreach (var weight in playerPatternWeights)
//         {
//             switch (weight.Key)
//             {
//                 case "prefers_left_lane":
//                     if (weight.Value > 0.6f)
//                         predictedPos.x = Mathf.Lerp(predictedPos.x, laneXPositions[0], weight.Value * 0.3f);
//                     break;
//                 case "prefers_right_lane":
//                     if (weight.Value > 0.6f)
//                         predictedPos.x = Mathf.Lerp(predictedPos.x, laneXPositions[2], weight.Value * 0.3f);
//                     break;
//                 case "quick_reactions":
//                     if (weight.Value > 0.7f)
//                         confidence *= 0.8f; // Less predictable
//                     break;
//                 case "predictable_patterns":
//                     if (weight.Value > 0.7f)
//                         confidence *= 1.2f; // More predictable
//                     break;
//             }
//         }
        
//         // Adjust based on recent prediction success
//         confidence *= currentPredictionAccuracy;
//     }
    
//     private void DetermineAIState()
//     {
//         float distanceToPlayer = Vector3.Distance(transform.position, player.position);
//         previousState = currentState;
        
//         // State transition logic based on prediction and current pattern
//         if (distanceToPlayer > detectionRange)
//         {
//             currentState = AIState.Learning;
//         }
//         else if (predictionConfidence > 0.8f && currentPattern.aggressiveness > 0.7f)
//         {
//             currentState = AIState.Intercepting;
//         }
//         else if (predictionConfidence > 0.6f && currentPattern.predictiveness > 0.6f)
//         {
//             currentState = AIState.Pursuing;
//         }
//         else if (isPlayerStruggling && currentPattern.adaptability > 0.7f)
//         {
//             currentState = AIState.Retreating; // Give player a break
//         }
//         else
//         {
//             currentState = AIState.Ambushing;
//         }
        
//         // Adapt behavior pattern based on player performance
//         AdaptBehaviorPattern();
//     }
    
//     private void AdaptBehaviorPattern()
//     {
//         // Monitor player performance
//         if (PlayerAnalyticsManager.Instance?.CurrentSession != null)
//         {
//             var session = PlayerAnalyticsManager.Instance.CurrentSession;
//             float hitRate = (float)session.totalObstaclesHit / Mathf.Max(1f, Time.time - session.sessionStart.Ticks);
            
//             isPlayerStruggling = hitRate > 0.5f; // Hitting obstacles frequently
            
//             // Adjust difficulty multiplier
//             if (isPlayerStruggling)
//             {
//                 difficultyMultiplier = Mathf.Lerp(difficultyMultiplier, 0.7f, Time.deltaTime * 0.5f);
//                 currentPattern = defensivePattern; // Switch to easier pattern
//             }
//             else if (hitRate < 0.1f) // Player is doing well
//             {
//                 difficultyMultiplier = Mathf.Lerp(difficultyMultiplier, 1.3f, Time.deltaTime * 0.3f);
//                 currentPattern = aggressivePattern; // Switch to harder pattern
//             }
//             else
//             {
//                 difficultyMultiplier = Mathf.Lerp(difficultyMultiplier, 1f, Time.deltaTime * 0.2f);
//                 currentPattern = adaptivePattern; // Balanced pattern
//             }
//         }
        
//         // Update player pattern weights based on observations
//         UpdatePlayerPatternWeights();
//     }
    
//     private void UpdatePlayerPatternWeights()
//     {
//         if (playerPositionHistory.Count < 10) return;
        
//         Vector3[] recentPositions = playerPositionHistory.ToArray();
        
//         // Analyze lane preferences
//         float leftTime = 0, centerTime = 0, rightTime = 0;
//         foreach (Vector3 pos in recentPositions)
//         {
//             if (pos.x < -1f) leftTime++;
//             else if (pos.x > 1f) rightTime++;
//             else centerTime++;
//         }
        
//         float total = recentPositions.Length;
//         playerPatternWeights["prefers_left_lane"] = leftTime / total;
//         playerPatternWeights["prefers_center_lane"] = centerTime / total;
//         playerPatternWeights["prefers_right_lane"] = rightTime / total;
        
//         // Analyze reaction time patterns
//         if (playerActionHistory.Count >= 5)
//         {
//             string[] actions = playerActionHistory.ToArray();
//             int patternChanges = 0;
//             for (int i = 1; i < actions.Length; i++)
//             {
//                 if (actions[i] != actions[i-1]) patternChanges++;
//             }
            
//             float changeRate = (float)patternChanges / actions.Length;
//             playerPatternWeights["predictable_patterns"] = 1f - changeRate;
//             playerPatternWeights["adapts_quickly"] = changeRate;
//         }
//     }
    
//     private void ExecuteCurrentBehavior()
//     {
//         Vector3 targetPosition = transform.position;
//         float moveSpeed = baseSpeed * difficultyMultiplier;
        
//         switch (currentState)
//         {
//             case AIState.Learning:
//                 // Observe player without being too aggressive
//                 targetPosition = new Vector3(transform.position.x, transform.position.y, 
//                                            player.position.z + 5f);
//                 moveSpeed *= 0.7f;
//                 break;
                
//             case AIState.Pursuing:
//                 // Move toward player's current position
//                 targetPosition = new Vector3(GetClosestLaneX(player.position.x), 
//                                            transform.position.y, player.position.z);
//                 break;
                
//             case AIState.Intercepting:
//                 // Move toward predicted position
//                 targetPosition = new Vector3(GetClosestLaneX(predictedPlayerPosition.x), 
//                                            transform.position.y, predictedPlayerPosition.z);
//                 moveSpeed *= 1.2f;
//                 break;
                
//             case AIState.Ambushing:
//                 // Position strategically based on prediction
//                 float ambushLane = GetAmbushLane();
//                 targetPosition = new Vector3(ambushLane, transform.position.y, 
//                                            player.position.z + UnityEngine.Random.Range(2f, 5f));
//                 break;
                
//             case AIState.Retreating:
//                 // Move away from player to give them space
//                 targetPosition = new Vector3(transform.position.x, transform.position.y, 
//                                            player.position.z - 3f);
//                 moveSpeed *= 0.5f;
//                 break;
//         }
        
//         // Smooth movement toward target
//         Vector3 direction = (targetPosition - transform.position).normalized;
//         direction.y = 0; // Keep AI on ground level
        
//         transform.position += direction * moveSpeed * Time.deltaTime;
        
//         // Look toward target
//         if (direction != Vector3.zero)
//         {
//             transform.LookAt(transform.position + direction);
//         }
//     }
    
//     private float GetAmbushLane()
//     {
//         // Choose lane based on prediction and player patterns
//         if (predictedPlayerAction == "move_left")
//             return laneXPositions[0]; // Left lane
//         else if (predictedPlayerAction == "move_right")
//             return laneXPositions[2]; // Right lane
//         else
//         {
//             // Choose based on player's least preferred lane
//             float minPreference = Mathf.Min(
//                 playerPatternWeights["prefers_left_lane"],
//                 playerPatternWeights["prefers_center_lane"],
//                 playerPatternWeights["prefers_right_lane"]
//             );
            
//             if (minPreference == playerPatternWeights["prefers_left_lane"])
//                 return laneXPositions[0];
//             else if (minPreference == playerPatternWeights["prefers_right_lane"])
//                 return laneXPositions[2];
//             else
//                 return laneXPositions[1];
//         }
//     }
    
//     private float GetClosestLaneX(float xPosition)
//     {
//         float closest = laneXPositions[0];
//         float minDistance = Mathf.Abs(xPosition - laneXPositions[0]);
        
//         for (int i = 1; i < laneXPositions.Length; i++)
//         {
//             float distance = Mathf.Abs(xPosition - laneXPositions[i]);
//             if (distance < minDistance)
//             {
//                 minDistance = distance;
//                 closest = laneXPositions[i];
//             }
//         }
        
//         return closest;
//     }
    
//     // Validation methods to track AI performance
//     public void ValidatePrediction(bool wasCorrect)
//     {
//         totalPredictions++;
//         if (wasCorrect) correctPredictions++;
        
//         currentPredictionAccuracy = (float)correctPredictions / totalPredictions;
//         predictionSuccessRates.Add(wasCorrect ? 1f : 0f);
        
//         // Keep only recent success rates
//         if (predictionSuccessRates.Count > 20)
//             predictionSuccessRates.RemoveAt(0);
            
//         // Update learning rate based on success
//         if (currentPredictionAccuracy > 0.8f)
//             learningRate = Mathf.Max(0.05f, learningRate * 0.95f); // Reduce learning rate when doing well
//         else if (currentPredictionAccuracy < 0.4f)
//             learningRate = Mathf.Min(0.2f, learningRate * 1.1f); // Increase learning rate when struggling
//     }
    
//     public void RecordInterceptionAttempt(bool successful)
//     {
//         totalInterceptionAttempts++;
//         if (successful) successfulInterceptions++;
        
//         // Update AI performance metrics for analytics
//         if (PlayerAnalyticsManager.Instance != null)
//         {
//             PlayerAnalyticsManager.Instance.LogGameEvent("ai_interception", transform.position, 
//                 new Dictionary<string, object>
                // {
//                     {"successful", successful},
//                     {"success_rate", (float)successfulInterceptions / totalInterceptionAttempts},
//                     {"prediction_accuracy", currentPredictionAccuracy}
//                 });
//         }
//     }
    
//     // Public methods for external integration
//     public float GetCurrentDifficulty() => difficultyMultiplier;
//     public string GetCurrentAIState() => currentState.ToString();
//     public float GetPredictionAccuracy() => currentPredictionAccuracy;
//     public Dictionary<string, float> GetPlayerPatternWeights() 
//     {
//         if (playerPatternWeights == null)
//             return new Dictionary<string, float>();
//         return new Dictionary<string, float>(playerPatternWeights);
//     }
// }
