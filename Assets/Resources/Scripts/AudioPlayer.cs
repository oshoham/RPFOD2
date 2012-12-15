using UnityEngine;

/*
 * Makes it easier to play sounds after an object is destroyed
 * and doesn't leave old AudioSources lying around.
 */
public class AudioPlayer : MonoBehaviour {
	public AudioSource source;
	public AudioClip clip;
	public float timeStarted;
	
	void Update() {
		// If the sound is over, destroy this.
		if(Time.time > timeStarted + clip.length) {
			Destroy(clip);
			Destroy(source);
			Destroy(gameObject);
		}
	}
	
	public static GameObject PlayAudio(string clip, float volume) {
		GameObject player = new GameObject("Audio player: " + clip);
		AudioPlayer script = player.AddComponent<AudioPlayer>();
		script.clip = Resources.Load(clip) as AudioClip;
		script.source = player.AddComponent<AudioSource>();
		script.source.clip = script.clip;
		script.source.Play();
		script.timeStarted = Time.time;
		return player;
	}
}