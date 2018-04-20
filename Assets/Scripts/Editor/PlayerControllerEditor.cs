using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor 
{
	private PlayerController Target;
	bool MTab,MCTab,ITab,CTab;

	void OnEnable(){
		Target = target as PlayerController;
	}

	public override void OnInspectorGUI ()
	{
		GUILayout.Space (5);
		GUILayout.Label ("Player Controller Physics ");
		GUILayout.Label("RolePlayGame (RPG):");
		GUILayout.Space (15);

		Target.EnableInput = EditorGUILayout.Toggle (new GUIContent("Enable Input","Activates the imput on controller"),Target.EnableInput);
		MouseTab ();
		GUILayout.Space (5);
		MovementTab ();
		GUILayout.Space (5);
		CharacterTab ();
		GUILayout.Space (5);
		InputTab ();
		GUILayout.Space (5);

		//base.DrawDefaultInspector ();
	}
	
	void MouseTab()
	{
		EditorGUILayout.BeginHorizontal("Button");
		GUILayout.Space (15);
		EditorGUILayout.BeginVertical ();
		MTab = EditorGUILayout.Foldout (MTab, "Mouse (Controller)", true);
		if (MTab) 
		{
			GUILayout.Space (5);
			GUI.skin.label.normal.textColor = Color.yellow;
			Target.MouseLookAt = EditorGUILayout.Toggle (new GUIContent("Mouse Look At"),Target.MouseLookAt);
			Target.MouseRayLength = EditorGUILayout.FloatField ("Mouse Ray Length", Target.MouseRayLength);
			Target.MouseRayMask = EditorGUILayout.MaskField ("Mouse Ray Mask", Target.MouseRayMask,CEditor.GetLayerMasks());
			Target.MouseRotationRate = EditorGUILayout.FloatField ("Mouse Rotation Rate", Target.MouseRotationRate);
			Target.MouseRotationAxis = EditorGUILayout.Vector3Field ("Mouse Rotation Axis", Target.MouseRotationAxis);
			Target.MouseAction = EditorGUILayout.IntField(new GUIContent("Mouse Action Index","-1 = Auto | 0 = Left Mouse | 1 = Right Mouse | 2 = Middle Mouse Buttons"), Target.MouseAction);
			Target.MouseVisible = EditorGUILayout.Toggle (new GUIContent("Mouse Visible","Activate show mouse in game."),Target.MouseVisible);
			Target.MouseMode = (CursorLockMode)EditorGUILayout.EnumPopup ("Mouse Mode", Target.MouseMode);
		}
		EditorGUILayout.EndVertical ();
		EditorGUILayout.EndHorizontal ();
	}
	
	void MovementTab()
	{
		EditorGUILayout.BeginHorizontal("Button");
		GUILayout.Space (15);
		EditorGUILayout.BeginVertical ();
		MCTab = EditorGUILayout.Foldout (MCTab, "Movement (Controller)", true);
		if (MCTab) 
		{
			EditorGUILayout.BeginVertical ("Box");
			GUILayout.Label ("Camera:");
			Target.MainCamera = EditorGUILayout.Toggle (new GUIContent("Main Camera","Activates the main or custom camera for the controller"), Target.MainCamera);
			if(!Target.MainCamera)
				Target.CameraObject = (GameObject)EditorGUILayout.ObjectField (new GUIContent("Camera Target","The camera object to manipulate"), Target.CameraObject, typeof(GameObject), true);
			Target.CameraOffset = EditorGUILayout.Vector3Field ("Camera Offset", Target.CameraOffset);
			Target.CameraSmooth = EditorGUILayout.FloatField ("Camera Smooth", Target.CameraSmooth);
			Target.CameraTurnSpeed = EditorGUILayout.FloatField ("Camera Turn Speed", Target.CameraTurnSpeed);
			Target.CameraTurnAxis = (CameraAxis)EditorGUILayout.EnumPopup ("Camera Turn Axis", Target.CameraTurnAxis);
			Target.CameraTarget = (GameObject)EditorGUILayout.ObjectField (new GUIContent("Camera Target","the target that the camera follows and look at in every frame."), Target.CameraTarget,typeof(GameObject),true);
			Target.CameraDeltaTime = EditorGUILayout.Toggle (new GUIContent("Camera Delta Smooth","uses the delta time to camera smooth system."), Target.CameraDeltaTime);
			EditorGUILayout.EndVertical ();
			GUILayout.Space (5);

			EditorGUILayout.BeginVertical ("Box");
			GUILayout.Label ("Character Movement:");
			Target.LookAtDirection = EditorGUILayout.Toggle (new GUIContent("Look At","Look at current movement direction of the controller"), Target.LookAtDirection);
			Target.LookAtDirectionRate = EditorGUILayout.FloatField ("Look At Rate", Target.LookAtDirectionRate);
			Target.LookAtDirectionMultiplier = EditorGUILayout.IntSlider ("Look At Multiplier", Target.LookAtDirectionMultiplier, -1, 1);
			Target.LookAtDirectionScale = EditorGUILayout.Vector3Field ("Look At Scale",Target.LookAtDirectionScale);
			Target.MovMode = (MovementMode)EditorGUILayout.EnumPopup ("Movement Mode", Target.MovMode);
			Target.AirControl = EditorGUILayout.Toggle ("Air Control", Target.AirControl);
			Target.AirControlScale = EditorGUILayout.FloatField ("Air Control Scale", Target.AirControlScale);
			Target.Speed = EditorGUILayout.FloatField ("Walk Speed", Target.Speed);
			Target.SpeedCap = EditorGUILayout.FloatField ("Cap Speed", Target.SpeedCap);
			Target.SpeedMode = (ForceMode)EditorGUILayout.EnumPopup ("Speed Mode", Target.SpeedMode);
			Target.JumpForce = EditorGUILayout.FloatField ("Jump Force", Target.JumpForce);
			Target.JumpCap = EditorGUILayout.IntField (new GUIContent("Jump Cap","how much times you can jump before start falling"), Target.JumpCap);
			Target.JumpMode = (ForceMode)EditorGUILayout.EnumPopup ("Jump Mode", Target.JumpMode);
			Target.GroundLength = EditorGUILayout.FloatField ("Ground Length", Target.GroundLength);
			EditorGUILayout.EndVertical ();
			GUILayout.Space (5);

		}
		EditorGUILayout.EndVertical ();
		EditorGUILayout.EndHorizontal ();
	}

	void InputTab()
	{
		EditorGUILayout.BeginHorizontal("Button");
		GUILayout.Space (15);
		EditorGUILayout.BeginVertical ();
		ITab = EditorGUILayout.Foldout (ITab, "Input (Controller)", true);
		if (ITab) 
		{
			GUI.skin.label.normal.textColor = Color.yellow;
			GUILayout.Label ("Movement:");
			Target.ForwardAction = EditorGUILayout.TextField ("Forward Action", Target.ForwardAction);
			Target.BackwardAction = EditorGUILayout.TextField ("Backward Action", Target.BackwardAction);
			Target.LeftAction = EditorGUILayout.TextField ("Left Action", Target.LeftAction);
			Target.RightAction = EditorGUILayout.TextField ("Right Action", Target.RightAction);
			Target.JumpAction = EditorGUILayout.TextField ("Jump Action", Target.JumpAction);
			Target.CrouchAction = EditorGUILayout.TextField ("Crouch Action", Target.CrouchAction);
			GUILayout.Label ("Mouse:");
			Target.FirstMouseAction = EditorGUILayout.TextField ("First Mouse Action", Target.FirstMouseAction);
			Target.SecondMouseAction = EditorGUILayout.TextField ("Second Mouse Action", Target.SecondMouseAction);
			Target.MiddleMouseAction = EditorGUILayout.TextField ("Middle Mouse Action", Target.MiddleMouseAction);
			Target.TurnLeftAction = EditorGUILayout.TextField ("Turn Left Action", Target.TurnLeftAction);
			Target.TurnRightAction = EditorGUILayout.TextField ("Turn Right Action", Target.TurnRightAction);
			GUILayout.Space (5);
		}
		EditorGUILayout.EndVertical ();
		EditorGUILayout.EndHorizontal ();
	}

	void CharacterTab()
	{
		EditorGUILayout.BeginHorizontal("Button");
		GUILayout.Space (15);
		EditorGUILayout.BeginVertical ();
		CTab = EditorGUILayout.Foldout (CTab, "Character (Controller)", true);
		if (CTab) 
		{
			GUI.skin.label.normal.textColor = Color.yellow;
			GUILayout.Label ("Character:");
			Target.Character = (GameObject)EditorGUILayout.ObjectField (new GUIContent("Character","the current controller character is similar to a child object but interpolated rotation and location for smooth control"), Target.Character, typeof(GameObject),true);
			Target.CharacterTag = EditorGUILayout.TagField ("Character Tag",Target.CharacterTag);
			Target.CharacterSmooth = EditorGUILayout.FloatField ("Character Smooth", Target.CharacterSmooth);
			Target.CharacterPosition = EditorGUILayout.Vector3Field ("Character Offset", Target.CharacterPosition);
			Target.CharacterRotation = CEditor.Vector4ToQuaternion(EditorGUILayout.Vector4Field ("Character Rotation",CEditor.QuaternionToVector4 (Target.CharacterRotation)));
			GUILayout.Space (5);
		}
		EditorGUILayout.EndVertical ();
		EditorGUILayout.EndHorizontal ();

	}
}
