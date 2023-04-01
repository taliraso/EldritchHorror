using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField]

    public bool isInteracting = false;
    public bool isTransitioning = false;
    public bool isFetching = false;

    public enum FACING { Up, Down, Right, Left}
    public FACING currentFacing = FACING.Down;

    private Interactable currentInteractable;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D boxCollider2D;

    private float duration = 1f;

    private bool beingDragged;
    private bool usingBed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        usingBed = true;
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;
    }

    private void Update()
    {
        if (!isFetching)
        {
            if (!isTransitioning)
            {
                //we only want to start interacting if we are not interacting
                if (!isInteracting)
                {
                    //INTERACTION
                    if (Input.GetButtonDown("Interact"))
                    {
                        Interactable interactableComponent = null;

                        //check facing direction
                        switch (currentFacing)
                        {
                            case (FACING.Up):
                                if (IsInteractable(transform.position + Vector3.up))
                                {
                                    interactableComponent = GetInteractable(transform.position + Vector3.up);
                                }
                                break;
                            case (FACING.Down):
                                if (IsInteractable(transform.position + Vector3.down))
                                {
                                    interactableComponent = GetInteractable(transform.position + Vector3.down);
                                }
                                break;
                            case (FACING.Right):
                                if (IsInteractable(transform.position + Vector3.right))
                                {
                                    interactableComponent = GetInteractable(transform.position + Vector3.right);
                                }
                                break;
                            case (FACING.Left):
                                if (IsInteractable(transform.position + Vector3.left))
                                {
                                    interactableComponent = GetInteractable(transform.position + Vector3.left);
                                }
                                break;
                            default:
                                print("ERROR - No Facing Direction Found");
                                break;
                        }

                        if (interactableComponent != null)
                        {
                            if (interactableComponent.isInteractable)
                            {
                                //we have something to interact with
                                interactableComponent.Interact();
                                currentInteractable = interactableComponent;
                                isInteracting = true;
                                animator.SetBool("isWalking", false);
                                animator.Play("player-interact-anim");
                            }
                        }
                    }
                }
                else
                {
                    //we are currently in an interaction
                    if (Input.GetButtonDown("Interact"))
                    {
                        if (currentInteractable.IsMoreText())
                        {
                            currentInteractable.Interact();
                        }
                        else if (Manager.Instance.isBuilding)
                        {
                            Manager.Instance.FinishBuildText();
                        }
                        else
                        {

                            //finish the interaction;
                            currentInteractable.Finish();
                            currentInteractable = null;
                            isInteracting = false;
                        }
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (!isFetching)
        {
            if (!isTransitioning)
            {
                //can only move if not interacting;
                if (!isInteracting)
                {
                    if (!usingBed)
                    {
                        //MOVEMENT
                        if (Input.GetAxisRaw("Horizontal") == 1f)
                        {
                            currentFacing = FACING.Right;

                            //MOVE right
                            rb.MovePosition(rb.position + Vector2.right * moveSpeed * Time.fixedDeltaTime);
                            animator.SetBool("isWalking", true);

                            spriteRenderer.flipX = true;
                        }

                        else if (Input.GetAxisRaw("Horizontal") == -1f)
                        {
                            currentFacing = FACING.Left;

                            //MOVE left
                            rb.MovePosition(rb.position + Vector2.left * moveSpeed * Time.fixedDeltaTime);
                            animator.SetBool("isWalking", true);

                            spriteRenderer.flipX = false;
                        }

                        else if (Input.GetAxisRaw("Vertical") == 1f)
                        {
                            currentFacing = FACING.Up;

                            //MOVE up
                            rb.MovePosition(rb.position + Vector2.up * moveSpeed * Time.fixedDeltaTime);
                            animator.SetBool("isWalking", true);
                        }

                        else if (Input.GetAxisRaw("Vertical") == -1f)
                        {
                            currentFacing = FACING.Down;

                            //MOVE down
                            rb.MovePosition(rb.position + Vector2.down * moveSpeed * Time.fixedDeltaTime);
                            animator.SetBool("isWalking", true);
                        }
                        else
                        {
                            animator.SetBool("isWalking", false);
                        }
                    }
                }
            }
        }
    }

    public bool IsInteractable(Vector3 location)
    {
        var colliders = Physics2D.OverlapCircleAll(location, 0.4f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Interactable" && collider.GetComponent<Interactable>().isInteractable)
            {
                return true;
            }
        }
        return false;

    }

    public Interactable GetInteractable(Vector3 location)
    {
        var colliders = Physics2D.OverlapCircleAll(location, 0.6f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Interactable" && collider.GetComponent<Interactable>().isInteractable)
            {
                return collider.GetComponent<Interactable>();
            }
        }
        return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "StepTrigger")
        {
            if (collision.GetComponent<Interactable>().isInteractable)
            {
                if (collision.GetComponent<StepTrigger>().silentTrigger)
                {
                    collision.GetComponent<StepTrigger>().Trigger();
                    currentInteractable = collision.GetComponent<Interactable>();
                }
                else
                {
                    animator.SetBool("isWalking", false);
                    collision.GetComponent<StepTrigger>().Trigger();
                    isInteracting = true;
                    currentInteractable = collision.GetComponent<Interactable>();
                }
            }
        }
    }

    public void StopInteracting()
    {
        currentInteractable = null;
        isInteracting = false;
    }

    public void StopWalking()
    {
        animator.SetBool("isWalking", false);
    }

    public void UpdateFacing()
    {
        if (currentFacing == FACING.Left)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    public void GoToTransfrom(Transform newTransform)
    {
        StartCoroutine(LerpPosition(newTransform.position));
    }

    public void Drag()
    {
        beingDragged = true;
    }

    private IEnumerator LerpPosition(Vector3 newPosition)
    {
        isFetching = true;
        Vector3 startPosition = transform.position;
        float timeElapsed = 0f;

        //start animation
        animator.SetBool("isWalking", true);

        if (beingDragged)
        {
            animator.SetBool("isWalking", false);
            animator.SetInteger("State", 5);
        }

        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, newPosition, (timeElapsed / duration));
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        //end animation
        animator.SetBool("isWalking", false);

        if (beingDragged)
        {
            animator.SetInteger("State", 0);
            beingDragged = false;
        }

        transform.position = newPosition;
        isFetching = false;
    }

    public void SetDuration(float newDuration)
    {
        duration = newDuration;
    }

    public void FaceLeft()
    {
        currentFacing = FACING.Left;
        UpdateFacing();
    }

    public void FaceRight()
    {
        currentFacing = FACING.Right;
        UpdateFacing();
    }

    public void ForceInteraction(Interactable interactableObject)
    {
        animator.SetBool("isWalking", false);
        interactableObject.Interact();
        isInteracting = true;
        currentInteractable = interactableObject;
    }

    public void WakeUpAnimation()
    {
        animator.SetInteger("State", 0);
        animator.Play("player-wake-anim");
        GetComponent<SetLayerOrderToNegY>().RenderLow = true;
        StartCoroutine(WakeUpDelay());
    }

    private IEnumerator WakeUpDelay()
    {
        yield return new WaitForSeconds(4.666f);
        animator.SetInteger("State", 0);
        animator.Play("player-idle-anim");
        spriteRenderer.flipX = true;
        usingBed = false;
        boxCollider2D.enabled = true;
        GetComponent<SetLayerOrderToNegY>().RenderLow = false;
    }

    public void SleepAnimation()
    {
        animator.SetInteger("State", 6);
    }

    public void StartWatering()
    {
        animator.SetInteger("State", 8);
    }

    public void ResetAnimation()
    {
        animator.SetInteger("State", 0);
        boxCollider2D.enabled = true;
    }

}