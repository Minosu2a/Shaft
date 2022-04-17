using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    #region Fields
    [SerializeField] private Animator _fade = null;
    [SerializeField] private Yarn.Unity.DialogueRunner _dialogueRunner = null;
    [SerializeField] private TMP_Text _text = null;
    [SerializeField] private Image _pauseMenu = null;

    [SerializeField] private GameObject _skipingText = null;
    [SerializeField] private GameObject _dialogueCanvas = null;

    [SerializeField] private GameObject _shaft = null;
    [SerializeField] private GameObject _by = null;


    private bool _isInIntro = true;
    private bool _pauseMenuActive = false;
    private float _skipingIntroTimeStamp = 0f;
    #endregion Fields

    #region Properties
    public bool IsInIntro => _isInIntro;

    #endregion Properties


    #region Methods
    private void Awake()
    {
        UIManager.Instance.UIController = this;    
    }
    private void Start()
    {
         StartCoroutine(StartIntroDelay());


    }

    private void Update()
    {

        if (Input.GetButtonDown("Cancel") && _isInIntro == false)
        {
             if(_pauseMenuActive == true)
             {
                    _pauseMenu.gameObject.SetActive(false);
                    _pauseMenuActive = false;
                    //  Time.timeScale = 1;
                }
             else
             {
                    _pauseMenu.gameObject.SetActive(true);
                    _pauseMenuActive = true;
                   // Time.timeScale = 0;
             }
        }
        else if(Input.GetButton("Cancel") && _isInIntro == true)
        {
            _skipingIntroTimeStamp += Time.deltaTime;
            _skipingText.gameObject.SetActive(true);


            if(_skipingIntroTimeStamp >= 2f)
            {
                _skipingText.gameObject.SetActive(false);
                _dialogueRunner.Stop();
                _dialogueCanvas.SetActive(false);

                CharacterManager.Instance.CharacterController.GameStart();
                _fade.SetTrigger("FadeIn");
                AudioManager.Instance.Start2DSound("S_Wake");
                StopCoroutine(StartIntroDelay());
                _isInIntro = false;
                AudioManager.Instance.StopElevator();

                // _dialogueRunner.StartDialogue("Nothing");

                // AudioManager.Instance.StopSound(ESoundType.ONESHOT2D, "S_Elevator");
            }
        }

        if(Input.GetButtonUp("Cancel") && _isInIntro == true)
        {
            _skipingIntroTimeStamp = 0f;
            _skipingText.gameObject.SetActive(false);
        }

    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Testttt");
    }

    public void Sound()
    {
        AudioManager.Instance.Start2DSound("S_Voice");
    }
    public void TooglePause()
    {
        GameManager.Instance.TogglePause();
    }
    #endregion Methods

    public void FadeOut()
    {
        _fade.SetTrigger("FadeOut");
    }

    public void FadeIn()
    {
        _fade.SetTrigger("FadeIn");
    }

    public void EndDialogue()
    {
        StartCoroutine(EndDialogueDelay());
    }

    IEnumerator EndDialogueDelay()
    {
        AudioManager.Instance.StopMusic();

        FadeOut();
        yield return new WaitForSeconds(2f);
        AudioManager.Instance.Start2DSound("M_Surprise");
        yield return new WaitForSeconds(5f);
        AudioManager.Instance.PlayMusic("S_ElevatorEnding");
        yield return new WaitForSeconds(20f);
        AudioManager.Instance.StopAmbiantWithFadeOut(40f);
        _shaft.SetActive(true);
        _by.SetActive(true);
        _fade.SetTrigger("Ending");
        yield return new WaitForSeconds(30f);
        AudioManager.Instance.StopMusicWithFadeOut(20f);
        yield return new WaitForSeconds(65f);
        _dialogueRunner.StartDialogue("Still");
        _text.color = Color.black;
        yield return new WaitForSeconds(10f);
        Application.Quit();
    }

    IEnumerator StartIntroDelay()
    {
        yield return new WaitForSeconds(40f);
        CharacterManager.Instance.CharacterController.GameStart();
        _fade.SetTrigger("FadeIn");
        AudioManager.Instance.Start2DSound("S_Wake");
        _isInIntro = false;
        
    }
}
