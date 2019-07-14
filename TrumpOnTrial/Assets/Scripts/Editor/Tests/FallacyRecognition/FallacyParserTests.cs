using FineGameDesign.FallacyRecognition;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;

namespace FineGameDesign.Tests.FallacyRecognition
{
    public static class FallacyParserTests
    {
        public static Fallacy[] ParseFromPrefab()
        {
            UnityEngine.Object prefab = AssetDatabase.LoadMainAssetAtPath("Assets/Prefabs/FallacyParser.prefab");
            GameObject parserObject = (GameObject)UnityEngine.Object.Instantiate(prefab);
            FallacyParser parser = parserObject.GetComponent<FallacyParser>();
            parser.ParseFallaciesOnce();
            return parser.Fallacies;
        }
    }
}
