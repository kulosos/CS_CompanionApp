#define COHERENT_UI_PRO_UNITY3D
#if UNITY_STANDALONE || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
#define COHERENT_UNITY_STANDALONE
#endif

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR || COHERENT_UNITY_STANDALONE || COHERENT_UNITY_UNSUPPORTED_PLATFORM
using Coherent.UI;
using Coherent.UI.Binding;
using System.Runtime.InteropServices;
#endif

/// <summary>
/// Component that needs to be attached to a camera and creates a Coherent UI Live Game View
/// </summary>
[AddComponentMenu("Coherent UI/Live Game View")]
public class CoherentUILiveGameView : MonoBehaviour {
	#if UNITY_EDITOR || COHERENT_UNITY_STANDALONE || COHERENT_UNITY_UNSUPPORTED_PLATFORM

	[HideInInspector]
	[SerializeField]
	private string m_LiveName = "MyLiveView";

	/// <summary>
	/// Gets or sets the Name of the live view
	/// </summary>
	/// <value>
	/// The Name of the view
	/// </value>
	[CoherentExposeProperty(Category = CoherentExposePropertyInfo.FoldoutType.General,
							PrettyName = "Name",
							Tooltip="Indicates the Name of the Live View available in the JavaScript page",
							IsStatic=true)]
	public string LiveName
	{
		get {
			return m_LiveName;
		}
		set {
			m_LiveName = value;
		}
	}

	[HideInInspector]
	[SerializeField]
	private int m_Width = 512;

	/// <summary>
	/// Gets or sets the Width of the live view
	/// </summary>
	/// <value>
	/// The width of the view
	/// </value>
	[CoherentExposeProperty(Category = CoherentExposePropertyInfo.FoldoutType.General,
							PrettyName = "Width",
							Tooltip="Indicates the Width that the Live Game View will have",
							IsStatic=true)]
	public int Width
	{
		get {
			return m_Width;
		}
		set {
			m_Width = value;
		}
	}

	[HideInInspector]
	[SerializeField]
	private int m_Height = 512;

	/// <summary>
	/// Gets or sets the Height of the live view
	/// </summary>
	/// <value>
	/// The height of the view
	/// </value>
	[CoherentExposeProperty(Category = CoherentExposePropertyInfo.FoldoutType.General,
							PrettyName = "Height",
							Tooltip="Indicates the Height that the Live Game View will have",
							IsStatic=true)]
	public int Height
	{
		get {
			return m_Height;
		}
		set {
			m_Height = value;
		}
	}

	[HideInInspector]
	[SerializeField]
	private UnityEngine.Object m_CameraGo;

	/// <summary>
	/// Gets or sets the GameObject with the Object to link to
	/// </summary>
	/// <value>
	/// The GameObject with the linked views
	/// </value>
	[CoherentExposeProperty(Category = CoherentExposePropertyInfo.FoldoutType.General,
							PrettyName = "Source Camera",
							Tooltip="The Camera GO that will render the Live Game View",
							IsStatic=true)]
	public UnityEngine.Object CameraGo
	{
		get {
			return m_CameraGo;
		}
		set {
			m_CameraGo = value;
		}
	}

	[HideInInspector]
	[SerializeField]
	private Texture2D m_SourceTexture;

	/// <summary>
	/// Gets or sets the source texture for the live view
	/// </summary>
	/// <value>
	/// The GameObject with the linked views
	/// </value>
	[CoherentExposeProperty(Category = CoherentExposePropertyInfo.FoldoutType.General,
							PrettyName = "Source Texture",
							Tooltip="A texture used as source for the Live View",
							IsStatic=true)]
	public Texture2D SourceTexture
	{
		get {
			return m_SourceTexture;
		}
		set {
			if(value != null &&
				value.width != m_Width &&
				value.height != m_Height)
			{
				Debug.Log("The size of the Live view texture must coincide with the Live View itself");
				return;
			}

			m_SourceTexture = value;
		}
	}
	
	[HideInInspector]
	[SerializeField]
	private bool m_ReadAlpha = false;

	/// <summary>
	/// Gets or sets if to read the alpha channel
	/// </summary>
	/// <value>
	/// If the alpha channel will be read from the camera
	/// </value>
	[CoherentExposeProperty(Category = CoherentExposePropertyInfo.FoldoutType.General,
							PrettyName = "Read alpha",
							Tooltip="Sets if the alpha channel will be read from the camera",
							IsStatic=true)]
	public bool ReadAlpha
	{
		get {
			return m_ReadAlpha;
		}
		set {
			m_ReadAlpha = value;
		}
	}

	private ImageData m_LiveView;

	public ImageData LiveView
	{
		get {
			return m_LiveView;
		}
	}

	private RenderTexture m_RenderTexture;
	private Texture2D m_Texture;

	void Start () {
		if(m_CameraGo == null && m_SourceTexture == null)
		{
			return;
		}

		if(m_SourceTexture != null &&
			m_SourceTexture.width != m_Width &&
			m_SourceTexture.height != m_Height)
		{
			throw new System.ArgumentException("The size of the Live view texture must coincide with the Live View itself");
		}

		if(m_CameraGo != null)
		{
			var cameraGo = (GameObject)m_CameraGo;
			cameraGo.AddComponent<CoherentCameraLive>();
			m_RenderTexture = new RenderTexture(m_Width, m_Height, 1, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
			cameraGo.GetComponent<Camera>().targetTexture = m_RenderTexture;
			m_Texture = new Texture2D(m_Width, m_Height, m_ReadAlpha ? TextureFormat.ARGB32 : TextureFormat.RGB24, false);
		}
		var view = GetComponent<CoherentUIView>();
		view.Listener.ReadyForBindings += (frameId, path, isMainFrame) => {

			if(m_CameraGo == null && m_SourceTexture == null)
				return;

			Color32[] data = null;
			if(m_SourceTexture == null)
			{
				data = new Color32[m_Width * m_Height];
				for(var i = 0; i < data.Length; ++i)
				{
					data[i].r = 0;
					data[i].g = 0;
					data[i].b = 0;
					data[i].a = 255;
				}
			}
			else
			{
				data = m_SourceTexture.GetPixels32(0);
			}
			var dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
			m_LiveView = view.View.CreateImageData(m_LiveName, m_Width, m_Height, dataHandle.AddrOfPinnedObject(), true);
			dataHandle.Free();

			if(m_CameraGo != null)
			{
				var camComp = ((GameObject)m_CameraGo).GetComponent<CoherentCameraLive>();
				camComp.SetTextures(this, m_RenderTexture, m_Texture);
			}
		};
	}

	void OnDestroy() {
		if(m_CameraGo != null)
		{
			var camComp = ((GameObject)m_CameraGo).GetComponent<CoherentCameraLive>();
			camComp.SetTextures(null, null, null);
		}
		m_LiveView = null;
	}

	public void UpdateFromCurrentTexture() {
		if(m_SourceTexture != null && m_LiveView != null)
		{
			Color32[] data = m_SourceTexture.GetPixels32(0);
			var dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
			m_LiveView.Update(dataHandle.AddrOfPinnedObject(), true);
			dataHandle.Free();
		}
	}

	#endif
}
