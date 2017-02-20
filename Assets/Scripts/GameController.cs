using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    private const string timerTextString = "{0}:{1}";
    private const string SHELVEDITEMS = "Shelved Items: {0}";
    private const string UNLOADEDITEMS = "Unloaded Items: {0}";
    private const string WINTEXT = "You won.";
    private const string LOSETEXT = "You lost.";
    private const string BASEHELP = @"
Arrow Keys: Move
Shift: Brake
A,S,D: Turn Head
Q,W,E: Lean side to side
CTRL/Alt: Fork up/down
Z/X: For forward/back";
    private const string UNLOADHELP = @"Unload packages in red shaded areas.
";
    private const string SHELVEHELP = @"Shelve packages on blue shaded areas.
";

    private int countDown;
    private System.DateTime currentTime;
    private System.DateTime startedTime;
    private bool started = false;
    public GameObject beginButton;
    public GameObject nextButton;
    public GameObject tryAgainButton;
    public GameObject timerObject;
    public GameObject winLoseText;
    public GameObject shelvedItemsText;
    public GameObject unloadedItemsText;
    public GameObject helpText;
    public SceneState sceneState;
    public carscript vehicle;
    public int defaultCountdown = 60;
    public int nextSceneIndex;
	
	// Update is called once per frame
	void FixedUpdate () {
        if (started)
        {
            if (sceneState.IsObjectiveMet())
            {
                UpdatedShelvedItems();
                UpdatedUnloadedItems();
                StopLevel();
            }
            else
            {
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    StopLevel();
                }
                else
                {
                    UpdatedShelvedItems();
                    UpdatedUnloadedItems();
                    currentTime = DateTime.Now;
                    TimeSpan ts = currentTime - startedTime;
                    Text timer = (Text)timerObject.GetComponent<Text>();
                    if (timer != null)
                    {
                        UpdateTimerOrStopLevel(ts, timer);
                    }
                    else
                    {
                        Debug.Log("the timer came back null.");
                    }
                }
            }
        }
	}

    public void StartLevel()
    {
        started = true;
        SetLightingInitialState();
        SetVehicleInitialState();
        SetTimerInitialState();
        SetInitialButtonState();
        SetScoreTextInitialState();
        SetHelpText();
    }

    public void StopLevel()
    {
        started = false;
        vehicle.ApplyBrake(100);
        vehicle.VehicleEnabled = false;
        tryAgainButton.SetActive(true);
        winLoseText.SetActive(true);

        Text winTextObj = winLoseText.GetComponent<Text>();
        if (sceneState.IsObjectiveMet())
        {
            winTextObj.text = WINTEXT;
            nextButton.SetActive(true);
        }
        else
        {
            winTextObj.text = LOSETEXT;
        }
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(this.nextSceneIndex);
    }

    private void UpdatedShelvedItems()
    {
        shelvedItemsText.GetComponent<Text>().text = string.Format(SHELVEDITEMS, sceneState.ShelvedItemCount);
    }

    private void UpdatedUnloadedItems()
    {
        unloadedItemsText.GetComponent<Text>().text = string.Format(UNLOADEDITEMS, sceneState.UnloadedItemCount);
    }

    private void SetScoreTextInitialState()
    {
        if (sceneState.unloadAreas.Length == 0)
        {
            unloadedItemsText.SetActive(false);
        }
        else
        {
            unloadedItemsText.SetActive(true);
        }
        if (sceneState.shelfAreas.Length == 0)
        {
            shelvedItemsText.SetActive(false);
        }
        else
        {
            shelvedItemsText.SetActive(true);
        }
    }

    private void SetVehicleInitialState()
    {
        vehicle.VehicleEnabled = true;
    }

    private void SetLightingInitialState()
    {
        RenderSettings.ambientIntensity = sceneState.ambientIntensity;
    }

    private void SetTimerInitialState()
    {
        startedTime = DateTime.Now;
        currentTime = startedTime;
        countDown = defaultCountdown;
    }

    private void SetInitialButtonState()
    {
        beginButton.SetActive(false);
        winLoseText.SetActive(false);
        tryAgainButton.SetActive(false);
    }

    private void SetHelpText()
    {
        string baseHelp = "";
        if (sceneState.shelfAreas.Length > 0)
        {
            baseHelp += SHELVEHELP;
        }
        if (sceneState.unloadAreas.Length > 0)
        {
            baseHelp += UNLOADHELP;
        }
        baseHelp += BASEHELP;
        helpText.GetComponent<Text>().text = baseHelp;
    }

    private void UpdateTimerOrStopLevel(TimeSpan ts, Text timer)
    {
        int timeDiff = countDown - Convert.ToInt32(ts.TotalSeconds);
        if (timeDiff >= 0)
        {
            int minutes = timeDiff / 60;
            int secondParth = timeDiff % 60;
            string timeToDislay = string.Format(timerTextString, minutes.ToString("00"), secondParth.ToString("00"));
            UpdateTimerWithTime(timer, timeToDislay);
        }
        else
        {
            StopLevel();
        }
    }

    private static void UpdateTimerWithTime(Text timer, string timeToDislay)
    {
        timer.text = timeToDislay;
    }

}
