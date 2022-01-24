using Unity.Entities;

public class RequestEmissionSwitchSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((
            ref RequestEmissionSwitchData requestEmissionSwitchData,
            in RequestEmissionSwitchInputData requestEmissionSwitchInputData) =>
        {
            requestEmissionSwitchData.topSwitchRequested = UnityEngine.Input.GetKeyDown(requestEmissionSwitchInputData.topSwitchKeyCode);
            requestEmissionSwitchData.backSwitchRequested = UnityEngine.Input.GetKeyDown(requestEmissionSwitchInputData.backSwitchKeyCode);
            requestEmissionSwitchData.leftSwitchRequested = UnityEngine.Input.GetKeyDown(requestEmissionSwitchInputData.leftSwitchKeyCode);
        }).Run();
    }
}
