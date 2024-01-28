using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GGJ.Ingame.Common
{
    public interface IGenerateCodeSystem
    {
        string GetGenerateCode(int minCount, int maxCount);
        string[] GetGenerateArrowCode(int minCount, int maxCount);
    }

    public class GenerateCodeSystem : IGenerateCodeSystem
    {
        public string GetGenerateCode(int minCount, int maxCount)
        {
            var code = new StringBuilder();
            var length = Random.Range(minCount, maxCount + 1);
            for (int i = 0; i < length; i++)
            {
                code.Append(GetRandomLowerCode().ToString());
            }
            return code.ToString();
        }

        public string[] GetGenerateArrowCode(int minCount, int maxCount)
        {
            var arrowCodes = new List<string>();
            var length = Random.Range(minCount, maxCount + 1);
            for (int i = 0; i < length; i++)
            {
                arrowCodes.Add(GetRandomArrowCode());
            }
            return arrowCodes.ToArray();
        }

        private char GetRandomLowerCode()
        {
            var isNumber = Random.Range(0, 2) == 1 ? true : false;
            var randomChar = isNumber ? Random.Range(48, 58) : Random.Range(97, 123);
            return (char)randomChar;
        }

        private string GetRandomArrowCode()
        {
            var arrowCode = new string[] { "UpArrow", "DownArrow", "LeftArrow", "RightArrow" };
            return arrowCode[Random.Range(0, arrowCode.Length)];
        }

    }
}
