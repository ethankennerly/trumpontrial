using FineGameDesign.FallacyRecognition;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;

namespace FineGameDesign.Tests.FallacyRecognition
{
    public static class ArgumentParserTests
    {
        public static Argument[] ParseFromPrefab()
        {
            UnityEngine.Object prefab = AssetDatabase.LoadMainAssetAtPath(
                "Assets/Prefabs/ArgumentParser.prefab");
            GameObject parserObject = (GameObject)UnityEngine.Object.Instantiate(prefab);
            ArgumentParser parser = parserObject.GetComponent<ArgumentParser>();
            parser.ParseArguments();
            return parser.Arguments;
        }
    }
}
