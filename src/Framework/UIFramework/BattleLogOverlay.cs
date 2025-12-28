using Godot;
using prototype_cardCrpg_code_class.BattleLog;
using prototype_cardCrpg_code_class.GSignalSystem;


namespace prototype_cardCrpg_code_class.Framework.UIFramework;

public partial class BattleLogOverlay : CanvasLayer
{
    private Control panel;
    private TextEdit text;
    private bool open = false;

    public override void _Ready()
    {
        panel = GetNode<Control>("Panel");
        text = GetNode<TextEdit>("Panel/TextEdit");

        text.Editable = false;
        panel.Visible = false;

        GSignalManager.Register<BattleLog.BattleLog.Signal_LogUpdated>(OnLogUpdated);
    }

    public override void _ExitTree()
    {
        GSignalManager.Unregister<BattleLog.BattleLog.Signal_LogUpdated>(OnLogUpdated);
    }

    public override void _Input(InputEvent _event)
    {
        if (_event is not InputEventKey key) return;
        if (!key.Pressed || key.Echo) return;

        if (key.Keycode == Key.F1)
        {
            open = !open;
            panel.Visible = open;

            if (open)
            {
                RefreshFullText(true);
            }

            GetViewport().SetInputAsHandled();
        }
    }

    private void OnLogUpdated(BattleLog.BattleLog.Signal_LogUpdated _sig)
    {
        if (!open) return;

        RefreshFullText(false);
    }

    private void RefreshFullText(bool forceScrollToBottom)
    {
        var wasAtBottom = text.ScrollVertical >= text.GetLineCount() - 2;

        var full = this.GetFullBattleLog();
        text.Text = full.TrimStart('\n', '\r');

        if (forceScrollToBottom || wasAtBottom)
            CallDeferred(nameof(ScrollToBottom));
    }

    private void ScrollToBottom()
    {
        text.ScrollVertical = text.GetLineCount();
    }
}