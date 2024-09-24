using NUnit.Framework;
using System;

namespace SimpleReactionMachine
{
    [TestFixture]
    public class Tester
    {
        private static IController controller = null!;
        private static IGui gui = null!;
        private static string displayText = string.Empty;
        private static int randomNumber = 0;
        private static int passed = 0;
        private static int totalAssertions = 0;

        [SetUp]
        public static void Setup()
        {
            controller = new SimpleReactionController();
            gui = new DummyGui();
            gui.Connect(controller);
            controller.Connect(gui, new RndGenerator());
            gui.Init();
            passed = 0;
            totalAssertions = 0;
        }

        [TearDown]
        public static void TearDown()
        {
            Console.WriteLine($"Total Assertions Passed: {passed}/{totalAssertions}");
        }

        [Test]
        public static void SimpleReactionTest()
        {
            // IDLE state tests
            DoReset('A', "Insert coin");
            DoGoStop('B', "Insert coin");
            DoTicks('C', 1, "Insert coin");

            // CoinInserted tests
            DoInsertCoin('D', "Press GO!");

            // READY state tests
            DoTicks('E', 1, "Press GO!");
            DoInsertCoin('F', "Press GO!");

            // GoStop tests
            randomNumber = 117;
            DoGoStop('G', "Wait...");

            // WAIT state tests
            DoTicks('H', randomNumber - 1, "Wait...");

            // RUN state tests
            DoTicks('I', 1, "0.00");
            DoTicks('J', 1, "0.01");
            DoTicks('K', 11, "0.12");
            DoTicks('L', 111, "1.23");

            // GoStop after RUN
            DoGoStop('M', "1.23");

            // STOP state tests
            DoTicks('N', 299, "1.23");
            DoTicks('O', 1, "Insert coin");

            // Coin inserted after STOP
            DoInsertCoin('P', "Press GO!");

            // READY -> WAIT tests
            randomNumber = 167;
            DoGoStop('Q', "Wait...");

            // WAIT -> IDLE after GoStop
            DoTicks('R', randomNumber - 1, "Wait...");
            DoGoStop('S', "Insert coin");
        }

        [Test]
        public static void ResetAndRestartTest()
        {
            // Restarting from different states
            gui.Init();
            DoReset('T', "Insert coin");

            randomNumber = 123;
            DoInsertCoin('U', "Press GO!");

            // Restart again
            gui.Init();
            DoReset('V', "Insert coin");

            // READY -> WAIT -> RUN -> STOP and reset
            randomNumber = 123;
            DoInsertCoin('W', "Press GO!");
            DoGoStop('X', "Wait...");
            gui.Init();
            DoReset('Y', "Insert coin");

            randomNumber = 137;
            DoInsertCoin('Z', "Press GO!");
            DoGoStop('a', "Wait...");
            DoTicks('b', randomNumber + 98, "0.98");
            gui.Init();
            DoReset('c', "Insert coin");

            randomNumber = 119;
            DoInsertCoin('d', "Press GO!");
            DoGoStop('e', "Wait...");
            DoTicks('f', randomNumber + 135, "1.35");
            DoGoStop('g', "1.35");

            // Resetting again
            gui.Init();
            DoReset('h', "Insert coin");
        }

        [Test]
        public static void TimeoutScenarioTest()
        {
            // Timeout during RUN state
            randomNumber = 120;
            DoInsertCoin('i', "Press GO!");
            DoGoStop('j', "Wait...");
            DoTicks('k', randomNumber + 199, "1.99");
            DoTicks('l', 50, "2.00");
        }

        // Utility methods for testing

        private static void DoReset(char ch, string expectedMsg)
        {
            controller.Init();
            GetMessage(ch, expectedMsg);
        }

        private static void DoGoStop(char ch, string expectedMsg)
        {
            controller.GoStopPressed();
            GetMessage(ch, expectedMsg);
        }

        private static void DoInsertCoin(char ch, string expectedMsg)
        {
            controller.CoinInserted();
            GetMessage(ch, expectedMsg);
        }

        private static void DoTicks(char ch, int n, string expectedMsg)
        {
            for (int t = 0; t < n; t++) controller.Tick();
            GetMessage(ch, expectedMsg);
        }

        private static void GetMessage(char ch, string expectedMsg)
        {
            totalAssertions++;  // Increment total assertions count
            Assert.AreEqual(expectedMsg.ToLower(), displayText.ToLower(),
                $"Test {ch} failed. Expected: {expectedMsg}, but got: {displayText}");
            passed++;
        }

        // DummyGui class implementation
        private class DummyGui : IGui
        {
            private IController controller = null!;

            public void Connect(IController controller)
            {
                this.controller = controller;
            }

            public void Init()
            {
                displayText = "?reset?";
            }

            public void SetDisplay(string msg)
            {
                displayText = msg;
            }
        }

        // RndGenerator class implementation
        private class RndGenerator : IRandom
        {
            public int GetRandom(int from, int to)
            {
                return randomNumber;
            }
        }
    }
}