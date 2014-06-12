using UnityEngine;
using System.Collections;

public class SendScoreScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// url to send post request
		string url = "http://bolttunes.com/arena/newscore";

		// Create post form
		WWWForm form = new WWWForm ();
		form.AddField("username","hey");
		form.AddField("userscore","8500");
		WWW www = new WWW (url, form);

		// Send form
		StartCoroutine (WaitForRequest (www));
	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		
		// check for errors
		if (www.error == null)
		{
			Debug.Log("Request Successful: " + www.data);
		} else {
			Debug.Log("Request Failed: "+ www.error);
		}    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
