using System;
using static ToolBox.TimeOfDay;

namespace ToolBox
{
    public class DayTime
    {
        public TimeOfDay TimeOfDay { get; private set; }
        public float TimeOfDayFactor { get; private set; }
        public long LastFrameTime { get; private set; }

        private float time = 0;

        public void Update(long frameTime)
        {
            LastFrameTime = frameTime;

            time = (time + frameTime / 2) % 24000;
            if (time >= 0 && time < Dawn.StartTime)
            {
                TimeOfDay = Night;
                TimeOfDayFactor = time / Dawn.StartTime;
            }
            else if (time >= Dawn.StartTime && time < Morning.StartTime)
            {
                TimeOfDay = Dawn;
                TimeOfDayFactor = (time - Dawn.StartTime) / (Morning.StartTime - Dawn.StartTime);
            }
            else if (time >= Morning.StartTime && time < Noon.StartTime)
            {
                TimeOfDay = Morning;
                TimeOfDayFactor = (time - Morning.StartTime) / (Noon.StartTime - Morning.StartTime);
            }
            else if (time >= Noon.StartTime && time < Evening.StartTime)
            {
                TimeOfDay = Noon;
                TimeOfDayFactor = (time - Noon.StartTime) / (Evening.StartTime - Noon.StartTime);
            }
            else if (time >= Evening.StartTime && time < Night.StartTime)
            {
                TimeOfDay = Evening;
                TimeOfDayFactor = (time - Evening.StartTime) / (Night.StartTime - Evening.StartTime);
            }
            else
            {
                TimeOfDay = Night;
                TimeOfDayFactor = (time - Evening.StartTime) / (Night.StartTime - Evening.StartTime);
            }
        }
    }

    public class TimeOfDay : IEquatable<TimeOfDay>
    {
        private readonly int id;
        public int StartTime { get; }

        public static TimeOfDay Dawn = new TimeOfDay(0, 5000);
        public static TimeOfDay Morning = new TimeOfDay(1, 8000);
        public static TimeOfDay Noon = new TimeOfDay(2, 12000);
        public static TimeOfDay Evening = new TimeOfDay(3, 19000);
        public static TimeOfDay Night = new TimeOfDay(4, 24000);

        private TimeOfDay(int id, int startTime)
        {
            this.id = id;
            StartTime = startTime;
        }

        public override bool Equals(object obj)
        {
            if (obj is TimeOfDay timeOfDay)
            {
                return Equals(timeOfDay);
            }

            return false;
        }

        public bool Equals(TimeOfDay other)
        {
            return id == other.id;
        }

        public override int GetHashCode()
        {
            return id;
        }
    }
}
