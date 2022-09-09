using UnityEngine;
using System.Collections;


namespace VFX_Explosion_Pack
{

	public class AnimHelper : MonoBehaviour {
		public ParticleSystem[] reseed_particles;
		public float selfdestruct_in = 4; // SET THIS TO 0 TO NOT SELFDESTRUCT!.


		void Awake () {
			uint set_random_seed = (uint) Random.Range(0, 9999999999);			
			for(int i = 0; i < reseed_particles.Length; i++){
				reseed_particles[i].randomSeed = set_random_seed;
				reseed_particles[i].Play();
			}
		}


		void Start (){
			if ( selfdestruct_in != 0){ 
				Destroy (gameObject, selfdestruct_in);
			}
		}
		
	}

}
