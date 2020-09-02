using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{

    Animation animationController;

    Dictionary<string, AnimationClip> animations = new Dictionary<string, AnimationClip>();

    AnimationClip currentIdleAnimation;

    public AnimationClip defaultIdleAnimation;

    void Awake()
    {
        animationController = GetComponentInChildren<Animation>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animationController.AddClip(defaultIdleAnimation, "Idle");
        playLoop("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        //if(animationController.isPlaying == false)
        //{
        //    playLoop("Idle");
        //}
    }

    public void addClip(string name, AnimationClip clip)
    {
        animationController.AddClip(clip, name);
    }

    public void play(string name, bool overrideCurrent = true)
    {
        animationController.wrapMode = WrapMode.Once;
        if(animationController.isPlaying == true)
        {
            if(overrideCurrent == true) { animationController.Play(name); }
            else { animationController.PlayQueued(name); }
        }
        else animationController.Play(name);

    }

    public void playLoop(string name)
    {
        animationController.wrapMode = WrapMode.Loop;
        animationController.Play(name);
    }

    public void playIdle()
    {
        playLoop("Idle");
    }

    public bool isPlaying
    {
        get
        {
            return animationController.isPlaying;
        }
    }


    public void queue(string name)
    {
        animationController.PlayQueued(name);
    }
}
