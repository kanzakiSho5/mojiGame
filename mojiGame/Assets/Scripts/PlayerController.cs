using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	GameManager gameMan;
	Animator animator;
	bool isAnimationEnded = false;
	int EndAnimationHash = 0;


    void Start()
    {
		animator = GetComponent<Animator>();
		gameMan = GameManager.Instance;
		isAnimationEnded = false;
    }

	private void Update()
	{
		// アニメーションが終わったら
		if(!isAnimationEnded && 
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && 
            EndAnimationHash != animator.GetCurrentAnimatorStateInfo(0).fullPathHash)
        {
            isAnimationEnded = true;
            EndAnimationHash = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
            print("Animation Ended!");
            if (gameMan.stage == 3)
            {
                gameMan.ChengeStartScene();
                return;
            }
            if (SceneController.CurrentScene == SceneType.Start)
                return;
			gameMan.StageMoveAnimationEnded();
		}
	}

    public void Init()
    {
        print("PlayerInit "+ gameMan.stage);
        animator.SetBool("IsClearStage01", false);
        animator.SetBool("IsClearStage02", false);
        animator.SetBool("IsClearStage03", false);
        if(gameMan.stage == 3)
            animator.SetTrigger("ReStart");
    }

    public void ClearStage(int stage)
	{

		animator.SetBool("IsClearStage0"+ (stage), true);
		isAnimationEnded = false;
	}
}
