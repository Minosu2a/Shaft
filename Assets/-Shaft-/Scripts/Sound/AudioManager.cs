using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{

    #region Fields
    [Header("Sound Datas")]
    [SerializeField] private SoundData[] _soundDatas = null;

    [Header("Sound & Music Sources")]
    [SerializeField] private AudioSource _mainSoundSource = null;
    [SerializeField] private AudioSource _3DSoundSource = null;

    [SerializeField] private AudioSource _musicSource = null;
    [SerializeField] private AudioSource _transitionSource = null;

    [SerializeField] private AudioSource _2DRepetitiveSoundSource = null;
    [SerializeField] private AudioSource _3DRepetitiveSoundSource = null;


    [Header("Volumes")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _soundsVolume = 1f;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _musicsVolume = 1f;

    private Dictionary<string, SoundData> _soundData = null;



    private Dictionary<string, AudioSource> _2DSources = null;
    private Dictionary<string, AudioSource> _2DRepetitiveSources = null;

    private Dictionary<string, AudioSource> _3DSources = null;
    private Dictionary<string, AudioSource> _3DRepetitiveSources = null;




    //Timer Réference/Attribut
    [Header("Default Timer Value")]
    [SerializeField] private float _fadeTick = 0.1f;

    //--- FADE IN
    private float _timeToFadeIn = 3f;

    private Timer _timerFadeInTick = null;
    private float _volumeFadeInTarget = 1f;
    private float _fadeInTickVolumeValue = 0;


    //--- FADE OUT
    private float _timeToFadeOut = 3f;

    private Timer _timerFadeOutTick = null;
    private float _volumeFadeOutTarget = 1f;
    private float _fadeOutTickVolumeValue = 0;

    //Can be used if main musics are used several time, to avoid confusion or error. If used new fonction for those main music has to be created
    // [Header("Music Names")]                                       
    // [SerializeField] private string _mainMenuTheme = null;
    #endregion Fields

    #region Property

    public float SoundsVolume
    {
        get
        {
            return _soundsVolume;
        }
        set
        {
            _soundsVolume = Mathf.Clamp(value, 0, 1);
            MainSoundVolumeUpdate();
        }
    }

    public float MusicsVolume
    {
        get
        {
            return _musicsVolume;
        }
        set
        {
            _musicsVolume = Mathf.Clamp(value, 0, 1);
            MusicVolumeUpdate();
        }
    }


    #endregion Property

    #region Methods



    #region Start
    public void Initialize()
    {

        _soundData = new Dictionary<string, SoundData>();

        if (_soundDatas.Length >= 1)
        {
            for (int i = 0; i < _soundDatas.Length; i++)
            {
                _soundData.Add(_soundDatas[i].Key, _soundDatas[i]);
            }

            _2DSources = new Dictionary<string, AudioSource>();
            _2DRepetitiveSources = new Dictionary<string, AudioSource>();

            _3DSources = new Dictionary<string, AudioSource>();
            _3DRepetitiveSources = new Dictionary<string, AudioSource>();


            _timerFadeInTick = new Timer();
            _timerFadeInTick.OnTick += FadeInTick;

            _timerFadeOutTick = new Timer();
            _timerFadeOutTick.OnTick += FadeOutTick;

        }


    }
    #endregion Start

    #region Volume Manager
    private void MainSoundVolumeUpdate()
    {
        _mainSoundSource.volume = _soundsVolume;
        _3DSoundSource.volume = _soundsVolume;
        _2DRepetitiveSoundSource.volume = _soundsVolume;
        _3DRepetitiveSoundSource.volume = _soundsVolume;
    }

    private void MusicVolumeUpdate()
    {
        _musicSource.volume = _musicsVolume;
        _transitionSource.volume = _musicsVolume;
    }
    #endregion Volume Manager


    #region Music
    public void PlayMusic(string key)
    {

        if (_soundData.ContainsKey(key) == false)
        {
            Debug.LogError("Fnct Play Music : Specified key not found for the audio file");
        }
        else
        {

            if (_soundData[key].Loop == false)   //Verify if the Soundata is set to loop, if it's not the case use PlaySoundOneShot instead
            {
                Debug.LogWarning("Fnct PlayMusic : The Soundata is not set as loop");
            }
            PlaySound(_musicSource, key);
        }

    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    public void PlayMusicWithFadeIn(string key, float speed)
    {

        if (_soundData.ContainsKey(key) == false)
        {
            Debug.LogError("Fnct Play Music with Fade In : Specified key not found for the audio file");
        }
        else
        {

            if (_soundData[key].Loop == false)   //Verify if the Soundata is set to loop, if it's not the case use PlaySoundOneShot instead
            {
                Debug.LogWarning("Fnct PlayMusicWithFadeIn : The Soundata is not set as loop");
            }

            _timeToFadeIn = speed;

            float numberofTick = _timeToFadeIn / _fadeTick;  //Calcul du nombre de tick en fonction de speed et la valeur fadeTick

            _fadeInTickVolumeValue = (_soundData[key].Volume * _musicsVolume) / numberofTick;  //Calcul du volume à incrémenter à chaque tick
            _volumeFadeInTarget = _soundData[key].Volume * _musicsVolume; //Calcul du volume que la source va avoir (Valeur Max)

            PlaySound(_musicSource, key);

            _musicSource.volume = 0; //A faire plus propre (comment?)

            _timerFadeInTick.StartTimer(_fadeTick);
        }


    }


    private void FadeInTick()
    {
        if (_musicSource.volume >= _volumeFadeInTarget)
        {
            _timerFadeInTick.StopTimer();
        }
        else
        {
            _musicSource.volume += _fadeInTickVolumeValue;

        }
    }


    public void StopMusicWithFadeOut(float speed)
    {

        if (!_musicSource.isPlaying)
        {
            Debug.LogWarning("Fnct Stop Music with Fade Out : There is no Audio playing to fade Out");
        }
        else
        {

            SwitchAudioSource(_musicSource, _transitionSource); //Switch du clip vers l'audio source transition

            _musicSource.Stop();


            _timeToFadeOut = speed;

            float numberofTick = _timeToFadeOut / _fadeTick;  //Calcul du nombre de tick en fonction de speed et la valeur fadeTick

            _fadeOutTickVolumeValue = _transitionSource.volume / numberofTick;  //Calcul du volume à incrémenter à chaque tick
            _volumeFadeOutTarget = 0; //Calcul du volume que la source va avoir (Valeur Max)

            _transitionSource.Play();

            _timerFadeOutTick.StartTimer(_fadeTick);
        }

    }
    private void SwitchAudioSource(AudioSource audioToChange, AudioSource audioTarget)
    {
        AudioClip clipToSwitch = audioToChange.clip;

        audioTarget.volume = audioToChange.volume;

        audioTarget.pitch = audioToChange.pitch;

        audioTarget.clip = clipToSwitch;

        audioTarget.time = audioToChange.time; //Démare la musique sur une nouvelle source âu même moment T que sur l'audio à changer
    }

    private void FadeOutTick()
    {
        if (_transitionSource.volume <= _volumeFadeOutTarget)
        {
            _timerFadeOutTick.StopTimer();
            _transitionSource.Stop();
        }
        else
        {
            _transitionSource.volume -= _fadeOutTickVolumeValue;

        }
    }


    public void SwitchMusicTransition(string key, float fadeInSpeed, float fadeOutSpeed)
    {

        if (_soundData.ContainsKey(key) == false)
        {
            Debug.LogError("Fnct Switch Music : Specified key not found for the audio file");

        }
        else
        {

            if (!_musicSource.isPlaying)
            {
                Debug.LogWarning("Fnct Switch Music : There is no Audio already playing to fade Out");
            }


            #region Audio Source Switch + Fade Out

            SwitchAudioSource(_musicSource, _transitionSource); //Switch du clip vers l'audio source transition


            _transitionSource.Play();

            #endregion Audio Source Switch



            #region Fade Out

            _timeToFadeOut = fadeOutSpeed;

            float numberofTickFadeOut = _timeToFadeOut / _fadeTick;  //Calcul du nombre de tick en fonction de speed et la valeur fadeTick

            _fadeOutTickVolumeValue = _transitionSource.volume / numberofTickFadeOut;  //Calcul du volume à incrémenter à chaque tick
            _volumeFadeOutTarget = 0; //Calcul du volume que la source va avoir (Valeur Max)

            _transitionSource.Play();

            _timerFadeOutTick.StartTimer(_fadeTick);

            #endregion Fade Out



            #region New Music Play + Fade In



            _timeToFadeIn = fadeInSpeed;

            float numberofTickFadeIn = _timeToFadeIn / _fadeTick;  //Calcul du nombre de tick en fonction de speed et la valeur fadeTick

            _fadeInTickVolumeValue = (_soundData[key].Volume * _musicsVolume) / numberofTickFadeIn;  //Calcul du volume à incrémenter à chaque tick
            _volumeFadeInTarget = _soundData[key].Volume * _musicsVolume; //Calcul du volume que la source va avoir (Valeur Max)

            PlaySound(_musicSource, key);

            _musicSource.volume = 0; //A faire plus propre (comment?)

            _timerFadeInTick.StartTimer(_fadeTick);

            #endregion New Music Play + Fade In

        }


    }
    #endregion Music




    #region 2DSound

    public void Start2DSound(string key)
    {
        if (_soundData.ContainsKey(key) == false)   //Verify if the Soundata is set to loop, if it's not the case use PlaySoundOneShot instead
        {
            Debug.LogError("Fnct StartSound2D : Specified key not found for the audio file");
        }
        else if (_soundData[key].Loop == true) //Verify if the Soundata is set to loop, if it's not the case use PlaySoundOneShot instead
        {
            AudioSource repSource2D = Instantiate(_2DRepetitiveSoundSource, transform);
            _2DRepetitiveSources.Add(key, repSource2D);

            PlaySound(repSource2D, key);
        }
        else
        {
            AudioSource oneShotSource2D = Instantiate(_mainSoundSource, transform);
            //_2DSources.Add(key, oneShotSource2D);

            PlaySound(oneShotSource2D, key);
        }
    }

    #endregion 2DSound



    #region 3DSound
    public void Start3DSound(string key, Transform position)
    {

        if (_soundData.ContainsKey(key) == false)   //Verify if the Soundata is set to loop, if it's not the case use PlaySoundOneShot instead
        {
            Debug.LogError("Fnct Start3DSound : Specified key not found for the audio file");
        }
        else if (_soundData[key].Loop == true)
        {
            AudioSource repSource3D = Instantiate(_3DRepetitiveSoundSource, position);
            _3DRepetitiveSources.Add(key, repSource3D);

            PlaySound(repSource3D, key);
        }
        else
        {
            AudioSource oneShotSource3D = Instantiate(_3DSoundSource, position);
           // _3DSources.Add(key, oneShotSource3D);

            PlaySound(oneShotSource3D, key);
        }
    }
    #endregion 3DSound



    #region Common Sounds

    private void PlaySound(AudioSource source, string key)
    {

        AudioClip clipToPlay = _soundData[key].Clip;

        source.volume = (_soundData[key].Volume * _soundsVolume);

        source.pitch = _soundData[key].Pitch;

        source.loop = _soundData[key].Loop;

        source.clip = clipToPlay;

        source.Play();
    }

    public void StopSound(ESoundType soundType, string audioSourceName)
    {
        GameObject audioSourceToDestoy;

        if (_soundData.ContainsKey(audioSourceName) == false)
        {
            Debug.LogError("Fnct StopSound : Specified key not found for the audio file");
        }

        switch (soundType)
        {
            case ESoundType.ONESHOT2D:

                if (_2DSources.Count == 0)
                {
                    Debug.LogWarning("Fnct StopSound : There is no audio source currently playing, the type of sound you are trying to stop is probably incorrect");
                }
                else
                {


                    audioSourceToDestoy = _2DSources[audioSourceName].gameObject;

                    _2DSources.Remove(audioSourceName);
                    Destroy(audioSourceToDestoy);
                }
                break;


            case ESoundType.REPETITIVE2D:

                if (_2DRepetitiveSources.Count == 0)
                {
                    Debug.LogWarning("Fnct StopSound : There is no audio source currently playing, the type of sound you are trying to stop is probably incorrect");
                }
                else
                {

                    audioSourceToDestoy = _2DRepetitiveSources[audioSourceName].gameObject;

                    _2DRepetitiveSources.Remove(audioSourceName);
                    Destroy(audioSourceToDestoy);
                }
                break;


            case ESoundType.ONESHOT3D:

                if (_3DSources.Count == 0)
                {
                    Debug.LogWarning("Fnct StopSound : There is no audio source currently playing, the type of sound you are trying to stop is probably incorrect");
                }
                else
                {
                    audioSourceToDestoy = _3DSources[audioSourceName].gameObject;

                    _3DSources.Remove(audioSourceName);
                    Destroy(audioSourceToDestoy);
                }
                break;


            case ESoundType.REPETITIVE3D:

                if (_3DRepetitiveSources.Count == 0)
                {
                    Debug.LogWarning("Fnct StopSound : There is no audio source currently playing, the type of sound you are trying to stop is probably incorrect");
                }
                else
                {
                    audioSourceToDestoy = _3DRepetitiveSources[audioSourceName].gameObject;

                    _3DRepetitiveSources.Remove(audioSourceName);
                    Destroy(audioSourceToDestoy);
                }
                break;

            default:
                Debug.LogWarning("Fnct StopSound : soundType is not correct");
                break;
        }


    }

    #endregion Common Sounds



    #region Repetitive Sounds

    public void ChangeRepetitiveSoundRate(string audioSourceName, float newRate)
    {

    }

    #endregion Repetitive Sounds



    #endregion Methods
}