#nullable enable
using UnityEngine;

namespace GGJ.Ingame.Common
{
    public interface IInputCodeSystem
    {
        public char? GetCurrentInputCode();
    }

    public class InputCodeSystem : MonoBehaviour, IInputCodeSystem
    {
        private string _currentInputCode;

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
        }

        private void UpdateInput()
        {
            //Debug.Log("Input: " + Input.inputString);
            _currentInputCode = Input.inputString;
        }

        public char? GetCurrentInputCode()
        {
            var parseResult = char.TryParse(_currentInputCode, out var resultCode);
            return parseResult ? resultCode : null; 
        }
    }
}
