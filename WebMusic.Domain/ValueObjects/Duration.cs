namespace WebMusic.Domain.ValueObjects
{
    public record Duration
    {
        public int TotalSeconds { get; }

        public Duration(int totalSeconds)
        {
            if (totalSeconds < 0)
                throw new ArgumentException("Duration cannot be negative", nameof(totalSeconds));

            TotalSeconds = totalSeconds;
        }

        public TimeSpan ToTimeSpan() => TimeSpan.FromSeconds(TotalSeconds);
        
        public string ToFormattedString()
        {
            var timeSpan = ToTimeSpan();
            return timeSpan.Hours > 0 
                ? $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}"
                : $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }

        public static implicit operator int(Duration duration) => duration.TotalSeconds;
        public static implicit operator Duration(int seconds) => new(seconds);
    }
}
