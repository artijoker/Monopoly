using System;


namespace Monopoly {
    class Loss : Card {

        public int SumLoss { get; }
        public Loss(string text, int sumLoss) : base(text) {
            SumLoss = sumLoss;
        }

        public override void Action(Player player) {
            Console.WriteLine(Text);
            Console.WriteLine($"С вашего счета было списано {SumLoss}$");
            player.Money -= SumLoss;
            Console.WriteLine();
        }
    }
}
