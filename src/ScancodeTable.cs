using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ShowKeys
{
    internal static class ScancodeTable
    {
        // -- Public --

        public static string GetLabelForScanCode(ushort sc)
        {
            if (s_map == null)
                _Init();

            if (s_map.ContainsKey(sc))
                return s_map[sc];
            else
                return $"(Unknown_Scancode:{sc})";
        }

        public static ushort[] GetAllValidScancodesForUS()
        {
            if (s_map == null)
                _Init();

            return s_map.Keys.ToArray();
        }

        // -- Internal --

        static void _Init()
        {
            s_map = new Dictionary<ushort, string>(255);

            StringReader sr = new StringReader(s_scancode_table_us);
            while (true)
            {
                string line = sr.ReadLine();
                if (line == null) break;

                var m = s_linepatt.Match(line);
                if (!m.Success) continue;

                string sc_dec = m.Groups["dec_sc"].Value;
                string key_label = m.Groups["label"].Value;

                if (!UInt16.TryParse(sc_dec, out ushort sc))
                    throw new InvalidDataException("Unable to parse scancode.");

                s_map.Add(sc, key_label);
            }
            sr.Close();
        }

        static Dictionary<ushort, string> s_map = null;

        static Regex s_linepatt = new Regex(@"(?nsix) ^ \|\s (?<hex_sc>0x[0-9A-F]{2}) \s\|\s (?<dec_sc>\d{1,3}) \s\|\s (?<label> [^\|]+ ) \s\| $",
                RegexOptions.CultureInvariant);

        // from https://gist.github.com/arithex/3e953d1eb096afe58ce05ba6846493e4
        static string s_scancode_table_us = @"
| Hex | Dec | Label |
|-----|-----|--------|
| 0x02 | 2 | NumRow 1 |
| 0x03 | 3 | NumRow 2 |
| 0x04 | 4 | NumRow 3 |
| 0x05 | 5 | NumRow 4 |
| 0x06 | 6 | NumRow 5 |
| 0x07 | 7 | NumRow 6 |
| 0x08 | 8 | NumRow 7 |
| 0x09 | 9 | NumRow 8 |
| 0x0A | 10 | NumRow 9 |
| 0x0B | 11 | NumRow 0 |
| 0x0C | 12 | Dash (Minus) / Underscore |
| 0x0D | 13 | Equals / Plus |
| 0x0E | 14 | Backspace |
| 0x0F | 15 | Tab |
| 0x10 | 16 | Q |
| 0x11 | 17 | W |
| 0x12 | 18 | E |
| 0x13 | 19 | R |
| 0x14 | 20 | T |
| 0x15 | 21 | Y |
| 0x16 | 22 | U |
| 0x17 | 23 | I |
| 0x18 | 24 | O |
| 0x19 | 25 | P |
| 0x1A | 26 | Left Brace |
| 0x1B | 27 | Right Brace |
| 0x1C | 28 | Enter |
| 0x1E | 30 | A |
| 0x1F | 31 | S |
| 0x20 | 32 | D |
| 0x21 | 33 | F |
| 0x22 | 34 | G |
| 0x23 | 35 | H |
| 0x24 | 36 | J |
| 0x25 | 37 | K |
| 0x26 | 38 | L |
| 0x27 | 39 | Semicolon / Colon |
| 0x28 | 40 | Apostrophe / Doublequote |
| 0x29 | 41 | Backquote / Tilde |
| 0x2B | 43 | Backslash / Pipe |
| 0x2C | 44 | Z |
| 0x2D | 45 | X |
| 0x2E | 46 | C |
| 0x2F | 47 | V |
| 0x30 | 48 | B |
| 0x31 | 49 | N |
| 0x32 | 50 | M |
| 0x33 | 51 | Comma / Left Bracket |
| 0x34 | 52 | Period / Right Bracket |
| 0x35 | 53 | Slash / Question Mark |
| 0x37 | 55 | NumPad Asterisk |
| 0x39 | 57 | Spacebar |
| 0x3A | 58 | Caps Lock |
| 0x3B | 59 | F1 |
| 0x3C | 60 | F2 |
| 0x3D | 61 | F3 |
| 0x3E | 62 | F4 |
| 0x3F | 63 | F5 |
| 0x40 | 64 | F6 |
| 0x41 | 65 | F7 |
| 0x42 | 66 | F8 |
| 0x43 | 67 | F9 |
| 0x44 | 68 | F10 |
| 0x47 | 71 | NumPad 7 |
| 0x48 | 72 | NumPad 8 |
| 0x49 | 73 | NumPad 9 |
| 0x4A | 74 | NumPad Dash (Minus) |
| 0x4B | 75 | NumPad 4 |
| 0x4C | 76 | NumPad 5 |
| 0x4D | 77 | NumPad 6 |
| 0x4E | 78 | NumPad Plus |
| 0x4F | 79 | NumPad 1 |
| 0x50 | 80 | NumPad 2 |
| 0x51 | 81 | NumPad 3 |
| 0x52 | 82 | NumPad 0 |
| 0x53 | 83 | NumPad Dot (Period) |
| 0x57 | 87 | F11 |
| 0x58 | 88 | F12 |
| 0x9C | 156 | NumPad Enter |
| 0xB5 | 181 | NumPad Slash (Divide) |
| 0xC7 | 199 | Home |
| 0xC8 | 200 | Up Arrow |
| 0xC9 | 201 | PgUp |
| 0xCB | 203 | Left Arrow |
| 0xCD | 205 | Right Arrow |
| 0xCF | 207 | End |
| 0xD0 | 208 | Down Arrow |
| 0xD1 | 209 | PgDown |
| 0xD2 | 210 | Insert |
| 0xD3 | 211 | Delete |
";
    }
}
