using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputProcessor : MonoBehaviour
{
    private EntityManager _entityManager;
    private EntityQuery _float2InputDataQuery;

    private Vector2 _movementInput = Vector2.zero;
    private Vector2 _lookInput = Vector2.zero;

    private void Awake()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _float2InputDataQuery = _entityManager.CreateEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[]
                {
                typeof(Float2InputData)
                }
        });
    }

    private void Update()
    {
        InputSystem.Update();
    }

    public void MovePlayerCharacter(InputAction.CallbackContext context)
    {
        //Debug.Log($"MovePlayerCharacter {context.ReadValue<Vector2>()}");
        _movementInput = context.ReadValue<Vector2>();
        _float2InputDataQuery.SetSingleton<Float2InputData>(new Float2InputData { moveActionData = _movementInput, lookActionData = _lookInput });
    }

    public void LookPlayerCharacter(InputAction.CallbackContext context)
    {
        //Debug.Log($"LookPlayerCharacter {context.ReadValue<Vector2>()} {context.phase}");
        if (context.phase == InputActionPhase.Performed)
        {
            _lookInput = context.ReadValue<Vector2>();
            _float2InputDataQuery.SetSingleton<Float2InputData>(new Float2InputData { moveActionData = _movementInput, lookActionData = _lookInput });
        }
    }
}
