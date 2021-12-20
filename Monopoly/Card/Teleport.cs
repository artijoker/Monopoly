using System;

namespace Monopoly {
    class Teleport : Card {

        public int NewPosition { get; }

        public Teleport(string text, int newPosition) : base(text) {
            NewPosition = newPosition;
        }

        public override void Action(Player player) {
            Console.WriteLine(Text);
            Console.WriteLine();
            player.Position = NewPosition;
            Game.Field[player.Position].Action(player);
        }
    }
}
