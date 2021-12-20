using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly {
    class Player {
        private IList<Property> _property;

        public string Name { get; }
        public int Money { get; set; }
        public int Position { get; set; }
        public int Prisoner { get; set; }
        public bool Bankrupt { get; set; }

        public Player(string name) {
            Name = name;
            Money = 1500;
            Position = 0;
            Prisoner = 0;
            _property = new List<Property>();
            Bankrupt = false;
        }

        public void AddProperty(Property property) {
            if (property.Color == ConsoleColor.DarkGray) 
                _property.Where(item => item.Color == ConsoleColor.DarkGray).ForEach(item => item.Rent += item.IncreaseRent);

            _property.Add(property);
        }
        public void RemoveProperty(Property property) => _property.Remove(property);

        public bool IsHaveProperty() => _property.Count > 0;
        public bool IsIntoMinus() => Money < 0;
        public bool IsPrisoner() => Prisoner > 0;
        public bool IsPledgeProperty() => _property.Where(property => property.IsPledged == false && property.IsBeSold == true).Count() > 0;
        public bool IsMortgagedProperty() => _property.Where(property => property.IsPledged == true).Count() > 0;

        public void PrintAllProperty() {
            if (IsHaveProperty()) 
                _property.ForEach(property => Console.WriteLine($"{property.Name}{(property.IsPledged == true ? " - Заложена" : "")}")); 
            else
                Console.WriteLine("У вас нет недвижимости");
        }
        public void PrintPropertyForSale() {
            _property.Where(property => property.IsBeSold == true)
                .ForEach(property => Console.WriteLine($"{property.Name}{(property.IsPledged == true ? " - Заложена" : "")}"));
        }


        public void PrintPropertyThatCanBePledged() {
            _property.Where(property => property.IsPledged == false && property.IsBeSold == true)
                .ForEach(property => Console.WriteLine($"{property.Name}"));
        }
        public void PrintMortgagedProperty() {
            _property.Where(property => property.IsPledged == true)
                .ForEach(property => Console.WriteLine($"{property.Name}"));
        }

        public Property GetProperty(string name) => _property.Where(item => item.Name == name).FirstOrDefault();
    }
}
