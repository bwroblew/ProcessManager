using System.Diagnostics;

namespace ProcessManager
{
    public class SingleProcess
    {
        public SingleProcess(Process process)
        {
            Process = process;
        }
        public Process Process { get; }
    }
}