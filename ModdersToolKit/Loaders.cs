using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using static ModdersToolKit.Main;
using UnityEngine;

namespace ModdersToolKit
{
    public class Loaders
    {
        public void LoadSound(string name, AudioType type, AudioClip[] reference)
        {
            MelonCoroutines.Start(this.LoadSoundCoroutine(name, type, reference, delegate (AudioClipData audioClipData, AudioClip[] yourNewArgument)
            {
                this.OnSoundLoaded(audioClipData, yourNewArgument);
            }));
        }

        private void OnSoundLoaded(AudioClipData audioClipData, AudioClip[] refr)
        {
            AudioClip clip = audioClipData.Clip;
            Array.Resize<AudioClip>(ref Main.moddedMusic, Main.moddedMusic.Length + 1);
            Main.moddedMusic[Main.moddedMusic.Length - 1] = clip;
        }

        private IEnumerator LoadSoundCoroutine(string name, AudioType type, AudioClip[] reference, Action<AudioClipData, AudioClip[]> onComplete)
        {
            string path = "file://" + Application.streamingAssetsPath + "/CustomSounds/" + name;
            WWW url = new WWW(path);
            yield return url;
            bool flag = string.IsNullOrEmpty(url.error);
            if (flag)
            {
                AudioClip loadedClip = url.GetAudioClip();
                loadedClip.name = name;
                MelonLogger.Msg("Loaded " + name + " as an audio clip");
                AudioClipData audioClipData = new AudioClipData(loadedClip, reference);
                if (onComplete != null)
                {
                    onComplete(audioClipData, reference);
                }
                loadedClip = null;
                audioClipData = null;
            }
            else
            {
                MelonLogger.Error("Error loading sound: " + url.error);
            }
            yield break;
        }
    }
}
