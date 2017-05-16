using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawn : MonoBehaviour
{
	public GameObject waterParticle;
	public string waterTag = "Water";
	public GameObject grassParticle;
	public string grassTag = "Grass";


	public float spawnDistance = 3f;
	public float particleLifetime = 2f;
	public float spawnTimer = 0.2f;

	float timer;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.up * -1f, out hit, spawnDistance))
		{
			timer += Time.deltaTime;
			if(timer>spawnTimer)
			{
				timer = 0f;
				if(hit.collider.tag == grassTag)
				{
					Debug.Log("Spawning grass particle");
					GameObject grassP = Instantiate(grassParticle, hit.point, grassParticle.transform.rotation);
					Destroy(grassP, particleLifetime);
				}
				else if(hit.collider.tag == waterTag)
				{
					Debug.Log("Spawning water particle");
					GameObject waterP = Instantiate(waterParticle, hit.point, waterParticle.transform.rotation);
					Destroy(waterP, particleLifetime);
				}
			}
			Debug.Log("Touching something !");
		}	
	}
}
