using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class NameGenerator : INameGenerator
    {
        private static readonly Random _random = new Random();
        private static IList<string> _firstNames;
        private static IList<string> _lastNames;
        private readonly SortedSet<string> _history = new SortedSet<string>();

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
            DateTime start = DateTime.UtcNow;
            int index;
            string name = null;
            string[] components = new string[2];
            bool found = false;
            while (name == null || !found)
            {
                if (DateTime.UtcNow.Subtract(start).TotalSeconds > 10.0)
                    throw new ApplicationException("Failed to generate name");
                index = _random.Next(_firstNames.Count);
                components[0] = _firstNames[index];
                index = _random.Next(_lastNames.Count);
                components[1] = _lastNames[index];
                name = string.Join(" ", components);
                found = _history.Add(name);
            }
            return name;
        }
    }
}
