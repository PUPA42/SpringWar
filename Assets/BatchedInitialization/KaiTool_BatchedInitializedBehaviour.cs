//#define Debug
using System.Collections;
using UnityEngine;
namespace KaiTool.BatchInitialization
{
    public abstract class KaiTool_BatchedInitializedBehaviour : MonoBehaviour
    {
        [Header("BatchedInitializedBehaviour")]
       // [HideInInspector]
        public bool isInitialized = false;
        public virtual void Init() {
            #region DebugInit
#if Debug
            print("Init");
#endif
#endregion
            isInitialized = true;
        }
        private void OnEnable()
        {
            if (!isInitialized) {
                KaiTool_BatchedInitializationManager.Instance.batchedBehavioutList.Add(this);
                if (!KaiTool_BatchedInitializationManager.Instance.IsStimulated) {
                    KaiTool_BatchedInitializationManager.Instance.TurnOn();
                }
            }
        }

    }
}