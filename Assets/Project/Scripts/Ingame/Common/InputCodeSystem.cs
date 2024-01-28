#nullable enable
using UnityEngine;

namespace GGJ.Ingame.Common
{
    public interface IInputCodeSystem
    {
        char? GetCurrentInputCode();
        string? GetCurrentKeyCode();
    }

    public class InputCodeSystem : MonoBehaviour, IInputCodeSystem
    {
        private string? _currentInputCode;
        private string? _currentKeyCode;

        private void Start()
        {
            Initialize("");
        }

        public void Initialize(string currentInputCode)
        {
            _currentInputCode = currentInputCode;
        }

        private void Update()
        {
            UpdateInput();
            UpdateInputArrow();
        }

        private void UpdateInput()
        {
            _currentInputCode = Input.inputString;
        }

        private void UpdateInputArrow()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _currentKeyCode = KeyCode.UpArrow.ToString();
            }
            else if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                _currentKeyCode = KeyCode.DownArrow.ToString();
            }
            else if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _currentKeyCode = KeyCode.LeftArrow.ToString();
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                _currentKeyCode = KeyCode.RightArrow.ToString();
            }
            else
            {
                _currentKeyCode = null;
            }
        }

        public char? GetCurrentInputCode()
        {
            var parseResult = char.TryParse(_currentInputCode, out var resultCode);
            return parseResult ? resultCode : null; 
        }

        public string? GetCurrentKeyCode()
        {
            return _currentKeyCode;
        }
    }
}
