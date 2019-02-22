using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BeatSaberTweaks;
using CustomUI.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VolumeTweaks
{
    class VolumeTweaks : MonoBehaviour
    {
        public static VolumeTweaks Instance = null;


        public static void OnLoad()
        {
            if (Instance != null) return;
            new GameObject("Volume Tweaks").AddComponent<VolumeTweaks>();
        }

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            
                DontDestroyOnLoad(gameObject);

                Console.WriteLine("VolumeTweaks started.");

                PauseMenuTweaks.OnLoad(transform);
            }
            else
            {
                Destroy(this);
            }
        }
     
    }
}
