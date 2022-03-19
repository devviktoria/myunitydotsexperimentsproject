using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial class InitSystem : SystemBase
{
    protected override void OnCreate()
    {
        if (!UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("PlayerLoopInfo"))
            Enabled = false;
    }

    protected override void OnUpdate()
    {
        UnityEngine.Debug.Log("InitSystem");
    }
}
