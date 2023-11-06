using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VFXData", menuName = "ScriptableObjects/VFXData", order = 1)]
public class VFXData : ScriptableObject
{
    [SerializeField] private List<InGameVFXData> inGameVfxData;

    public Dictionary<string, InGameVFXData> data = null;
    public Dictionary<string, InGameVFXData> Data {
        get
        {
            if (data == null)
            {
                data = new Dictionary<string, InGameVFXData>();
                foreach (var item in inGameVfxData)
                {
                    data.Add(item.id, item);
                }
            }

            return data;
        }
    }

    public void SpawnVFX(string id, Vector3 position, float destroyAfterSeconds)
    {
        if (Data.ContainsKey(id))
        {
            GameObject spawnedParticle = Instantiate(Data[id].particleEffect, position, Quaternion.identity);
            Destroy(spawnedParticle, destroyAfterSeconds);
        }
    }
}

[System.Serializable]
public struct InGameVFXData
{
    public string id;
    public GameObject particleEffect;
}

