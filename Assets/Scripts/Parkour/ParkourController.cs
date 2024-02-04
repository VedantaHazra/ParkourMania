using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
    public EnvironmentChecker environmentChecker;
    bool playerInAction;
    public Animator animator;
    public PlayerScript playerScript;
   [SerializeField] NewParkourAction jumpAction;

    [Header("Parkour Action Area")]
    public List<NewParkourAction> newParkourAction;

    private void Update()
    {
        Jump();
    }

    private void Jump()
    {
        if(Input.GetButton("Jump") && !playerInAction)
        {
            var hitData = environmentChecker.CheckObstacle();
            if(hitData.hitFound)
            {
                foreach(var action in newParkourAction)
                {
                    if(action.CheckAvailable(hitData, transform))
                    {
                        StartCoroutine(PerformParkourAction(action));
                        break;
                    }
                }
                
            }
            else
            {
                if(playerScript.onSurface)
                {
                StartCoroutine(PerformParkourAction(jumpAction));
                }
                playerScript.CheckJump();
                
            }
        }
    }

    IEnumerator PerformParkourAction(NewParkourAction action)
    {
        playerInAction = true;
        playerScript.SetControl(action.SetControl);

        animator.CrossFade(action.AnimationName, 0.2f);
        yield return null;

        var animationState = animator.GetNextAnimatorStateInfo(0);
        if(!animationState.IsName(action.AnimationName))
        {
            Debug.LogError("Animation Name is incorrect");
        }

        
        float timerCounter = 0f;
        while(timerCounter<= animationState.length)
        {
            timerCounter += Time.deltaTime;

           //Make player look towards the obstacle
            if(action.LookAtObstacle)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, action.RequiredRotation, playerScript.rotationSpeed * Time.deltaTime);
            }

            if(action.AllowTargetMatching)
            {
                CompareTarget(action);
            }

            yield return null;
        }

        yield return new WaitForSeconds(action.ParkourActionDelay);

        playerScript.SetControl(true);
        playerInAction = false;


    }

    void CompareTarget(NewParkourAction action)
    {
        animator.MatchTarget(action.ComparePosition, transform.rotation,action.CompareBodyPart, new MatchTargetWeightMask(action.ComparePositionWeight, 0), action.CompareStartTime, action.CompareEndTime);
    }
}
