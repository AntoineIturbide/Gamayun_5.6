using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAudio : MonoBehaviour {

    public Avatar2.Character caracter;

    public bool gliding = false;
    public bool stalling = false;

    public void Start()
    {
        caracter.state.air_push.state.eOnCast += delegate
        {
            AkSoundEngine.PostEvent("Wings_flap", this.gameObject);
        };

    }

    public void Update()
    {
        // Gliding
        bool gliding = caracter.GetRelativeSpeed() > 0.125f;
        if (gliding != this.gliding)
        {
            this.gliding = gliding;
            if (gliding)
            {
                AkSoundEngine.PostEvent("Avatar_Glide_Start", this.gameObject);
            }
            else
            {
                AkSoundEngine.PostEvent("Avatar_Glide_Stop", this.gameObject);
                AkSoundEngine.PostEvent("Avatar_Break", this.gameObject);
            }
        }

        // Stalling
        bool stalling = caracter.state.stall_factor.get_value() > 0.25f;
        if (stalling != this.stalling)
        {
            this.stalling = stalling;
            if (stalling)
            {
                AkSoundEngine.PostEvent("Avatar_Glide_Stalling", this.gameObject);
            }
        }
    }

}
    