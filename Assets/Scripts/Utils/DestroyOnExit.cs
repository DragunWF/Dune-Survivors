using UnityEngine;

public class DestroyOnExit : StateMachineBehaviour
{
    // This function is called when a transition starts from this state.
    // It's the perfect "animation just finished" event.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 'animator' is the Animator component on your prefab.
        // We get its GameObject and destroy it.
        Destroy(animator.gameObject);
    }
}