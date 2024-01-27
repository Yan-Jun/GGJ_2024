using Cysharp.Threading.Tasks;
using GGJ.Ingame.Common;
using GGJ.Ingame.UI;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace GGJ.Ingame.Controller
{
    public class InputCodeController : MonoBehaviour
    {
        [SerializeField] private InputCodePanel _targetInputCodePanel;

        private string _nextArrangeCode;
        private int _nextCodeIndex;
        private bool _isWaitComplete;

        private IInputCodeSystem _inputCodeSystem;
        private IInputCodePanel _inputCodePanel;

        private void Start ()
        {
            Initialize(
                FindFirstObjectByType<InputCodeSystem>(),
                _targetInputCodePanel);
        }

        private void Initialize(IInputCodeSystem inputCodeSystem, IInputCodePanel inputCodePanel)
        {
            _inputCodeSystem = inputCodeSystem;
            _inputCodePanel = inputCodePanel;

            Assert.IsNotNull(_inputCodeSystem);
            Assert.IsNotNull(_inputCodePanel);

            _nextArrangeCode = GenerateTestCode();
            //Debug.Log($"Next code: {_nextArrangeCode}");

            _inputCodePanel.ClearAll();
            _inputCodePanel.SetInputCodes(_nextArrangeCode);

        }

        private void Update()
        {
            if (_isWaitComplete)
                return;

            UpdateCheckCode();
        }

        public void UpdateCheckCode()
        {
            var inputCode = _inputCodeSystem.GetCurrentInputCode();
            if (inputCode.HasValue && _nextArrangeCode[_nextCodeIndex] == char.ToLower(inputCode.Value))
            {
                _inputCodePanel.SetCompletedIndex(_nextCodeIndex, char.ToLower(inputCode.Value));
                _nextCodeIndex++;

                var isCompleted = CheckInputComplete(_nextCodeIndex, _nextArrangeCode);
                if (isCompleted)
                {
                    CompleteCode().Forget();
                    PlayerStat.i.AddWallPoint("Wall_1",1000);
                    return;
                }
                //Debug.Log($"Next type code: {_nextArrangeCode[_nextCodeIndex]}");
            }
        }

        public bool CheckInputComplete(int currentIndex, string arrangeCode)
        {
            return currentIndex == arrangeCode.Length;
        }

        public async UniTask CompleteCode()
        {
            _isWaitComplete = true;
            _nextArrangeCode = GenerateTestCode();
            _nextCodeIndex = 0;

            await UniTask.Delay(TimeSpan.FromSeconds(.5));

            _inputCodePanel.ClearAll();
            await _inputCodePanel.SetInputCodes(_nextArrangeCode);

            _isWaitComplete = false;
        }

        private string GenerateTestCode()
        {
            var simpleCodes = new string[]
            {
                "aaabbb",
                "1111111",
            };
            return simpleCodes[Random.Range(0, simpleCodes.Length)];
        }

    }
}