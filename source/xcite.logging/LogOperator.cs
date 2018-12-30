namespace xcite.logging {
    /// <summary> Implements the log stream writing based on the current configuration. </summary>
    public class LogOperator {
        private readonly LogConfiguration _config;
        private readonly TextFormatter _textFormatter;
        private readonly LogStreamManager _streamManager;

        /// <inheritdoc />
        public LogOperator(LogConfiguration config, TextFormatter textFormatter, LogStreamManager streamManager) { 
            _config = config;
            _textFormatter = textFormatter;
            _streamManager = streamManager;
        }

        /// <summary> Writes the given <paramref name="logData"/> to all registered log streams. </summary>
        public void Write(LogData logData) {
            string pattern = _config.Pattern;
            ILogStream[] logStreams = _config.Streams;
            
            string record = _textFormatter.FormatValue(pattern, logData);
            _streamManager.Write(logStreams, logData.name, logData.level, record);

            if (logData.exception != null) {
                LogData exLgDt = new LogData {
                    name = logData.name, level = logData.level,
                    value = logData.exception.ToString(),
                    exception = logData.exception
                };

                string exRec = _textFormatter.FormatValue(pattern, exLgDt);
                _streamManager.Write(logStreams, exLgDt.name, exLgDt.level, exRec);
            }
        }
    }
}