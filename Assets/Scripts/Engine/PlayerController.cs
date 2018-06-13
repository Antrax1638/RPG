using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum InputMode
{
    Game,
    Interface,
    GameAndInterface,
    None
}

[System.Serializable]
public enum MovementMode
{
	None,
	Mouse,
	MouseKeyboard,
	Keyboard,
}

public enum CameraAxis
{
	UpAxis,
	ForwardAxis,
	RightAxis,
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
	[Header("Mouse")]
	public bool MouseLookAt;
	public float MouseRayLength = 40.0f;
	public float MouseRotationRate = 10.0f;
	public LayerMask MouseRayMask;
	public int MouseAction = 0;
	public Vector3 MouseRotationAxis = Vector3.up;
	public CursorLockMode MouseMode = CursorLockMode.None;
	public bool MouseVisible = true;

	[Header("Character")]
	public GameObject Character;
	public Vector3 CharacterPosition = Vector3.zero;
	public Quaternion CharacterRotation = Quaternion.identity;
	public float CharacterSmooth = 1.0f;
	public string CharacterTag = "Untagged";

	[Header("Camera")]
	public bool MainCamera = true;
	public GameObject CameraObject;
	public Vector3 CameraOffset = Vector3.zero;
	public float CameraSmooth = 1.0f;
	public float CameraTurnSpeed = 0.753f;
	public CameraAxis CameraTurnAxis = CameraAxis.UpAxis;
	public GameObject CameraTarget;
	public bool CameraDeltaTime = false;

	[Header("Movement")]
	public bool LookAtDirection;
	public float LookAtDirectionRate = 5.5f;
	public int LookAtDirectionMultiplier = -1;
	public Vector3 LookAtDirectionScale = Vector3.up;
	public bool AirControl;
	public float AirControlScale = 1.0f;
	public float GroundLength;
	public MovementMode MovMode;
	public float Speed = 10.5f;
	public float SpeedCap = 14.5f;
	public ForceMode SpeedMode = ForceMode.Force;
	public float JumpForce = 7.3f;
	public ForceMode JumpMode = ForceMode.Impulse;
	public int JumpCap = 1;

    [Header("Input:")]
    public InputMode InputMode;
    public string 	ForwardAction = "Forward",
					RightAction = "Right",
					LeftAction = "Left",
					BackwardAction = "Backward",
					FirstMouseAction = "LeftMouse",
					SecondMouseAction = "RightMouse",
					MiddleMouseAction = "MiddleMouse",
					JumpAction = "Jump",
					CrouchAction = "Crouch",
					TurnLeftAction = "TurnLeft",
					TurnRightAction = "TurnRight";

	//Components:
	Rigidbody BodyComponent;
	Camera CameraComponent;
	CapsuleCollider ColliderComponent;

	//Private:
	RaycastHit MouseRayHit,GroundRayHit;
	bool MouseHit,IsGrounded,EnabledInput;
	int JumpCount = 0;
	Vector3 Direction = Vector3.zero;
	float AirControlSpeed = 0.0f;


	void Awake () 
	{
		BodyComponent = GetComponent<Rigidbody> ();
		if (!BodyComponent)
			Debug.LogError ("Player Controller: Rigidbody Is Null");
		
		CameraComponent = (MainCamera) ? Camera.main : CameraObject.GetComponent<Camera>();
		if (!CameraComponent)
			Debug.LogError ("Player Controller: Camera is Null");

		ColliderComponent = GetComponent<CapsuleCollider> ();
		if(!ColliderComponent)
			Debug.LogError ("Player Controller: Collider is Null");

		Character.transform.position = transform.position;

		if (!CameraTarget)
			CameraTarget = transform.gameObject;
	}

	void Start()
	{
		if (Character) {
			Character.tag = CharacterTag;
		}
	}

	void Update()
	{
		Cursor.lockState = MouseMode;
		Cursor.visible = MouseVisible;
	}

	void FixedUpdate () 
	{
        EnabledInput = (InputMode == InputMode.Game || InputMode == InputMode.GameAndInterface);

        CameraUpdate ();
		CharacterUpdate ();
        Movement();
	}

	void CharacterUpdate()
	{
		if (Character)
		{
			Vector3 CharacterPositionOffset = transform.position + CharacterPosition;
			Quaternion CharacterRotationOffset = transform.rotation * CharacterRotation;

			Character.transform.position = Vector3.Lerp(Character.transform.position, CharacterPositionOffset ,CharacterSmooth);
			Character.transform.rotation = Quaternion.Lerp (Character.transform.rotation, CharacterRotationOffset, CharacterSmooth);
		}
	}

	void CameraUpdate()
	{
		if (CameraComponent)
		{
			if (Input.GetButton(TurnLeftAction) && EnabledInput) 
			{
				switch (CameraTurnAxis)
				{
					case CameraAxis.ForwardAxis: CameraOffset = Quaternion.AngleAxis (-CameraTurnSpeed, Vector3.forward) * CameraOffset;break;
					case CameraAxis.RightAxis: CameraOffset = Quaternion.AngleAxis (-CameraTurnSpeed, Vector3.right) * CameraOffset;break;
					case CameraAxis.UpAxis: CameraOffset = Quaternion.AngleAxis (-CameraTurnSpeed, Vector3.up) * CameraOffset;break;
					default: break;
				}
			}
			if (Input.GetButton (TurnRightAction) && EnabledInput) 
			{
				switch (CameraTurnAxis)
				{
				case CameraAxis.ForwardAxis: CameraOffset = Quaternion.AngleAxis (CameraTurnSpeed, Vector3.forward) * CameraOffset;break;
				case CameraAxis.RightAxis: CameraOffset = Quaternion.AngleAxis (CameraTurnSpeed, Vector3.right) * CameraOffset;break;
				case CameraAxis.UpAxis: CameraOffset = Quaternion.AngleAxis (CameraTurnSpeed, Vector3.up) * CameraOffset;break;
				default: break;
				}
			}

			float CameraRate = (CameraDeltaTime) ? CameraSmooth * Time.deltaTime : CameraSmooth;
			CameraComponent.transform.position = Vector3.Lerp (CameraComponent.transform.position, CameraTarget.transform.position + CameraOffset, CameraRate);
			CameraComponent.transform.LookAt (CameraTarget.transform.position);
		}
	}

	void Movement ()
	{
		if (CameraComponent /*&& (MovMode == MovementMode.Mouse || MovMode == MovementMode.MouseKeyboard)*/)
		{
			Vector3 MouseWorldPos = CameraComponent.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, CameraComponent.nearClipPlane));
			Vector3 Direction = MouseWorldPos - CameraComponent.transform.position;
			Direction *= MouseRayLength;
			MouseHit = Physics.Raycast (CameraComponent.transform.position, Direction, out MouseRayHit, MouseRayLength, MouseRayMask);
		}

		bool Look = false;
		Look |= Input.GetButton (FirstMouseAction) && MouseAction == 0;
		Look |= Input.GetButton (SecondMouseAction) && MouseAction == 1;
		Look |= Input.GetButton (MiddleMouseAction) && MouseAction == 2;
		Look |= MouseAction < 0;

        if (MouseLookAt && Look && EnabledInput) 
		{
			Vector3 Direction = transform.position - MouseRayHit.point;
			Direction = Vector3.RotateTowards (transform.forward, Direction.normalized, MouseRotationRate * Time.fixedDeltaTime,0.0f);
			Direction = Direction.normalized;
			Quaternion Rotation = Quaternion.LookRotation (Direction);

			transform.rotation = new Quaternion (
				Rotation.x * MouseRotationAxis.x, 
				Rotation.y * MouseRotationAxis.y, 
				Rotation.z * MouseRotationAxis.z, 
				Rotation.w
			);
		}
			
		Speed = Mathf.Clamp (Speed, float.MinValue, SpeedCap);
		IsGrounded = Physics.SphereCast (transform.position, ColliderComponent.radius,Vector3.down, out GroundRayHit,GroundLength);
		JumpCount = (IsGrounded) ? 0 : JumpCount;
		IsGrounded = (AirControl && !IsGrounded) ? true : IsGrounded;
		AirControlSpeed = (AirControl && !IsGrounded) ? AirControlScale : 1.0f;
		int InputButton = Convert.ToInt32 (Input.GetButton (FirstMouseAction) || Input.GetButton(SecondMouseAction) || Input.GetButton(MiddleMouseAction));

		if (IsGrounded && EnabledInput)
		{
			Direction = Vector3.zero;
			switch (MovMode)
			{
			case MovementMode.Mouse:
				if (MouseLookAt && Look) 
				{
					Direction = -transform.forward * 2;
					Direction = Direction.normalized;
					Direction *= Speed * AirControlSpeed * InputButton;
				} 
				/*else
				{
					Direction = MouseRayHit.point - transform.position;
					Direction *= 99999;
					Direction = transform.TransformDirection(Direction).normalized;
					Direction *= Speed * AirControlSpeed * InputButton;
				}*/
				BodyComponent.AddForce (Direction, SpeedMode);
			break;
			case MovementMode.MouseKeyboard:
				Direction = new Vector3 (Input.GetAxis (LeftAction) + Input.GetAxis (RightAction), 0.0f, Input.GetAxis (ForwardAction) + Input.GetAxis (BackwardAction));
				Direction = CameraComponent.transform.TransformDirection (Direction);
				Direction = new Vector3 (Direction.x * 99999, 0.0f, Direction.z * 99999);
				Direction = Direction.normalized;
				Direction *= Speed * AirControlSpeed;

				if (Direction.magnitude <= 0.0f)
				{
					if (MouseLookAt && Look) 
					{
						Direction = -transform.forward * 2;
						Direction = Direction.normalized;
						Direction *= Speed * AirControlSpeed * InputButton;
					} 
					/*else 
					{
						Direction = MouseRayHit.point - transform.position;
						Direction *= 99999;
						Direction = transform.TransformDirection (Direction).normalized;
						Direction = Direction * AirControlSpeed * Speed * InputButton;
					}*/
				}
				BodyComponent.AddForce (Direction, SpeedMode);
			break;
			case MovementMode.Keyboard:
				Direction = new Vector3 (Input.GetAxis (LeftAction) + Input.GetAxis (RightAction), 0.0f, Input.GetAxis (ForwardAction) + Input.GetAxis (BackwardAction));
				Direction = CameraComponent.transform.TransformDirection (Direction);
				Direction = new Vector3 (Direction.x * 99999, 0.0f, Direction.z * 99999);
				Direction = Direction.normalized;
				Direction *= Speed * AirControlSpeed;

				Vector3 RotationDelta = Vector3.RotateTowards (transform.forward, Direction.normalized, 5.0f, 0.0f);
				Quaternion Rotate = Quaternion.LookRotation (RotationDelta);
				transform.rotation = new Quaternion (Rotate.x * 0, Rotate.y * 1, Rotate.z * 0, Rotate.w);
				BodyComponent.AddForce (Direction, SpeedMode);
			break;
			default: break;
			}
		}

		BodyComponent.velocity = Vector3.ClampMagnitude (BodyComponent.velocity, SpeedCap);
		if (LookAtDirection && BodyComponent) 
		{
			if (BodyComponent.velocity.magnitude > 1) 
			{
				LookAtDirectionMultiplier = Mathf.Clamp (LookAtDirectionMultiplier, -1, 1);
				float MaxDeltaRate = LookAtDirectionRate * Time.fixedDeltaTime;
				Vector3 Direction = BodyComponent.velocity * LookAtDirectionMultiplier;
				Direction = Vector3.RotateTowards (transform.forward, Direction, MaxDeltaRate, 0.0f);
				Quaternion LookAtRotation = Quaternion.LookRotation (Direction);
				transform.rotation = new Quaternion (
					LookAtRotation.x * LookAtDirectionScale.x,
					LookAtRotation.y * LookAtDirectionScale.y,
					LookAtRotation.z * LookAtDirectionScale.z,
					LookAtRotation.w
				);

			}
		}


		if (Input.GetButtonDown (JumpAction) && JumpCount < JumpCap && EnabledInput)
		{
			BodyComponent.AddForce ( Vector3.up * JumpForce, JumpMode);
			JumpCount++;
		}
	}

	public Camera GetControllerCamera()
	{
		return CameraComponent;
	}

	public Rigidbody GetControllerBody()
	{
		return BodyComponent;
	}
		
	void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			//Gizmos.DrawRay (transform.position, transform.forward);
			Gizmos.DrawCube (MouseRayHit.point, new Vector3 (0.2f, 0.2f, 0.2f));
			Gizmos.DrawWireCube (transform.position, ColliderComponent.bounds.size);
			Gizmos.color = Color.red;
			Gizmos.DrawRay (transform.position, Direction);
		}
	}
}
