using System;

namespace Monopoly {
    class Profit : Card {

        public int SumProfit { get; }

        public Profit(string text, int sumProfit) : base(text) {
            SumProfit = sumProfit;
        }

        public override void Action(Player player) {
            Console.WriteLine(Text);
            Console.WriteLine($"На ваш счет поступило {SumProfit}$");
            player.Money += SumProfit;
            Console.WriteLine();
        }

    }
}
