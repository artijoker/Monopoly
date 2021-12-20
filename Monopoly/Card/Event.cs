using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly {
    class Event : SpecialCell {
        private IList<Card> _cards;
        private int position;

        public Event() : base("Событие", ConsoleColor.White) {
            _cards = null;
            position = 0;
        }

        public void AddCards(IList<Card> cards) => _cards = cards;

        public override void Action(Player player) {
            Console.WriteLine($"Вы попали на клетку {Name}");
            _cards[position].Action(player);
            position++;
            if (position == _cards.Count) {
                _cards.Shuffle();
                position = 0;
            }
        }


    }
}
