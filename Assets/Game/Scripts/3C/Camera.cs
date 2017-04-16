﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Avatar2
{
    public class Camera : MonoBehaviour
    {
        #region Configuration
        /*****************
         * CONFIGURATION *
         *****************/
        [System.Serializable]
        public class Configuration
        {
            [Header("Watched Character")]
            // Character
            public Character character;
        }

        // Configuration Instance
        public Configuration config;
        #endregion
        
        #region State
        /*********
         * STATE *
         *********/
        public class State
        {

        }

        public State state = new State();
        #endregion
        
        #region Unity
        /*********
         * UNITY *
         *********/

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            WakeUp();
        }

        private void Update()
        {

        }

        private void FixedUpdate()
        {
            Behave(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            Vector3 current_camera_position = transform.position;
            Vector3 current_watched_position = config.character.transform.position;
            Quaternion current_watched_rotation = config.character.transform.rotation;
            Quaternion current_camera_rotation = transform.rotation;

            Vector3 from_watched_to_camera = current_camera_position - current_watched_position;
            Vector3 from_camera_to_watched = current_watched_position - current_camera_position;

            /// ROTATION            
            // Target rotation
            Quaternion target_camera_rotation = Quaternion.LookRotation(from_camera_to_watched, current_watched_rotation * Vector3.up);
            // @improvement : Smoothing
            Quaternion new_camera_rotation = target_camera_rotation;

            // Rotation
            transform.rotation = new_camera_rotation;            
        }

        private void OnPreCull()
        {

        }

        #endregion

        #region Behaviour
        /*************
         * BEHAVIOUR *
         *************/

        private void Init()
        {

        }

        private void WakeUp()
        {
            
        }

        private void Behave(float dt)
        {
            Vector3     current_camera_position     = transform.position;
            Vector3     current_watched_position    = config.character.transform.position;
            Quaternion  current_watched_rotation    = config.character.transform.rotation;
            Quaternion  current_camera_rotation     = transform.rotation;

            Vector3     from_watched_to_camera      = current_camera_position - current_watched_position;
            Vector3     from_camera_to_watched      = current_watched_position - current_camera_position;

            /// POSITION
            // Target position
            const float distance_to_watched = 24f;
            Vector3 target_camera_position = current_watched_rotation * -Vector3.forward;
            target_camera_position = target_camera_position.normalized * distance_to_watched;
            target_camera_position += current_watched_position;
            // @improvement : Smoothing
            Vector3 new_camera_position = target_camera_position;
            
            /// Apply
            // Position
            transform.position = new_camera_position;
            //// Rotation
            //transform.rotation = new_camera_rotation;            
        }

        #endregion
    }
}
