using System;
using System.Text;
using Godot;
using prototype_cardCrpg_code_class.GSignalSystem;

namespace prototype_cardCrpg_code_class.BattleLog;

public static class BattleLog
{
    #region Signal
    public struct Signal_LogUpdated
    {
        public LogEntry _newLog;
    }
    #endregion
    public struct LogEntry
    {
        public System.DateTime recordTime;
        public string log;
        
        public string ToLog() => $"[{recordTime.ToString("HH:mm:ss")}]{log}";
    }
    private static readonly System.Collections.Generic.List<LogEntry> logs = new();
    
    private static string GetFullBattleLogs()
    {
        StringBuilder sb = new();
        foreach (var logEntry in logs)
        {
            sb.Append($"\n");
            sb.Append(logEntry.ToLog());
        }
        return sb.ToString();
    }
    
    public static void ToBattleLog(this string _sourceString)
    {
        LogEntry newLog = new LogEntry
        {
            recordTime = DateTime.Now,
            log = _sourceString
        };
        logs.Add(newLog);
        GSignalManager.Emit(new Signal_LogUpdated {_newLog = newLog});
    }

    public static string GetFullBattleLog(this Node _requester)
    {
        return GetFullBattleLogs();
    }
}