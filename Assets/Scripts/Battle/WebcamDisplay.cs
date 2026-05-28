using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class WebcamDisplay : MonoBehaviour
{
    [SerializeField] private int requestedWidth = 320;
    [SerializeField] private int requestedHeight = 240;
    [SerializeField] private int requestedFPS = 30;

    private WebCamTexture _cam;
    private RawImage _display;
    private AspectRatioFitter _fitter;

    void Start()
    {
        _display = GetComponent<RawImage>();
        _fitter = GetComponent<AspectRatioFitter>();

        if (WebCamTexture.devices.Length == 0)
        {
            Debug.LogWarning("WebcamDisplay: No webcam found.");
            return;
        }

        _cam = new WebCamTexture(WebCamTexture.devices[0].name, requestedWidth, requestedHeight, requestedFPS);
        _display.texture = _cam;
        _cam.Play();
    }

    void Update()
    {
        if (_cam == null) return;

        // Pause/resume webcam with game pause state
        bool paused = UIManager.Instance != null && UIManager.Instance.IsPaused;
        if (paused && _cam.isPlaying) { _cam.Pause(); return; }
        if (!paused && !_cam.isPlaying) _cam.Play();

        if (!_cam.didUpdateThisFrame) return;

        if (_fitter != null && _cam.width > 16)
            _fitter.aspectRatio = (float)_cam.width / _cam.height;

        _display.uvRect = new Rect(1, 0, -1, 1);
    }

    void OnDestroy()
    {
        if (_cam != null && _cam.isPlaying)
            _cam.Stop();
    }
}
