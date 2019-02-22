using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using BeatSaberTweaks;
using CustomUI.BeatSaber;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ReflectionUtil = VolumeTweaks.Utilities.ReflectionUtil;

namespace VolumeTweaks
{
    public class PauseMenuTweaks : MonoBehaviour
    {
        public GameObject pauseMenuWindow;

        public static PauseMenuTweaks Instance;
        private PauseMenuManager pauseMenuManager;
        private Button continueButton;
        private TextMeshProUGUI vtwVolumeLabel;

        public static void OnLoad(Transform parent)
        {
            if (Instance != null) return;
            new GameObject("Pause Menu Tweaks").AddComponent<PauseMenuTweaks>().transform.parent = parent;
        }


        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this);
            }
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene)
        {
            try
            {
                if (SceneUtils.isGameScene(scene))
                {

                    pauseMenuManager = Resources.FindObjectsOfTypeAll<PauseMenuManager>().First();
                    if (pauseMenuManager != null)
                    {

                        var VolumeTweaksWindow = Instantiate(pauseMenuManager.transform.transform, pauseMenuManager.transform.parent.transform);
                        VolumeTweaksWindow.name = "VolumeTweaksWindow";
                        VolumeTweaksWindow.localPosition = new Vector3(-1.3f, 0, -0.6f);
                        VolumeTweaksWindow.Rotate(0, -45, 0);

                        var vtwCanvas = VolumeTweaksWindow.transform.Find("Wrapper").Find("UI").Find("Canvas");

                        var vtwHeadingTextObject = vtwCanvas.GetChild(0);
                        vtwHeadingTextObject.name = "vtwHeadingTextObject";
                        var vtwheading = vtwHeadingTextObject.GetComponent<TextMeshProUGUI>();
                        vtwheading.text = "Volume Tweaks";

                        var vtwDifficultyLabel = vtwCanvas.GetChild(1);
                  

                        var buttonsContainer = vtwCanvas.Find("Buttons");
                        var menubtn = buttonsContainer.GetChild(1);
                        var restartbtn = buttonsContainer.GetChild(0);
                        var continuebtn = buttonsContainer.GetChild(2);


                 
                        Destroy(menubtn.gameObject);

                        var oldDecreaseButton = restartbtn.GetComponent<Button>();
                        var decreaseButton = Instantiate(oldDecreaseButton, oldDecreaseButton.transform.parent);
                        decreaseButton.name = "vtwDecreaseButton";
                        decreaseButton.SetButtonText("Decrease Volume");
                        decreaseButton.onClick.AddListener(decrementVolume_Click);



                        var newLabel = Instantiate(vtwDifficultyLabel, buttonsContainer);
                        newLabel.name = "volumeLabel";
                        vtwVolumeLabel = newLabel.GetComponent<TextMeshProUGUI>();
                        vtwVolumeLabel.text = (Settings.NoteMissVolume * 100).ToString();


                        var oldincreaseButton = continuebtn.GetComponent<Button>();
                        var increaseButton = Instantiate(oldincreaseButton, oldincreaseButton.transform.parent);
                        increaseButton.name = "vtwIncreaseButton";
                        increaseButton.SetButtonText("Increase Volume");
                        increaseButton.onClick.AddListener(incrementVolume_Click);

                        Destroy(vtwDifficultyLabel.gameObject);
                        Destroy(oldincreaseButton.gameObject);
                        Destroy(oldDecreaseButton.gameObject);

                        NoteHitVolume.OnLoad(oldDecreaseButton.transform);
                     


                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Done fucked up: " + e);
            }
        }

        private void incrementVolume_Click()
        {
            Settings.NoteHitVolume = Settings.NoteHitVolume + 0.1f;
            Settings.NoteMissVolume = Settings.NoteMissVolume + 0.1f;

            if (Settings.NoteHitVolume > 1f)
                Settings.NoteHitVolume = 1f;

            if (Settings.NoteMissVolume > 1f)
                Settings.NoteMissVolume = 1f;
            vtwVolumeLabel.text = (Settings.NoteMissVolume * 100).ToString();
            NoteHitVolume.Instance.LoadingDidFinishEvent();
        }
        private void decrementVolume_Click()
        {
            Settings.NoteHitVolume = Settings.NoteHitVolume - 0.1f;
            Settings.NoteMissVolume = Settings.NoteMissVolume - 0.1f;

            if (Settings.NoteHitVolume < 0f)
                Settings.NoteHitVolume = 0f;

            if (Settings.NoteMissVolume < 0f)
                Settings.NoteMissVolume = 0f;

            vtwVolumeLabel.text = (Settings.NoteMissVolume * 100).ToString();
            NoteHitVolume.Instance.LoadingDidFinishEvent();
        }
    }
}
