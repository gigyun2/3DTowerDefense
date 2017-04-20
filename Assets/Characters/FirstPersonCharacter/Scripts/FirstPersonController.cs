using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson {
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class FirstPersonController : AttackableController {
        public int money;
        //private GameObject holding;
        public List<GameObject> towers;
        public List<GameObject> barriers;
		public List<GameObject> traps;
        public List<GameObject> weapons;


        [SerializeField]
        private bool m_IsWalking;
        [SerializeField]
        private float m_WalkSpeed;
        [SerializeField]
        private float m_RunSpeed;
        [SerializeField]
        [Range(0f, 1f)]
        private float m_RunstepLenghten;
        [SerializeField]
        private float m_JumpSpeed;
        [SerializeField]
        private float m_StickToGroundForce;
        [SerializeField]
        private float m_GravityMultiplier;
        [SerializeField]
        private MouseLook m_MouseLook;
        [SerializeField]
        private bool m_UseFovKick;
        [SerializeField]
        private FOVKick m_FovKick = new FOVKick();
        [SerializeField]
        private bool m_UseHeadBob;
        [SerializeField]
        private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField]
        private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField]
        private float m_StepInterval;
        [SerializeField]
        private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField]
        private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField]
        private AudioClip m_LandSound;           // the sound played when character touches back on ground.

        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;

        // Use this for initialization
        override public void Start () {
            base.Start();
            // TODO: add list of current objects
            weapons.Add((GameObject)Resources.Load("Weapon"));
            towers.Add((GameObject)Resources.Load("Tower"));
            barriers.Add((GameObject)Resources.Load("Barrier"));
			traps.Add ((GameObject)Resources.Load ("Trap"));
			//holding = GameObject.Find("Weapon");

            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
            m_MouseLook.Init(transform, m_Camera.transform);
        }


        // Update is called once per frame
        private void Update () {
            base.Update();
            /*if (Input.GetMouseButtonDown(0) && holding != null) {
                switch (holding.tag) {
					case "Tower":
						Destroy (holding);
						buildTower (0);
						holding = Instantiate (weapons [0]);
						holding.transform.SetParent (this.transform.GetChild (1));
                        break;
                    case "Barrier":
						Destroy(holding);
                        buildBarrier(0);
						holding = Instantiate(weapons[0]);
						holding.transform.SetParent (this.transform.GetChild (1));
                        break;
					case "Trap":
						Destroy(holding);
						buildTrap (0);
						holding = Instantiate(weapons[0]);
						holding.transform.SetParent (this.transform.GetChild (1));
						break;
                    case "Weapon":
                        fire();
                        break;
                }
            }
            */
			if (Input.GetMouseButton(0)) {
				fire();
			}
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				//Instantiate (weapons [0], transform.position + new Vector3 (0.6f, 0.25f, 1), transform.rotation);
			} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
				buildTower (0);
			} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
				buildBarrier (0);
			} else if (Input.GetKeyDown (KeyCode.Alpha4)) {
				buildTrap (0);
			}

            /*if (Input.GetKeyDown(KeyCode.Alpha1)) {
                Destroy(holding);
                holding = Instantiate(weapons[0], transform.position + new Vector3(0.6f, 0.25f, 1), transform.rotation);
				holding.transform.SetParent (this.transform.GetChild (1));
			} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                Destroy(holding);
				holding = Instantiate(towers[0]);
				holding.transform.SetParent (this.transform.GetChild (1));
				holding.transform.localPosition = new Vector3 (0.5f, -0.5f, 1);
				holding.transform.localScale = new Vector3 (0.05f, 0.05f, 0.05f);
			} else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                Destroy(holding);
                holding = Instantiate(barriers[0]);
				holding.transform.SetParent (this.transform.GetChild (1));
            }
			*/
			/*
            if (holding.tag.Equals("Weapon")) {
                holding.transform.position = this.transform.position + Vector3.right;
            } else {
                //if (Physics.Raycast())
            }
			*/

            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump) {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded) {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded) {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
        }


        
		private void buildTower (int index) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 100f);
            if (hit.collider != null && hit.collider.tag.Equals("Tile_placeable")) {
				GameObject tower = GameObject.Instantiate(towers[index]);
				tower.transform.position = hit.collider.transform.position;
                Quaternion quaternion = Quaternion.LookRotation(this.transform.position - hit.transform.position);
                quaternion.x = 0;
                quaternion.z = 0;
                tower.transform.rotation = quaternion;
                towers.Add(tower);
            } else {

                GameObject damageText = new GameObject();
                damageText.name = "Failed";
                TextMesh textMesh = damageText.AddComponent<TextMesh>();
                textMesh.text = "Inappropriate position";
                textMesh.fontSize = 4;
                textMesh.anchor = TextAnchor.MiddleCenter;
                damageText.transform.position = hit.collider.transform.position;
                damageText.transform.rotation = Camera.main.transform.rotation;
                GameObject.Destroy(damageText, 2f);
            }
        }

        private void buildBarrier (int index) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			Physics.Raycast(ray, out hit, 100f);
			if (hit.collider != null && hit.collider.tag.Equals("Tile_walkable")) {
				GameObject go = GameObject.Instantiate(barriers[index]);
				go.transform.position = hit.collider.transform.position + Vector3.up;
            } else {

                GameObject damageText = new GameObject();
                damageText.name = "Failed";
                TextMesh textMesh = damageText.AddComponent<TextMesh>();
                textMesh.text = "Inappropriate position";
                textMesh.fontSize = 4;
                textMesh.anchor = TextAnchor.MiddleCenter;
                damageText.transform.position = hit.collider.transform.position;
                damageText.transform.LookAt(this.transform.position);
                GameObject.Destroy(damageText, 2f);
            }
        }

		private void buildTrap (int index) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			Physics.Raycast(ray, out hit, 100f);
			if (hit.collider != null && hit.collider.tag.Equals("Tile_walkable")) {
				GameObject go = GameObject.Instantiate(traps[index]);
				go.transform.position = hit.collider.transform.position + Vector3.up * 0.1f;
                go.transform.rotation = Quaternion.identity;

            } else {

                GameObject damageText = new GameObject();
                damageText.name = "Failed";
                TextMesh textMesh = damageText.AddComponent<TextMesh>();
                textMesh.text = "Inappropriate position";
                textMesh.fontSize = 4;
                textMesh.anchor = TextAnchor.MiddleCenter;
                damageText.transform.position = hit.collider.transform.position;
                damageText.transform.LookAt(this.transform.position);
                GameObject.Destroy(damageText, 2f);
            }
        }

        private void changeWeapon (int index) {
            //holding = weapons[index];
        }

        private void fire () {
            if (cd <= 0) {
                GameObject gun = GameObject.Find("Weapon");
                Transform firepoint = gun.transform.FindChild("FirePoint");
                GameObject bullet = Instantiate((GameObject)Resources.Load("Projectile"), firepoint);
                bullet.transform.localScale = new Vector3 (5, 5, 5);
                bullet.GetComponent<ProjectileController>().direction = firepoint.forward;
                bullet.GetComponent<ProjectileController>().flySpeed = 10;
                bullet.GetComponent<ProjectileController>().atk = 5;
                bullet.transform.SetParent(null);
                cd = 1 / speed;
            }
        }

        protected override void die () {
            // popup Die UI
        }


        private void PlayLandingSound () {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void FixedUpdate () {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x * speed;
            m_MoveDir.z = desiredMove.z * speed;


            if (m_CharacterController.isGrounded) {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump) {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
            } else {
                m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            m_MouseLook.UpdateCursorLock();
        }


        private void PlayJumpSound () {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle (float speed) {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0)) {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep)) {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio () {
            if (!m_CharacterController.isGrounded) {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        private void UpdateCameraPosition (float speed) {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob) {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded) {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed * (m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            } else {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput (out float speed) {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1) {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0) {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }


        private void RotateView () {
            m_MouseLook.LookRotation(transform, m_Camera.transform);
        }


        private void OnControllerColliderHit (ControllerColliderHit hit) {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below) {
                return;
            }

            if (body == null || body.isKinematic) {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }
    }
}
