using Splat;

namespace KrossKlient.Logging
{
    public class KrossLogger : ILogger
    {
        private readonly KrossEventSource _inner;


        public KrossLogger(KrossEventSource inner = null)
        {
            _inner = inner ?? Locator.Current.GetService<KrossEventSource>();
        }

        public void Write(string message, LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    _inner.Debug(message);
                    break;
                case LogLevel.Error:
                    _inner.Error(message);
                    break;
                case LogLevel.Fatal:
                    _inner.Critical(message);
                    break;
                case LogLevel.Info:
                    _inner.Info(message);
                    break;
                case LogLevel.Warn:
                    _inner.Warn(message);
                    break;
            }
        }

        public LogLevel Level { get; set; }
    }
}