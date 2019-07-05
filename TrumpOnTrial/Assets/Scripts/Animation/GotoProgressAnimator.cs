using System;
using UnityEngine;

namespace FineGameDesign.Animation
{
    [Serializable]
    public struct AnimatedProgress
    {
        public Animator animator;
        public string animationName;
        public float progress;
    }

    public sealed class GotoProgressAnimator : MonoBehaviour
    {
        [SerializeField]
        private AnimatedProgress m_AnimatedProgress;

        private void Start()
        {
            ProgressAnimator.GotoProgress(m_AnimatedProgress);
        }

        public void Goto(float progress)
        {
            ProgressAnimator.GotoProgress(m_AnimatedProgress, progress);
        }
    }

    public static class ProgressAnimator
    {
        public static void GotoProgress(AnimatedProgress animatedProgress, float progress)
        {
            animatedProgress.progress = progress;
            GotoProgress(animatedProgress);
        }

        public static void GotoProgress(AnimatedProgress animatedProgress)
        {
            StopAtNormalizedTime(
                animatedProgress.animator,
                animatedProgress.animationName,
                animatedProgress.progress
            );
        }

        /// <summary>
        /// Useful for a progress bar or any animation.
        /// This method expects the entire timeline interpolates from no progress to full progress.
        /// </summary>
        /// <remarks>
        /// Adapted from:
        /// <a href="https://answers.unity.com/questions/1272641/jump-to-a-specific-frame-in-an-animator.html">
        /// Answer by leftoversalad · May 03, 2017 at 07:51 AM
        /// </a>
        /// </remarks>
        public static void StopAtNormalizedTime(Animator animator, string animationName, float progress)
        {
            if (animator.speed != 0f)
            {
                animator.speed = 0f;
            }

            animator.Play(animationName, -1, progress);
        }
    }
}
