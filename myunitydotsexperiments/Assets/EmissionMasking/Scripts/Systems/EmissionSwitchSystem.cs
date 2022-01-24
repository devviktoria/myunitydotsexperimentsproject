using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;

public class EmissionSwitchSystem : SystemBase
{
    private EntityQuery m_MouseGroup;

    protected override void OnCreate()
    {
        m_MouseGroup = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[] { typeof(RequestEmissionSwitchData) }
        });
    }

    protected override void OnUpdate()
    {
        NativeArray<RequestEmissionSwitchData> requestEmissionSwitchDataArray =
            m_MouseGroup.ToComponentDataArray<RequestEmissionSwitchData>(Allocator.TempJob);
        bool topSwitchRequested = requestEmissionSwitchDataArray[0].topSwitchRequested;
        bool backSwitchRequested = requestEmissionSwitchDataArray[0].backSwitchRequested;
        bool leftSwitchRequested = requestEmissionSwitchDataArray[0].leftSwitchRequested;

        requestEmissionSwitchDataArray[0] = new RequestEmissionSwitchData
        {
            topSwitchRequested = false,
            backSwitchRequested = false,
            leftSwitchRequested = false
        };

        requestEmissionSwitchDataArray.Dispose();


        Entities.ForEach((
            ref TopEmissionFloatOverride topEmissionFloatOverride,
            ref BackEmissionFloatOverride backEmissionFloatOverride,
            ref LeftEmissionFloatOverride leftEmissionFloatOverride) =>
        {
            if (topSwitchRequested)
            {
                topEmissionFloatOverride.Value = 1 - topEmissionFloatOverride.Value;
            }

            if (backSwitchRequested)
            {
                backEmissionFloatOverride.Value = 1 - backEmissionFloatOverride.Value;
            }

            if (leftSwitchRequested)
            {
                leftEmissionFloatOverride.Value = 1 - leftEmissionFloatOverride.Value;
            }


        }).Schedule();
    }
}
