using System;

namespace SimpleReactionMachine
{
    public abstract class GameState
    {
        protected SimpleReactionController controller;

        public GameState(SimpleReactionController controller)
        {
            this.controller = controller;
        }

        public abstract void CoinInserted();
        public abstract void GoStopPressed();
        public abstract void Tick();
    }

    public class NoCoinInserted : GameState
    {
        public NoCoinInserted(SimpleReactionController controller) : base(controller)
        {
            controller.Gui.SetDisplay("Insert coin");
        }

        public override void CoinInserted()
        {
            controller.SetState(new CoinInsertedState(controller));
            controller.Gui.SetDisplay("Press GO!");
        }

        public override void GoStopPressed() { }
        public override void Tick() { }
    }

    public class CoinInsertedState : GameState
    {
        public CoinInsertedState(SimpleReactionController controller) : base(controller) { }

        public override void CoinInserted() { }

        public override void GoStopPressed()
        {
            controller.SetState(new Delay(controller));
            controller.Gui.SetDisplay("Wait...");
        }

        public override void Tick() { }
    }

    public class Delay : GameState
    {
        private int waitTime;

        public Delay(SimpleReactionController controller) : base(controller)
        {
            waitTime = controller.Random.GetRandom(100, 250);
        }

        public override void CoinInserted() { }

        public override void GoStopPressed()
        {
            controller.SetState(new NoCoinInserted(controller));
            controller.Gui.SetDisplay("Insert coin");
        }

        public override void Tick()
        {
            waitTime--;
            if (waitTime <= 0)
            {
                controller.SetState(new WaitForReaction(controller));
                controller.Gui.SetDisplay("0.00");
            }
        }
    }

    public class WaitForReaction : GameState
    {
        private int timer = 0;
        private bool canPress = false;

        public WaitForReaction(SimpleReactionController controller) : base(controller) { }

        public override void CoinInserted() { }

        public override void GoStopPressed()
        {
            if (canPress)
            {
                controller.SetState(new ReactionTimeDisplay(controller, timer / 100.0));
                controller.Gui.SetDisplay($"{timer / 100.0:F2}");
            }
        }

        public override void Tick()
        {
            timer ++;
            canPress = true;
            controller.Gui.SetDisplay($"{timer / 100.0:F2}");
            if (timer >= 200)
            {
                controller.SetState(new ReactionTimeDisplay(controller, timer / 100.0));
            }
        }
    }

    public class ReactionTimeDisplay : GameState
    {
        private int displayTime = 300;
        private double reactionTime;

        public ReactionTimeDisplay(SimpleReactionController controller, double reactionTime) : base(controller)
        {
            this.reactionTime = reactionTime;
            controller.Gui.SetDisplay($"{reactionTime:F2}");
        }

        public override void CoinInserted() { }

        public override void GoStopPressed()
        {
            controller.SetState(new NoCoinInserted(controller));
            controller.Gui.SetDisplay("Insert coin");
        }

        public override void Tick()
        {
            displayTime--;
            if (displayTime <= 0)
            {
                controller.SetState(new NoCoinInserted(controller));
                controller.Gui.SetDisplay("Insert coin");
            }
        }
    }

    public class SimpleReactionController : IController
    {
        public IGui Gui { get; private set; }
        public IRandom Random { get; private set; }
        private GameState currentState;

        public SimpleReactionController() { }

        public void Connect(IGui gui, IRandom random)
        {
            Gui = gui;
            Random = random;
            Init();
        }

        public void Init()
        {
            currentState = new NoCoinInserted(this);
        }

        public void SetState(GameState state)
        {
            currentState = state;
        }

        public void CoinInserted()
        {
            currentState.CoinInserted();
        }

        public void GoStopPressed()
        {
            currentState.GoStopPressed();
        }

        public void Tick()
        {
            currentState.Tick();
        }
    }
}
