using Godot;
using prototype_cardCrpg_code_class.GSignalSystem;

// 给“类 BattleLog”起一个别名，避免和“命名空间 BattleLog”撞名
using BattleLogClass = prototype_cardCrpg_code_class.BattleLog.BattleLog;

namespace prototype_cardCrpg_code_class.Framework.UIFramework;

public partial class BattleLogTester : Node2D
{
    private int counter = 0;
    public override void _Ready()
    {
        GSignalManager.Register<BattleLogClass.Signal_LogUpdated>(OnLogUpdated);
        BattleLogClass.ToBattleLog("BattleLogTester Ready");
    }

    public override void _ExitTree()
    {
        GSignalManager.Unregister<BattleLogClass.Signal_LogUpdated>(OnLogUpdated);
    }
    public override void _UnhandledInput(InputEvent _event)
    {
        if (@_event is not InputEventKey key) return;
        if (!key.Pressed || key.Echo) return; 

        if (key.Keycode == Key.K)
        {
            counter++;
            BattleLogClass.ToBattleLog($"K single log #{counter}");
        }
        else if (key.Keycode == Key.L)
        {
            for (int i = 0; i < 10; i++)
            {
                counter++;
                BattleLogClass.ToBattleLog($"L burst log #{counter} (i={i})");
            }
        }
       
    }
    private void OnLogUpdated(BattleLogClass.Signal_LogUpdated _sig)
    {
        GD.Print(_sig._newLog.ToLog());
    }
}