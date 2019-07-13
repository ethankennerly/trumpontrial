# Problem

1. Unity built in [2018 or later.](https://forum.unity.com/threads/warning-cs0649-not-suppressed-properly-when-field-is-marked-as-serializefield.556009/)
1. Private serialized field with no assignment. Example:

        [SerializeField]
        private ResultsUI m_ResultsUI;

# Expected Result

- No console warning.

# Solution

Unity could fix the root cause:

[Feature Request: Use new DiagnosticSuppressor API to suppress CS0649 on SerializeField](https://forum.unity.com/threads/feature-request-use-new-diagnosticsuppressor-api-to-suppress-cs0649-on-serializefield.697514/)

Otherwise, using the default assignment causes some compilers in IDEs, like JetBrains Rider, to warn that a variable was redundantly assigned the default value.

# Workaround

        [SerializeField]
        private ResultsUI m_ResultsUI = default;

Value types, like a `struct` cannot be assigned `null`, yet reference types and value types can be assigned `default`.

Replace in all C# files. Here is vim sed syntax:

        :args **/*.cs
        :argdo %s/\(\[SerializeField\]\n\?[^=]\+\);/\1 = default;/e | update

If you use a different regular-expression search and replace you may need to remove some of the backslashes.

This handles serialize field being on the same line or next line. Otherwise, the replacement is equivalent to [this Visual Studio replacement.](http://answers.unity.com/answers/1617519/view.html)

# Other Options

Other options mentioned in [these kinds of forum threads](https://forum.unity.com/threads/serializefield-warnings.560878/) are to:

- Use `pragma` to ignore.
    - But that's 2 extra lines of noise to the code.
- Use an `.rsp` file.
    - But that's not portable for shipping packages.
