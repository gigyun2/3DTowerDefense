using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]

public class rigidBodySleep_Activated : MonoBehaviour
{

		public rigidBodyProperty myRigidBodyProperty = new rigidBodyProperty ();

		public AudioSource defaultHitSound;

		private bool Coll = false;
	
		void Awake ()
		{
				//	CancelInvoke ("rigidBodySleep");
				Time.timeScale = 0;
	

				this.audio.volume = 1;
				defaultHitSound.audio.volume = 1;
	
				foreach (Rigidbody RigidB in GameObject.FindObjectsOfType(typeof(Rigidbody))) {
						if (RigidB.gameObject.tag.Contains ("WallLayer")) {
								RigidB.name = RigidB.GetInstanceID ().ToString ();
								RigidB.Sleep ();
								RigidB.gameObject.audio.volume = 1;
								RigidB.gameObject.audio.rolloffMode = AudioRolloffMode.Linear;
						}
				}	 
	
				Time.timeScale = 1;
		}

		void OnCollisionExit (Collision other)
		{ 
				Coll = false;
		} 


		void  OnCollisionEnter (Collision other)
		{ 
				Coll = true;
				if (other.collider.tag.ToString () == "WallLayer") {
						myRigidBodyProperty = other.collider.gameObject.GetComponent<setRigidBodyProperty> ().myRigidBodyProperty;
						other.collider.gameObject.audio.clip = myRigidBodyProperty.ExplosionSound;
						Dest (other.collider.gameObject.name);
						if (myRigidBodyProperty.ExplosionSound) {
								other.collider.gameObject.audio.Play ();
						}
						if (myRigidBodyProperty.particleExplosion) {
								#if DEBUG
								Debug.Log ("Explosion");
								#endif
								Instantiate (myRigidBodyProperty.particleExplosion, other.collider.gameObject.transform.position, Quaternion.identity);
						}
						if (myRigidBodyProperty.destroyedObject) {
								GameObject objectDestroyed = (GameObject)Instantiate (myRigidBodyProperty.destroyedObject, other.collider.gameObject.transform.position, Quaternion.identity);
								objectDestroyed.audio.clip = myRigidBodyProperty.ExplosionSound;
						 
								objectDestroyed.audio.volume = 1;
								objectDestroyed.audio.Play ();
								GameObject.Destroy (other.collider.gameObject);
						}
				} else {
						if (defaultHitSound != null && other.collider.tag.ToString () != "Terrain") {
								defaultHitSound.gameObject.audio.volume = 1;
								defaultHitSound.gameObject.audio.Play ();
						}
				}
		}

		void Dest (string Obj)
		{
		
				Rigidbody RigidObj = GameObject.Find (Obj).GetComponent <Rigidbody> ();
				if (RigidObj && Coll == true) {
						RigidObj.isKinematic = false;
						RigidObj.WakeUp (); 
						OnRigidBody (RigidObj);
				}
		}

		void OnRigidBody (Rigidbody RB)
		{
				if (Coll == true) {
						if (myRigidBodyProperty.particleSmoke && RB.name.IndexOf ("particle") == -1) {
								GameObject parent = (GameObject)Instantiate (myRigidBodyProperty.particleSmoke, new Vector3 (0, 0, 0), Quaternion.identity);
								RB.name = RB.name + "particle";
								parent.transform.parent = RB.transform;
								parent.transform.localPosition = Vector3.zero;
								parent.transform.localRotation = Quaternion.EulerRotation (0, 0, 0);
						}
				}
		}
		/*	void rigidBodySleep ()
		{
				foreach (Rigidbody RigidB in GameObject.FindObjectsOfType(typeof(Rigidbody))) {
						float speed = Vector3.Distance (new Vector3 (0, 0, 0), RigidB.velocity);

						if (speed < 0.00008602529f) {
								RigidB.Sleep ();
								//	Debug.Log (RigidB.gameObject.name + "speed:" + speed);
						}
				}	 
		}
	 	void OnDesroy ()
		{ 
				CancelInvoke ("rigidBodySleep");
		} */
}
