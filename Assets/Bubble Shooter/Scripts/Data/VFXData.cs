using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;

[CreateAssetMenu(fileName = "VFXData", menuName = "ScriptableObjects/VFXData", order = 1)]
public class VFXData : BaseKeyValueConfig<InGameVFXData>
{
    public static void SpawnVFX(string id, Vector3 position, float destroyAfterSeconds)
    {
        if (Data != null 
            && Data.ContainsKey(id))
        {
            GameObject spawnedParticle = Instantiate(Data[id].particleEffect, position, Quaternion.identity);
            Destroy(spawnedParticle, destroyAfterSeconds);
        }
    }
}

#region Data structures

[System.Serializable]
public struct InGameVFXData : IKeyValueConfigData
{
    public string ID => id;

    public string id;
    public GameObject particleEffect;
}

#endregion
