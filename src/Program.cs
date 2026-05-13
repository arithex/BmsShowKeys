using System;
using System.Collections.Generic;
using System.IO;

namespace ShowKeys
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine($"Expected: path to key file");
                return;
            }

            // Validate key file location.
            string key_path = args[0];
            if (!File.Exists(key_path))
            {
                Console.Error.WriteLine($"File note found: {key_path}");
                return;
            }

            // Init 8 buckets, each containing all scancodes (one bucket for each modifier-combo).
            ushort[] valid_scancodes = ScancodeTable.GetAllValidScancodesForUS();

            List<ushort>[] modifier_buckets = new List<ushort>[8] {
                new List<ushort>(valid_scancodes),
                new List<ushort>(valid_scancodes),
                new List<ushort>(valid_scancodes),
                new List<ushort>(valid_scancodes),
                new List<ushort>(valid_scancodes),
                new List<ushort>(valid_scancodes),
                new List<ushort>(valid_scancodes),
                new List<ushort>(valid_scancodes)
            };

            // Parse key file -- remove used keys from appropriate bucket.
            foreach (var binding in KeyfileParser.GetKeyLines(key_path))
            {
                if (binding.Scancode > 255) // ignore 0xFFFFFFFF
                    continue;

                var bucket = modifier_buckets[binding.ModifierFlags];

                ushort sc = (ushort)(binding.Scancode);
                if (bucket.Contains(sc))
                    bucket.Remove(sc);
            }

            // Output remaining (unassigned) keys, grouped by bucket.
            Console.WriteLine("\n--- UNASSIGNED - UNSHIFTED ---");
            _ReportScancodes(modifier_buckets[0]);

            Console.WriteLine("\n--- UNASSIGNED - SHIFT ---");
            _ReportScancodes(modifier_buckets[1]);

            Console.WriteLine("\n--- UNASSIGNED - CTRL ---");
            _ReportScancodes(modifier_buckets[2]);

            Console.WriteLine("\n--- UNASSIGNED - SHIFT+CTRL ---");
            _ReportScancodes(modifier_buckets[3]);

            Console.WriteLine("\n--- UNASSIGNED - ALT ---");
            _ReportScancodes(modifier_buckets[4]);

            Console.WriteLine("\n--- UNASSIGNED - SHIFT+ALT ---");
            _ReportScancodes(modifier_buckets[5]);

            Console.WriteLine("\n--- UNASSIGNED - CTRL+ALT ---");
            _ReportScancodes(modifier_buckets[6]);

            Console.WriteLine("\n--- UNASSIGNED - SHIFT+CTRL+ALT ---");
            _ReportScancodes(modifier_buckets[7]);

            return;
        }

        static void _ReportScancodes(List<ushort> bucket)
        {
            foreach (ushort sc in bucket)
            {
                Console.WriteLine(ScancodeTable.GetLabelForScanCode(sc));
            }
        }

    }
}