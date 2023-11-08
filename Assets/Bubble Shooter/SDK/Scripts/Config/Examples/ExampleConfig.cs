using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TestConfig", order = 1)]
    public class ExampleConfig : BaseKeyValueConfig<ExampleKeyValueData>
    {

    }

    [System.Serializable]
    public struct ExampleKeyValueData : IKeyValueConfigData
    {
        public string ID => testId;

        public string testId;
        public int testInt;
    }
}
