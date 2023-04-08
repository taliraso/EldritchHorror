using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class Manager : MonoBehaviour
{
    private static Manager _instance;
    public GameObject FadeOutPrefab;
    public GameObject FadeInPrefab;
    public GameObject blackScreenPrefab;

    public GameObject textBox;
    public TMP_Text textBoxContents;
    private AudioSource textBoxAudio;
    public GameObject speechBubble;
    public TMP_Text speechBubbleContents;
    private AudioSource speechBubbleAudio;
    public GameObject radioText;
    public TMP_Text radioTextContents;
    private AudioSource radioTextAudio;

    public GameObject endMessage;

    public float textBuildSpeed = 0.1f;
    public bool isBuilding = false;

    private IEnumerator buildTextCoroutine;

    private string finishText;
    private TMP_Text finishTMPText;

    private float bubbleHoriSize;
    private float bubbleVertSize;
    public float extraRoom = 1;

    public AudioClip beepHigh;
    public AudioClip beepLow;

    public bool spawnStartingFade;
    public float startingFadeDuration;

    public static bool firstExitMessage = false;
    public static bool secondExitMessage = false;
    public static bool thirdExitMessage = false;

    public static bool firstCoffee = false;
    public static bool secondCoffee = false;
    public static bool thirdCoffee = false;
    public static bool fourthCoffee = false;
    public static bool fifthCoffee = false;
    public static bool sixthCoffee = false;

    public GameObject plantAlive;
    public GameObject plantDead;
    public static bool wateredPlantToday = true;

    public static Manager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        //DontDestroyOnLoad(this.gameObject);

        textBoxAudio = textBoxContents.GetComponent<AudioSource>();
        speechBubbleAudio = speechBubbleContents.GetComponent<AudioSource>();
        radioTextAudio = radioTextContents.GetComponent<AudioSource>();

    }

    private void Start()
    {
        if (spawnStartingFade)
        {
            SpawnFadeIn(startingFadeDuration);
        }

        if (SceneManager.GetActiveScene().name == "Day1")
        {
            ResetAchievementProgress();
        }

        if (wateredPlantToday)
        {
            plantAlive.SetActive(true);
            plantDead.SetActive(false);

            if (SceneManager.GetActiveScene().name == "Week5")
            {
                GameObject.Find("SteamManager").GetComponent<SteamManager>().TriggerAchievement("PLANT_ACHIEVEMENT");
            }

            wateredPlantToday = false;
        }
        else
        {
            plantAlive.SetActive(false);
            plantDead.SetActive(true);

            GameObject.Find("SteamManager").GetComponent<SteamManager>().TriggerAchievement("DEAD_PLANT_ACHIEVEMENT");
        }
    }

    private void LateUpdate()
    {
        textBox.transform.position = (Vector2)Camera.main.transform.position;
    }

    public void SpawnFadeOut(float duration, bool end)
    {
        var newObject = Instantiate(FadeOutPrefab);
        var fadeComp = newObject.GetComponent<Fade>();
        fadeComp.FadeTime = duration;
        if (end)
        {
            fadeComp.endFade = true;
        }
        newObject.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f);
        
    }

    public void SpawnFadeIn(float duration)
    {
        var newObject = Instantiate(FadeInPrefab);
        newObject.GetComponent<Fade>().FadeTime = duration;
        newObject.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f);
    }

    public void SpawnBlackScreen()
    {
        var newObject = Instantiate(blackScreenPrefab);
        newObject.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f);
    }

    public void DisplayText(string text, DialogueNode.DIALOGUETYPE dialogueType, Transform textTransform)
    {
        ClearText();

        switch (dialogueType)
        {
            case DialogueNode.DIALOGUETYPE.TextBox:
                //show text box
                textBox.SetActive(true);

                //set to correct position
                textBox.transform.position = (Vector2)Camera.main.transform.position;

                //start coroutine to show each char one at a time;
                buildTextCoroutine = BuildText(text, dialogueType, textBoxContents, true);
                break;
            case DialogueNode.DIALOGUETYPE.SpeechBubble:
                //show text box
                speechBubble.SetActive(true);

                //set to correct position
                if (textTransform != null)
                {
                    speechBubble.transform.position = new Vector3(Mathf.Clamp(textTransform.position.x, Camera.main.transform.position.x - 6.42f + extraRoom, Camera.main.transform.position.x + 6.42f - extraRoom),
                                                                  Mathf.Clamp(textTransform.position.y, Camera.main.transform.position.y - 2.70f + extraRoom, Camera.main.transform.position.y + 2.70f - extraRoom), 0f);
                }
                else
                {
                    speechBubble.transform.position = Camera.main.transform.position;
                }

                //start coroutine to show each char one at a time;
                buildTextCoroutine = BuildText(text, dialogueType, speechBubbleContents, true);
                break;
            case DialogueNode.DIALOGUETYPE.RadioText:
                //show text box
                radioText.SetActive(true);

                //set to correct position
                if (textTransform != null)
                {
                    radioText.transform.position = new Vector3(Mathf.Clamp(textTransform.position.x, Camera.main.transform.position.x - 6.42f + extraRoom, Camera.main.transform.position.x + 6.42f - extraRoom),
                                                                  Mathf.Clamp(textTransform.position.y, Camera.main.transform.position.y - 2.70f + extraRoom, Camera.main.transform.position.y + 2.70f - extraRoom), 0f);
                }
                else
                {
                    radioText.transform.position = Camera.main.transform.position;
                }

                //start coroutine to show each char one at a time;
                buildTextCoroutine = BuildText(text, dialogueType, radioTextContents, true);
                break;
            default:
                break;
        }

        StartCoroutine(buildTextCoroutine);
    }

    private IEnumerator BuildText(string text, DialogueNode.DIALOGUETYPE dialogueType, TMP_Text TMPText, bool invisibleCharacters)
    {
        isBuilding = true;
        finishText = text;
        finishTMPText = TMPText;

        for (int i = 0; i <= text.Length; i++)
        {
            string tempText = text.Substring(0, i);
            if (invisibleCharacters)
            {
                tempText += "<color=#00000000>" + text.Substring(i, text.Length - i) + "</color>";
            }
            TMPText.text = tempText;

            if (i < text.Length)
            {
                if (text.Substring(i, 1) != " ")
                {
                    //play a beep
                    switch (dialogueType)
                    {
                        case DialogueNode.DIALOGUETYPE.TextBox:
                            textBoxAudio.Play();
                            break;
                        case DialogueNode.DIALOGUETYPE.SpeechBubble:
                            speechBubbleAudio.Play();
                            break;
                        case DialogueNode.DIALOGUETYPE.RadioText:
                            radioTextAudio.Play();
                            break;
                        default:
                            break;
                    }
                }
            }

            yield return new WaitForSeconds(textBuildSpeed);
        }
        //end of build text
        isBuilding = false;
    }

    public void FinishBuildText()
    {
        StopCoroutine(buildTextCoroutine);

        finishTMPText.text = finishText;
        //play a beep

        isBuilding = false;
    }

    public void ClearText()
    {
        textBoxContents.GetComponent<TextMeshProUGUI>().text = "";
        textBox.SetActive(false);

        speechBubbleContents.GetComponent<TextMeshPro>().text = "";
        speechBubble.SetActive(false);

        radioTextContents.GetComponent<TextMeshPro>().text = "";
        radioText.SetActive(false);
    }

    public void End()
    {
        //fade out, lock input
        SpawnFadeOut(2, true);
        GameObject.Find("Player").GetComponent<PlayerController>().isTransitioning = true;

        //spawn thank you message
        StartCoroutine(ShowEndMessage());
    }

    public IEnumerator ShowEndMessage()
    {
        yield return new WaitForSeconds(2f);

        endMessage.SetActive(true);
    }

    public void WateredPlant()
    {
        wateredPlantToday = true;
    }

    public void ResetAchievementProgress()
    {
        print("resetting");
        firstExitMessage = false;
        secondExitMessage = false;
        thirdExitMessage = false;
        firstCoffee = false;
        secondCoffee = false;
        thirdCoffee = false;
        fourthCoffee = false;
        fifthCoffee = false;
        sixthCoffee = false;
        wateredPlantToday = true;
    }
}
