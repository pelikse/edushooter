using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoadingOverlayManager : MMSingleton<LoadingOverlayManager>, MMEventListener<MMGameEvent>
{
    #region Debugging Buttons
    [MMInspectorButton("FadeInBtn")]
    /// A test button to test adding coins
    public bool DebugFadeIn;
    protected virtual void FadeInBtn()
    {
        ChangeLoadingState(LoadingOverlayState.FadingIn);
    }

    [MMInspectorButton("FadeOutBtn")]
    /// A test button to test adding coins
    public bool DebugFadeOut;
    protected virtual void FadeOutBtn()
    {
        ChangeLoadingState(LoadingOverlayState.FadingOut);
    }
    #endregion

    [System.Serializable]
    public enum LoadingOverlayState
    {
        Idle,
        FadingIn,
        Loading,
        FadingOut,
        Timeout
    }

    [Space, Space]

    [SerializeField][MMReadOnly] private LoadingOverlayState CurrentLoadingState = LoadingOverlayState.Idle;

    [Space]

    [SerializeField] private CanvasGroup OverlayContainer;

    [Space]

    [SerializeField] private float FadeDuration = 0.3f;
    [SerializeField] private float TimeoutDuration = 20f;

    [Space]

    public UnityEvent OnIdle;

    public UnityEvent OnFadingIn;

    public UnityEvent OnFadingOut;

    public UnityEvent OnLoading;

    public UnityEvent OnTimeout;

    [Space,Space]

    [SerializeField][MMReadOnly] private float CurrentWaitTime = 0f;
    //cache for blocking processes
    [SerializeField][MMReadOnly] private int RunningProcesses = 0;

    //constants
    const int MIN_BLOCKING_PROCESS = 0, MAX_BLOCKING_PROCESS = 999;

    //cache
    private float _fadeTime, _opacity, _lastTimer;



    private void Start()
    {
        //loading overlay always starts off as idle
        OverlayContainer.gameObject.SetActive(false);
        
    }

    private void Update()
    {
        if (CurrentLoadingState.Equals(LoadingOverlayState.Loading))
        {
            CurrentWaitTime = Time.time - _lastTimer;

            //if the loading screen has been active for more than the wait duration
            //call for timeout
            if (Time.time - _lastTimer >= TimeoutDuration)
            {
                //stop loading
                ChangeLoadingState(LoadingOverlayState.Timeout);

                QuestionElement.AcceptCallback callback = () =>
                {
                    Debug.Log("quitting application");
                    Application.Quit();
                };

                QuestionElement.TryGetInstance().DisplayQuestion("Something went wrong, please restart the application!", callback);
            }
        }
    }

    private void ChangeLoadingState(LoadingOverlayState state)
    {
        switch (state)
        {
            case LoadingOverlayState.Idle:
                OnIdleState();
                break;

            case LoadingOverlayState.FadingIn:
                OnFadingInState();
                break;

            case LoadingOverlayState.FadingOut:
                OnFadingOutState();
                break;

            case LoadingOverlayState.Loading:
                OnLoadingState();
                break;

            case LoadingOverlayState.Timeout:
                OnTimeoutState();
                break;
        }

        //switch state
        CurrentLoadingState = state;
    }

    // a simple implementation so the loading screen can track the number of
    // active processes
    #region Loading Process Management
        
    public void AddBlockingProcess()
    {
        // if we're starting from no processes then fade in
        if (RunningProcesses == 0)
        {
            ChangeLoadingState(LoadingOverlayState.FadingIn);
        }
        RunningProcesses += 1;
    }

    public void EndBlockingProcess()
    {
        // if we're exiting all processes then fade out
        if (RunningProcesses == 1)
        {
            ChangeLoadingState(LoadingOverlayState.FadingOut);
        }
        RunningProcesses = Mathf.Clamp(RunningProcesses - 1, MIN_BLOCKING_PROCESS, MAX_BLOCKING_PROCESS);
    }

    #endregion

    #region State Handlers
    private void OnIdleState()
    {
        //stop all fading
        StopAllCoroutines();

        //turn off the overlay
        OverlayContainer.gameObject.SetActive(false);
        OverlayContainer.alpha = 0f;

        OnIdle?.Invoke();
    }

    private void OnFadingInState()
    {
        StopAllCoroutines();

        //turn on the overlay but invisible
        OverlayContainer.alpha = 0f;
        OverlayContainer.gameObject.SetActive(true);

        //fade it in
        StartCoroutine(FadeInOverlay());

        OnFadingIn?.Invoke();
    }

    private void OnFadingOutState()
    {
        StopAllCoroutines();

        //fade
        OverlayContainer.alpha = 1f;
        OverlayContainer.gameObject.SetActive(true);

        StartCoroutine(FadeOutOverlay());

        OnFadingOut?.Invoke();
    }

    private void OnLoadingState()
    {
        StopAllCoroutines();

        //start tracking time
        _lastTimer = Time.time;

        OnLoading?.Invoke();
    }

    private void OnTimeoutState()
    {
        StopAllCoroutines();

        //announce the timeout so processes stop
        MMGameEvent.Trigger("LoadingTimeout");

        OnTimeout?.Invoke();
    }

    #endregion

    #region Coroutine

    private IEnumerator FadeInOverlay()
    {
        _fadeTime = 0f;

        while (_fadeTime < FadeDuration)
        {
            _fadeTime += Time.deltaTime;
            _opacity = _fadeTime / FadeDuration;
       
            OverlayContainer.alpha = _opacity;
            yield return null;
        }

        OverlayContainer.alpha = 1f;

        //transition into loading
        ChangeLoadingState(LoadingOverlayState.Loading);
    }

    private IEnumerator FadeOutOverlay()
    {
        _fadeTime = 0f;

        while (_fadeTime < FadeDuration)
        {
            _fadeTime += Time.deltaTime;
            _opacity = 1 - (_fadeTime / FadeDuration);

            OverlayContainer.alpha = _opacity;
            yield return null;
        }

        OverlayContainer.alpha = 0f;
        OverlayContainer.gameObject.SetActive(false);

        //transition into idle
        ChangeLoadingState(LoadingOverlayState.Idle);
    }

    #endregion

    #region EventListeners
    protected virtual void OnEnable()
    {
        this.MMEventStartListening<MMGameEvent>();
    }

    protected virtual void OnDisable()
    {
        this.MMEventStopListening<MMGameEvent>();
    }

    public virtual void OnMMEvent(MMGameEvent gameEvent)
    {
        //switch (gameEvent.EventName)
        //{
        //    case "ActivateLoading":
        //        ChangeLoadingState(LoadingOverlayState.FadingIn);
        //        break;

        //    case "DeactivateLoading":
        //        ChangeLoadingState(LoadingOverlayState.FadingOut);
        //        break;
        //}
    }
    #endregion
}
