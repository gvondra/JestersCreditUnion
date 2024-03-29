﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class NameGenerator : INameGenerator
    {
        private static IList<string> _firstNames;
        private static IList<string> _lastNames;
        private int _firstNameIndex;
        private int _lastNameIndex;

        public static async Task Initialize(
            string firstNamesDirectory = "FirstNames",
            string lastNamesDirectory = "LastNames")
        {
            _firstNames = ImmutableList<string>.Empty.AddRange(await LoadNames(firstNamesDirectory));
            _lastNames = ImmutableList<string>.Empty.AddRange(await LoadNames(lastNamesDirectory));
        }

        private static async Task<IEnumerable<string>> LoadNames(string directoryName)
        {
            SortedSet<string> names = new SortedSet<string>();
            if (Directory.Exists(directoryName))
            {
                string[] files = Directory.GetFiles(directoryName) ?? Array.Empty<string>();
                for (int i = 0; i < files.Length; i += 1)
                {
                    using (FileStream stream = new FileStream(files[i], FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, false))
                        {
                            while (stream.Position < stream.Length)
                            {
                                string line = (await reader.ReadLineAsync())?.Trim();
                                if (!string.IsNullOrEmpty(line))
                                    names.Add(line);
                            }
                            reader.Close();
                        }
                        stream.Close();
                    }
                }
            }
            return names;
        }

        public string GenerateName()
        {
            if (_lastNameIndex >= _lastNames.Count)
                throw new ApplicationException("Unable to generate unique name");
            string[] components = new string[]
            {
                _firstNames[_firstNameIndex],
                _lastNames[_lastNameIndex]
            };
            _firstNameIndex = (_firstNameIndex + 1) % _firstNames.Count;
            if (_firstNameIndex == 0)
                _lastNameIndex += 1;
            return string.Join(" ", components);
        }
    }
}
