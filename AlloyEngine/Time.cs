using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alloy
{
    public static class Time
    {
        public static double DeltaTime { get; private set; }
        public static double UnscaledDeltaTime { get; private set; }
        public static double TimeSinceStartup { get; private set; }
        public static double UnscaledTimeSinceStartup { get; private set; }

        public static double TimeScale { get; set; } = 1.0f;

        public static void UpdateTime(double deltaTime)
        {
            DeltaTime = deltaTime * TimeScale;
            UnscaledDeltaTime = deltaTime;
            TimeSinceStartup += DeltaTime;
            UnscaledTimeSinceStartup += UnscaledDeltaTime;
        }
    }
}
