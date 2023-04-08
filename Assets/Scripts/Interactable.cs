using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using FMODUnity;

public class Interactable : MonoBehaviour
{
    [Header("Interactable Settings")]
    public bool isInteractable = true;
    //private bool PlayerinTrigger = false;
    public bool deactiveAfterInteraction = false;

    [Header("Graphics Settings")]
    public bool hasGraphics = true;
    public bool isDog = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public Sprite interactableGraphic;
    public Sprite nonInteractableGraphic;

    [Header("Text Settings")]
    public bool showText;
    private int textIndex = 0;
    private bool moreText = false;
    public DialogueNode[] nodes;

    [Header("Transition Settings")]
    public float transitionTime = 0.5f;
    public bool roomTransition;
    public Camera newCamera;
    public bool sceneTransition;
    public string sceneName;
    public Transform spawnLocation;
    public bool sleepAnim;
    public bool wakeAnim;

    [Header("Interaction Events")]
    public UnityEvent onInteractionStart;
    [FormerlySerializedAs("onInteraction")]
    public UnityEvent onInteractionEnd;
    public UnityEvent onTransition;

    private void Awake()
    {
        if (hasGraphics)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (isDog)
        {
            animator = GetComponent<Animator>();
        }

        UpdateGraphics();
    }

    private void OnEnable()
    {
        UpdateGraphics();
    }

    public void Interact()
    {
        if (sleepAnim)
        {
            GameObject _playerObj = GameObject.Find("Player");
            _playerObj.GetComponent<BoxCollider2D>().enabled = false;
            _playerObj.transform.position = new Vector3(4.445f, 0.295f, 0);
            _playerObj.GetComponent<PlayerController>().SleepAnimation();
            _playerObj.GetComponentInChildren<SpriteRenderer>().flipX = false;
        }

        //Dialogue display
        if (showText)
        {
            //Only progress dialogue if currently not building dialogue
            if (!Manager.Instance.isBuilding)
            {
                if (textIndex == 0)
                {
                    onInteractionStart.Invoke();
                }

                //dispaly new text and trigger node events
                Manager.Instance.DisplayText(nodes[textIndex].dialogue, nodes[textIndex].dialogueType, nodes[textIndex].dialogueTransform);
                nodes[textIndex].dialogueEvent.Invoke();

                //is there more text to display?
                if (textIndex == nodes.Length - 1)
                {
                    textIndex = 0;
                    moreText = false;
                }
                else
                {
                    textIndex++;
                    moreText = true;
                }
            }
            else
            {
                //The player hit niteract while text was building
                Manager.Instance.FinishBuildText();
            }
        }
        else
        {
            onInteractionStart.Invoke();
            Finish();
        }
    }

    public void SilentInteract()
    {
        onInteractionStart.Invoke();

        if (deactiveAfterInteraction)
        {
            SetInteractable(false);
        }
    }
    public void Finish()
    {
        //if we showed text, we need to clear it all
        if (showText)
        {
            Manager.Instance.ClearText();
            textIndex = 0;
        }

        //Scene transition
        if (sceneTransition)
        {
            //send start location info to the Manager
            Manager.Instance.SpawnFadeOut(transitionTime, false);
            StartCoroutine(DelaySceneLoad(sceneName, transitionTime));
            StartCoroutine(DelayInvoke(transitionTime));
        }

        //Room transition
        else if (roomTransition)
        {
            Manager.Instance.SpawnFadeOut(transitionTime, false);
            StartCoroutine(DelayRoomLoad(transitionTime));
            StartCoroutine(DelayInvoke(transitionTime));
        }

        //End of Interaction event
        else
        {
            onInteractionEnd.Invoke();
        }

        //Deactivate if set
        if (deactiveAfterInteraction)
        {
            SetInteractable(false);
        }
    }

    public bool IsMoreText()
    {
        return moreText;
    }

    IEnumerator DelaySceneLoad(string sceneName, float time)
    {
        yield return new WaitForSeconds(time);
        onTransition.Invoke();
        Manager.Instance.SpawnBlackScreen();
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator DelayRoomLoad(float time)
    {
        //Player code
        GameObject _playerObj = GameObject.Find("Player");
        PlayerController _playerCont = _playerObj.GetComponent<PlayerController>();
        _playerCont.isTransitioning = true;
        _playerCont.StopWalking();

        yield return new WaitForSeconds(time);

        //Trigger events
        onTransition.Invoke();

        //Switch camera
        Camera.main.gameObject.SetActive(false);
        newCamera.gameObject.SetActive(true);

        //Fade in
        Manager.Instance.SpawnFadeIn(transitionTime);

        //Teleport player to spawnpoint
        _playerObj.transform.position = spawnLocation.position;
        _playerCont.StopInteracting();
        _playerCont.isTransitioning = false;

        if (wakeAnim)
        {
            GameObject _playerObj2 = GameObject.Find("Player");
            _playerObj.transform.position = new Vector3(4.5f, 0f, 0f);
            _playerObj2.GetComponent<PlayerController>().WakeUpAnimation();
        }
    }

    IEnumerator DelayInvoke(float time)
    {
        yield return new WaitForSeconds(time);

        onInteractionEnd.Invoke();
    }

    public void SetInteractable(bool state)
    {
        isInteractable = state;
        UpdateGraphics();
    }

    /*private void OnTriggerEnter2D(Collider2D other) //edit
    {
        if (other.tag == "Player")
        {
            PlayerinTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) //edit
    {
        if (other.tag == "Player")
        {
            PlayerinTrigger = false;
        }
    }*/

    public void UpdateGraphics()
    {
        if (hasGraphics && spriteRenderer != null)
        {
            if (isInteractable) //edit made
            {
                if (interactableGraphic != null)
                {
                    spriteRenderer.enabled = true;
                    spriteRenderer.sprite = interactableGraphic;
                }
                else
                {
                    spriteRenderer.enabled = false;
                }
            }
            else
            {
                if (nonInteractableGraphic != null)
                {
                    spriteRenderer.enabled = true;
                    spriteRenderer.sprite = nonInteractableGraphic;
                }
                else
                {
                    spriteRenderer.enabled = false;
                }
            }
        }

        if (isDog)
        {
            if (isInteractable)
            {
                animator.SetInteger("DogState", 2);
            }
            else
            {
                animator.SetInteger("DogState", 0);
            }
        }
    }
}


[System.Serializable]
public struct DialogueNode
{
    [TextArea]
    public string dialogue;
    public DIALOGUETYPE dialogueType;
    public Transform dialogueTransform;
    public UnityEvent dialogueEvent;

    public enum DIALOGUETYPE
    {
        TextBox,
        SpeechBubble,
        RadioText
    }
}