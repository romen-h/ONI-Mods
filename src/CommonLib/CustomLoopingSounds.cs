
using FMOD;

using FMODUnity;

using UnityEngine;

namespace RomenH.CommonLib
{
	public class CustomLoopingSounds : KMonoBehaviour, IRenderEveryTick
	{
		public string soundName;

		public bool shouldPauseOnGamePause = true;

		public bool enableCulling = true;

		public bool enableCameraScaling = true;

		public float volume = 1f;

		public float falloffDistanceSq = 64f;

		public Vector3 velocity = Vector3.zero;

		private FMOD.Channel sound;

		protected override void OnSpawn()
		{
			base.OnSpawn();

			sound = AudioUtil.CreateSound(soundName);
		}

		public void RenderEveryTick(float dt)
		{
			SoundCuller soundCuller = CameraController.Instance.soundCuller;

			if (shouldPauseOnGamePause && Time.timeScale == 0f)
			{
				sound.setPaused(true);
				return;
			}

			bool audible = !enableCulling || (enableCameraScaling && soundCuller.IsAudible(transform.position, falloffDistanceSq)) || soundCuller.IsAudibleNoCameraScaling(transform.position, falloffDistanceSq);
			if (!audible)
			{
				sound.setPaused(true);
				return;
			}

			sound.setPaused(false);

			Vector3 soundPos = transform.position;
			soundPos.z = 0;

			float velocityScale = TuningData<LoopingSoundManager.Tuning>.Get().velocityScale;
			VECTOR scaledSoundPos = SoundEvent.GetCameraScaledPosition(soundPos, false).ToFMODVector();
			VECTOR scaledSoundVel = (velocity * velocityScale).ToFMODVector();
			sound.set3DAttributes(ref scaledSoundPos, ref scaledSoundVel);
		}

		protected override void OnCleanUp()
		{
			base.OnCleanUp();

			if (sound.hasHandle())
			{
				sound.isPlaying(out bool playing);
				if (playing) sound.stop();
				sound.clearHandle();
			}
		}
	}
}
