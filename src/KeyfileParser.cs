using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ShowKeys
{
    internal static class KeyfileParser
    {
        // -- Public --

        public struct KeyBinding
        {
            public uint Scancode;
            public byte ModifierFlags;
            public string CallbackName;
            public string Description;
        }

        public static IEnumerable<KeyBinding> GetKeyLines(string path)
        {
            Regex keyline_pattern = new Regex(@"(?nsix) #ExplicitCapture, Singleline, IgnoreCase, IgnorePatternWhitespace
                ^\s*
                    (?<callbackName> [A-Z][A-Z0-9_]+)
                \s+
                    (?<soundId> \x2D? \d+ )
                \s+
                    (?<unused> \d+ ) #note: typically 0 but some older keyfiles have digits here
                \s+
                    (?<keyScancodeHex> 0(x[0-9A-F]{1,8})? )
                \s+
                    (?<keyModifierFlags> [0-7] )
                \s+
                    (?<chordScancodeHex> 0(x[0-9A-F]{1,8})? )
                \s+
                    (?<chordModifierFlags> [0-7] )
                \s+
                    (?<displayFlags> \x2D? \d )
                \s+
                    (?<descriptionStringDQ> \x22 [^\x22]* \x22 )
                    (
                        \s*
                        (?<trailingComment> \x23 .* ) # optional trailing comment
                    )?
                \s*$"
                , RegexOptions.CultureInvariant);

            using (StreamReader sr = File.OpenText(path))
            {
                while (true)
                {
                    string line = sr.ReadLine();
                    if (line == null) break;
                    if (line == String.Empty) continue;

                    var m = keyline_pattern.Match(line);
                    if (!m.Success) continue;

                    var binding = new KeyBinding()
                    {
                        CallbackName = m.Groups["callbackName"].Value,
                        Description = m.Groups["descriptionStringDQ"].Value,
                        Scancode = Convert.ToUInt32(m.Groups["keyScancodeHex"].Value, fromBase:16),
                        ModifierFlags = Convert.ToByte(m.Groups["keyModifierFlags"].Value, fromBase: 10)
                    };

                    yield return binding;
                }
            }
        }

    }
}
