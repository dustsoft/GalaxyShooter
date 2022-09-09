using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VFX_Explosion_Pack {

	public class GameManager : MonoBehaviour {
		public TextMesh text_fx_name;
		public GameObject[] fx_prefabs;
		public int index_fx = 0;
		private bool FOLLOW = false;
		private Transform Trail = null;
		private Ray ray;
		private RaycastHit2D ray_cast_hit;



		void Start () 
		{
			text_fx_name.text = "[" + (index_fx + 1) + "] " + fx_prefabs[ index_fx ].name;
		}


		void Update () 
		{
			TrailFollowMouse();
			if (Input.GetMouseButtonDown (0)) 
			{
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				ray_cast_hit = Physics2D.Raycast(new Vector2(ray.origin.x, ray.origin.y), new Vector2(0,0));
				if (ray_cast_hit) 
				{
					switch(ray_cast_hit.transform.name){
					case "BG":
						if(index_fx != 21){
							Instantiate(fx_prefabs[ index_fx ], new Vector3(ray.origin.x, ray.origin.y, 0), Quaternion.identity);
						}
						break;
					case "UI-arrow-right":
						ray_cast_hit.transform.SendMessage("Go");
						index_fx++;
						if(index_fx >= fx_prefabs.Length)
							index_fx = 0;
						text_fx_name.text = "[" + (index_fx + 1) + "] " + fx_prefabs[ index_fx ].name;
						if(index_fx == 21){
						text_fx_name.text = "[21] Trail (Move mouse)";
						GameObject o_temp;
						ray = Camera.main.ScreenPointToRay(Input.mousePosition);
						if(Trail == null){
							o_temp = Instantiate(fx_prefabs[ 21 ], new Vector3(ray.origin.x, ray.origin.y, 0), Quaternion.identity);
							Trail = o_temp.transform;
						}
							
						FOLLOW = true;
						}else{
							if(Trail != null){
								Destroy(Trail.gameObject);
								Trail = null;
							}
							FOLLOW = false;
						}
							break;
					case "UI-arrow-left":
						ray_cast_hit.transform.SendMessage("Go");
						index_fx--;
						if(index_fx <= -1)
							index_fx = fx_prefabs.Length - 1;
						text_fx_name.text = "[" + (index_fx + 1) + "] " + fx_prefabs[ index_fx ].name;
						if(index_fx == 21){
						text_fx_name.text = "[21] Trail (Move mouse)";
						GameObject o_temp;
						ray = Camera.main.ScreenPointToRay(Input.mousePosition);
						if(Trail == null){
							o_temp = Instantiate(fx_prefabs[ 21 ], new Vector3(ray.origin.x, ray.origin.y, 0), Quaternion.identity);
							Trail = o_temp.transform;
						}
							
						FOLLOW = true;
						}else{
							if(Trail != null){
								Destroy(Trail.gameObject);
								Trail = null;
							}
							FOLLOW = false;
						}
						break;
					case "Instructions":
						Destroy(ray_cast_hit.transform.gameObject);
						break;
					}
				}
			}
			//Change-FX keyboard..	
			if ( Input.GetKeyDown("z") || Input.GetKeyDown("left") ){
				GameObject.Find("UI-arrow-left").SendMessage("Go");
				index_fx--;
				if(index_fx <= -1)
					index_fx = fx_prefabs.Length - 1;
				text_fx_name.text = "[" + (index_fx + 1) + "] " + fx_prefabs[ index_fx ].name;
				if(index_fx == 21){
					text_fx_name.text = "[21] Trail (Move mouse)";
					GameObject o_temp;
					ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					if(Trail == null){
						o_temp = Instantiate(fx_prefabs[ 21 ], new Vector3(ray.origin.x, ray.origin.y, 0), Quaternion.identity);
						Trail = o_temp.transform;
					}
						
					FOLLOW = true;
				}else{
					if(Trail != null){
						Destroy(Trail.gameObject);
						Trail = null;
					}
					FOLLOW = false;
				}
			}

			if ( Input.GetKeyDown("x") || Input.GetKeyDown("right")){
				GameObject.Find("UI-arrow-right").SendMessage("Go");
				index_fx++;
				if(index_fx >= fx_prefabs.Length)
					index_fx = 0;
				text_fx_name.text = "[" + (index_fx + 1) + "] " + fx_prefabs[ index_fx ].name;
				if(index_fx == 21){
					text_fx_name.text = "[21] Trail (Move mouse)";
					GameObject o_temp;
					ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					if(Trail == null){
						o_temp = Instantiate(fx_prefabs[ 21 ], new Vector3(ray.origin.x, ray.origin.y, 0), Quaternion.identity);
						Trail = o_temp.transform;
					}
						
					FOLLOW = true;
				}else{
					if(Trail != null){
						Destroy(Trail.gameObject);
						Trail = null;
					}
					FOLLOW = false;
				}
			}

			if ( Input.GetKeyDown("space") ){
				if(index_fx != 21)
					Instantiate(fx_prefabs[ index_fx ], new Vector3(0, 0, 0), Quaternion.identity);
			}
		}


		void TrailFollowMouse(){
			if(FOLLOW){
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Trail.position = new Vector3(ray.origin.x, ray.origin.y, 0);
			}
		}


	}
}