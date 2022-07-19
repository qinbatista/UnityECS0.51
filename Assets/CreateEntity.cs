using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;

public class CreateEntity : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject gameObjectPrefab;
    [SerializeField] int xSize = 10;
    [SerializeField] int ySize = 10;
    [Range(0.1f, 2f)]
    [SerializeField] float spacing = 1f;
    Entity entityPrefab;
    World defaultWorld;
    EntityManager entityManager;
    void Start()
    {
        defaultWorld = World.DefaultGameObjectInjectionWorld;
        entityManager = defaultWorld.EntityManager;
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, null);
        entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObjectPrefab, settings);
        // InstantiateEntity(new float3(4f,0,4f));
        InstantiateEntityGrid(entityPrefab, xSize, ySize, spacing);
    }
    void InstantiateEntity(Entity entityPrefab, float3 position)
    {
        Entity entity = entityManager.Instantiate(entityPrefab);
        entityManager.SetComponentData(entity, new Translation { Value = position });
    }
    void InstantiateEntityGrid(Entity entityPrefab, int dimX, int dimY, float spacing = 1f)
    {
        for (int x = 0; x < dimX; x++)
        {
            for (int y = 0; y < dimY; y++)
            {
                InstantiateEntity(entityPrefab, new float3(x * spacing, 0, y * spacing));
            }
        }
    }
}
