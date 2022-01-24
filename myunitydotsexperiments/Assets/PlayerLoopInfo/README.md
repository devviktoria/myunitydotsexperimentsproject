# Player Loop Experiment
I wondered if there are both MonoBehaviours and Systems how the player loop is structured. That is the reason why I created this scene.

## The Scripts folder
Contains the scipts used in this scene.
- AMonoBehaviourScript: should be put on a not converted entity. Logs the monobehaviour events.
- InitSystem: logs the InitializationSystemGroup in DOTS.
- UpdateSystem: contains systems that are logs some of the systems that are arunning in the SimulationSystemGroup in DOTS.
- PreLateUpdateSystem: logs the PresentationSystemGroup in DOTS

## Running the scene
I suggest start this scene with paused mode and the step a couple more frames and chack the logs.

## The conclusion
The player loop with DOTS and MonoBehaviour:
- MonoBehaviour.Awake (only once)
   - InitializationSystemGroup (updated at the end of the Initialization phase of the player loop) (runs more then once!!!!!)
        - BeginInitializationEntityCommandBufferSystem
        - CopyInitialTransformFromGameObjectSystem
        - SubSceneLiveLinkSystem
        - SubSceneStreamingSystem
        - EndInitializationEntityCommandBufferSystem
- MonoBehaviour.Start (only once)
- MonoBehaviour.FixedUpdate
- MonoBehaviour.Update
- SimulationSystemGroup (updated at the end of the Update phase of the player loop)
    - BeginSimulationEntityCommandBufferSystem
    - FixedStepSimulationSystemGroup
    - TransformSystemGroup
        - EndFrameParentSystem
        - CopyTransformFromGameObjectSystem
        - EndFrameTRSToLocalToWorldSystem
        - EndFrameTRSToLocalToParentSystem
        - EndFrameLocalToParentSystem
        - CopyTransformToGameObjectSystem
    - LateSimulationSystemGroup
    - EndSimulationEntityCommandBufferSystem
- MonoBehaviour.LateUpdate
- PresentationSystemGroup (updated at the end of the PreLateUpdate phase of the player loop)
    - BeginPresentationEntityCommandBufferSystem
    - CreateMissingRenderBoundsFromMeshRenderer
    - RenderingSystemBootstrap
    - RenderBoundsUpdateSystem
    - RenderMeshSystem
    - LODGroupSystemV1
    - LodRequirementsUpdateSystem
    - EndPresentationEntityCommandBufferSystem
