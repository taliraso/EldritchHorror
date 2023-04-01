using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogControls : MonoBehaviour
{
    public bool lastDayHallway;

    public float duration = 1f;

    private Animator animator;
    private bool nextFacing;
    private bool nextFacingRight;
    private SpriteRenderer spriteRenderer;

    private bool returnBall = false;
    private bool goBack = false;
    private Vector3 returnPosition;
    private Vector3 fetchPosition;
    private FetchBall ball;

    private bool hasBall;

    private bool updateAfterFacing = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetDuration(float newDuration)
    {
        duration = newDuration;
    }

    public void GoToTransfrom(Transform newTransform)
    {
        StartCoroutine(LerpPosition(newTransform.position));
    }

    private IEnumerator LerpPosition(Vector3 newPosition)
    {
        Vector3 startPosition = transform.position;
        float timeElapsed = 0f;

        if (hasBall)
        {
            animator.SetInteger("DogState", 7);
        }
        else
        {
            animator.SetInteger("DogState", 1);
        }

        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, newPosition, (timeElapsed / duration));
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = newPosition;
        animator.SetInteger("DogState", 0);

        if (nextFacing)
        {
            if (nextFacingRight)
            {
                FaceRight();
            }
            else
            {
                FaceLeft();
            }
            nextFacing = false;
        }

        if (lastDayHallway)
        {
            animator.SetInteger("DogState", 11);
        }

        if (updateAfterFacing)
        {
            StartCoroutine(DelayUpdateGraphics(0));
        }

        if (returnBall)
        {
            if (!goBack)
            {
                GetComponent<BoxCollider2D>().enabled = false;

                //dog picks up ball

                //ANIMATION TODO: PICKUP BALL

                ball.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.5f);


                //return back to ball start position
                FaceRight();
                hasBall = true;
                StartCoroutine(LerpPosition(returnPosition));
                goBack = true;
            }
            else
            {
                //dog drop ball
                //ANIMATION TODO: Drop BALL
                yield return new WaitForSeconds(0.5f);

                ball.gameObject.SetActive(true);
                ball.transform.position = returnPosition;

                //go to ready to fetch position
                FaceLeft();
                hasBall = false;
                StartCoroutine(LerpPosition(fetchPosition));
                returnBall = false;
            }
        }
        else
        {
            if (goBack)
            {
                //end of fetch
                goBack = false;

                GameObject player = GameObject.Find("Player");

                player.GetComponent<PlayerController>().isFetching = false;
                player.GetComponent<BoxCollider2D>().enabled = true;

                GetComponent<BoxCollider2D>().enabled = true;
                ball.GetComponent<BoxCollider2D>().enabled = true;
                ball.CompletedFetch();
            }
        }
    }

    public void SetupFetch(Vector3 ballStartPos, Vector3 dogWaitPos, FetchBall ballComp)
    {
        returnBall = true;
        returnPosition = ballStartPos;
        fetchPosition = dogWaitPos;
        ball = ballComp;
    }

    public void setAnimation(int animationInt)
    {
        animator.SetInteger("DogState", animationInt);
    }

    public void setAnimationForDuration(int animationInt)
    {
        animator.SetInteger("DogState", animationInt);

        StartCoroutine(DelayUpdateGraphics(duration));
    }

    private IEnumerator DelayUpdateGraphics(float delayDuration)
    {
        yield return new WaitForSeconds(delayDuration);

        GetComponent<Interactable>().UpdateGraphics();
    }

    private IEnumerator DelayAnimationState(int animationInt)
    {
        yield return new WaitForSeconds(duration);

        animator.SetInteger("DogState", animationInt);
    }

    public void FaceLeft()
    {
        transform.localScale = new Vector3(5f, 5f, 5f);
    }

    public void FaceRight()
    {
        transform.localScale = new Vector3(-5f, 5f, 5f);
    }

    public void NextFaceLeft()
    {
        nextFacing = true;
        nextFacingRight = false;
    }

    public void NextFaceRight()
    {
        nextFacing = true;
        nextFacingRight = true;
    }

    public void NextFaceNone()
    {
        nextFacing = false;
    }

    public void UpdateVisualsAfterFacing()
    {
        updateAfterFacing = true;
    }

    public void FadeOut()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            spriteRenderer.color = new Color(1f,1f,1f,Mathf.Lerp(1f, 0f, (timeElapsed / duration)));
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
