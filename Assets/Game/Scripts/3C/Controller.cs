using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using Tweening;

namespace Avatar2
{
    public class Controller : MonoBehaviour {

        #region Configuration
        /*****************
         * CONFIGURATION *
         *****************/
        [Serializable]
        public class Configuration
        {
            [Header("Controlled Character")]
            // Character
            public Character character;

            [Header("Input")]
            public Utility.Controller.GamepadXbox xbox_gamepad;
            
            [Header("Orientation")]
            // Rotation Around X
            public Utility.Controller.AxisNegPosXbox rotationAroundX = Utility.Controller.AxisNegPosXbox.LEFT_STICK_Y;
            public bool inverseRotAroundX = false;
            public float timeToReachTargetRotX = 0.75f;

            // Rotation Around Y
            public Utility.Controller.AxisNegPosXbox rotationAroundY = Utility.Controller.AxisNegPosXbox.LEFT_STICK_X;
            public bool inverseRotAroundY = true;
            public float timeToReachTargetRotY = 0.75f;

            [Header("Camera")]
            // Camera rotation X
            public Utility.Controller.AxisNegPosXbox cameraRotationX = Utility.Controller.AxisNegPosXbox.RIGHT_STICK_X;
            public bool inverseCameraRotationX = false;

            // Rotation Around Y
            public Utility.Controller.AxisNegPosXbox cameraRotationY = Utility.Controller.AxisNegPosXbox.RIGHT_STICK_Y;
            public bool inverseCameraRotationY = false;
            [Space(4)]
            public float timeToReachCameraRotation = 0.5f;


            [Header("Wings")]
            public Utility.Controller.AxisPositiveXbox wings = Utility.Controller.AxisPositiveXbox.RIGHT_TRIGGER;
            //public float timeToReachTargetWings = 0.125f;


            [Header("Slow")]
            public Utility.Controller.AxisPositiveXbox slow = Utility.Controller.AxisPositiveXbox.LEFT_TRIGGER;


        }

        // Configuration Instance
        public Configuration config = new Configuration();
        #endregion

        #region State
        /*********
         * STATE *
         *********/
        public class State
        {
            #region Smoothing
            ///////////////
            // Smoothing //
            ///////////////

            // Rotation Around X
            public Smooth<float> rotation_around_x;
            public float rotation_around_x_time_to_reach_target;
            private float rotation_around_x_stick_tick(float current, float target, float dt)
            {
                float time_to_reach_target = rotation_around_x_time_to_reach_target;
                return time_to_reach_target > 0 ?
                    Mathf.MoveTowards(current, target, dt * (2f * (1f / time_to_reach_target))) :
                    target;
            }

            // Rotation Around Y
            public Smooth<float> rotation_around_y;
            public float rotation_around_y_time_to_reach_target;
            private float rotation_around_y_tick(float current, float target, float dt)
            {
                float time_to_reach_target = rotation_around_y_time_to_reach_target;
                return time_to_reach_target > 0 ?
                    Mathf.MoveTowards(current, target, dt * (2f * (1f / time_to_reach_target))) :
                    target;
            }
                        
            public void TickSmooth(float dt)
            {
                // Orientation
                // Rotation Around X
                rotation_around_x.tick(dt);
                // Rotation Around Y
                rotation_around_y.tick(dt);
            }
            #endregion

            Utility.Controller.IInputVector<float> airPushInput;

            public float slow_input = 0;

            public Tween<Vector2> cameraRotation = new Tween<Vector2>(Vector2.zero, UnityTick.FIXED_UPDATE, Easing.DynaEase.Out);

            public void Init(
                float rotation_around_x_stick_time_to_reach_target,
                float rotation_around_y_time_to_reach_target
                )
            {

                // Rotation Around X
                rotation_around_x = new Smooth<float>(0, rotation_around_x_stick_tick);
                this.rotation_around_x_time_to_reach_target = rotation_around_x_stick_time_to_reach_target;
                // Rotation Around Y
                rotation_around_y = new Smooth<float>(0, rotation_around_y_tick);
                this.rotation_around_y_time_to_reach_target = rotation_around_y_time_to_reach_target;

            }
        }

        public State state = new State();
        
        public Utility.Controller.IInputVector<float> GetWingsInput()
        {
            return config.xbox_gamepad.GetAxisPositive(config.wings);
        }
        #endregion

        #region Unity
        /*********
         * UNITY *
         *********/

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            HandleInputs();
            state.TickSmooth(Time.deltaTime);
            Control(config.character, Time.deltaTime);
        }
        #endregion

        #region Behaviour
        /*************
         * BEHAVIOUR *
         *************/
        
        private void Init()
        {
            // Initialize State
            state.Init(
                config.timeToReachTargetRotX,
                config.timeToReachTargetRotY              
                );
        }
        
        private void HandleInputs()
        {
            var gamepad = config.xbox_gamepad;
            
            // Rotation Around X
            state.rotation_around_x.set_target(
                (config.inverseRotAroundX ? -1 : 1) * gamepad.GetAxisNegPos(config.rotationAroundX).value
                );

            // Rotation Around Y
            state.rotation_around_y.set_target(
                (config.inverseRotAroundY ? -1 : 1) * gamepad.GetAxisNegPos(config.rotationAroundY).value
                );
            
            state.slow_input = 1 - gamepad.GetAxisPositive(config.slow).value;
            
            // Camera
            state.cameraRotation.SetTarget(
                new Vector2(
                    (config.inverseCameraRotationX ? -1 : 1) * gamepad.GetAxisNegPos(config.cameraRotationX).value,
                    (config.inverseCameraRotationY ? -1 : 1) * gamepad.GetAxisNegPos(config.cameraRotationY).value
                    )
                );
            state.cameraRotation.time_factor = 1 / config.timeToReachCameraRotation;

            Debug.Log(state.cameraRotation.get_value());
        }

        private void Control(Character chara, float dt)
        {

        }
        #endregion

    }
}