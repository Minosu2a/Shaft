using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    #region Fields
    [SerializeField] private Animator _fade = null;
    [SerializeField] private Transform _soundLocation = null;
    #endregion Fields
    #region Property
    #endregion Property


    #region Methods
    private void Awake()
    {
        UIManager.Instance.UIController = this;    
    }
    private void Start()
    {
         StartCoroutine(StartIntroDelay());


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
        FadeOut();
        yield return new WaitForSeconds(18f);

        yield return new WaitForSeconds(20f);
    }

    IEnumerator StartIntroDelay()
    {
        yield return new WaitForSeconds(40f);
        CharacterManager.Instance.CharacterController.GameStart();
        _fade.SetTrigger("FadeIn");
        AudioManager.Instance.Start2DSound("S_Wake");
    }
}
