using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tweening;

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
            public Controller controller;

            public UnityEngine.Camera unityCamera;

            [Header("Animations")]
            public Animator unityAnimator;

            [Header("FoV")]
            public float fovAtMinSpeed = 50f;
            public float fovAtMaxSpeed = 90f;

            [Header("Disance")]
            public float distAtMinSpeed = 24f;
            public float distAtMaxSpeed = 16f;

            [Header("Offsets")]
            public Vector3 watchedOffsetAtMinSpeed = Vector3.zero;
            public Vector3 watchedOffsetAtMaxSpeed = Vector3.zero;
            public float stickOffset = 0f;

            [Header("Control")]
            public Vector2 controlAnlge = Vector2.one * 30f;

            [Header("Trail")]
            public Material mTrailMat;

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
            public Tween<Quaternion> smooth_rotation = new Tween<Quaternion>(Quaternion.identity, UnityTick.FIXED_UPDATE, SmoothRotationEasing, 1 / 3.5f);
            public static Quaternion SmoothRotationEasing(Quaternion current, Quaternion target, float dt, float time_factor)
            {
                float angle = Quaternion.Angle(current, target);
                //angle = angle * angle;
                return Quaternion.Slerp(current, target, angle * dt * time_factor);
            }

            public Tween<Vector2> smooth_target_offset = new Tween<Vector2>(Vector2.zero, UnityTick.FIXED_UPDATE, Easing.DynaEase.Out, 1 / 1.25f);

            // Trail
            public Tween<float> mTrailOpacity = new Tween<float>(0.2f, UnityTick.UPDATE, Easing.DynaEase.Out, 1 / 0.125f);
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
            InitAnimator();
        }

        private void OnDestroy()
        {
            KillAnimator();
        }

        private void Start()
        {
            WakeUp();
        }

        private void Update()
        {
            UnityEngine.Camera unity_camera = config.unityCamera;
            RefreshFov(unity_camera);
            RefreshOffet();

            RefreshAnimator();

            RefreshTrail();
        }

        private void FixedUpdate()
        {
            Behave(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            Vector3 current_camera_position = transform.position;
            Vector3 current_watched_position = GetWatchedPosition();
            Quaternion current_watched_rotation = config.character.transform.rotation;
            Quaternion current_camera_rotation = transform.rotation;

            Vector3 from_watched_to_camera = current_camera_position - current_watched_position;
            Vector3 from_camera_to_watched = current_watched_position - current_camera_position;

            /// ROTATION            
            // Target rotation
            float stall_factor = config.character.state.stall_factor.get_value();
            Quaternion target_camera_rotation =
                Quaternion.LookRotation(
                    from_camera_to_watched,
                    Vector3.Slerp(
                        current_watched_rotation * Vector3.up,
                        Vector3.up,
                        stall_factor
                    )
                );

            Quaternion turn_rotation = Quaternion.AngleAxis(5 * -config.controller.state.rotation_around_y.get_value(), Vector3.forward);
            target_camera_rotation *= turn_rotation;

            //Vector2 control_camera_rotation_input = config.controller.state.cameraRotation.get_value();
            //Quaternion control_camera_rotation = Quaternion.Euler(control_camera_rotation_input.y * config.controlAnlge.y, control_camera_rotation_input.x * config.controlAnlge.x, 0);
            //target_camera_rotation *= control_camera_rotation;

            state.smooth_rotation.SetTarget(target_camera_rotation);
            // @improvement : Smoothing
            Quaternion new_camera_rotation = target_camera_rotation; state.smooth_rotation.get_value();

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
            Quaternion  current_watched_rotation    = config.character.transform.rotation;
            Quaternion  current_camera_rotation     = transform.rotation;
            Vector3     current_camera_position     = transform.position;
            Vector3     current_watched_position    = GetWatchedPosition();

            Vector3     from_watched_to_camera      = current_camera_position - current_watched_position;
            Vector3     from_camera_to_watched      = current_watched_position - current_camera_position;


            float stall_factor = config.character.state.stall_factor.get_value();

            /// POSITION
            // Target position
            float dist_from_avatar = GetDistFromAvatar();

            // Camera control
            Vector2 control_camera_rotation_input = config.controller.state.cameraRotation.get_value();
            Quaternion control_camera_rotation = Quaternion.Euler(control_camera_rotation_input.y * config.controlAnlge.y, control_camera_rotation_input.x * config.controlAnlge.x, 0);

            Vector3 target_camera_position = current_watched_rotation * control_camera_rotation * -Vector3.forward;



            target_camera_position = Vector3.Slerp(target_camera_position, Vector3.ProjectOnPlane(current_watched_rotation * Vector3.up, Vector3.up), stall_factor);
            target_camera_position = target_camera_position.normalized * dist_from_avatar;
            target_camera_position += current_watched_position;

            float dist_to_target = Vector3.Distance(current_camera_position, target_camera_position);
            float time_to_reach_target = stall_factor * 0.5f;
            // @improvement : Smoothing
            Vector3 new_camera_position;
            if(time_to_reach_target > 0)
            {
                new_camera_position = Vector3.MoveTowards(current_camera_position, target_camera_position, (dist_to_target * dt) / time_to_reach_target);
            }
            else
            {
                new_camera_position = target_camera_position;
            }

            /// Apply
            // Position
            transform.position = new_camera_position;
            //// Rotation
            //transform.rotation = new_camera_rotation;            
        }

        public void RefreshOffet()
        {
            Vector2 offset = new Vector2(config.controller.state.rotation_around_y.get_value(), -config.controller.state.rotation_around_x.get_value());
            state.smooth_target_offset.SetTarget(offset);
        }

        public void RefreshFov(UnityEngine.Camera camera)
        {
            float relative_speed = config.character.GetRelativeSpeed();
            float fov = Mathf.Lerp(config.fovAtMinSpeed, config.fovAtMaxSpeed, relative_speed);
            camera.fieldOfView = fov;
        }

        public Vector3 GetWatchedPosition()
        {
            float stall_factor = config.character.state.stall_factor.get_value();
            Quaternion current_watched_rotation = config.character.transform.rotation;
            Vector3 current_watched_position = config.character.transform.position;
            float relative_speed = config.character.GetRelativeSpeed();

            Vector3 smooth_target_offset = state.smooth_target_offset.get_value();
            float smooth_target_offset_magnitude = smooth_target_offset.magnitude;
            smooth_target_offset *= 1 - stall_factor; // Dont apply offset if stalling
            //smooth_target_offset *= 1 - relative_speed; // Dont apply if fast
            smooth_target_offset *= config.stickOffset; // Apply offset strengh chosen in config
            smooth_target_offset = current_watched_rotation * smooth_target_offset; // Apply rotation

            Vector3 watched_offset = current_watched_rotation * GetWatchedOffset();
            watched_offset *= (1 - smooth_target_offset_magnitude); // Dont apply offset if player is changing direction
            watched_offset *= 1 - stall_factor; // Dont apply offset if stalling
            
            current_watched_position += watched_offset;
            current_watched_position += smooth_target_offset;
            return current_watched_position;
        }

        public float GetDistFromAvatar()
        {
            float relative_speed = config.character.GetRelativeSpeed();
            float dist_from_avatar = Mathf.Lerp(config.distAtMinSpeed, config.distAtMaxSpeed, relative_speed);

            return dist_from_avatar;
        }

        public Vector3 GetWatchedOffset()
        {
            float relative_speed = config.character.GetRelativeSpeed();
            return Vector3.Lerp(config.watchedOffsetAtMinSpeed, config.watchedOffsetAtMaxSpeed, relative_speed);
        }
        public void InitAnimator()
        {
            config.character.state.air_push.state.eOnCast += OnWingsCast;

        }
        public void KillAnimator()
        {
            config.character.state.air_push.state.eOnCast -= OnWingsCast;

        }

        public void RefreshAnimator()
        {
            Animator anim = config.unityAnimator;
            Transform transform = anim.transform.parent;

            // Turn
            anim.SetBool("Turning", Mathf.Abs(config.controller.state.rotation_around_y.get_value()) > 0.1);            
            Quaternion turn_rotation = Quaternion.AngleAxis(20 * -config.controller.state.rotation_around_y.get_value(), Vector3.forward);
            transform.localRotation = turn_rotation;

            // Dive
            anim.SetBool("Diving", Mathf.Abs(config.character.state.wings_deployment.state.wings_deployment) < 0.5f);
        }
        public void OnWingsCast()
        {
            Animator anim = config.unityAnimator;
            anim.SetTrigger("Flap");
        }

        public void RefreshTrail()
        {
            Character chara = config.character;
            bool flapping = chara.state.air_push.state.ability_progress > 0f;
            state.mTrailOpacity.SetTarget(flapping ? 0.0f : Mathf.Lerp(0.75f, 0.2f, config.character.state.wings_deployment.state.wings_deployment));
            state.mTrailOpacity.time_factor = flapping ? 1f / 0.05f : 1f / 2f;

            config.mTrailMat.SetFloat("_Opacity", state.mTrailOpacity.get_value());
        }


        #endregion
    }
}
