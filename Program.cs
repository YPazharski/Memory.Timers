using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnitLite;

class Program
{
	static void Main(string[] args)
	{
		//var stopWatch = new Stopwatch();
		//var elapsedMs = stopWatch.ElapsedMilliseconds;
		new AutoRun().Execute(args);
	}
}
