using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Monopoly {
    class Field {
        private readonly IList<ICell> _cells;
        private readonly Event _event;
        private readonly IList<IGrouping<ConsoleColor, Street>> _groupedStreet;

        public Field(XElement map, XElement property, XElement card) {

            _event = new Event();
            _event.AddCards(MakeCardArray(card));
            IList<XElement> cells = map.Elements().ToList();

            IList<Street> streets = property
                .Element("Streets")
                .Elements("Street")
                .Select(xElement => Street.FromXElement(xElement))
                .ToList();

            IList<Railway> railways = property
                .Element("Railways")
                .Elements("Railway")
                .Select(xElement => Railway.FromXElement(xElement))
                .ToList();

            _groupedStreet = streets.GroupBy(street => street.Color).ToList();

            _cells = new List<ICell>();
            FillField(cells, streets, railways);
        }

        private void FillField(IList<XElement> cells, IList<Street> streets, IList<Railway> railways) {

            for (int indexCell = 0, indexStreet = 0, indexRailway = 0; indexCell < cells.Count; indexCell++) {
                if (cells[indexCell].Name == "Street")
                    _cells.Add(streets[indexStreet++]);
                else if (cells[indexCell].Name == "Railway")
                    _cells.Add(railways[indexRailway++]);
                else if (cells[indexCell].Name == "Event")
                    _cells.Add(_event);
                else if (cells[indexCell].Name == "Start")
                    _cells.Add(new Start());
                else if (cells[indexCell].Name == "Prison")
                    _cells.Add(new Prison());
                else if (cells[indexCell].Name == "Tax")
                    _cells.Add(new Tax());
                else if (cells[indexCell].Name == "Imprison")
                    _cells.Add(new Imprison());
                else if (cells[indexCell].Name == "FreeParking")
                    _cells.Add(new FreeParking());
                else if (cells[indexCell].Name == "Fine")
                    _cells.Add(new Fine());
                else {
                    throw new ArgumentException($"{cells[indexCell].Name} является неизвестный аргументом");
                }
            }
        }

        private IList<Card> MakeCardArray(XElement card) {
            List<Card> cards = new List<Card>();

            cards.AddRange(card.Element("Teleports")
                    .Elements("Teleport")
                    .Select(teleport => new Teleport((string)teleport.Element("Text"), (int)teleport.Element("NewPosition")))
                    .ToList());

            cards.AddRange(card.Element("Profits")
                    .Elements("Profit")
                    .Select(profit => new Profit((string)profit.Element("Text"), (int)profit.Element("SumProfit")))
                    .ToList());

            cards.AddRange(card.Element("Losses")
                    .Elements("Loss")
                    .Select(loss => new Loss((string)loss.Element("Text"), (int)loss.Element("SumLoss")))
                    .ToList());

            cards.Shuffle();

            return cards;
        }

        public bool IsMonopoly(Player player) {
            return _groupedStreet
                .Select(group => group.All(street => street.Owner == player))
                .Any(result => result == true);
        }
        public IReadOnlyList<Street> GetStreetsWhichPlayerHasMonopoly(Player player) {
            return _groupedStreet
                .Where(group => group.All(street => street.Owner == player))
                .SelectMany(group => group)
                .ToList();
        }
        public ICell this[int index] => _cells[index];
        public int FieldSize => _cells.Count;
    }
}
