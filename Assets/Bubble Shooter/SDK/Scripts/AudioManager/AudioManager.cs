using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    public class AudioManager : SerializeSingleton<AudioManager>
    {
        [SerializeField] private List<AudioData> audioData;

        private Dictionary<string, AudioClip> AudioData;

        private void Awake()
        {
            AudioData = new Dictionary<string, AudioClip>();

            foreach (var item in audioData)
            {
                AudioData.Add(item.audioID, item.audioClip);
            }
        }

        public void PlayAudioClipWithAutoDestroy(string id)
        {
            if (AudioData.ContainsKey(id))
            {
                GameObject audio = new GameObject();
                audio.AddComponent<AudioSource>();
                audio.GetComponent<AudioSource>().PlayOneShot(AudioData[id]);

                float length = AudioData[id].length + 1;
                Destroy(audio, length);
            }
            else
            {
                Debug.LogError("[AudioManager] Mentioned soundid is not present");
            }
        }

        public void PlayAudioClip(string id, float destoryTime)
        {
            if (AudioData.ContainsKey(id))
            {
                GameObject audio = new GameObject();
                audio.AddComponent<AudioSource>();
                audio.GetComponent<AudioSource>().PlayOneShot(AudioData[id]);

                Destroy(audio, destoryTime);
            }
            else
            {
                Debug.LogError("[AudioManager] Mentioned soundid is not present");
            }
        }
    }

    [System.Serializable]
    public class AudioData
    {
        public string audioID;
        public AudioClip audioClip;
    }
}
