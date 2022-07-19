using System.ComponentModel;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class CreateSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float elapsedTime = (float)Time.ElapsedTime;
        Entities.ForEach((ref Translation trans, ref CreateComponent moveSpeed) =>
        {
            trans.Value.x = trans.Value.x + 0.01f;
            trans.Value.y = trans.Value.y + 0.01f;
            trans.Value.z = trans.Value.z + 0.01f;
        }).Schedule();
    }
}
