using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory.Timers
{
    public class Timer : IDisposable
    {
        TextWriter Writer { get; }
        Stopwatch Stopwatch { get; set; } = new Stopwatch();
        long ElapsedMs { get => Stopwatch.ElapsedMilliseconds; }
        public string Name { get; } = string.Empty;
        public int Level { get; }
        bool IsDisposed { get; set; }
        Timer(TextWriter writer, string name, int level = 0) 
        {
            Writer = writer;
            Name = name;
            Level = level;
            Stopwatch.Start();
        }

        public void Dispose()
        {
            Stopwatch.Stop();
            var reportLine = FormatReportLine(Name, Level, ElapsedMs);
            Writer.Write(reportLine);
        }

        public static Timer Start(TextWriter writer, string name = "*")
        {
            return new Timer(writer, name);
        }

        private static string FormatReportLine(string timerName, int level, long value)
        {
            var intro = new string(' ', level * 4) + timerName;
            return $"{intro,-20}: {value}\n";
        }
    }
}
