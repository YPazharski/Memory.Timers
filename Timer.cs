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
        public string Name { get; } = string.Empty;
        public Timer Parent { get; set; }
        LinkedList<string> Report { get; set; }
        public int Level { get; }
        Stopwatch Stopwatch { get; set; } = new Stopwatch();
        long Value { get => Stopwatch.ElapsedMilliseconds; }
        long ChildrenValue { get; set; }
        public bool HadChildren { get; private set; }
        Timer(TextWriter writer, string name, Timer parent = null, int level = 0) 
        {
            Writer = writer ?? throw new ArgumentNullException(nameof(writer));
            Name = name;
            Parent = parent;
            Report = new LinkedList<string>();
            Level = level;
            Stopwatch.Start();
        }

        public void Dispose()
        {
            Stopwatch.Stop();
            var reportLine = FormatReportLine(Name, Level, Value);
            Report.AddFirst(reportLine);
            if (HadChildren)
                AddRestReportLine();
            if (Parent != null)
                ReportToParent();
            else
                WriteReport();
        }

        private void WriteReport()
        {
            foreach (var line in Report)
            {
                Writer.Write(line);
            }
        }

        private void ReportToParent()
        {
            Parent.ChildrenValue += Value;
            foreach (var line in Report)
            {
                Parent.Report.AddLast(line);
            }
        }

        void AddRestReportLine()
        {
            var restTime = Value - ChildrenValue;
            var restReportLine = FormatReportLine("Rest", Level + 1, restTime);
            Report.AddLast(restReportLine);
        }

        public static Timer Start(TextWriter writer, string name = "*")
        {
            return new Timer(writer, name);
        }

        public Timer StartChildTimer(string name)
        {
            HadChildren = true;
            return new Timer(Writer, name, this, Level + 1);
        }

        private static string FormatReportLine(string timerName, int level, long value)
        {
            var intro = new string(' ', level * 4) + timerName;
            return $"{intro,-20}: {value}\n";
        }
    }
}
