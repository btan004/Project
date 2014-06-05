using UnityEngine;
using System.Collections;

public class SlimeTrapScript : MonoBehaviour {

	//the victim of the slime trap
	private GameObject victim;
	private Vector3 victimPosition;
	public bool HasVictim = false;
	public float Duration = 2f;



	void Start () 
	{
		renderer.material.color = Color.green;
	}
	
	void Update()
	{
		if (HasVictim)
		{
			Duration -= Time.deltaTime;
		}

		if (Duration <= 0)
			Destroy(this.gameObject);

	}

	void LateUpdate () 
	{
		//make sure the victim stays in place
		if (HasVictim)
		{
			victim.transform.position = victimPosition;
		}
	}

	public void ActivateTrap(PlayerScript player)
	{
		this.victim = player.gameObject;
		victimPosition = player.gameObject.transform.position;
		HasVictim = true;

		if (!audio.isPlaying && HasVictim)
			audio.Play();
	}
}
