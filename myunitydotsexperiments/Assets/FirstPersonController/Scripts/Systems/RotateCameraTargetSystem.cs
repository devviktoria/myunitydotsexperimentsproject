using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

public partial class RotateCameraTargetSystem : SystemBase
{
    protected unsafe override void OnUpdate()
    {
        Float2InputData float2InputData = GetSingleton<Float2InputData>();
        float deltaTime = Time.fixedDeltaTime;

        //UnityEngine.Debug.Log($"float2InputData.lookActionData: {float2InputData.lookActionData}");

        Dependency = Entities
            .WithName("RotateCameraTarget")
                .WithoutBurst()
                .ForEach((ref Rotation rotation,
                    ref DotsCameraTargetData dotsCameraTargetData) =>
                {
                    float verticalAngularRotation = -float2InputData.lookActionData.y * dotsCameraTargetData.verticalRotationSpeed * deltaTime;
                    //UnityEngine.Debug.Log($"verticalAngularRotation {verticalAngularRotation}");

                    float calculatedRotation = dotsCameraTargetData.currentVerticalAngle + verticalAngularRotation;
                    if (calculatedRotation > dotsCameraTargetData.maxVerticalAngle)
                    {
                        verticalAngularRotation = dotsCameraTargetData.maxVerticalAngle - dotsCameraTargetData.currentVerticalAngle;
                        dotsCameraTargetData.currentVerticalAngle = dotsCameraTargetData.maxVerticalAngle;
                    }
                    else if (calculatedRotation < dotsCameraTargetData.minVerticalAngle)
                    {
                        verticalAngularRotation = dotsCameraTargetData.minVerticalAngle - dotsCameraTargetData.currentVerticalAngle;
                        dotsCameraTargetData.currentVerticalAngle = dotsCameraTargetData.minVerticalAngle;
                    }
                    else
                    {
                        dotsCameraTargetData.currentVerticalAngle = calculatedRotation;
                    }

                    // if (math.abs(verticalAngularRotation) > float.Epsilon)
                    // {
                    //     UnityEngine.Debug.Log($"verticalAngularRotation {verticalAngularRotation} {dotsCameraTargetData.currentVerticalAngle}");
                    // }
                    quaternion rotationToAdd = quaternion.AxisAngle(math.right(), verticalAngularRotation);
                    quaternion currentRotation = rotation.Value;

                    rotation.Value = math.mul(currentRotation, rotationToAdd);

                }).Schedule(Dependency);
        Dependency.Complete();

        float2InputData.lookActionData = new float2(0.0f, 0.0f);

        SetSingleton<Float2InputData>(float2InputData);
    }
}
