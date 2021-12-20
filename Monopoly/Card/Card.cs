using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly {
    abstract class Card : Event {
        public string Text { get; }
        public Card(string text) {
            Text = text;
        }
        public abstract override void Action(Player player);
    }
}
