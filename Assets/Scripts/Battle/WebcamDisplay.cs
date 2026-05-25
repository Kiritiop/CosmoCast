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
        if (_cam == null || !_cam.didUpdateThisFrame) return;

        // Keep aspect ratio correct once the camera reports real dimensions
        if (_fitter != null && _cam.width > 16)
            _fitter.aspectRatio = (float)_cam.width / _cam.height;

        // Mirror horizontally so it feels like a mirror, not a surveillance feed
        _display.uvRect = new Rect(1, 0, -1, 1);
    }

    void OnDestroy()
    {
        if (_cam != null && _cam.isPlaying)
            _cam.Stop();
    }
}
