using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject setting;
    public GameObject creditScreen;
    public Scrollbar scrollbar_BGM;
    public Scrollbar scrollbar_SoundEffects;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OpenTitleScreen()
    {
        titleScreen.SetActive(true);
        setting.SetActive(false);
        creditScreen.SetActive(false);
    }
    public void CloseTitleScreen() 
    {
        titleScreen.SetActive(false);
        setting.SetActive(false);
        creditScreen.SetActive(false);
    }
    public void StartNewGame()
    {
        //might be good to just instantiate a game object for the actual game. 
    }
    public void StartExistingGame()
    {
        //download the player preference 
        //might be good to just instantiate a game object for the actual game. 
    }
    public void OpenSetting()
    {
        setting.SetActive(true);
    }
    public void CloseSetting()
    {
        setting.SetActive(false);
    }
    public void OpenCreditScreen()
    {
        creditScreen.SetActive(true);
    }
    public void CloseCreditScreen()
    {
        creditScreen.SetActive(false);
    }
    public void ChangeBGM_Volume()
    {
        float bgm_value = scrollbar_BGM.value;
        //modify the audioSource for BGM based on bgm_value;
    }
    public void ChangeSoundEffects_Volume()
    {
        float soundEffects_value = scrollbar_SoundEffects.value;
        //modify the audioSource for Sound Effect based on soundEffects_value;

    }
    public void ExitGame()
    {

    }
}
