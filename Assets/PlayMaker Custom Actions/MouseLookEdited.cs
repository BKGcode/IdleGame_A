﻿// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	/// <summary>
	/// Action version of Unity's builtin MouseLook behaviour.
	/// TODO: Expose invert Y option.
	/// </summary>
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Rotates a GameObject based on mouse movement. Minimum and Maximum values can be used to constrain the rotation.")]
	public class MouseLookEdited : FsmStateAction
	{
		public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }

		[RequiredField]
		[Tooltip("The GameObject to rotate.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("The axes to rotate around.")]
		public RotationAxes axes = RotationAxes.MouseXAndY;

		[RequiredField]
		[Tooltip("Sensitivity of movement in X direction.")]
		public FsmFloat sensitivityX;

		[RequiredField]
		[Tooltip("Sensitivity of movement in Y direction.")]
		public FsmFloat sensitivityY;

		[HasFloatSlider(-360,360)]
        [Tooltip("Clamp rotation around X axis. Set to None for no clamping.")]
		public FsmFloat minimumX;

		[HasFloatSlider(-360, 360)]
        [Tooltip("Clamp rotation around X axis. Set to None for no clamping.")]
        public FsmFloat maximumX;

		[HasFloatSlider(-360, 360)]
        [Tooltip("Clamp rotation around Y axis. Set to None for no clamping.")]
        public FsmFloat minimumY;

		[HasFloatSlider(-360, 360)]
        [Tooltip("Clamp rotation around Y axis. Set to None for no clamping.")]
        public FsmFloat maximumY;

        public FsmFloat sideRecoil;
        public FsmFloat upRecoil;

		public FsmBool pause;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		

		float rotationX;
		float rotationY;

		public override void Reset()
		{
			gameObject = null;
			axes = RotationAxes.MouseXAndY;
			sensitivityX = 15f;
			sensitivityY = 15f;
			minimumX = new FsmFloat {UseVariable = true};
            maximumX = new FsmFloat { UseVariable = true };
			minimumY = -60f;
			maximumY = 60f;
			everyFrame = true;
		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				Finish();
				return;
			}

			// Make the rigid body not change rotation
			// TODO: Original Unity script had this. Expose as option?
		    var rigidbody = go.GetComponent<Rigidbody>();
            if (rigidbody != null)
			{
				rigidbody.freezeRotation = true;
			}

            // initialize rotation

		    rotationX = go.transform.localRotation.eulerAngles.y;
			rotationY = 0;


            DoMouseLook();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoMouseLook();
		}

		void DoMouseLook()
		{
			if (pause.Value == false)
            {

				var go = Fsm.GetOwnerDefaultTarget(gameObject);
				if (go == null)
				{
					return;
				}

				var transform = go.transform;

				switch (axes)
				{
					case RotationAxes.MouseXAndY:

						transform.localEulerAngles = new Vector3(GetYRotation(), GetXRotation(), 0);
						break;

					case RotationAxes.MouseX:

						transform.eulerAngles = new Vector3(transform.eulerAngles.x, GetXRotation(), 0);
						break;

					case RotationAxes.MouseY:

						transform.localEulerAngles = new Vector3(-GetYRotation(), transform.localEulerAngles.y, 0);
						break;
				}
			}
		
		}

		float GetXRotation()
		{
			rotationX += sideRecoil.Value + Input.GetAxis("Mouse X") * sensitivityX.Value;
			rotationX = ClampAngle(rotationX, minimumX, maximumX);
            sideRecoil.Value = 0;
			return rotationX;
		}

		float GetYRotation()
		{
			rotationY += upRecoil.Value + Input.GetAxis("Mouse Y") * sensitivityY.Value;
			rotationY = ClampAngle(rotationY, minimumY, maximumY);
            upRecoil.Value = 0;
            return rotationY;
		}

		// Clamp function that respects IsNone
		static float ClampAngle(float angle, FsmFloat min, FsmFloat max)
		{
			if (!min.IsNone && angle < min.Value)
			{
				angle = min.Value;
			}

			if (!max.IsNone && angle > max.Value)
			{
				angle = max.Value;
			}
			
			return angle;
		}
	}
}