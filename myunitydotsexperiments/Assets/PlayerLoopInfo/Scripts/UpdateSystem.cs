using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class UpdateSystem01 : SystemBase
{
    protected override void OnCreate()
    {
        if (!UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("PlayerLoopInfo"))
            Enabled = false;
    }

    protected override void OnUpdate()
    {
        UnityEngine.Debug.Log("UpdateSystem01 SimulationSystemGroup");
    }
}

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial class UpdateSystem02 : SystemBase
{
    protected override void OnCreate()
    {
        if (!UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("PlayerLoopInfo"))
            Enabled = false;
    }

    protected override void OnUpdate()
    {
        UnityEngine.Debug.Log("UpdateSystem02 FixedStepSimulationSystemGroup");
    }
}

[UpdateInGroup(typeof(TransformSystemGroup))]
public partial class UpdateSystem03 : SystemBase
{
    protected override void OnCreate()
    {
        if (!UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("PlayerLoopInfo"))
            Enabled = false;
    }

    protected override void OnUpdate()
    {
        UnityEngine.Debug.Log("UpdateSystem03 TransformSystemGroup");
    }
}

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial class UpdateSystem04 : SystemBase
{
    protected override void OnCreate()
    {
        if (!UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("PlayerLoopInfo"))
            Enabled = false;
    }

    protected override void OnUpdate()
    {
        UnityEngine.Debug.Log("UpdateSystem04 LateSimulationSystemGroup");
    }
}

