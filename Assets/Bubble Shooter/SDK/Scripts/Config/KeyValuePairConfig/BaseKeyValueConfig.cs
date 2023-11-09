using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    public class BaseKeyValueConfig<T> : BaseConfig where T: IKeyValueConfigData
    {
        [SerializeField]
        private List<T> data;

        public static Dictionary<string, T> Data;

        [ContextMenu("Refresh")]
        public override void Refresh()
        {
            Data = new Dictionary<string, T>();
            foreach (var item in data)
            {
                Data.Add(item.ID, item);
            }
        }
    }

    public interface IKeyValueConfigData
    {
        public string ID { get; }
    }
}