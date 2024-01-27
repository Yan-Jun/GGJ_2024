using Cysharp.Threading.Tasks;
using GGJ.Ingame.Common;
using GGJ.Ingame.UI;
using System;
using UnityEngine;
using UnityEngine.Assertions;

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
        private IGenerateCodeSystem _generateCodeSystem;


        private void Start ()
        {
            Initialize(
                FindFirstObjectByType<InputCodeSystem>(),
                _targetInputCodePanel,
                new GenerateCodeSystem());
        }

        private void Initialize(IInputCodeSystem inputCodeSystem, IInputCodePanel inputCodePanel, IGenerateCodeSystem generateCodeSystem)
        {
            _inputCodeSystem = inputCodeSystem;
            _inputCodePanel = inputCodePanel;
            _generateCodeSystem = generateCodeSystem;

            Assert.IsNotNull(_inputCodeSystem);
            Assert.IsNotNull(_inputCodePanel);

            _nextArrangeCode = _generateCodeSystem.GetGenerateCode(6, 10);
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
            _nextArrangeCode = _generateCodeSystem.GetGenerateCode(6, 10);
            _nextCodeIndex = 0;

            await UniTask.Delay(TimeSpan.FromSeconds(.5));

            _inputCodePanel.ClearAll();
            await _inputCodePanel.SetInputCodes(_nextArrangeCode);

            _isWaitComplete = false;
        }
    }
}