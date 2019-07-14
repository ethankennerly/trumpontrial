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
            Debug.LogWarning("ParseFromPrefab: TODO");
            return ArgumentsTableTests.AssertOneArgumentTableColumnsNamedAC().arguments;
        }
    }
}
