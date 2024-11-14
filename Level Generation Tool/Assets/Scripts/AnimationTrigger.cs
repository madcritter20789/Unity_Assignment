using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public Animator animator;
    public string animationName;

    public void TriggerAnimation()
    {
        if (animator != null && !string.IsNullOrEmpty(animationName))
        {
            animator.Play(animationName);
        }
    }
}
