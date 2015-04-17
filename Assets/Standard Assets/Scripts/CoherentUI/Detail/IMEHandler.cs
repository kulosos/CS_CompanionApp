#if UNITY_STANDALONE || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
#define COHERENT_UNITY_STANDALONE
#endif

#if UNITY_EDITOR || COHERENT_UNITY_STANDALONE
using UnityEngine;
using System.Collections;

namespace Coherent.UI
{
public class IMEHandler : MonoBehaviour {

	public enum ViewPlane
	{
		VP_XpositiveYpositive,
		VP_XnegativeYpositive,
		VP_YpositiveZnegative,
		VP_YpositiveZpositive,
		VP_XnegativeZpositive,
		VP_XnegativeZnegative
	}


	// Use this for initialization
	void Start () {
		m_View = gameObject.GetComponent<CoherentUIView>();

		m_View.Listener.ViewCreated += this.OnViewCreated;

		SubscribeForIMEEvents();
	}

	// Update is called once per frame
	void Update () {
		if(m_View.View != null && m_View.ReceivesInput
			&& Input.imeCompositionMode == IMECompositionMode.On)
		{
			OnCaretRectChanged(
				m_LastCaretRectPosX,
				m_LastCaretRectPosY,
				m_LastCaretRectWidth,
				m_LastCaretRectHeight);
		}
	}

	void OnDisable()
	{
		m_View.EnableIME = false;

		var view = m_View.View;
		if(view == null)
		{
			if(m_View.Listener != null)
			{
				m_View.Listener.ViewCreated -= this.OnViewCreated;
			}

			if(m_View.ReceivesInput)
			{
				Input.imeCompositionMode = IMECompositionMode.Off;
			}
		}
		else
		{
			view.IMEActivate(false);
		}

		UnsubscribeForIMEEvents();
	}

	void OnEnable()
	{
		if(m_View == null)
		{
			m_View = gameObject.GetComponent<CoherentUIView>();
		}
		m_View.EnableIME = true;

		var view = m_View.View;
		if(view == null)
		{
			if(m_View.Listener != null)
			{
				m_View.Listener.ViewCreated += this.OnViewCreated;
			}

			if(m_View.ReceivesInput)
			{
				Input.imeCompositionMode = IMECompositionMode.On;
			}
		}
		else
		{
			view.IMEActivate(true);
		}
		SubscribeForIMEEvents();
	}

	void OnGUI()
	{
			View view = m_View.View;
			Event currentEvent = Event.current;

			if(!m_View.ReceivesInput
				&& view != null && currentEvent != null
				&& currentEvent.type == EventType.MouseDown)
			{
				var mouseEventData = Coherent.UI.InputManager.ProcessMouseEvent(
					currentEvent);
				mouseEventData.Type = MouseEventData.EventType.MouseDown;
				mouseEventData.X = -1;
				mouseEventData.Y = -1;

				view.MouseEvent(mouseEventData);
			}
	}

	void OnDestroy()
	{
		if(m_View == null)
		{
			return;
		}
		UnsubscribeForIMEEvents();
	}

	public delegate Vector3 CalculateIMECandidateListPositionHandler(
		uint x, uint y, uint width, uint height);
	public CalculateIMECandidateListPositionHandler
		CalculateIMECandidateListPosition;

	private void OnViewCreated(View view)
	{
		view.IMEActivate(true);
	}

	void OnCaretRectChanged(uint x, uint y, uint width, uint height)
	{
		m_LastCaretRectPosX = x;
		m_LastCaretRectPosY = y;
		m_LastCaretRectWidth = width;
		m_LastCaretRectHeight = height;

		if (CalculateIMECandidateListPosition != null)
		{
			Input.compositionCursorPos = CalculateIMECandidateListPosition(
				x, y, width, height);
			return;
		}

		Vector3 caretPosition = new Vector3();
		if(gameObject.GetComponent<Camera>() == null)
		{
			caretPosition = GetCaretPositionInWorldSpace(x, y);
			caretPosition = Camera.main.WorldToScreenPoint(caretPosition);
			caretPosition.y = Screen.height - caretPosition.y;
		}
		else
		{
			caretPosition.x = x;
			caretPosition.y = y;
			caretPosition.z = 0;

			if(!m_View.UseCameraDimensions)
			{
				caretPosition.x *=
					(gameObject.GetComponent<Camera>().pixelWidth / m_View.Width);
				caretPosition.y *=
					(gameObject.GetComponent<Camera>().pixelHeight / m_View.Height);
			}
		}
		caretPosition.y += height;
		Input.compositionCursorPos = caretPosition;
	}

	void OnIMEShouldCancelComposition()
	{
		Input.imeCompositionMode = IMECompositionMode.Off;
	}

	void OnTextInputTypeChanged(
		TextInputControlType type,
		bool canComposeInline)
	{
		if(type != TextInputControlType.TICT_None &&
			type != TextInputControlType.TICT_Password
			&& canComposeInline && m_View.ReceivesInput)
		{
			Input.imeCompositionMode = IMECompositionMode.On;
		}
		else
		{
			Input.imeCompositionMode = IMECompositionMode.Off;
		}
	}

	private Vector3 GetCaretPositionInWorldSpace(uint x, uint y)
	{
		float xRatio = (float)(x) / m_View.Width;
		float yRatio = 1 - (float)(y) / m_View.Height;

		Bounds bounds = gameObject.GetComponent<Collider>().bounds;
		Vector3 extents = bounds.extents;
		Vector3 caretPosition = bounds.center - extents;

		ViewPlane planeOrientation = GetViewPlaneOrientation();
		switch(planeOrientation)
		{
		case ViewPlane.VP_XpositiveYpositive:
			caretPosition.x += xRatio * bounds.size.x;
			caretPosition.y += yRatio * bounds.size.y;
			break;
		case ViewPlane.VP_YpositiveZpositive:
			caretPosition.y += yRatio * bounds.size.y;
			caretPosition.z += xRatio * bounds.size.z;
			break;
		case ViewPlane.VP_XnegativeZnegative:
			caretPosition.x += (1 - xRatio) * bounds.size.x;
			caretPosition.z += (1 - yRatio) * bounds.size.z;
			break;
		case ViewPlane.VP_XnegativeYpositive:
			caretPosition.x += (1 - xRatio) * bounds.size.x;
			caretPosition.y += yRatio * bounds.size.y;
			break;
		case ViewPlane.VP_YpositiveZnegative:
			caretPosition.y += yRatio * bounds.size.y;
			caretPosition.z += (1 - xRatio) * bounds.size.z;
			break;
		case ViewPlane.VP_XnegativeZpositive:
			caretPosition.x += (1 - xRatio) * bounds.size.x;
			caretPosition.z += yRatio * bounds.size.z;
			break;
		}
		return caretPosition;
	}

	private ViewPlane GetViewPlaneOrientation()
	{
		MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
		Vector3 normal = meshCollider.sharedMesh.normals[0];

		float xAbs = Mathf.Abs(normal.x);
		float yAbs = Mathf.Abs(normal.y);
		float zAbs = Mathf.Abs(normal.z);

		if(xAbs > yAbs && xAbs > zAbs)
		{
			return (normal.x > 0)
				? ViewPlane.VP_YpositiveZpositive
				: ViewPlane.VP_YpositiveZnegative;
		}

		if(yAbs > xAbs && yAbs > zAbs)
		{
			return (normal.y > 0)
				? ViewPlane.VP_XnegativeZnegative
				: ViewPlane.VP_XnegativeZpositive;
		}

		return (normal.z > 0)
			? ViewPlane.VP_XnegativeYpositive
			: ViewPlane.VP_XpositiveYpositive;
	}

	private void SubscribeForIMEEvents()
	{
		if(m_View.Listener != null && !m_SubscribedForEvents)
		{
			m_View.Listener.CaretRectChanged +=
				this.OnCaretRectChanged;
			m_View.Listener.TextInputTypeChanged +=
				this.OnTextInputTypeChanged;
			m_View.Listener.IMEShouldCancelComposition +=
				this.OnIMEShouldCancelComposition;

			m_SubscribedForEvents = true;
		}
	}

	private void UnsubscribeForIMEEvents()
	{
		if(m_View.Listener != null && m_SubscribedForEvents)
		{
			m_View.Listener.CaretRectChanged -=
				this.OnCaretRectChanged;
			m_View.Listener.TextInputTypeChanged -=
				this.OnTextInputTypeChanged;
			m_View.Listener.IMEShouldCancelComposition -=
				this.OnIMEShouldCancelComposition;

			m_SubscribedForEvents = false;
		}
	}


	private uint m_LastCaretRectPosX = 0;
	private uint m_LastCaretRectPosY = 0;
	private uint m_LastCaretRectWidth = 0;
	private uint m_LastCaretRectHeight = 0;

	private CoherentUIView m_View;

	bool m_SubscribedForEvents = false;
}
}
#endif
