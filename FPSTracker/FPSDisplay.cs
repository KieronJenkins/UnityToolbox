using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private float _deltaTime = 0.0f;
    private float _minimumDeltaTime;
    private float _maximumDeltaTime;
    private float _averageDeltaTime;

    private float _maximumFPS = float.MinValue;
    private float _minimumFPS = float.MaxValue;
    private float _averageFPS = 0.0f;
    private float _framesPerSecond;

    private string _displayFPSText;

    [Header("Frames Per Second")] 
    [Space] [SerializeField] private Color _textColor = Color.white;

    [Space] [SerializeField] private int _textSize = 3;
    [Space] [SerializeField] private Vector2 _textPlacement = new Vector2(20, 20);
    [Space]
    [Header("Device Information")] 
    [Space] [SerializeField] private bool _displayDeviceInfo = false;
    [Space]
    [Header("Development Testing")] 
    [Space] [SerializeField] private int _targetFramerate = 60;

    private bool _CheckMinimumFPS = false;

    private float _minimumFPSTimerStart = 0.0f;
    private float _minimumFPSTimerEnd = 2.0f;

    private void Awake()
    {
        Application.targetFrameRate = _targetFramerate;
    }

    private void Update()
    {
        _minimumFPSTimerStart += Time.deltaTime;

        if (_minimumFPSTimerStart >= _minimumFPSTimerEnd && !_CheckMinimumFPS)
        {
            _CheckMinimumFPS = true;
        }

        CheckSceneFPS();
    }

    private void OnGUI()
    {
        int _screenWidth = Screen.width, _screenHeight = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(_textPlacement.x, _textPlacement.y, _screenWidth, _screenHeight * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = _screenHeight * _textSize / 100;
        style.normal.textColor = _textColor;

        if (_displayDeviceInfo)
        {
            _displayFPSText = string.Format(
                "Current: {0:0.0} ms ({1:0.} FPS)\n" +
                "Maximum: {4:0.0} ms ({5:0.} FPS)\n" +
                "Minimum: {2:0.0} ms ({3:0.} FPS)\n" + "Average: {6:0.0} ms ({7:0.} FPS)\n\n" +
                "Device Model: {8}\n" + "Processor Type: {9}",
                _deltaTime * 1000.0f, _framesPerSecond,
                _minimumDeltaTime * 1000.0f, _minimumFPS,
                _maximumDeltaTime * 1000.0f, _maximumFPS,
                _averageDeltaTime * 1000.0f, _averageFPS,
                SystemInfo.deviceModel, SystemInfo.processorType);
            GUI.Label(rect, _displayFPSText, style);
        }
        else
        {
            _displayFPSText = string.Format(
                "Current: {0:0.0} ms ({1:0.} FPS)\n" +
                "Maximum: {4:0.0} ms ({5:0.} FPS)\n" +
                "Minimum: {2:0.0} ms ({3:0.} FPS)\n" + "Average: {6:0.0} ms ({7:0.} FPS)\n",
                _deltaTime * 1000.0f, _framesPerSecond,
                _minimumDeltaTime * 1000.0f, _minimumFPS,
                _maximumDeltaTime * 1000.0f, _maximumFPS,
                _averageDeltaTime * 1000.0f, _averageFPS);
            GUI.Label(rect, _displayFPSText, style);
        }
    }

    private void CheckSceneFPS()
    {
        _deltaTime = Time.unscaledDeltaTime;

        _framesPerSecond = 1.0f / _deltaTime;

        _averageFPS = (_averageFPS + _framesPerSecond) / 2.0f;
        _averageDeltaTime = 1.0f / _averageFPS;

        if (_CheckMinimumFPS)
        {
            if (_framesPerSecond > _maximumFPS)
            {
                _maximumFPS = _framesPerSecond;
                _maximumDeltaTime = _deltaTime;

                if (_maximumFPS > _targetFramerate)
                {
                    _maximumFPS = _targetFramerate;
                }
            }

            if (_framesPerSecond < _minimumFPS)
            {
                _minimumFPS = _framesPerSecond;
                _minimumDeltaTime = _deltaTime;
            }
        }
    }
}
