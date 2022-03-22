using UnityEngine;
using Unity.Entities;
using Unity.Physics;

public enum QueryMode
{
    ClosestHit,
    AllDistanceHits,
    OtherAllHits,
    OtherClosestHit
}

public struct DOTSQueryDisplayerData : IComponentData
{
    public float Distance;
    public QueryMode QueryMode;
    public bool DrawSurfaceNormal;
    public bool distanceHitValid;
    public DistanceHit distanceHit;
}

public class DOTSQueryDisplayerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public float Distance = 10.0f;
    public QueryMode QueryMode;
    public bool DrawSurfaceNormal = true;
    public DOTSQueryDisplayer dOTSQueryDisplayer;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dOTSQueryDisplayer.hitDataEntity = entity;

        dstManager.AddComponentData(entity, new DOTSQueryDisplayerData
        {
            Distance = Distance,
            QueryMode = QueryMode,
            DrawSurfaceNormal = DrawSurfaceNormal,
            distanceHitValid = false,
            distanceHit = new DistanceHit()
        });
    }
}
